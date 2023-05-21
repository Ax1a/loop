using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeAnimationStart : MonoBehaviour
{
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        var state = animator.GetCurrentAnimatorStateInfo(0);
        animator.Play(state.fullPathHash, 0, Random.Range(0f, 1f));
    }

}
