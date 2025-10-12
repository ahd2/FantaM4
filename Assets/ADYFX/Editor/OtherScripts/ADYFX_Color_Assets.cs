//    [SerializeField]
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;//为包含数组的类开启序列化[Serializable]  以让其他类中的数组能引用此类
using System.Linq;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using bf = System.Reflection.BindingFlags;
//[SerializeField]
/// <summary>
/// 存储所有帧的颜色
/// </summary>
public class ADYFX_Color_Assets : ScriptableObject
{
    //[SerializeField]
    public List<ADYFX_EditorColors> gifColors = new List<ADYFX_EditorColors>();//存储所有帧颜色组
    public Vector2 gifWH;
    public int gifDuration;
}
[Serializable]//using System;为包含数组的类开启序列化[Serializable]  以让其他类中的数组能引用此类
public class ADYFX_EditorColors//存储单针颜色组
{
    public Color[] colors = new Color[0];
}