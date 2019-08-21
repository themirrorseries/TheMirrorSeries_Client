﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalAttack : SkillBase
{
    public NormalAttack()
    {
        skillId = (int)SkillEunm.SkillID.normalAck;
        skillName = "普通攻击";
        cd = 0;
    }
    public override void Release()
    {
        if (isMpEnough(playerAttribute.mpMax))
        {
            AddMp(-playerAttribute.mp);
            onSkill();
        }
    }
    public override void onSkill()
    {
        AnimationControl anim = GetComponent<AnimationControl>();
        anim.Attack();
        GameObject light = Instantiate(ResourcesTools.getLight(1));
        // 固定光线初始位置y方向
        light.transform.position = new Vector3(transform.position.x, light.transform.position.y, transform.position.z)
                                    + transform.forward.normalized * 2;
        LightManager lightMgr = light.GetComponent<LightManager>();
        lightMgr.Init(RoomData.room.Speed, RoomData.room.Count, transform.forward.x, transform.forward.z);
        FightScene.instance.Lights.Add(light);
    }
}
