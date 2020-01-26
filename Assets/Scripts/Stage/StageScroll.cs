using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

[System.Serializable]
public struct stagePartition
{
    [Header("左端の角度")]
    public float minAngle;

    [Header("右端の角度")]
    public float maxAngle;

    [Header("箸の行数")]
    public int chopStickRowNum;

    [Header("箸の列数")]
    public int chopStickColumnNum;

    [Header("箸の色(デバッグ用)")]
    public Color color;
}

/// <summary>
/// ステージを無限スクロールさせるためのスクリプト。
/// </summary>
public class StageScroll : MonoBehaviour
{

    /* public変数*/
    [HideInInspector]
    public int playerPosCount = 0;                          //プレイヤーがいくつステージを超えて来たか

    [HideInInspector]
    public bool isStoped = false;

    public bool debugMode;

    [SerializeField]
    [Header("ステージの長さ")]
    public int goalPosCount;                                //ゴールステージの距離

    [SerializeField]
    [Header("ステージの進む速さ")]
    public float speed;                   //ステージが移動するスピード

    [SerializeField]
    [Header("ステージの角の範囲(割合)")]
    [Range(0.05f, 1.0f)]
    public float cornerMargin;

    [SerializeField]
    [Header("角の設定")]
    public bool cornerSetting;

    /* --- SerializeFieldの変数 --- */

    [SerializeField]
    [Header("箸の行数")]
    public int chopStickRowNum;

    [SerializeField]
    [Header("箸の列数")]
    public int chopStickColumnNum;

    [SerializeField]
    [Header("角に箸が来る割合")]
    [Range(0, 1.0f)]
    public float cornerRate;

    [SerializeField]
    [Header("縦方向のばらつき")]
    [Range(0, 10f)]
    public float dispertion;

    [SerializeField]
    [Header("真ん中の箸の行数")]
    public int centerChopStickRow;

    [SerializeField]
    [Header("真ん中の列数")]
    public int centerChopStickColumn;

    [SerializeField]
    [Header("真ん中の縦方向のばらつき")]
    [Range(0, 10f)]
    public float centerDispertion;

    [SerializeField]
    [Header("角の箸の行数")]
    public int cornerChopStickRow;

    [SerializeField]
    [Header("角の箸の列数")]
    public int cornerChopStickColumn;

    [SerializeField]
    [Header("角の縦方向のばらつき")]
    [Range(0, 10f)]
    public float cornerDispertion;

    [HideInInspector]
    [Header("ステージ上の場所ごとに設定")]
    public bool isPartition;

    [HideInInspector]
    [Header("ステージ上の区分(isPartition = true)")]
    public stagePartition[] stagePartitions;

    [HideInInspector]
    public float tmpSpeed;

    [Header("")]
    [SerializeField] private GameObject stageObj;           //ステージ
    [SerializeField] private GameObject startStageObj;      //ゲーム開始時に生成するステージ
    [SerializeField] private GameObject goalStageObj;       //ゴールステージ
    [SerializeField] private Slider progress;
    [SerializeField] private GameObject goalEffect;

    [Header("")]
    [SerializeField] private GameObject playerAxis;

    /* --- 変数 ---*/

    /* --- private変数 --- */
    private List<GameObject> stages = new List<GameObject>();       //生成したステージのリスト
    private bool isGoal = false;


    // Start is called before the first frame update
    void Start()
    {
        progress.minValue = 0;
        progress.maxValue = goalPosCount - 2;
        progress.value = 0;

        goalEffect.GetComponent<ParticleSystem>().Stop();

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

        if (debugMode)
        {
            for (int i = 0; i < stages.Count; i++)
            {
                stages[i].transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < stages.Count; i++)
        {
            if (!isStoped) stages[i].transform.Translate(Vector3.back * tmpSpeed / 10.0f);
            else
            {
                goalEffect.GetComponent<ParticleSystem>().Play();
                playerAxis.GetComponent<NewPlayerMove>().movable = false;
            }

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

                if (debugMode)
                {
                    stages[stages.Count - 1].transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
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
