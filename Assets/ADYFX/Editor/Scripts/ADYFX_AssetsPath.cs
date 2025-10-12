using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class ADYFX_AssetsPath 
{
    [MenuItem("ADYFX/打开工程文件夹 _F12", false, 5001)]
    static void OpenProjectPath()
    {
        string aa = Application.dataPath;
        string[] split = aa.Split('/');//按反斜杠对字符串进行分割
        string newpath = "";
        for (int i = 0; i < split.Length - 1; i++)
        {
            if (i != 0)
            {
                newpath = newpath + "/" + split[i];
            }
            else
            {
                newpath = newpath + split[i];
            }
        }
        Application.OpenURL("file://" + newpath);
        Debug.Log("打开了assets路径" + newpath);
    }
    [MenuItem("ADYFX/其他工具/打开所选文件在系统中的目录 %F12", false, 2533)]
    static void OpenProjectPath1()
    {
        if (Selection.assetGUIDs.Length > 0)
        {
            string cc = ADYFX_Editor.GetFullPath(ADYFX_Editor.GetOBJ(AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[0])));
            Application.OpenURL("file://" + cc);
            Debug.Log("打开了所选文件的目录" + cc);
        }
    }
}
