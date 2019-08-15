using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalAttack : SkillBase
{
    public NormalAttack()
    {
        skillId = SkillEunm.normalAck;
        skillName = "普通攻击";
        cd = 0;
    }
    public override void Release(DeltaDirection direction)
    {
        if (isMpEnough(playerAttribute.mpMax))
        {
            AddMp(-playerAttribute.mp);
            onSkill();
        }
    }
    public override void onSkill()
    {
        GameObject light = Instantiate(ResourcesTools.getLight(1));
        light.transform.eulerAngles = transform.eulerAngles;
        LightManager lightMgr = light.GetComponent<LightManager>();
        lightMgr.Init(GameData.room.Speed, GameData.room.Count, GameData.room.X, GameData.room.Z);
    }
}
