using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ステージを無限スクロールさせるためのスクリプト。
/// </summary>
public class StageScroll : MonoBehaviour
{
    /* public変数*/
    [HideInInspector]
    public int playerPosCount = 0;                          //プレイヤーがいくつステージを超えて来たか

    [Header("ゴールまでの距離")]
    public int goalPosCount;                                //ゴールステージの距離

    /* --- SerializeFieldの変数 --- */
    [Header("一つのステージのパーツに箸が何行か")]
    [SerializeField] private int chopStickRowNum;

    [Header("一つのステージのパーツに箸が何列か")]
    [SerializeField] private int chopStickColumnNum;
    
    [Header("ステージの進む速さ")]
    public float speed;                   //ステージが移動するスピード

    [HideInInspector]
    public float tmpSpeed;

    [Header("")]
    [SerializeField] private GameObject stageObj;           //ステージ
    [SerializeField] private GameObject startStageObj;      //ゲーム開始時に生成するステージ
    [SerializeField] private GameObject goalStageObj;       //ゴールステージ
    [SerializeField] private Slider progress;               

    /* --- 変数 ---*/

    /* --- private変数 --- */
    private List<GameObject> stages = new List<GameObject>();       //生成したステージのリスト
    private bool isGoal = false;


    // Start is called before the first frame update
    void Start()
    {
        progress.minValue = 0;
        progress.maxValue = goalPosCount;
        progress.value = 0;

        stageObj.GetComponent<PutChopStickPoint>().chopStickRowNum = chopStickRowNum;
        stageObj.GetComponent<PutChopStickPoint>().chopStickColumnNum = chopStickColumnNum;

        goalStageObj.GetComponent<PutChopStickPoint>().chopStickRowNum = chopStickRowNum;
        goalStageObj.GetComponent<PutChopStickPoint>().chopStickColumnNum = chopStickColumnNum;

        //初めにステージを10個生成
        for (int i = 0; i < 10; i++)
        {
            if (i == goalPosCount)
            {
                stages.Add(Instantiate(goalStageObj, new Vector3(0, 0, i * 100), new Quaternion()));
                goalPosCount = 100;
            }
            else if (i < 1)
                stages.Add(Instantiate(startStageObj, new Vector3(0, 0, i * 100), new Quaternion()));
            else
                stages.Add(Instantiate(stageObj, new Vector3(0, 0, i * 100), new Quaternion()));

        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < stages.Count; i++)
        {
            //AddForceによってステージを後ろへ移動させていく。
            stages[i].GetComponent<Rigidbody>().AddForce(tmpSpeed * ((Vector3.back * tmpSpeed) - stages[i].GetComponent<Rigidbody>().velocity));

            if (stages[i].transform.position.z < -200)
            {
                //プレイ時間内か
                if (playerPosCount + 10 < goalPosCount)
                {
                    if(!isGoal) stages.Add(Instantiate(stageObj, new Vector3(0, 0, (stages.Count - 2) * 100), new Quaternion()));
                    else stages.Add(Instantiate(startStageObj, new Vector3(0, 0, (stages.Count - 2) * 100), new Quaternion()));
                }
                else
                {
                    stages.Add(Instantiate(goalStageObj, new Vector3(0, 0, (stages.Count - 2) * 100), new Quaternion()));
                    isGoal = true;
                    goalPosCount = 100;      //ゴールステージの後ろにもステージを続けるため
                }

                //ステージの廃棄
                Destroy(stages[0]);
                stages.RemoveAt(0);
                playerPosCount++;
                progress.value = playerPosCount;
            }
        }
    }
}
