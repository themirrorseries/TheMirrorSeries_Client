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
        }
    }
    public override void beforeSkill()
    {
        base.beforeSkill();
    }
    public override void onSkill()
    {
        passTime = 0;
        needUpdate = (int)SkillEunm.SkillState.Release;
    }
    public override void UpdateState(float deltaTime)
    {
        if (needUpdate == (int)SkillEunm.SkillState.Init)
        {
            return;
        }
        else if (needUpdate == (int)SkillEunm.SkillState.Release)
        {
            if (passTime + deltaTime < delayTime)
            {
                passTime += deltaTime;
                // 更新进度条
            }
            else
            {
                List<GameObject> players = findEnemys();
                for (int i = 0; i < players.Count; ++i)
                {
                    PlayerAttribute attr = players[i].GetComponent<PlayerAttribute>();
                    if (attr.inChaosCount == 0)
                    {
                        attr.isChaos = true;
                        // 头顶播放混乱特效
                    }
                    attr.inChaosCount++;
                }
                needUpdate = 1;
                passTime = (int)SkillEunm.SkillState.Duration;
            }
        }
        else
        {
            if (passTime + deltaTime < durationTime)
            {
                passTime += deltaTime;
            }
            else
            {
                List<GameObject> players = findEnemys();
                for (int i = 0; i < players.Count; ++i)
                {
                    PlayerAttribute attr = players[i].GetComponent<PlayerAttribute>();
                    if (attr.inChaosCount == 1)
                    {
                        attr.isChaos = false;
                        // 取消头顶播放混乱特效
                    }
                    attr.inChaosCount--;
                }
                needUpdate = (int)SkillEunm.SkillState.Init;
                passTime = 0;
            }
        }
    }
}
