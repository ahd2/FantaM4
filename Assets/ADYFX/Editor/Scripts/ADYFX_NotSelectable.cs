using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ADYFX_NotSelectable
{
    [MenuItem("ADYFX/特效辅助/※选中的物体以外全都设为无法被选中,如果不选中任何物体按下快捷键则恢复所有物体的状态 _F10", false, 2001)]
    static void Selectexcept()
    {
        List<Transform> sons = new List<Transform>();
        GameObject[] seleGo = Selection.gameObjects;
        Transform[] go = new Transform[0];
        List<GameObject> gos = new List<GameObject>();
        foreach (GameObject objj in UnityEngine.Object.FindObjectsOfType(typeof(GameObject)))
        {
            gos.Add(objj);
            objj.hideFlags = HideFlags.HideInHierarchy;//物体不出现在场景列表中
        }
        Debug.Log(seleGo.Length);
        for (int i = 0; i < seleGo.Length; i++)
        {
            go = seleGo[i].GetComponentsInChildren<Transform>(true);
            for (int a = 0; a < go.Length; a++)
            {
                sons.Add(go[a]);
            }
        }
        //Selection.activeObject = null;
        for (int i = 0; i < sons.Count; i++)
        {
            sons[i].hideFlags = HideFlags.None;//
        }
        if (seleGo.Length == 0)
        {
            foreach (GameObject objj in UnityEngine.Object.FindObjectsOfType(typeof(GameObject)))
            {
                objj.hideFlags = HideFlags.None;//物体不出现在场景列表中
            }
        }
    }
}
