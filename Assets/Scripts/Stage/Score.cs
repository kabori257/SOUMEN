using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Score : MonoBehaviour
{
    int soumenCount;
    public static RANK scoreRank { get; set; }

    [Header("スコア:良 (この値以下が\"不可\")")]
    public int ryou;

    [Header("スコア:優")]
    public int yuu;

    [Header("スコア:秀")]
    public int shuu;

    public Sprite[] spriteArray = new Sprite[4];
    public GameObject imageObj;
    public GameObject txt;

    private PlayerManager playerManager;

	//デバッグ用
	[SerializeField] private int score;

    struct Rank
    {
        public float ryou;
        public float yuu;
        public float syuu;
    }
    Rank rank = new Rank();

    public enum RANK
    {
        Huka, Ryou, Yuu, Syuu
    }

    // Start is called before the first frame update
    void Start()
    {
        playerManager = GameObject.Find("PlayerStatusManager").GetComponent<PlayerManager>();
        soumenCount = playerManager.hp;

        rank.ryou = ryou;
        rank.yuu = yuu;
        rank.syuu = shuu;

        scorejudge();
        Ranking();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Ranking()
    {
        //結果を表示
        imageObj.GetComponent<Image>().sprite = spriteArray[(int)scoreRank];
        txt.GetComponent<TextMeshProUGUI>().text = soumenCount.ToString();
    }

    void scorejudge()
    {
        //評価によって結果を判定する
        if (soumenCount <= ryou)
        {
            scoreRank = RANK.Huka;
        }
        else if (soumenCount <= rank.yuu)
        {
            scoreRank = RANK.Ryou;
        }
        else if (soumenCount <= rank.syuu)
        {
            scoreRank = RANK.Yuu;
        }
        else
        {
            scoreRank = RANK.Syuu;
        }
    }
}
