using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControl : MonoBehaviour
{
    private Animator animator;
    private int state = AnimaState.IDLE;
    void Awake()
    {
        // animator = GetComponent<Animator>();
        // animator.SetInteger(AnimaState.state, state);
    }
    // Start is called before the first frame update
    void Start()
    {

    }
    public void Run()
    {
        if (state != AnimaState.RUN)
        {
            state = AnimaState.RUN;
            // animator.SetInteger(AnimaState.state, AnimaState.RUN);
        }
    }

    public void Idle()
    {
        if (state != AnimaState.IDLE)
        {
            state = AnimaState.IDLE;
            // animator.SetInteger(AnimaState.state, AnimaState.IDLE);
        }
    }

    public void Attack()
    {
        if (state != AnimaState.ATTACK)
        {
            state = AnimaState.ATTACK;
            // animator.SetInteger(AnimaState.state, AnimaState.ATTACK);
        }
    }

    public void Repulse()
    {
        if (state != AnimaState.REPULSE)
        {
            state = AnimaState.REPULSE;
            // animator.SetInteger(AnimaState.state, AnimaState.REPULSE);
        }
    }
}
