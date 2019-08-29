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
        if (RoomData.isMainRole(playerAttribute.seat))
        {
            FightScene.instance.audioController.SoundPlay(AudioEunm.attack);
        }
        GameObject light = Instantiate(ResourcesTools.getLight(1));
        LightManager lightMgr = light.GetComponent<LightManager>();
        lightMgr.Init(RoomData.room.Speed + playerAttribute.bounces, RoomData.room.Count, gameObject);
        FightScene.instance.Lights.Add(light);
    }
    public override void UpdateState(float deltaTime)
    {
        return;
    }
}
