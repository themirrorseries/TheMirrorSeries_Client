using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalAttack : SkillBase
{
    public NormalAttack()
    {
        skillId = (int)SkillEunm.SkillID.normalAck;
        skillName = "普通攻击";
        cd = 0;
    }
    public override void Release()
    {
        if (isMpEnough(playerAttribute.mpMax))
        {
            AddMp(-playerAttribute.mp);
            onSkill();
        }
    }
    public override void onSkill()
    {
        AnimationControl anim = GetComponent<AnimationControl>();
        anim.Attack();
        GameObject light = Instantiate(ResourcesTools.getLight(1));
        LightManager lightMgr = light.GetComponent<LightManager>();

        lightMgr.Init(RoomData.room.Lights[0].Speed, (int)RoomData.room.Lights[0].Count, gameObject);
        FightScene.instance.Lights.Add(light);
    }
    public override void UpdateState(float deltaTime)
    {
        return;
    }
}
