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
        passTime = 0;
        needUpdate = (int)SkillEunm.SkillState.Release;
    }
    public override void onDelay(float deltaTime)
    {
        // 更新进度条
    }
    public override void afterDelay()
    {
        List<GameObject> players = findEnemys();
        for (int i = 0; i < players.Count; ++i)
        {
            PlayerAttribute attr = players[i].GetComponent<PlayerAttribute>();
            if (!attr.isChaos)
            {
                PlayerEffect effect = players[i].GetComponent<PlayerEffect>();
                effect.Play(EffectEunm.CHAOS);
            }
            attr.inChaosCount++;
        }
    }
    public override void afterDuration()
    {
        List<GameObject> players = findEnemys();
        for (int i = 0; i < players.Count; ++i)
        {
            PlayerAttribute attr = players[i].GetComponent<PlayerAttribute>();
            if (attr.inChaosCount == 1)
            {
                PlayerEffect effect = players[i].GetComponent<PlayerEffect>();
                effect.Stop(EffectEunm.CHAOS);
            }
            attr.inChaosCount--;
        }
    }
}
