using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// タイトルシーンから操作方法シーンへの遷移
/// </summary>
public class SceneTransitionManager : MonoBehaviour
{
    /* public変数*/

    /* --- SerializeFieldの変数 --- */

    /* --- 変数 ---*/

    /* --- private変数 --- */
    private SoundManager soundManager;
    private MyJoyCon myJoyCon;
    private AudioSource audioSource;
    private AudioSource SM_AudioSource;

    private bool isChanged = true;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);

        soundManager = GameObject.Find("SoundManager(Clone)").GetComponent<SoundManager>();
        myJoyCon = GameObject.Find("JoyConManager(Clone)").GetComponent<MyJoyCon>();

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = soundManager.decisionSE;

        SM_AudioSource = soundManager.audioSource;
    }

    // Update is called once per frame
    void Update()
    {

        //遷移する際に押してもらうボタン。
        if ((Input.GetKeyDown(KeyCode.Space) || myJoyCon.GetAnyButtonDown())
            && SceneManager.GetActiveScene().buildIndex != 2 && isChanged)
        {
            SceneTransition(false);
        }
    }

    public void SceneTransition(bool isDead)
    {
        isChanged = false;

        StartCoroutine(SceneTransitionCoroutine((SceneManager.GetActiveScene().buildIndex + 1) % 4, isDead));
    }

    //効果音の長さを引数に、その分だけ待機する。
    private IEnumerator SceneTransitionCoroutine(int sceneIndex, bool isDead)
    {
        bool isBGM_Changed = false;

        if (SM_AudioSource.clip.name != soundManager.bgmArray[sceneIndex].name)
        {
            if (soundManager.bgmLoops[sceneIndex]) SM_AudioSource.loop = true;
            else SM_AudioSource.loop = false;

            SM_AudioSource.clip = soundManager.bgmArray[sceneIndex];
            SM_AudioSource.volume = soundManager.bgmVolume[sceneIndex];

            isBGM_Changed = true;
        }

        if (sceneIndex == 0)
            Destroy(GameObject.Find("PlayerStatusManager"));

        if (sceneIndex != 3 || isDead)
            SceneManager.LoadScene(sceneIndex);

        if (isBGM_Changed)  soundManager.playBGM(false);

        audioSource.clip = soundManager.sceneTransitionSE_Array[sceneIndex];
        audioSource.volume = soundManager.sceneTransitionSE_Volume[sceneIndex];
        audioSource.Play();

        yield return new WaitForSeconds(audioSource.clip.length);

        if (sceneIndex == 2)
        {
            audioSource.Play();
            yield return new WaitForSeconds(audioSource.clip.length);
            audioSource.Play();
            yield return new WaitForSeconds(audioSource.clip.length);
        }

        if (isBGM_Changed) SM_AudioSource.Play();

        isChanged = true;
    }
}
