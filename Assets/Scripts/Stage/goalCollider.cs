using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class goalCollider : MonoBehaviour
{
    private float soundLength;

    private SceneTransitionManager sceneTransitionManager;

    // Start is called before the first frame update
    void Start()
    {
        sceneTransitionManager = GameObject.Find("SceneTransitionManager(Clone)").GetComponent<SceneTransitionManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameCountDown.isGoal = true;
            sceneTransitionManager.SceneTransition(true);
        }
    }
}
