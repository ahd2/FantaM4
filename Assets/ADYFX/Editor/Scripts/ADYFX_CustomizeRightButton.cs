using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class ADYFX_CustomizeRightButton
{
  [MenuItem("GameObject/新建粒子系统/新建  粒子1", false, 11)]

    static public void CreatePar0() 

    {

        string nname = "aaa";

        GameObject ady = GameObject.Instantiate(ADYFX_Editor.GetOBJ("6bc10bc1a7c12d143b451e97a3995c09",true) as GameObject); 

        if (Selection.gameObjects.Length >= 1)

        {

            ady.transform.parent = Selection.gameObjects[0].transform;

        }

        ady.name = nname;

        Selection.activeObject = ady;

        EditorGUIUtility.PingObject(ady);

    }

  [MenuItem("GameObject/新建粒子系统/通过菜单 ADYFX>设置>新建粒子系统设置  添加", false, 11)]

    static public void CreatePar1() 

    {

        string nname = "aaa";

        GameObject ady = GameObject.Instantiate(ADYFX_Editor.GetOBJ("6bc10bc1a7c12d143b451e97a3995c09",true) as GameObject); 

        if (Selection.gameObjects.Length >= 1)

        {

            ady.transform.parent = Selection.gameObjects[0].transform;

        }

        ady.name = nname;

        Selection.activeObject = ady;

        EditorGUIUtility.PingObject(ady);

    }

}