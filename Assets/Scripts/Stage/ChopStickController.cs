using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChopStickController : MonoBehaviour
{   
    [SerializeField] private GameObject playerManagerObject;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject chopStick;

    [SerializeField] private float attackRange;
    [SerializeField] private int attackSpeed;

    public GameObject targetPoint;

    private bool isAttack;              //はしが攻撃ターゲットのポジションに届いたらtrue
    private Vector3 attackStartPos;
    private float attackRatio = 0;
    private BoxCollider collider;

    public PlayerManager playerManager;

    private AudioSource audioSource;
    private SoundManager soundManager;

    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
        {
            player = GameObject.Find("Player");
        }

        if (playerManagerObject == null)
        {
            playerManagerObject = GameObject.Find("PlayerStatusManager");
            playerManager = playerManagerObject.GetComponent<PlayerManager>();
        }

        collider = GetComponent<BoxCollider>();

        playerManager = playerManagerObject.GetComponent<PlayerManager>();
        chopStick.transform.LookAt(targetPoint.transform);

        audioSource = GetComponent<AudioSource>();
        soundManager = GameObject.Find("SoundManager(Clone)").GetComponent<SoundManager>();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < attackRange)
        {
            isAttack = true;
            attackStartPos = chopStick.transform.position;
        }

        if (isAttack)
        {
            attackRatio += Time.deltaTime * attackSpeed;
            chopStick.transform.position = Vector3.Lerp(attackStartPos, targetPoint.transform.position, attackRatio);

            if (attackRatio >= 1)
            {
                if (Random.Range(0, 7) == 0)
                {
                    audioSource.clip = soundManager.itadakimasuSE;
                    audioSource.volume = soundManager.itadakimasuVolume;
                    audioSource.Play();
                }

                attackRatio = 0;
                isAttack = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !other.gameObject.GetComponent<PlayerController>().isJump)
        {
            playerManager.Damaged(transform.position);
        }
    }
}
