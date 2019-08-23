﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiveThunder : SkillBase
{
    public FiveThunder()
    {
        skillId = (int)SkillEunm.SkillID.fiveThunder;
        skillName = "五雷轰顶";
        cd = 45;
    }
    public override void Release()
    {
        beforeSkill();
        onSkill();
    }
    public override void onSkill()
    {
        List<GameObject> players = findEnemys();
        for (int i = 0; i < players.Count; ++i)
        {
            PlayerEffect effect = players[i].GetComponent<PlayerEffect>();
            effect.Play(EffectEunm.THUNDER);
            PlayerAttribute attr = players[i].GetComponent<PlayerAttribute>();
            // 减少当前生命值的50%
            attr.ChangeHp(-Mathf.Floor(attr.hp / 2));
            if (attr.isDied)
            {
                AnimationControl anim = players[i].GetComponent<AnimationControl>();
                anim.Death();
                FightScene.instance.AddDeath(attr.seat, attr.bounces);
                return;
            }
        }
    }
}
