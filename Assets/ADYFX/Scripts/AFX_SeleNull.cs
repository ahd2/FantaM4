using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[ExecuteAlways]//这个物体在生成0.3秒后将取消自身的选中
public class AFX_SeleNull : MonoBehaviour
{
    private void Awake()
    {
        Invoke("SetSelenull", 0.3f);
    }
    void SetSelenull()
    {
        Selection.activeObject = null;
    }
}
