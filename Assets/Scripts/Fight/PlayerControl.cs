using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Protobuf;

public class PlayerControl : MonoBehaviour
{
    ETCJoystick joystick;
    private float speed = 0.1f;
    private Animator animator;
    private int state = AnimaState.IDLE;
    public int seat;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetInteger(AnimaState.state, state);
    }
    public void Init(int seatId)
    {
        seat = seatId;
        if (seat == GameData.seat)
        {
            joystick = GameObject.Find("Joystick").GetComponent<ETCJoystick>();
            joystick.onMove.AddListener(onMoveHandler);
            joystick.onMoveEnd.AddListener(onMoveEndHandler);
        }
    }
    void onMoveHandler(Vector2 position)
    {
        SendMoveMsg(position.x, position.y);
    }
    void onMoveEndHandler()
    {
        SendMoveMsg(0, 0);
    }
    void SendMoveMsg(float x, float y)
    {
        MoveDTO move = new MoveDTO();
        move.Roomid = GameData.room.Roomid;
        move.Seat = GameData.seat;
        move.X = x;
        move.Y = y;
        this.WriteMessage((int)MsgTypes.TypeFight, (int)FightTypes.MoveCreq, move.ToByteArray());
    }
    public void onMoveMsgHandler(float x, float y)
    {
        if (x != 0 && y != 0)
        {
            if (state != AnimaState.RUN)
            {
                state = AnimaState.RUN;
                animator.SetInteger(AnimaState.state, AnimaState.RUN);
            }
            float angle = Mathf.Atan2(x, y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
            transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
        }
        else
        {
            if (state != AnimaState.IDLE)
            {
                state = AnimaState.IDLE;
                animator.SetInteger(AnimaState.state, AnimaState.IDLE);
            }
        }

    }
    // Update is called once per frame
    void Update()
    {

    }
}
