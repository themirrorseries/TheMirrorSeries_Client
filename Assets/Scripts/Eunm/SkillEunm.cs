using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SkillEunm
{
    public enum SkillBtn
    {
        // 普通攻击按钮
        ack = 0,
        // 技能按钮1
        skill1 = 1,
        // 技能按钮2
        skill2 = 2,
        // 技能按钮3
        skill3 = 3,
        // 技能按钮4
        skill4 = 4
    }
    public enum SkillID
    {
        // 用于标记是否是空帧
        empty = -2,
        // 用于标记是否是技能
        notSkill = -1,
        // 普通攻击
        normalAck = 1,
        // 保护罩
        protectiveCover = 2,
        // 群体混乱
        groupChaos = 3,
        // 五雷轰顶
        fiveThunder = 4,
        // 黑夜降临
        nightBringer = 5
    }
    public enum SkillState
    {
        // 初始化
        Init = -1,
        // 释放状态
        Release = 0,
        // 持续状态
        Duration = 1
    }
}
