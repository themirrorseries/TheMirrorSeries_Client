using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightBringer : SkillBase
{
    public NightBringer()
    {
        skillId = (int)SkillEunm.SkillID.nightBringer;
        skillName = "黑夜降临";
        cd = 45;
        durationTime = 5;
        skillScope = 8;
    }
    public override void onSkill()
    {
        if (!BreakOtherSkillDuration())
        {
            playerAttribute.inSelfNight = true;
            playerAttribute.inNight = false;
        }
        else
        {
            playerAttribute.inSelfNight = false;
            playerAttribute.inNight = true;
        }
        passTime = 0;
        needUpdate = (int)SkillEunm.SkillState.Duration;
        FightScene.instance.audioController.SoundPlay(AudioEunm.nightBringer);
        List<GameObject> players = findRemainEnemys();
        for (int i = 0; i < players.Count; ++i)
        {
            PlayerAttribute attr = players[i].GetComponent<PlayerAttribute>();
            attr.inNight = true;
        }
        FightScene.instance.myselfControl.action.BeforeNight(skillScope);
    }
    public override void afterDuration()
    {
        FightScene.instance.myselfControl.action.AfterNight();
        playerAttribute.inSelfNight = false;
        playerAttribute.inNight = false;
        List<GameObject> players = findRemainEnemys();
        for (int i = 0; i < players.Count; ++i)
        {
            PlayerAttribute attr = players[i].GetComponent<PlayerAttribute>();
            attr.inNight = false;
            attr.inSelfNight = false;
        }
    }
    public bool BreakOtherSkillDuration()
    {
        List<GameObject> players = findRemainEnemys();
        for (int i = 0; i < players.Count; ++i)
        {
            NightBringer night = players[i].GetComponent<NightBringer>();
            if (night != null)
            {
                if (night.needUpdate == (int)SkillEunm.SkillState.Duration)
                {
                    PlayerAttribute attr = players[i].GetComponent<PlayerAttribute>();
                    attr.inSelfNight = false;
                    attr.inNight = true;
                    if (RoomData.isMainRole(attr.seat))
                    {
                        FightScene.instance.myselfControl.action.AfterNight();
                        FightScene.instance.myselfControl.action.BeforeNight(skillScope);
                    }
                    night.needUpdate = (int)SkillEunm.SkillState.BreakDuration;
                    return true;
                }
            }
        }
        return false;
    }
    public override void onBreakDuration(float deltaTime)
    {
        needUpdate = (int)SkillEunm.SkillState.Init;
    }
}
