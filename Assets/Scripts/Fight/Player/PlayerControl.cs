﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Protobuf;

public class PlayerControl : MonoBehaviour
{
    public float speed = 10f;
    private PlayerAttribute playerAttribute;
    private AnimationControl animationControl;
    private PlayerSkill playerSkill;
    private PlayerAction playerAction;
    private PlayerChildren playerChildren;
    // 上一次反弹的光线
    private GameObject lastCollideLight;
    // 上一次反弹光线的时间
    private float lastCollideTime;
    // 间隔时间
    private float spaceTime = 0.2f;
    private string moveJoyStick = "MoveJoyStick";
    // Start is called before the first frame update
    void Start()
    {
        lastCollideTime = -spaceTime;
    }
    public void Init(PlayerDTO player)
    {
        attr.seat = player.Seat;
        if (RoomData.isMainRole(player.Seat))
        {
            EasyJoystick.On_JoystickMove += onMoveHandler;
            EasyJoystick.On_JoystickMoveStart += onMoveStartHandler;
            EasyJoystick.On_JoystickMoveEnd += onMoveEndHandler;
            FrameActions.instance.Init();
        }
        attr.Init();
        skill.Init();
        children.Init(attr.seat);
    }

    public void Ack()
    {
        SendSkillMsg((int)SkillEunm.SkillBtn.ack, 0, 0);
    }
    public void Skill()
    {
        SendSkillMsg((int)SkillEunm.SkillBtn.skill, 0, 0);
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
    void onMoveStartHandler(MovingJoystick move)
    {
        FrameActions.instance.needAdd = false;
    }
    void onMoveHandler(MovingJoystick move)
    {
        if (move.joystickName != moveJoyStick) return;
        SendMoveMsg(move.joystickAxis.x, move.joystickAxis.y, Time.deltaTime);
    }
    void onMoveEndHandler(MovingJoystick move)
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
            Collider[] hitColliders = Physics.OverlapSphere(transform.position + transform.forward * deltaTime * speed, FightScene.instance.wallDistance, LayerMask.GetMask(LayerEunm.WALL));
            if (hitColliders.Length == 0)
            {
                transform.Translate(Vector3.forward * deltaTime * speed, Space.Self);
                if (RoomData.isMainRole(attr.seat))
                {
                    action.Night();
                }
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
        action.UpdateState(deltaTime);
        skill.UpdateSkills(deltaTime);
        attr.UpdateState(deltaTime);
    }
    public void LightCollision(GameObject light, Vector3 direction)
    {
        if (lastCollideLight == light && FightScene.instance.gameTime - lastCollideTime < spaceTime)
        {
            return;
        }
        else
        {
            lastCollideLight = light;
            lastCollideTime = FightScene.instance.gameTime;
        }
        float val = Vector3.Dot(transform.forward, direction);
        // 点积结果为负=>正面
        if (val < 0)
        {
            skill.ChangeCd((int)SkillEunm.SkillBtn.skill, attr.damage_cd);
            if (RoomData.isMainRole(attr.seat))
            {
                FightScene.instance.audioController.SoundPlay(AudioEunm.damage_normal);
            }
            action.CheckRepulse(FightScene.instance.wallDistance * 1.5f, direction);
        }
        else
        {
            attr.ChangeHp(attr.damage_hit);
            if (RoomData.isMainRole(attr.seat))
            {
                FightScene.instance.audioController.SoundPlay(AudioEunm.damage_hit);
            }
            if (attr.isDied)
            {
                anim.Death();
                if (RoomData.isMainRole(attr.seat))
                {
                    FightScene.instance.audioController.SoundPlay(AudioEunm.death);
                }
                action.CheckDeath(FightScene.instance.wallDistance, direction);
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
    public PlayerAction action
    {
        get
        {
            if (playerAction == null)
            {
                playerAction = GetComponent<PlayerAction>();
            }
            return playerAction;
        }
    }
    public PlayerChildren children
    {
        get
        {
            if (playerChildren == null)
            {
                playerChildren = GetComponent<PlayerChildren>();
            }
            return playerChildren;
        }
    }
}
