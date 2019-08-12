﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Protobuf;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    ETCJoystick joystick;
    public float speed = 20f;
    private Animator animator;
    private int state = AnimaState.IDLE;
    public int seat;
    private float distance = 2f;
    private float repulseDistance = 10f;
    private bool canMove = true;
    private float hp = 20;
    private float mp = 0;
    [SerializeField]
    private TextMesh hpText;
    [SerializeField]
    private TextMesh mpText;
    // Start is called before the first frame update
    void Start()
    {

        joystick = GameObject.Find("Joystick").GetComponent<ETCJoystick>();
        joystick.onMove.AddListener(onMoveHandler);
        joystick.onMoveEnd.AddListener(onMoveEndHandler);

        hpText.text = "hp:" + hp.ToString();
        mpText.text = "mp:" + mp.ToString();
        // animator = GetComponent<Animator>();
        // animator.SetInteger(AnimaState.state, state);
    }
    public void ChangeHp(float value)
    {
        if (hp - value > 0)
        {
            hp -= value;
        }
        else
        {
            hp = 0;
        }
        hpText.text = "hp:" + hp.ToString();
    }
    public void ChangeMp(float value)
    {
        // 暂时写这里
        if (mp + value <= 5)
        {
            mp += value;
        }
        else
        {
            mp = 5;
        }
        mpText.text = "mp:" + mp.ToString();
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
        if (canMove)
        {
            if (position.x != 0 || position.y != 0)
            {
                if (state != AnimaState.RUN)
                {
                    state = AnimaState.RUN;
                    // animator.SetInteger(AnimaState.state, AnimaState.RUN);
                }
                float angle = Mathf.Atan2(position.x, position.y) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
                // 球形射线检测
                Collider[] hitColliders = Physics.OverlapSphere(transform.position + transform.forward * speed * Time.deltaTime, distance);
                bool judge = false;
                for (int i = 0; i < hitColliders.Length; ++i)
                {
                    if (hitColliders[i].gameObject.tag == "Wall")
                    {
                        judge = true;
                        break;
                    }
                }
                if (!judge)
                {
                    transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
                }
            }
        }
        return;
        SendMoveMsg(position.x, position.y, Time.deltaTime);
        /* 
        // 纵向,横向射线检测
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, distance)
            || Physics.Raycast(transform.position, transform.right, out hit, distance)
            || Physics.Raycast(transform.position, -transform.right, out hit, distance)
        {
            if (hit.collider.tag != "Wall")
            {
                transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
            }
        }
        else
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
        }
        */
    }
    void onMoveEndHandler()
    {
        return;
        SendMoveMsg(0, 0, 0);
    }
    void SendMoveMsg(float x, float y, float deltaTime)
    {
        MoveDTO move = new MoveDTO();
        move.Roomid = GameData.room.Roomid;
        move.Seat = GameData.seat;
        move.X = x;
        move.Y = y;
        move.DeltaTime = deltaTime;
        this.WriteMessage((int)MsgTypes.TypeFight, (int)FightTypes.MoveCreq, move.ToByteArray());
    }
    public void onMoveMsgHandler(float x, float y, float deltaTime)
    {
        if (x != 0 || y != 0)
        {
            if (state != AnimaState.RUN)
            {
                state = AnimaState.RUN;
                // animator.SetInteger(AnimaState.state, AnimaState.RUN);
            }
            float angle = Mathf.Atan2(x, y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
            // 球形射线检测
            Collider[] hitColliders = Physics.OverlapSphere(transform.position + transform.forward * speed * deltaTime, distance);
            bool judge = false;
            for (int i = 0; i < hitColliders.Length; ++i)
            {
                if (hitColliders[i].gameObject.tag == "Wall")
                {
                    judge = true;
                    break;
                }
            }
            if (!judge)
            {
                transform.Translate(Vector3.forward * speed * deltaTime, Space.Self);
            }
        }
        else
        {
            if (state != AnimaState.IDLE)
            {
                state = AnimaState.IDLE;
                // animator.SetInteger(AnimaState.state, AnimaState.IDLE);
            }
        }

    }
    // Update is called once per frame
    void Update()
    {

    }
    public void LightCollision(Vector3 direction)
    {
        float val = Vector3.Dot(transform.forward, direction);
        // 点积结果为正=>正面
        if (val > 0)
        {
            ChangeHp(2);
            ChangeMp(1);
            // 更改人物状态为击退(一个不可移动的状态)
            canMove = false;
            // 播放击退动画
            // 射线相交计算
            RaycastHit hit;
            if (Physics.Raycast(transform.position, direction, out hit, distance + repulseDistance))
            {
                if (hit.collider.tag == "Wall")
                {
                    // ps:乘以1.5,否则会卡在无法移动的区域里面
                    float dir = Vector3.Distance(transform.position, hit.point) - distance * (float)1.5;
                    transform.Translate(direction.normalized * dir, Space.World);
                }
                else
                {
                    transform.Translate(direction.normalized * repulseDistance, Space.World);
                }
            }
            else
            {
                transform.Translate(direction.normalized * repulseDistance, Space.World);
            }
            canMove = true;
        }
        else
        {
            ChangeHp(7);
        }
    }
}
