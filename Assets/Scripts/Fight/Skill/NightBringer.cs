﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightBringer : SkillBase
{
    public NightBringer()
    {
        skillId = (int)SkillEunm.SkillID.nightBringer;
        skillName = "黑夜降临";
        cd = 5;
        durationTime = 5;
        skillScope = 8;
    }
    public override void Release()
    {
        if (isEndCd())
        {
            beforeSkill();
            onSkill();
        }
    }
    public override void onSkill()
    {
        playerAttribute.inSelfNight = true;
        passTime = 0;
        needUpdate = (int)SkillEunm.SkillState.Duration;
        List<GameObject> players = findEnemys();
        for (int i = 0; i < players.Count; ++i)
        {
            PlayerAttribute attr = players[i].GetComponent<PlayerAttribute>();
            attr.inNight = true;
        }
        PlayerAction action = GetComponent<PlayerAction>();
        action.BeforeNight(playerAttribute.seat, skillScope);
    }
    public override void afterDuration()
    {
        playerAttribute.inSelfNight = false;
        List<GameObject> players = findEnemys();
        for (int i = 0; i < players.Count; ++i)
        {
            PlayerAttribute attr = players[i].GetComponent<PlayerAttribute>();
            attr.inNight = false;
        }
        PlayerAction action = GetComponent<PlayerAction>();
        action.AfterNight(playerAttribute.seat);
    }
}