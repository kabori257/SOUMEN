using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの回転軸にアタッチする、プレイヤーを移動させるスクリプト。
/// ジャンプはここから呼び出される。
/// </summary>
public class NewPlayerMove : MonoBehaviour
{
    /* public変数*/
    public bool isLanding = false;
    public bool movable = false;

    /* --- SerializeFieldの変数 --- */
    [SerializeField] private GameObject playerManageObj;
    [SerializeField] private GameObject player;

    /* --- 変数 ---*/

    /* --- private変数 --- */
    private PlayerController playerController;
    private PlayerManager playerManager;

    private AudioSource audioSource;
    private SoundManager soundManager;

    private float speed = 0;
    private float dashTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        playerController = player.GetComponent<PlayerController>();
        playerManager = playerManageObj.GetComponent<PlayerManager>();

        audioSource = GetComponent<AudioSource>();
        soundManager = GameObject.Find("SoundManager(Clone)").GetComponent<SoundManager>();

        speed = playerManager.speed;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 roll = transform.rotation.eulerAngles;

        //プレイヤーが着地した瞬間か。PlayerControllerのjump()よりtrueにされる。
        if (isLanding)
        {
            Land(ref roll);
        }
        else //プレイヤーが着地した瞬間ではなかったら
        {
            //ジャンプ中ではなかったら移動処理
            if (!playerController.isJump && movable)
            {
                Move(ref roll);
            }
        }
    }

    /// <summary>
    /// プレイヤーの移動処理。
    /// ジャンプのトリガー管理も含む。
    /// </summary>
    /// <param name="roll"></param>
    void Move(ref Vector3 roll)
    {
        if (MyJoyCon.joyconDec.button == Joycon.Button.SHOULDER_2)
        {
            if(dashTime < playerManager.dashTime)
            {
                dashTime += Time.deltaTime;
                playerManager.speed = Mathf.Lerp(speed, playerManager.dashSpeed, dashTime / playerManager.dashTime);
            }

            if (audioSource.clip != soundManager.speedUpSE || !audioSource.isPlaying)
            {
                audioSource.clip = soundManager.speedUpSE;
                audioSource.volume = soundManager.jumpVolume;
                audioSource.Play();
            }
        }
        else
        {
            if (dashTime > 0)
            {
                dashTime -= Time.deltaTime;
                playerManager.speed = Mathf.Lerp(speed, playerManager.dashSpeed, dashTime / playerManager.dashTime);
            }
            
            if (audioSource.clip == soundManager.speedUpSE && audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
        
        roll.z -= playerManager.speed * MyJoyCon.joyconDec.accel.y;
        roll.z += DebugMode();

        if (Mathf.DeltaAngle(0, roll.z) > 90) roll.z = 90;
        else if(Mathf.DeltaAngle(0, roll.z) < -90) roll.z = -90;

        transform.rotation = Quaternion.Euler(roll);

        //プレイヤーの軸に対する角度の絶対値が90度以上になったらジャンプ
        if (Mathf.Abs(Mathf.DeltaAngle(0, transform.rotation.eulerAngles.z)) >= 90f && dashTime / playerManager.dashTime >= 1)
        {
            audioSource.clip = soundManager.jumpSE;
            audioSource.volume = soundManager.jumpVolume;
            audioSource.Play();

            playerController.isJump = true;
            playerController.SearchHuman();
            //SoundFlagManager.isJump = true;
            Time.timeScale = 0.5f;
            dashTime = 0;
            playerManager.speed = speed;
        }
    }

    /// <summary>
    /// 着地時にプレイヤーの位置を少し中心に戻す処理。
    /// </summary>
    /// <param name="roll"></param>
    void Land(ref Vector3 roll)
    {
        roll.z = Mathf.Lerp(Mathf.DeltaAngle(0, transform.rotation.eulerAngles.z), 0, Time.deltaTime);
        Time.timeScale = Mathf.Lerp(0.5f, 1, Time.deltaTime);

        transform.rotation = Quaternion.Euler(roll);

        if (90 - Mathf.Abs(Mathf.DeltaAngle(0, transform.rotation.eulerAngles.z)) > 10)
        {
            isLanding = false;
            Time.timeScale = 1;

            audioSource.clip = soundManager.landSE;
            audioSource.volume = soundManager.landlVolume;
            audioSource.Play();
        }
    }

    /// <summary>
    /// デバッグ用の矢印キーでの移動。
    /// </summary>
    /// <returns></returns>
    float DebugMode()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            return -playerManager.speed / 2;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            return playerManager.speed / 2;
        }
        return 0;
    }

}
