using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
/// <summary>
/// 用于提供编辑器模式下的时间
/// </summary>
[InitializeOnLoad]
public class ADYFX_EditorTime
{
    static private float lowtime;//上一次的编辑器运行时间
    /// <summary>
    /// 以每秒固定帧率增加的时间（默认30帧s）
    /// </summary>
    static public float time;
    /// <summary>
    /// 帧率  设定每多少秒走一帧 默认0.033334f走一帧
    /// </summary>
    static public float zhenlv = 0.033334f; 
    static ADYFX_EditorTime()
    {
        EditorApplication.update += Update;
        ReTime();
        Rezhenlv30();
        //Debug.Log("测试");
        return;
    }
    static void Update()//每次update用当前编辑器运行时间减上一次的运行时间 如果大于等于设定的帧率才刷新时间
    {
        if ((UnityEngine.Time.realtimeSinceStartup - lowtime) >= zhenlv)
        {
            lowtime = UnityEngine.Time.realtimeSinceStartup;
            time += 1;
        }
    }
    /// <summary>
    /// 重置时间为0
    /// </summary>
    static public void ReTime() 
    {
        time = 0;
    }
    /// <summary>
    /// 设置帧率为10
    /// </summary>
    static public void Rezhenlv10()
    {
        zhenlv = 0.1f;
    }
    /// <summary>
    /// 设置帧率为30
    /// </summary>
    static public void Rezhenlv30()
    {
        zhenlv = 0.033334f;
    }
    /// <summary>
    /// 设置帧率为60
    /// </summary>
    static public void Rezhenlv60()
    {
        zhenlv = 0.016667f;
    }
    /// <summary>
    /// 设置帧率为120
    /// </summary>
    static public void Rezhenlv120()
    {
        zhenlv = 0.008334f;
    }
    /// <summary>
    /// 设置帧率为30
    /// </summary>
    static public void Rezhenlv1()
    {
        zhenlv = 1f;
    }
    /// <summary>
    /// 设置帧率为15
    /// </summary>
    static public void Rezhenlv15()
    {
        zhenlv = 0.066667f;
    }
    /// <summary>
    /// 设定一个自定义帧率（默认0.033334f走一帧）
    /// </summary>
    /// <param name="zhenlv1"></param>
    static public void Rezhenlv(float zhenlv1)
    {
        zhenlv = 0.066667f;
    }
    /// <summary>
    /// 返回当前帧率
    /// </summary>
    /// <returns></returns>
    static public float ReturnZhenlv()
    {
        return zhenlv;
    }
}