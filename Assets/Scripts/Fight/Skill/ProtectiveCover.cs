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
        skillName = "保护罩";
        cd = 45;
        durationTime = 5;
        skillScope = 10;
    }
    void Start()
    {
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
        capsuleCollider.radius += skillScope;
        playerAttribute.hasProtection = true;
        // 保护罩UI开启
        passTime = 0;
        needUpdate = (int)SkillEunm.SkillState.Duration;
    }
    public override void UpdateState(float deltaTime)
    {
        if (needUpdate == (int)SkillEunm.SkillState.Init)
        {
            return;
        }
        else if (needUpdate == (int)SkillEunm.SkillState.Duration)
        {
            if (passTime + deltaTime < durationTime)
            {
                passTime += deltaTime;
            }
            else
            {
                boxCollider.enabled = true;
                capsuleCollider.enabled = false;
                capsuleCollider.radius -= skillScope;
                playerAttribute.hasProtection = false;
                // 保护罩UI关闭
                needUpdate = (int)SkillEunm.SkillState.Init;
            }
        }
    }
}
