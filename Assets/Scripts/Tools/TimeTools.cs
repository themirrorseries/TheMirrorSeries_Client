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
    // 秒=>x分x秒
    public static string Num2TimeString(float value)
    {
        // 向上取整
        int value_int = Mathf.CeilToInt(value);
        string time = "";
        if ((value_int / 60) > 0)
        {
            time = time + (value_int / 60).ToString() + "分";
        }
        value_int = value_int - (value_int / 60) * 60;
        if (value > 0)
        {
            time = time + value_int.ToString() + "秒";
        }
        return time;
    }
}
