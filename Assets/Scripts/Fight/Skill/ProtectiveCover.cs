using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectiveCover : SkillBase
{
    private Coroutine protectionCoroutine;
    public ProtectiveCover()
    {
        skillId = SkillEunm.protectiveCover;
        skillName = "保护罩";
        cd = 45;
        durationTime = 5;
        skillScope = 10;
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
        protectionCoroutine = StartCoroutine(Protection(skillScope, durationTime));
    }
    // 保护罩协程
    IEnumerator Protection(float range, int duration)
    {
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        CapsuleCollider capsuleCollider = GetComponent<CapsuleCollider>();
        boxCollider.enabled = false;
        capsuleCollider.enabled = true;
        capsuleCollider.radius += range;
        playerAttribute.hasProtection = true;
        // 保护罩UI开启
        yield return new WaitForSeconds(duration);
        boxCollider.enabled = true;
        capsuleCollider.enabled = false;
        capsuleCollider.radius -= range;
        playerAttribute.hasProtection = false;
        // 保护罩UI关闭
        StopCoroutine(protectionCoroutine);
    }
}
