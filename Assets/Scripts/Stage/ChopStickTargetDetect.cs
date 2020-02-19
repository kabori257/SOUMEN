using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChopStickTargetDetect : MonoBehaviour
{
    [SerializeField] private GameObject chopStickEdge;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TargetDetect(GameObject target)
    {
        chopStickEdge.GetComponent<ChopStickController>().targetPoint = target;
    }
}
