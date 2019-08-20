using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TimeTools
{
    private static DateTime begin = new DateTime(1970, 1, 1, 0, 0, 0);
    // 获取当前时间戳
    public static long getTimeStamp()
    {
        TimeSpan st = DateTime.UtcNow - begin;
        return Convert.ToInt64(st.TotalSeconds);
    }
    public static long addTimeStamp(int value)
    {
        TimeSpan st = DateTime.UtcNow.AddSeconds(value) - begin;
        return Convert.ToInt64(st.TotalSeconds);
    }
    public static string Num2TimeString(float value)
    {
        Debug.Log(value);
        return "1分40秒";
    }
}
