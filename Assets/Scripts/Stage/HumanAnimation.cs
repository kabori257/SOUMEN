using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanAnimation : MonoBehaviour
{
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0) {
            animator.SetFloat("speed", 1);
            animator.Play(Animator.StringToHash("Take 001"), 0, 0.0f);
        }

        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1){
            animator.SetFloat("speed", -1);
            animator.Play(Animator.StringToHash("Take 001"), 0, 1.0f);
        }

    }
}
