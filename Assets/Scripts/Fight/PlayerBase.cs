using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    protected Animator animator;
    // Start is called before the first frame update
    public void Start()
    {
        animator = GetComponent<Animator>();
        Idle();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Run()
    {
        animator.SetInteger(AnimaState.state, AnimaState.RUN);
        Debug.Log("Run");
    }
    public void Idle()
    {
        animator.SetInteger(AnimaState.state, AnimaState.IDLE);
        Debug.Log("Idle");
    }
}
