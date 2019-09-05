using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupChaos : SkillBase
{
    private bool isBroken = false;
    public int brokenSeat = -1;
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
        BreakOtherSkillRelease();
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
    public bool BreakOtherSkillRelease()
    {
        List<GameObject> players = findRemainEnemys();
        for (int i = 0; i < players.Count; ++i)
        {
            GroupChaos chaos = players[i].GetComponent<GroupChaos>();
            if (chaos != null)
            {
                if (chaos.needUpdate == (int)SkillEunm.SkillState.Release)
                {
                    chaos.brokenSeat = playerAttribute.seat;
                    chaos.needUpdate = (int)SkillEunm.SkillState.BreakRelease;
                    return true;
                }
            }
        }
        return false;
    }
    public bool BreakOtherSkillDuration()
    {
        List<GameObject> players = findRemainEnemys();
        for (int i = 0; i < players.Count; ++i)
        {
            GroupChaos chaos = players[i].GetComponent<GroupChaos>();
            if (chaos != null)
            {
                if (chaos.needUpdate == (int)SkillEunm.SkillState.Duration)
                {
                    chaos.needUpdate = (int)SkillEunm.SkillState.BreakDuration;
                    return true;
                }
            }
        }
        return false;
    }
    public override void afterDuration()
    {
        List<GameObject> players = findRemainPlayers();
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
        if (passTime + deltaTime < durationTime)
        {
            passTime += deltaTime;
        }
        else
        {
            List<GameObject> players = findRemainEnemys();
            for (int i = 0; i < players.Count; ++i)
            {
                PlayerAttribute attr = players[i].GetComponent<PlayerAttribute>();
                if (attr.seat == brokenSeat)
                {
                    attr.isChaos = false;
                    PlayerEffect effect = players[i].GetComponent<PlayerEffect>();
                    if (effect.isPlay(EffectEunm.CHAOS))
                    {
                        effect.Stop(EffectEunm.CHAOS);
                    }
                    brokenSeat = -1;
                }
            }
        }
    }
    public override void onBreakDelay(float deltaTime)
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
                PlayerAttribute attr = players[i].GetComponent<PlayerAttribute>();
                if (attr.seat == brokenSeat)
                {
                    PlayerChildren playerChildren = players[i].GetComponent<PlayerChildren>();
                    playerChildren.title.progressActive(false);
                    attr.isChaos = true;
                    PlayerEffect effect = players[i].GetComponent<PlayerEffect>();
                    if (!effect.isPlay(EffectEunm.CHAOS))
                    {
                        effect.Play(EffectEunm.CHAOS);
                    }
                }
            }
            passTime = 0;
            needUpdate = (int)SkillEunm.SkillState.BreakDuration;
        }
    }
}
