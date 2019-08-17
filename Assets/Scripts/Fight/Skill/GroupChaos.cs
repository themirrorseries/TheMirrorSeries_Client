﻿using System.Collections;
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
        cd = 5;
        durationTime = 50;
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
        Debug.Log("我开始读条了");
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
    // 混乱协程()
    IEnumerator Chaos(int duration)
    {
        isLastCoroutineRun = 1;
        List<GameObject> players = findAllPlayers();
        for (int i = 0; i < players.Count; ++i)
        {
            PlayerControl playerControl = players[i].GetComponent<PlayerControl>();
            if (playerControl.seat != GameData.seat)
            {
                playerControl.attr().isChaos = true;
                // 头顶播放混乱特效
            }
        }
        yield return new WaitForSeconds(duration);
        for (int i = 0; i < players.Count; ++i)
        {
            PlayerControl playerControl = players[i].GetComponent<PlayerControl>();
            if (playerControl.seat != GameData.seat)
            {
                playerControl.attr().isChaos = false;
                // 取消头顶播放混乱特效
            }
        }
        isLastCoroutineRun = 0;
        StopCoroutine(chaosCoroutine);
    }
}
