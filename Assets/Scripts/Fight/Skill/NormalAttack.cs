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
            onSkill(direction);
        }
    }
    public override void onSkill(DeltaDirection direction)
    {
        GameObject light = Instantiate(ResourcesTools.getLight(1));
        // 固定光线初始位置y方向
        light.transform.position = new Vector3(transform.position.x, light.transform.position.y, transform.position.z)
                                    + transform.forward.normalized * 2;
        LightManager lightMgr = light.GetComponent<LightManager>();
        lightMgr.Init(GameData.room.Speed, GameData.room.Count, transform.forward.x, transform.forward.z);
    }
}
