using System.Collections;
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
        if (isEndCd())
        {
            beforeSkill();
            onSkill();
        }
    }
    public override void onSkill()
    {
        List<GameObject> players = findRemainEnemys();
        for (int i = 0; i < players.Count; ++i)
        {
            PlayerAttribute attr = players[i].GetComponent<PlayerAttribute>();
            // 减少当前生命值的50%(向上取整)
            attr.ChangeHp(-Mathf.Floor(attr.hp / 2));
            PlayerChildren children = players[i].GetComponent<PlayerChildren>();
            children.thunder.SetActive(true);
            FightScene.instance.audioController.SoundPlay(AudioEunm.fiveThunder);
            if (attr.isDied)
            {
                AnimationControl anim = players[i].GetComponent<AnimationControl>();
                anim.Death();
                PlayerAction action = players[i].GetComponent<PlayerAction>();
                action.AfterDeath();
                if (RoomData.isMainRole(attr.seat))
                {
                    FightScene.instance.audioController.SoundPlay(AudioEunm.death);
                }
                FightScene.instance.AddDeath(attr.seat, attr.bounces);
            }
        }
    }
    public override void UpdateState(float deltaTime)
    {
        return;
    }
}
