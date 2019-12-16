using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
public class GameCountDown : MonoBehaviour
{
    /* public変数*/
    public static bool isGoal = false;

    /* --- SerializeFieldの変数 --- */
    [SerializeField] private Vector2 imageSize;
    [SerializeField] private Sprite[] images;
    [SerializeField] private GameObject playerAxis;

    /* --- 変数 ---*/

    /* --- private変数 --- */
    private NewPlayerMove playerMove;

    private bool isStart = true;

    private int imageIndex = 0;

    private float soundLength;
    private float time = 0;

    private Image image;
    private RectTransform rectTransform;

    private StageScroll stageScroll;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        image.sprite = images[0];

        rectTransform = GetComponent<RectTransform>();
        soundLength = GameObject.Find("SoundManager(Clone)").GetComponent<SoundManager>().taikoSE.length;

        stageScroll = GameObject.Find("StageScrollManager").GetComponent<StageScroll>();

        stageScroll.tmpSpeed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (isStart || isGoal)
        {
            if (isGoal) image.color = new Color(1, 1, 1, 1);

            time += Time.deltaTime;            

            rectTransform.sizeDelta = imageSize - imageSize * time / soundLength;
            image.color = new Color(1, 1, 1, 1 - (time / soundLength) - 0.2f);

            if (time > soundLength)
            {
                if(imageIndex + 1 < images.Length) image.sprite = images[++imageIndex];

                time = 0;
                rectTransform.sizeDelta = imageSize;

                if (imageIndex == 3)
                {
                    stageScroll.tmpSpeed = stageScroll.speed;
                    playerAxis.GetComponent<NewPlayerMove>().movable = true;
                }

                if (imageIndex == 4) isStart = false;
                else image.color = new Color(1, 1, 1, 1);

                if (isGoal) isGoal = false;
            }
        }
    }
}
