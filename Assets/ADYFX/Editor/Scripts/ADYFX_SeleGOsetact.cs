using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ADYFX_SeleGOsetact 
{

    [MenuItem("ADYFX/特效辅助/※当前选中的物体设为显示或隐藏状态 _F11", false, 2000)]
    static void SetACTTRUE()
    {
        GameObject[] txgo = new GameObject[0];
        txgo = new GameObject[0];
        txgo = Selection.gameObjects;
        for (int i = 0; i < txgo.Length; i++)
        {
            if (txgo[i].gameObject.activeInHierarchy)
            {
                Undo.RecordObject(txgo[i].gameObject, "setActive False");
                txgo[i].gameObject.SetActive(false);
            }
            else
            {
                Undo.RecordObject(txgo[i].gameObject, "setActive True");
                txgo[i].gameObject.SetActive(true);
            }
        }
    }
}
