using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiveThunder : SkillBase
{
    public FiveThunder()
    {
        skillId = (int)SkillEunm.SkillID.fiveThunder;
        skillName = "五雷轰顶";
        cd = 5;
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
            // 头顶播放被雷劈特效
            PlayerAttribute attr = players[i].GetComponent<PlayerAttribute>();
            // 减少当前生命值的50%
            attr.ChangeHp(-attr.hp / 2);
        }
    }
}
