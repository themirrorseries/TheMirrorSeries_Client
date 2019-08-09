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
        animator.SetInteger(AnimaState.state, AnimaState.IDLE);
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
        MoveDTO move = new MoveDTO();
        move.Roomid = GameData.room.Roomid;
        move.Seat = GameData.seat;
        move.X = position.x;
        move.Y = position.y;
        this.WriteMessage((int)MsgTypes.TypeFight, (int)FightTypes.MoveCreq, move.ToByteArray());
        return;
        float angle = Mathf.Atan2(position.x, position.y) * Mathf.Rad2Deg;
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
