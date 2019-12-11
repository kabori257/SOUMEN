using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SingletonManager : MonoBehaviour
{
    static bool isInstantiated = false;
    [SerializeField] private GameObject joyConManager;
    [SerializeField] private GameObject soundManager;
    [SerializeField] private GameObject sceneTransitionManager;

    private AudioSource audioSource;

    void Awake()
    {
        if (!isInstantiated)
        {
            isInstantiated = true;

            Instantiate(joyConManager, transform.position, Quaternion.identity);

            Instantiate(soundManager, transform.position, Quaternion.identity);

            Instantiate(sceneTransitionManager, transform.position, Quaternion.identity);
        }
    }

    // Start is called before the sfirst frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
