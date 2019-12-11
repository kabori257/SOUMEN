using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 移動と着地以外のプレイヤーの処理
/// </summary>
public class PlayerController : MonoBehaviour
{
    /* public変数*/
    public bool isJump = false;             //ジャンプ中か
    public bool isJumpDown = false;         //ジャンプ時に上昇しているか落下しているか

    /* --- SerializeFieldの変数 --- */
    [SerializeField] GameObject playerManageObj;
    [SerializeField] GameObject playerRotateAxis;
    [SerializeField] GameObject[] attackSoumens;
    [SerializeField] private float triggerValue;        //そうめんを撃つJoyConの角度変化のボーダー

    /* --- 変数 ---*/

    /* --- private変数 --- */
    private PlayerManager playerManager;    //プレイヤーのステータスなどを管理するスクリプト
    private NewPlayerMove newPlayerMove;    //プレイヤーの移動を管理するスクリプト。PlayerAxisにアタッチされている。
      
    private SortedDictionary<float, GameObject> humanDictionary = new SortedDictionary<float, GameObject>();    //ジャンプした時に近くの人間をリストに格納する

    private int currentLevel = 0;   //そうめんの大きさのレベル
    
    private float waitTime = 0;     //そうめんを撃つクールダウンを待っている時間

    private Vector3 jumpStartPos;        //ジャンプした瞬間のポジション

    private AudioSource audioSource;
    private SoundManager soundManager;

    // Start is called before the first frame update
    void Start()
    {
        playerManager = playerManageObj.GetComponent<PlayerManager>();
        newPlayerMove = playerRotateAxis.GetComponent<NewPlayerMove>();

        audioSource = GetComponent<AudioSource>();
        soundManager = GameObject.Find("SoundManager(Clone)").GetComponent<SoundManager>();
    }

    // Update is called once per frame
    void Update()
    {

        Jump();

        Shot();
    }

    /// <summary>
    /// ジャンプの処理。
    /// isJumpがFalseの時はジャンプ時の位置を保存するために、現在の位置を記録し続け、
    /// PlayerMoveからisJumpをtrueにされたときはジャンプの処理が実行される。
    /// </summary>
    public void Jump()
    {
        Vector3 jumpPos = transform.position;   //ジャンプ処理後のポジションを記録し続ける。

        if (isJump)
        {
            this.transform.localRotation = Quaternion.Euler(90, 0, 0);
            SearchHuman();

            //最高高度に達した場合落下処理に切り替える
            if (jumpPos.y > jumpStartPos.y + playerManager.jumpHeight - 0.1f)
            {
                isJumpDown = true;
            }


            if (!isJumpDown)    //ジャンプの上昇中だったら、プレイヤーを上昇させる。ジャンプの時の座標を線形補完する。
            {
                jumpPos = Vector3.Lerp(jumpPos, new Vector3(jumpStartPos.x, jumpStartPos.y + playerManager.jumpHeight, jumpStartPos.z),
                    Time.deltaTime * playerManager.jumpSpeed);
            }
            else                //ジャンプの落下中だったら、プレイヤーを落下させる。落下時の座標を線形補完する。
            {
                jumpPos = Vector3.Lerp(jumpPos, jumpStartPos, Time.deltaTime * playerManager.jumpSpeed);

                //ジャンプ開始時の位置と、現在の位置の距離が0.1以下になったら着地の処理をする。
                if(Vector3.Distance(jumpStartPos, jumpPos) < 0.1f)
                {
                    isJump = false;
                    isJumpDown = false;

                    jumpPos = jumpStartPos;

                    newPlayerMove.isLanding = true;
                    this.transform.localRotation = Quaternion.Euler(0, 0, 0);

                    humanDictionary.Clear();
                }
            }

            //ジャンプ処理後の最終的なポジションを格納
            transform.position = jumpPos;
        }
        else
        {
            //初めにジャンプした位置を記録
            jumpStartPos = transform.position;
        }
    }

    /// <summary>
    /// そうめんから一定距離の人間をリストに格納するメソッド。近くにいるの要素数を返す。
    /// </summary>
    public void SearchHuman()
    {
        humanDictionary.Clear();
        //タグ指定されたオブジェクトを配列で取得する
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Human"))
        {
            //自身と取得したオブジェクトの距離を取得
            float tmpDis = Vector3.Distance(obj.transform.position, transform.position);

            if (tmpDis < playerManager.shotRange)
            {
                humanDictionary.Add(tmpDis, obj);
            }
        }
    }

    /// <summary>
    /// そうめんを撃つ処理。
    /// </summary>
    private void Shot()
    {
        List<GameObject> activeSoumenList = new List<GameObject>();

        if (waitTime < playerManager.shotCoolTime)
        {
            waitTime += Time.deltaTime;
        }

        if (isJump)
        {

            if (humanDictionary.Count > 0)
            {
                //this.transform.LookAt(humanDictionary.First().Value.transform);

                //JoyConが振られたら（x, y, zの加速度の合計がtriggerValueより多かったら）かつ、クールダウンが終わっていたら。
                if ((Mathf.Abs(MyJoyCon.joyconDec.accel.x ) + Mathf.Abs(MyJoyCon.joyconDec.accel.y)
                    + Mathf.Abs(MyJoyCon.joyconDec.accel.z) > triggerValue || Input.GetKey(KeyCode.Space)) && waitTime >= playerManager.shotCoolTime)
                {
                    audioSource.clip = soundManager.shotSE;
                    audioSource.volume = soundManager.shotVolume;
                    audioSource.Play();

                    waitTime = 0;

                    foreach (GameObject attackSoumen in attackSoumens)
                    {
                        if (attackSoumen.transform.parent.gameObject.activeSelf) activeSoumenList.Add(attackSoumen);
                    }

                    foreach (GameObject activeSoumen in activeSoumenList)
                    {
                        activeSoumen.GetComponent<HookShot>().target = humanDictionary.First().Value;
                        activeSoumen.GetComponent<HookShot>().targetDist = humanDictionary.First().Key;
                        activeSoumen.GetComponent<HookShot>().AttackSoumenHandleEnable(true);
                        activeSoumen.GetComponent<HookShot>().trigger = true;
                    }

                    playerManager.AddSoumen();

                    activeSoumenList.Clear();
                    humanDictionary.Remove(humanDictionary.First().Key);
                }
            }
        }
    }
}
