using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class ADYFX_Rename : ScriptableWizard
{
    [Header("勾选即可选择文件夹（自动查找子级）  取消勾选仅可选中文件（只能选中文件来执行）")]
    [Header("【勾选可选文件夹会把文件夹的字符也重命名  且无法判断文件夹中的资源类型  可能会错误的把不属于你管理的资源重命名】")]
    [Header("···········选择资源 然后执行 即可按下方字符替换规则进行重命名·············")]
    public bool isfolder = false;
    [Header("添加前缀")]
    public string qianzui;
    [Header("是否添加后缀序号")]
    public bool ishouzui = false;
    [Header("添加添加后缀序号前插入字符")]
    public string houzui = "_";
    [Header("后缀序号起始值")]
    public int intvalue = 0;
    [Header("勾选则直接按前缀与序号重命名  跳过字符替换")]
    public bool rename1 = false;
    [Header("字符替换：检测到TargetA的字符则替换为TargetB  否则反之 (对应序号)")]
    [Header("-------------------------------------------------------------")]
    public bool AtoB = true;
    public string[] TargetA = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
    public string[] TargetB = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
    [Space(50)]
    public string ADY521 = "YYDS";
    public string bilibiliSpace = "https://space.bilibili.com/7234711";
    //[Header("以下是上次处理的数据")]
    //[Space(50)]
     Object[] objects = new Object[0];
     List<string> paths = new List<string>();
    string tempstr = "";
    int tempvalue;
    [MenuItem("ADYFX/其他工具/※批量修改文件命名_(替换字符工具)", false, 2515)]//在主菜单唤起对话框，注意添加在Hierarchy中不能唤起对话框
    static void duihuakaung()
    {
        ScriptableWizard.DisplayWizard<ADYFX_Rename>("批量修改文件命名_(替换字符工具)", "关闭", "执行重命名");//泛型输入脚本名字，弹出对应脚本内容的弹框，弹框中显示的内容是对应脚本中的public变量                                                                   //Debug.Log("弹对话框");
    }
    public void OnWizardUpdate()//监测所有变量的变化，当有变化时调用
    {
        //errorString = "11";//弹框显示警告信息
        ADY521 = "YYDS";
        bilibiliSpace = "https://space.bilibili.com/7234711";
    }
    void OnWizardOtherButton()//第二个按钮的点击事件
    {
        zhixing();
    }
    public void zhixing()
    {
        //SelectionMode.DeepAssets
        objects = new GameObject[0];
        paths = new List<string>();
        if (isfolder)
        {
            objects = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets);
            //objects = Selection.GetFiltered(typeof(UnityEngine.Texture2D), SelectionMode.DeepAssets);
        }
        else 
        {
            objects = Selection.objects;
        }
        //
        for (int s = 0; s < objects.Length; s++)
        {
            paths.Add(ADYFX_Editor.GetPath(objects[s]));
        }
        if (objects.Length < 1)
        {
            Debug.Log(string.Format("<color=#FF776C>{0}</color>", "你没有选中 或选择的物体中不包含任何预制体、gameobject！"));
        }
        else
        {
            if (rename1)
            {
                for (int i = 0; i < objects.Length; i++)
                {
                    if (ishouzui)
                    {
                        AssetDatabase.RenameAsset(paths[i], qianzui  + houzui + (intvalue+i));
                        tempvalue += i;
                    }
                    else
                    {
                        AssetDatabase.RenameAsset(paths[i], qianzui + tempstr);
                    }
                }
            }
            else 
            {
                for (int i = 0; i < objects.Length; i++)
                {
                    string tempstr1 = objects[i].name;
                    if (TargetA.Length >= 1 && TargetB.Length >= 1 && TargetA.Length == TargetB.Length)
                    {
                        for (int j = 0; j < TargetA.Length; j++)
                        {
                            if (AtoB)
                            {
                                tempstr = tempstr1.Replace(TargetA[j], TargetB[j]);
                                tempstr1 = tempstr;
                                //Debug.Log(tempstr);
                            }

                            else
                            {
                                tempstr = tempstr1.Replace(TargetB[j], TargetA[j]);
                                tempstr1 = tempstr;
                            }
                        }
                        if (ishouzui)
                        {
                            AssetDatabase.RenameAsset(paths[i], qianzui + tempstr + houzui + (intvalue + i));
                        }
                        else
                        {
                            AssetDatabase.RenameAsset(paths[i], qianzui + tempstr);
                        }
                    }
                }
            }
        }
        AssetDatabase.Refresh();
    }
}
