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
        if (needUpdate != (int)SkillEunm.SkillState.Init)
        {
            afterDuration();
        }
        playerAttribute.inSelfNight = true;
        passTime = 0;
        needUpdate = (int)SkillEunm.SkillState.Duration;
        FightScene.instance.audioController.SoundPlay(AudioEunm.nightBringer);
        List<GameObject> players = findEnemys();
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
        List<GameObject> players = findEnemys();
        for (int i = 0; i < players.Count; ++i)
        {
            PlayerAttribute attr = players[i].GetComponent<PlayerAttribute>();
            attr.inNight = false;
        }
    }
    public void BreakOtherSkill()
    {
        List<GameObject> players = findEnemys();
        for (int i = 0; i < players.Count; ++i)
        {
            NightBringer night = players[i].GetComponent<NightBringer>();
            if (night.needUpdate == (int)SkillEunm.SkillState.Duration)
            {
                night.needUpdate = (int)SkillEunm.SkillState.BreakDuration;
            }
        }
    }
    public override void onBreakDuration(float deltaTime)
    {
        FightScene.instance.myselfControl.action.AfterNight();
        playerAttribute.inSelfNight = false;
        needUpdate = (int)SkillEunm.SkillState.Init;
    }
}
