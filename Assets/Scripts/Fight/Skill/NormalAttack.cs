using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalAttack : SkillBase
{
    public NormalAttack()
    {
        skillId = (int)SkillEunm.SkillID.normalAck;
        skillName = "普通攻击";
        cd = 5;
    }
    public override void Start()
    {
        base.Start();
        diffCd(-cd);
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
        AnimationControl anim = GetComponent<AnimationControl>();
        anim.Attack();
        if (RoomData.isMainRole(playerAttribute.seat))
        {
            FightScene.instance.audioController.SoundPlay(AudioEunm.attack);
        }
        GameObject light = Instantiate(ResourcesTools.getLight(1));
        LightManager lightMgr = light.GetComponent<LightManager>();

        float speed = ((RoomData.room.Speed + playerAttribute.bounces) > lightMgr.speedRange[lightMgr.speedRange.Length - 1]) ?
                        lightMgr.speedRange[lightMgr.speedRange.Length - 1] : (RoomData.room.Speed + playerAttribute.bounces);
        lightMgr.Init(speed, RoomData.room.Count, gameObject);
        FightScene.instance.Lights.Add(light);
    }
    public override void UpdateState(float deltaTime)
    {
        return;
    }
}
