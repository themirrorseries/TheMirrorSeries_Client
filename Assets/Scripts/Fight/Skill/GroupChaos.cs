using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupChaos : SkillBase
{
    private Coroutine chaosCoroutine;
    private Coroutine releaseCoroutine;
    // 上一次协程是否还在运行(-1=>未初始化,1=>正在运行,0=>停止运行)
    private int isLastCoroutineRun = -1;
    public GroupChaos()
    {
        skillId = SkillEunm.groupChaos;
        skillName = "群体混乱";
        cd = 45;
        durationTime = 5;
        delayTime = 3;
    }
    public override void Release(DeltaDirection direction)
    {
        if (isEndCd())
        {
            beforeSkill();
        }
    }
    IEnumerator ReleaseProgress(int delay, int duration)
    {
        float totalTime = 0;
        int index = 0;
        // 总进度100,除以释法时间,等到平均每秒需要增长的进度,然后根据Time.deltaTime去累计
        int time = Mathf.FloorToInt(100 / delay);
        while (index < time && totalTime < delay)
        {
            totalTime += Time.deltaTime;
            ++index;
            // 更新进度条
            yield return null;
        }
        if (isLastCoroutineRun == 1)
        {
            StopCoroutine(chaosCoroutine);
        }
        chaosCoroutine = StartCoroutine(Chaos(duration));
        StopCoroutine(releaseCoroutine);
    }
    public override void beforeSkill()
    {
        base.beforeSkill();
        releaseCoroutine = StartCoroutine(ReleaseProgress(delayTime, durationTime));
    }
    // 混乱协程
    // 这种写法下,时间会叠加
    // eg:当前混乱时间剩余1.5s,有新的技能释放到玩家身上,会重新计时5s
    IEnumerator Chaos(int duration)
    {
        isLastCoroutineRun = 1;
        List<GameObject> players = findEnemys();
        for (int i = 0; i < players.Count; ++i)
        {
            PlayerAttribute attr = players[i].GetComponent<PlayerAttribute>();
            attr.isChaos = true;
            // 头顶播放混乱特效
        }
        yield return new WaitForSeconds(duration);
        for (int i = 0; i < players.Count; ++i)
        {
            PlayerAttribute attr = players[i].GetComponent<PlayerAttribute>();
            attr.isChaos = false;
            // 取消头顶播放混乱特效
        }
        isLastCoroutineRun = 0;
        StopCoroutine(chaosCoroutine);
    }
}
