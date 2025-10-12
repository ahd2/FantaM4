using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
//[ExecuteAlways]

public class ADYFX_ResetTransform 
{

    [MenuItem("ADYFX/特效辅助/重置位置 %&Q", false, 2297)]
    static public void SetTran() 
    {
        if (Selection.gameObjects.Length > 0 ) 
        {
            Selection.gameObjects[0].transform.localPosition = new Vector3(0,0,0);
        }
    }
    [MenuItem("ADYFX/特效辅助/重置旋转 %&W", false, 2298)]
    static public void SetTran1()
    {
        if (Selection.gameObjects.Length > 0)
        {
            Selection.gameObjects[0].transform.localEulerAngles = new Vector3(0, 0, 0);
        }
    }
    [MenuItem("ADYFX/特效辅助/重置缩放 %&E", false, 2299)]
    static public void SetTran2()
    {
        if (Selection.gameObjects.Length > 0)
        {
            Selection.gameObjects[0].transform.localScale = new Vector3(1, 1, 1);
        }
    }



}
