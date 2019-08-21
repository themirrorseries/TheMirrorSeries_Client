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
        capsuleCollider.radius += skillScope;
        playerAttribute.hasProtection = true;
        PlayerChildren playerChildren = GetComponent<PlayerChildren>();
        playerChildren.cover.SetActive(true);
        playerChildren.cover.transform.localScale = playerChildren.cover.transform.localScale +
            new Vector3(skillScope, skillScope, skillScope);
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
                PlayerChildren playerChildren = GetComponent<PlayerChildren>();
                playerChildren.cover.SetActive(false);
                playerChildren.cover.transform.localScale = playerChildren.cover.transform.localScale -
                    new Vector3(skillScope, skillScope, skillScope);
                needUpdate = (int)SkillEunm.SkillState.Init;
            }
        }
    }
}
