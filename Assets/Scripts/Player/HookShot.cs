using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Obi;

/// <summary>
/// そうめんを撃つスクリプト。
/// </summary>
public class HookShot : MonoBehaviour
{

    /* public変数*/
    public bool trigger = false;    //そうめんを伸ばすフラグ
    public bool isCollide = false;
    public float targetDist;        //ターゲットとの最初の距離
    public GameObject target;       //そうめんを伸ばすターゲット

    /* --- SerializeFieldの変数 --- */
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerStatusManager;

    /* --- 変数 ---*/

    /* --- private変数 --- */
    private PlayerManager playerManager;

    // Start is called before the first frame update
    void Start()
    {
        playerManager = playerStatusManager.GetComponent<PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        SoumenStretch();
    }

    /// <summary>
    /// そうめんを伸縮させる関数
    /// </summary>
    void SoumenStretch()
    {
        if (trigger)
        {
            if (!isCollide)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, targetDist / 10);
                if (Vector3.Distance(transform.position, target.transform.position) < 0.1f) isCollide = true;
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, targetDist / 10);
                if (Vector3.Distance(transform.position, player.transform.position) < 0.1f)
                {
                    transform.localPosition = new Vector3(0, -3, 0);
                    AttackSoumenHandleEnable(false);
                    isCollide = false;
                    trigger = false;
                    playerManager.changeLevel();
                }
            }
        }
    }

    public void AttackSoumenHandleEnable(bool flag)
    {
        GetComponent<ObiParticleHandle>().enabled = flag;
    }
}
