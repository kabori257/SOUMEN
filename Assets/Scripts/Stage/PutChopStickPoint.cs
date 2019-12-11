using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PutChopStickPoint : MonoBehaviour
{
    /* public変数*/
    public int chopStickRowNum;
    public int chopStickColumnNum;
    public GameObject[] chopStickPrefabs;

    /* --- SerializeFieldの変数 --- */
    [SerializeField] private GameObject chopStickAxis;

    /* --- 変数 ---*/

    /* --- private変数 --- */
    List<GameObject> chopStickAxises = new List<GameObject>();
    List<GameObject> chopStickPoints = new List<GameObject>();
    private float chopStickInterval;

    // Start is called before the first frame update
    void Start()
    {
        chopStickInterval = 100.0f / chopStickRowNum;

        for (int i = 0; i < chopStickColumnNum; i++)
        {
            for (int j = 0; j < chopStickRowNum; j++)
            {
                GameObject tempChopStickAxis = Instantiate(chopStickAxis,
                    this.transform.localPosition + new Vector3(0, 0, (50 - chopStickInterval / 2) - chopStickInterval * j),
                    Quaternion.identity, transform);

                chopStickAxises.Add(tempChopStickAxis);

                chopStickPoints.Add(tempChopStickAxis.transform.GetChild(0).gameObject);
            }
        }

        for (int i = 0; i < chopStickAxises.Count; i++)
        {
            Vector3 rotate = chopStickAxises[i].transform.eulerAngles;

            rotate.z = Random.Range(-90, 90);

            chopStickAxises[i].transform.eulerAngles = rotate;
        }

        for (int i = 0; i < chopStickPoints.Count; i++)
        {
            GameObject chopStick = Instantiate(chopStickPrefabs[Random.Range(0, chopStickPrefabs.Length)],
            chopStickPoints[i].transform.position, Quaternion.identity, transform);

            chopStick.GetComponent<ChopStickTargetDetect>().TargetDetect(chopStickPoints[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
