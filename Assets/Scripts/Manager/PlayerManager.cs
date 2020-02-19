using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// プレイヤーのステータスを管理するためのスクリプト。
/// </summary>
public class PlayerManager : MonoBehaviour
{
    /* public変数*/
    [Header("そうめんの本数")]
    public int hp;                      //そうめんの本数

    [Header("そうめんを撃てる距離")]
    public float shotRange;             //そうめんを撃てる距離

    [Header("そうめんを撃つクールタイム")]
    public float shotCoolTime;          //そうめんを撃つクールタイム

    [Header("速さ")]
    public float speed;                 //そうめんの速さ

    [Header("ダッシュ時の最大速度")]
    public float dashSpeed;             //ダッシュした時のそうめんの最大速度

    [Header("何秒ダッシュボタンを押すと最大速度になるか")]
    public float dashTime;           //何秒ダッシュボタンを押すと最大速度になるか

    [Header("ジャンプの高さ")]
    public float jumpHeight;            //ジャンプの高さ

    [Header("ジャンプの速さ")]
    public float jumpSpeed;             //ジャンプの速さ

    /* --- SerializeFieldの変数 --- */
    [Header("各レベルの本数")]
    public int[] hpLevel;     //大きさが変わる本数の段階

    [Header("各レベルの受けるダメージ")]
    [SerializeField] private int[] damageLevel;      //そうめん中心部に当たった時の減る本数

    [Header("各レベルの仲間をキャッチする本数")]
    [SerializeField] private int[] addLevel;

    [Header("")]
    [SerializeField] private Sprite hpHalfIcon;
    [SerializeField] private Sprite hpIcon;
    [SerializeField] private Slider progress;
    [SerializeField] private Image[] hpImages;
    [SerializeField] private GameObject[] soumens;

    [SerializeField] private GameObject player;     //プレイヤーのゲームオブジェクト

    /* --- 変数 ---*/

    /* --- private変数 --- */
    private int currentLevel = 0;   //現在のレベル
    private int[] soumenActives = {1, 3, 6, 10, 13, 17};

    private int hpHalfIconValue;
    private int hpIconValue;

    private AudioSource audioSource;

    private HookShot hookShot;
    private SoundManager soundManager;
    private SceneTransitionManager sceneTransitionManager;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);

        soundManager = GameObject.Find("SoundManager(Clone)").GetComponent<SoundManager>();
        sceneTransitionManager = GameObject.Find("SceneTransitionManager(Clone)").GetComponent<SceneTransitionManager>();

        audioSource = GetComponent<AudioSource>();

        hpHalfIconValue = hpLevel[hpLevel.Length - 1] / 20;
        hpIconValue = hpHalfIconValue * 2;

        changeLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 箸にぶつかった時にそうめんが減るスクリプト。箸オブジェクトのpositionを引数に呼び出されます。
    /// </summary>
    public void Damaged(Vector3 colPosition)
    {
        float damageRatio, collideDistance, collideSize;    //減る本数の割合

        BoxCollider boxCollider = player.GetComponent<BoxCollider>();

        collideSize = player.transform.lossyScale.x * boxCollider.size.x / 2;

        collideDistance = Mathf.Abs(colPosition.x - player.transform.position.x);
        if (collideDistance >= 0.5f) collideDistance = 0.45f;

        //x軸における中心からの距離の割合
        damageRatio = 1 - Mathf.Abs(collideDistance / collideSize);

        if (currentLevel != 0)
        {
            hp -= (int)(damageLevel[currentLevel] * damageRatio);
        }
        else
        {
            hp = 0;
        }

        audioSource.clip = soundManager.eatSE;
        audioSource.volume = soundManager.eatVolume;
        audioSource.Play();

        if (hp <= 0)
        {
            hp = 0;
            sceneTransitionManager.SceneTransition(true);
        }

        changeLevel();
    }

    /// <summary>
    /// そうめんが増える時のスクリプト。増える本数を引数に呼び出されます。
    /// </summary>
    public void AddSoumen()
    {
        hp += addLevel[currentLevel];

        audioSource.clip = soundManager.catchSE;
        audioSource.volume = soundManager.catchVolume;
        audioSource.Play();

        //AddSoumenを呼び出した時ではなく、伸ばしたそうめんが戻ってきたタイミングで呼び出すため、
        //ここでchangeLevel()は呼び出さない。
        //changeLevel();
    }

    public void changeLevel()
    {
        int i;

        for (i = 0; i < hpLevel.Length; i++){
            if (hp <= hpLevel[i]) break;
        }

        if (currentLevel < i)
        {
            audioSource.clip = soundManager.sizeUpSE;
            audioSource.volume = soundManager.sizeUpVolume;
            audioSource.Play();
        }
        if (currentLevel > i)
        {
            audioSource.clip = soundManager.sizeDownSE;
            audioSource.volume = soundManager.sizeDownVolume;
            audioSource.Play();
        }

        currentLevel = i;

        for (int j = 0; j < soumens.Length; j++)
        {
            if (j < soumenActives[currentLevel]) soumens[j].SetActive(true);
            else soumens[j].SetActive(false);
        }

        for (int j = 0; j < hpImages.Length; j++)
        {
            if (j < hp / hpIconValue)
            {
                hpImages[j].sprite = hpIcon;
                hpImages[j].enabled = true;
            }
            else
            {
                hpImages[j].enabled = false;
            }
        }
        if (hp % hpIconValue != 0 && hp / hpIconValue < 10)
        {
            hpImages[hp / hpIconValue].sprite = hpHalfIcon;
            hpImages[hp / hpIconValue].enabled = true;
        }
    }
}
