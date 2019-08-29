using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FPSCounter : MonoBehaviour
{
    // Attach this to a GUIText to make a frames/second indicator.
    // It calculates frames/second over each updateInterval,
    // so the display does not keep changing wildly.
    // It is also fairly accurate at very low FPS counts (<10).
    // We do this not by simply counting frames per interval, but
    // by accumulating FPS for each frame. This way we end up with
    // correct overall FPS even if the interval renders something like
    // 5.5f frames.
    #region 字段
    float updateInterval = 1.0f;
    private float accum = 0.0f;        // FPS accumulated over the interval
    private float frames = 0;      // Frames drawn over the interval
    private float timeleft; // Left time for current interval
    private float fps = 15.0f;         // Current FPS
    private float lastSample;
    private float gotIntervals = 0;
    GUIStyle titleStyle = new GUIStyle();
    #endregion
    #region 属性
    #endregion
    #region Unity回调函数
    void Start()
    {
        timeleft = updateInterval;
        lastSample = Time.realtimeSinceStartup;
        titleStyle.fontSize = 20;
        titleStyle.normal.textColor = new Color(46f / 256f, 163f / 256f, 256f / 256f, 256f / 256f);
    }//Start ()_end
    void Awake()
    {
        Application.targetFrameRate = 60;
    }
    void Update()
    {
        ++frames;
        float newSample = Time.realtimeSinceStartup;
        float deltaTime = newSample - lastSample;
        lastSample = newSample;
        timeleft -= deltaTime;
        accum += 1.0f / deltaTime;
        // Interval ended - update GUI text and start new interval
        if (timeleft <= 0.0f)
        {
            // display two fractional digits (f2 format)
            fps = accum / frames;
            // guiText.text = fps.ToString("f2");
            timeleft = updateInterval;
            accum = 0.0f;
            frames = 0;
            ++gotIntervals;
        }
    }//Update ()_end
    #endregion
    #region 自建方法
    void OnGUI()
    {
        GUI.Label(new Rect(50, 50, 200, 200), "FPS:" + fps.ToString("f2"), titleStyle);
    }
    #endregion
}