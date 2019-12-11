using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Score : MonoBehaviour
{
    int soumenCount;

    public static RANK scoreRank { get; set; }

    public Sprite[] spriteArray = new Sprite[4];
    public GameObject imageObj;
    public GameObject txt;

    private PlayerManager playerManager;

	private int maxSoumenCount;

	//デバッグ用
	public int score;

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
        //初期値
        //soumenCount = PlayerManager.HP;
        playerManager = GameObject.Find("PlayerStatusManager").GetComponent<PlayerManager>();

        soumenCount = playerManager.hp;
        maxSoumenCount = playerManager.hpLevel[playerManager.hpLevel.Length - 1];

        rank.ryou = maxSoumenCount / 3.0f;
        rank.yuu = (maxSoumenCount / 3.0f) * 2;
        rank.syuu = maxSoumenCount;

        scorejudge();
        Ranking();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Ranking()
    {
        //switch (scoreRank)
        //{
        //    case RANK.Huka:break;
        //    case RANK.Ryou:break;
        //    case RANK.Yuu:break;
        //    case RANK.Syuu:break;
        //    default:break;
        //}

        //結果を表示
        imageObj.GetComponent<Image>().sprite = spriteArray[(int)scoreRank];
        txt.GetComponent<TextMeshProUGUI>().text = "" + soumenCount;
    }

    void scorejudge()
    {
        //評価によって結果を判定する
        if (soumenCount <= 0)
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
