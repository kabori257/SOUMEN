using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class goalCollider : MonoBehaviour
{
    private float soundLength;

    private SceneTransitionManager sceneTransitionManager;
    private StageScroll stageScroll;

    private float time = 0;
    private bool isGoal = false;

    // Start is called before the first frame update
    void Start()
    {
        sceneTransitionManager = GameObject.Find("SceneTransitionManager(Clone)").GetComponent<SceneTransitionManager>();
        stageScroll = GameObject.Find("StageScrollManager").GetComponent<StageScroll>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isGoal)
        {
            time += Time.deltaTime;

            if(time > 3.0f) sceneTransitionManager.SceneTransition(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameCountDown.isGoal = true;
            isGoal = true;
            stageScroll.isStoped = true;
        }
    }
}
