using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupChaos : SkillBase
{
    public GroupChaos()
    {
        skillId = (int)SkillEunm.SkillID.groupChaos;
        skillName = "群体混乱";
        cd = 45;
        durationTime = 5;
        delayTime = 3;
    }
    public override void onSkill()
    {
        BreakOtherSkill();
        FightScene.instance.audioController.SoundPlay(AudioEunm.groupChaosDelay);
        passTime = 0;
        needUpdate = (int)SkillEunm.SkillState.Release;
        List<GameObject> players = findRemainEnemys();
        for (int i = 0; i < players.Count; ++i)
        {
            PlayerChildren playerChildren = players[i].GetComponent<PlayerChildren>();
            playerChildren.title.progressActive(true);
        }
    }
    public override void onDelay(float deltaTime)
    {
        List<GameObject> players = findRemainEnemys();
        for (int i = 0; i < players.Count; ++i)
        {
            PlayerChildren playerChildren = players[i].GetComponent<PlayerChildren>();
            playerChildren.title.progressView(passTime / delayTime);
        }
    }
    public override void afterDelay()
    {
        List<GameObject> players = findRemainEnemys();
        for (int i = 0; i < players.Count; ++i)
        {
            PlayerChildren playerChildren = players[i].GetComponent<PlayerChildren>();
            playerChildren.title.progressActive(false);
            PlayerAttribute attr = players[i].GetComponent<PlayerAttribute>();
            if (RoomData.isMainRole(attr.seat))
            {
                FightScene.instance.audioController.SoundPlay(AudioEunm.groupChaosDuration);
            }
            attr.isChaos = true;
            PlayerEffect effect = players[i].GetComponent<PlayerEffect>();
            if (!effect.isPlay(EffectEunm.CHAOS))
            {
                effect.Play(EffectEunm.CHAOS);
            }
        }
    }
    public void BreakOtherSkill()
    {
        List<GameObject> players = findRemainEnemys();
        for (int i = 0; i < players.Count; ++i)
        {
            GroupChaos chaos = players[i].GetComponent<GroupChaos>();
            if (chaos != null)
            {
                if (chaos.needUpdate == (int)SkillEunm.SkillState.Release)
                {
                    chaos.needUpdate = (int)SkillEunm.SkillState.BreakRelease;
                }
                else if (chaos.needUpdate == (int)SkillEunm.SkillState.Duration)
                {
                    chaos.needUpdate = (int)SkillEunm.SkillState.BreakDuration;
                }
            }

        }
    }
    public override void afterDuration()
    {
        List<GameObject> players = findRemainEnemys();
        for (int i = 0; i < players.Count; ++i)
        {
            PlayerAttribute attr = players[i].GetComponent<PlayerAttribute>();
            attr.isChaos = false;
            PlayerEffect effect = players[i].GetComponent<PlayerEffect>();
            if (effect.isPlay(EffectEunm.CHAOS))
            {
                effect.Stop(EffectEunm.CHAOS);
            }
        }
    }
    public override void onBreakDuration(float deltaTime)
    {
        if (passTime + deltaTime < delayTime)
        {
            passTime += deltaTime;
            onDelay(deltaTime);
        }
        else
        {
            FightScene.instance.audioController.SoundPlay(AudioEunm.groupChaosDelay);
            List<GameObject> players = findRemainEnemys();
            for (int i = 0; i < players.Count; ++i)
            {
                PlayerChildren playerChildren = players[i].GetComponent<PlayerChildren>();
                playerChildren.title.progressActive(false);
            }
            needUpdate = (int)SkillEunm.SkillState.Init;
            passTime = 0;
        }
    }
}
