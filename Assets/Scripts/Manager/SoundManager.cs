using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 再生したい音のフラグを他スクリプトからtrueにすると、再生します。
/// </summary>
public class SoundManager : MonoBehaviour
{
    /* public変数*/
    [Header("決定音")]
    public AudioClip decisionSE;                        //決定（ボタンを押した時）した時の音
    [Range(0, 1)] public float decisionVolume;

    [Header("キャンセル音")]
    public AudioClip cancelSE;                          //キャンセルの音
    [Range(0, 1)] public float cancelVolume;

    [Header("Title -> Tutorial")]
    public AudioClip hyoushigiSE;                       //拍子木の音
    [Range(0, 1)] public float hyoushigiVolume;

    [Header("Result -> Title")]
    public AudioClip shishiodoshiSE;                    //ししおどしの音
    [Range(0, 1)] public float shishiodoshiVolume;

    [Header("Tutorial -> GameScene")]
    public AudioClip taikoSE;                           //太鼓の音
    [Range(0, 1)] public float taikoVolume;

    [Header("GameScene -> Result")]
    public AudioClip taikoX2SE;                           //太鼓の音
    [Range(0, 1)] public float taikoX2Volume;

    [Header("ジャンプ")]
    public AudioClip jumpSE;                            //ジャンプ時の効果音
    [Range(0, 1)] public float jumpVolume;

    [Header("着水")]
    public AudioClip landSE;                            //着水した時の効果音
    [Range(0, 1)] public float landlVolume;

    [Header("仲間をキャッチ")]
    public AudioClip catchSE;                           //仲間のそうめんをキャッチした時の音
    [Range(0, 1)] public float catchVolume;

    [Header("そうめんを撃つ音")]
    public AudioClip shotSE;                            //そうめんを撃つ時の音
    [Range(0, 1)] public float shotVolume;

    [Header("いただきます")]
    public AudioClip itadakimasuSE;                     //いただきますの音
    [Range(0, 1)] public float itadakimasuVolume;

    [Header("麺をすする")]
    public AudioClip eatSE;                             //そうめんをすする音
    [Range(0, 1)] public float eatVolume;

    [Header("はしの音")]
    public AudioClip chopStickSE;                       //箸の音
    [Range(0, 1)] public float chopStickVolume;

    [Header("サイズアップ")]
    public AudioClip sizeUpSE;                          //そうめんがサイズアップする時の音
    [Range(0, 1)] public float sizeUpVolume;

    [Header("サイズダウン")]
    public AudioClip sizeDownSE;                        //そうめんがサイズダウンする時の音
    [Range(0, 1)] public float sizeDownVolume;

    [Header("スピードアップ")]
    public AudioClip speedUpSE;                         //スピードアップする時の音
    [Range(0, 1)] public float speedUpVolume;

    [Header("スローモーション")]
    public AudioClip slowMotionSE;                      //スローモーションの時の音
    [Range(0, 1)] public float slowMotionVolume;

    [Header("水の流れる音")]
    public AudioClip waterSE;                           //水の流れる音
    [Range(0, 1)] public float waterVolume;

    [Header("タイトルBGM")]
    public AudioClip titleBGM;                          //タイトルのBGM
    [Range(0, 1)] public float titleBGM_Volume;
    public bool titleLoop;                              //タイトルのBGMをループさせるか

    [Header("チュートリアルBGM")]
    public AudioClip tutorialBGM;                       //チュートリアルのBGM
    [Range(0, 1)] public float tutorialBGM_Volume;
    public bool tutorialLoop;                           //チュートリアルのBGMをループさせるか

    [Header("ゲームシーンのBGM")]
    public AudioClip gameSceneBGM;                      //ゲームシーンのBGM
    [Range(0, 1)] public float gameScene_Volume;
    public bool gameSceneLoop;                          //ゲームシーンのBGMをループさせるか

    [Header("リザルトシーンのBGM")]
    public AudioClip resultBGM;                         //リザルトシーンのBGM;
    [Range(0, 1)] public float resultBGM_Volume;
    public bool resultLoop;                             //リザルトシーンのBGMをループさせるか

    [HideInInspector] public AudioSource audioSource;
    [HideInInspector] public AudioClip[] bgmArray = new AudioClip[4];
    [HideInInspector] public float[] bgmVolume = new float[4];
    [HideInInspector] public AudioClip[] sceneTransitionSE_Array = new AudioClip[4];
    [HideInInspector] public float[] sceneTransitionSE_Volume = new float[4];
    [HideInInspector] public bool[] bgmLoops = new bool[4];

    /* --- SerializeFieldの変数 --- */

    /* --- 変数 ---*/

    /* --- private変数 --- */


    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        audioSource = GetComponent<AudioSource>();

        bgmArray[0] = titleBGM;
        bgmArray[1] = tutorialBGM;
        bgmArray[2] = gameSceneBGM;
        bgmArray[3] = resultBGM;

        bgmVolume[0] = titleBGM_Volume;
        bgmVolume[1] = tutorialBGM_Volume;
        bgmVolume[2] = gameScene_Volume;
        bgmVolume[3] = resultBGM_Volume;

        bgmLoops[0] = titleLoop;
        bgmLoops[1] = tutorialLoop;
        bgmLoops[2] = gameSceneLoop;
        bgmLoops[3] = resultLoop;

        sceneTransitionSE_Array[0] = shishiodoshiSE;
        sceneTransitionSE_Array[1] = hyoushigiSE;
        sceneTransitionSE_Array[2] = taikoSE;
        sceneTransitionSE_Array[3] = taikoX2SE;

        sceneTransitionSE_Volume[0] = shishiodoshiVolume;
        sceneTransitionSE_Volume[1] = hyoushigiVolume;
        sceneTransitionSE_Volume[2] = taikoVolume;
        sceneTransitionSE_Volume[3] = taikoX2Volume;

        audioSource.clip = bgmArray[SceneManager.GetActiveScene().buildIndex];
        audioSource.volume = bgmVolume[SceneManager.GetActiveScene().buildIndex];
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void playBGM(bool play)
    {
        if (play) audioSource.Play();
        else audioSource.Stop();
    }
}
