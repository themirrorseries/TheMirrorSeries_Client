using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControl : MonoBehaviour
{
    private Animator animator;
    private int state = AnimaState.IDLE;
    void Awake()
    {
        animator = GetComponent<Animator>();
        animator.SetInteger(AnimaState.state, state);
    }
    // Start is called before the first frame update
    void Start()
    {

    }
    public void Run()
    {
        if (state != AnimaState.RUN && state != AnimaState.DEATH && state != AnimaState.ATTACK)
        {
            state = AnimaState.RUN;
            animator.SetInteger(AnimaState.state, AnimaState.RUN);
        }
    }

    public void IdleAfterRun()
    {
        if (state != AnimaState.DEATH)
        {
            state = AnimaState.IDLE;
            animator.SetInteger(AnimaState.state, AnimaState.IDLE);
        }
    }

    public void Idle()
    {
        if (state != AnimaState.IDLE && state != AnimaState.DEATH && state != AnimaState.ATTACK)
        {
            state = AnimaState.IDLE;
            animator.SetInteger(AnimaState.state, AnimaState.IDLE);
        }
    }

    public void Attack()
    {
        if (state != AnimaState.ATTACK && state != AnimaState.DEATH)
        {
            state = AnimaState.ATTACK;
            animator.SetInteger(AnimaState.state, AnimaState.ATTACK);
        }
    }

    public void Death()
    {
        if (state != AnimaState.DEATH)
        {
            state = AnimaState.DEATH;
            animator.SetInteger(AnimaState.state, AnimaState.DEATH);
        }
    }
}
