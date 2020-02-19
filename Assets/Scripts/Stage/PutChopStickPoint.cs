using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PutChopStickPoint : MonoBehaviour
{
    /* public変数*/
    public GameObject[] chopStickPrefabs;

    /* --- SerializeFieldの変数 --- */
    [SerializeField] private GameObject chopStickAxis;

    /* --- 変数 ---*/

    /* --- private変数 --- */
    private StageScroll stageScroll;
    List<GameObject> chopStickAxises = new List<GameObject>();
    List<GameObject> chopStickPoints = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        stageScroll = GameObject.Find("StageScrollManager").GetComponent<StageScroll>();

        if (stageScroll.cornerSetting)
        {
            PutChopStick(-90 + (180 * stageScroll.cornerMargin / 2) + 7, 90 - (180 * stageScroll.cornerMargin / 2) - 7,
                stageScroll.centerChopStickRow, stageScroll.centerChopStickColumn, stageScroll.centerDispertion, false);
            PutChopStick(-90, -90 + (180 * stageScroll.cornerMargin / 2), stageScroll.cornerChopStickRow, stageScroll.cornerChopStickColumn, stageScroll.cornerDispertion, true);
            PutChopStick(90 - (180 * stageScroll.cornerMargin / 2), 90f, stageScroll.cornerChopStickRow, stageScroll.cornerChopStickColumn, stageScroll.cornerDispertion, true);
        }
        else
        {
            PutChopStick(-90, 90, stageScroll.chopStickRowNum, stageScroll.chopStickColumnNum, stageScroll.cornerMargin, stageScroll.cornerRate, stageScroll.dispertion);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void PutChopStick(float minAngle, float maxAngle, int chopStickRowNum, int chopStickColumnNum, float dispertion, bool isCorner)
    {
        float chopStickInterval = 100.0f / chopStickRowNum;

        for (int i = 0; i < chopStickColumnNum; i++)
        {
            for (int j = 0; j < chopStickRowNum; j++)
            {
                Vector3 tempPos = this.transform.position + new Vector3(0, 0, (50 - chopStickInterval / 2) - chopStickInterval * j + Random.Range(-dispertion, dispertion));

                if (tempPos.z + 35 > stageScroll.startAreaRange) {

                    GameObject tempChopStickAxis;

                    tempChopStickAxis = Instantiate(chopStickAxis,
                        tempPos,
                        Quaternion.identity, transform);

                    chopStickAxises.Add(tempChopStickAxis);

                    chopStickPoints.Add(tempChopStickAxis.transform.GetChild(0).gameObject);
                }
            }
        }

        for (int i = 0; i < chopStickAxises.Count; i++)
        {
            Vector3 rotate = chopStickAxises[i].transform.eulerAngles;

            rotate.z = Random.Range(minAngle, maxAngle);

            chopStickAxises[i].transform.eulerAngles = rotate;
        }

        for (int i = 0; i < chopStickPoints.Count; i++)
        {
            GameObject chopStick = Instantiate(chopStickPrefabs[Random.Range(0, chopStickPrefabs.Length)],
            chopStickPoints[i].transform.position, Quaternion.identity, transform);

            if (stageScroll.debugMode)
            {
                if (isCorner)
                    chopStick.GetComponent<Renderer>().material.color = Color.blue;
                else
                    chopStick.GetComponent<Renderer>().material.color = Color.red;

                chopStick.GetComponent<MeshRenderer>().enabled = true;
            }
            else
            {

                chopStick.GetComponent<MeshRenderer>().enabled = false;
            }

            chopStick.GetComponent<ChopStickTargetDetect>().TargetDetect(chopStickPoints[i]);
        }

        chopStickAxises.Clear();
        chopStickPoints.Clear();
    }

    void PutChopStick(float minAngle, float maxAngle, int chopStickRowNum, int chopStickColumnNum, float cornerMargin, float cornerRate, float dispertion)
    {
        float chopStickInterval = 100.0f / chopStickRowNum;

        for (int i = 0; i < chopStickColumnNum; i++)
        {
            for (int j = 0; j < chopStickRowNum; j++)
            {
                Vector3 tempPos = this.transform.position + new Vector3(0, 0, (50 - chopStickInterval / 2) - chopStickInterval * j + Random.Range(-dispertion, dispertion));

                if (tempPos.z + 35 > stageScroll.startAreaRange)
                {

                    GameObject tempChopStickAxis = Instantiate(chopStickAxis,
                    this.transform.localPosition + new Vector3(0, 0, (50 - chopStickInterval / 2) - chopStickInterval * j + Random.Range(-dispertion, dispertion)),
                    Quaternion.identity, transform);

                    chopStickAxises.Add(tempChopStickAxis);

                    chopStickPoints.Add(tempChopStickAxis.transform.GetChild(0).gameObject);
                }
            }
        }

        for (int i = 0; i < chopStickAxises.Count; i++)
        {
            Vector3 rotate = chopStickAxises[i].transform.eulerAngles;
            float rate = Random.value * 100;

            if (i < chopStickAxises.Count * cornerRate / 2.0)
            {
                rotate.z = Random.Range(minAngle, minAngle + (180 * cornerMargin / 2));
            }
            else if (i < chopStickAxises.Count * cornerRate)
            {
                rotate.z = Random.Range(maxAngle - (180 * cornerMargin / 2), maxAngle);
            }
            else
            {
                rotate.z = Random.Range(minAngle + (180 * cornerMargin / 2) + 7, maxAngle - (180 * cornerMargin / 2) - 7);
            }

            /*
            if (rate <= cornerRate * 100)
            {
                if (rate < cornerRate * 50)
                {
                    rotate.z = Random.Range(minAngle, minAngle + (180 * cornerMargin / 2));
                }
                else
                {
                    rotate.z = Random.Range(maxAngle - (180 * cornerMargin / 2), maxAngle);
                }
            }
            else
            {
                rotate.z = Random.Range(minAngle + (180 * cornerMargin / 2) + 7, maxAngle - (180 * cornerMargin / 2) - 7);
            }
            */

            chopStickAxises[i].transform.eulerAngles = rotate;
        }

        for (int i = 0; i < chopStickPoints.Count; i++)
        {
            GameObject chopStick = Instantiate(chopStickPrefabs[Random.Range(0, chopStickPrefabs.Length)],
            chopStickPoints[i].transform.position, Quaternion.identity, transform);

            if (stageScroll.debugMode)
            {
                chopStick.GetComponent<MeshRenderer>().enabled = true;
            }
            else
            {
                chopStick.GetComponent<MeshRenderer>().enabled = false;
            }

            chopStick.GetComponent<ChopStickTargetDetect>().TargetDetect(chopStickPoints[i]);
        }
    }
}
