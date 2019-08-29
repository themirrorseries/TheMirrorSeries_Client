using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectiveCover : SkillBase
{
    private BoxCollider boxCollider;
    private CapsuleCollider capsuleCollider;
    public ProtectiveCover()
    {
        skillId = (int)SkillEunm.SkillID.protectiveCover;
        skillName = "粒子护盾";
        cd = 45;
        durationTime = 5;
        skillScope = 10;
    }
    public override void Start()
    {
        base.Start();
        boxCollider = GetComponent<BoxCollider>();
        capsuleCollider = GetComponent<CapsuleCollider>();
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
        boxCollider.enabled = false;
        capsuleCollider.enabled = true;
        playerAttribute.hasProtection = true;
        PlayerEffect playerEffect = GetComponent<PlayerEffect>();
        playerEffect.Play(EffectEunm.COVER);
        FightScene.instance.audioController.SoundPlay(AudioEunm.protectiveCover);
        passTime = 0;
        needUpdate = (int)SkillEunm.SkillState.Duration;
    }
    public override void afterDuration()
    {
        boxCollider.enabled = true;
        capsuleCollider.enabled = false;
        playerAttribute.hasProtection = false;
        PlayerEffect playerEffect = GetComponent<PlayerEffect>();
        playerEffect.Stop(EffectEunm.COVER);
    }
}
