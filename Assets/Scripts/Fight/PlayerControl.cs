using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    ETCJoystick joystick;
    private float speed = 10f;
    private Animator animator;
    private int state = AnimaState.IDLE;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetInteger(AnimaState.state, AnimaState.IDLE);
        joystick = GameObject.Find("Joystick").GetComponent<ETCJoystick>();
        joystick.onMove.AddListener(onMoveHandler);
        joystick.onMoveEnd.AddListener(onMoveEndHandler);
    }
    void onMoveHandler(Vector2 arg)
    {
        float angle = Mathf.Atan2(arg.x, arg.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
        transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
        if (state != AnimaState.RUN)
        {
            state = AnimaState.RUN;
            animator.SetInteger(AnimaState.state, AnimaState.RUN);
        }
    }
    void onMoveEndHandler()
    {
        if (state != AnimaState.IDLE)
        {
            state = AnimaState.IDLE;
            animator.SetInteger(AnimaState.state, AnimaState.IDLE);
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
