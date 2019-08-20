﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Protobuf;

public class PlayerControl : MonoBehaviour
{
    ETCJoystick joystick;
    public float speed = 10f;
    // 墙壁距离
    private float wallDistance = 2f;
    private PlayerAttribute playerAttribute;
    private AnimationControl animationControl;
    private PlayerSkill playerSkill;
    private RepulseAction repulseAction;
    // Start is called before the first frame update
    void Start()
    {
    }
    public void Init(int seatId)
    {
        attr.seat = seatId;
        if (seatId == RoomData.seat)
        {
            joystick = GameObject.Find("Joystick").GetComponent<ETCJoystick>();
            joystick.onMoveStart.AddListener(onMoveStartHandler);
            joystick.onMove.AddListener(onMoveHandler);
            joystick.onMoveEnd.AddListener(onMoveEndHandler);
            FrameActions.instance.Init();
        }
        attr.Init();
        skill.Init();
    }

    public void Ack()
    {
        SendSkillMsg((int)SkillEunm.SkillBtn.ack, 0, 0);
    }
    public void Skill1()
    {
        SendSkillMsg((int)SkillEunm.SkillBtn.skill1, 0, 0);
    }

    public void Skill2()
    {
        SendSkillMsg((int)SkillEunm.SkillBtn.skill2, 0, 0);
    }

    public void Skill3()
    {
        SendSkillMsg((int)SkillEunm.SkillBtn.skill3, 0, 0);
    }
    void SendSkillMsg(int skillNum, float x, float y)
    {
        if (attr.isDied)
        {
            return;
        }
        if (!FrameActions.instance.isLock)
        {
            FrameInfo skillInfo = new FrameInfo();
            skillInfo.Skillid = skillNum;
            skillInfo.DeltaTime = Time.deltaTime;
            FrameActions.instance.Add(skillInfo);
        }
    }
    void onMoveStartHandler()
    {
        FrameActions.instance.needAdd = false;
    }
    void onMoveHandler(Vector2 position)
    {
        SendMoveMsg(position.x, position.y, Time.deltaTime);
    }
    void onMoveEndHandler()
    {
        SendMoveMsg(0, 0, 0);
        FrameActions.instance.needAdd = true;
    }
    void SendMoveMsg(float x, float y, float deltaTime)
    {
        if (attr.isDied)
        {
            return;
        }
        if (!FrameActions.instance.isLock)
        {
            DeltaDirection direction = new DeltaDirection();
            direction.X = x;
            direction.Y = y;
            FrameInfo move = new FrameInfo();
            move.Skillid = (int)SkillEunm.SkillID.notSkill;
            move.Move = direction;
            move.DeltaTime = Time.deltaTime;
            FrameActions.instance.Add(move);
        }
    }
    public void onMsgHandler(FrameInfo frameInfo, float deltaTime)
    {
        // 空帧
        if (frameInfo.Skillid == (int)SkillEunm.SkillID.empty)
        {
            return;
        }
        // 判断是技能还是移动
        else if (frameInfo.Skillid == (int)SkillEunm.SkillID.notSkill)
        {
            Move(frameInfo.Move, deltaTime);
        }
        else
        {
            Skill(frameInfo.Skillid);
        }
    }
    private void Move(DeltaDirection direction, float deltaTime)
    {
        if (direction.X != 0 || direction.Y != 0)
        {
            anim.Run();
            float angle = Mathf.Atan2(direction.X, direction.Y) * Mathf.Rad2Deg;
            // 混乱状态下,操作与移动方向相反
            if (attr.isChaos)
            {
                angle += 180;
            }
            transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
            if (!attr.canMove) return;
            // 球形射线检测
            Collider[] hitColliders = Physics.OverlapSphere(transform.position + transform.forward * deltaTime * speed, wallDistance, LayerMask.GetMask(LayerEunm.WALL));
            if (hitColliders.Length == 0)
            {
                transform.Translate(Vector3.forward * deltaTime * speed, Space.Self);
            }
        }
        else
        {
            anim.Idle();
        }
    }
    private void Skill(int skillNum)
    {
        skill.Release(skillNum);
    }
    public void UpdateState(float deltaTime)
    {
        repulse.Repulse(deltaTime);
        skill.UpdateSkills(deltaTime);
    }
    public void LightCollision(Vector3 direction)
    {
        float val = Vector3.Dot(transform.forward, direction);
        // 点积结果为负=>正面
        if (val < 0)
        {
            attr.ChangeHp(-2);
            if (attr.isDied)
            {
                FightScene.instance.AddDeath(attr.seat, attr.bounces);
                return;
            }
            attr.ChangeMp(5);
            anim.Repulse();
            repulse.Check(wallDistance, direction);
        }
        else
        {
            attr.ChangeHp(-7);
            if (attr.isDied)
            {
                FightScene.instance.AddDeath(attr.seat, attr.bounces);
            }
        }
    }
    public PlayerAttribute attr
    {
        get
        {
            if (playerAttribute == null)
            {
                playerAttribute = GetComponent<PlayerAttribute>();
            }
            return playerAttribute;
        }
    }
    public AnimationControl anim
    {
        get
        {
            if (animationControl == null)
            {
                animationControl = GetComponent<AnimationControl>();
            }
            return animationControl;
        }
    }
    public PlayerSkill skill
    {
        get
        {
            if (playerSkill == null)
            {
                playerSkill = GetComponent<PlayerSkill>();
            }
            return playerSkill;
        }
    }
    public RepulseAction repulse
    {
        get
        {
            if (repulseAction == null)
            {
                repulseAction = GetComponent<RepulseAction>();
            }
            return repulseAction;
        }
    }
}
