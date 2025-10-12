using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ADYFX_AddAnimation : MonoBehaviour
{
    [MenuItem("CONTEXT/Transform/添加Animation组件")]//第一个CONTEXT 是固定的 第二个是组件 第三个是方法命名
    static public void ADDAnimation(MenuCommand cmd)//在其他组件上扩展，MenuCommand是获取其他组件，这个值是unity传的 根据组件类型获取对应组件
    {
        Transform go = (Transform)cmd.context;//强制转型从MenuCommand.context获得组件
        Animation anima;
        if (go.GetComponent<Animation>())
        {
            //anima = go.GetComponent<Animation>();
        }
        else
        {//如果没有找到animation组件
            anima = go.gameObject.AddComponent<Animation>();//添加组件
            //配置文件str1   [0]记录新建动画片段的保存路径  【1】记录组件上的动画命名和文件命名  
            ADYFX_Common_Assets assets = ADYFX_Editor.GetOBJ("a2c52f1b3c2a5184faa54387299b96cb", true) as ADYFX_Common_Assets;//拿到配置文件
            if (assets != null) 
            {
                AnimationClip ani1;//声明临时动画片段
                string newpath = assets.strs1[0] +"/"+ go.name + assets.strs1[1] + ".anim";//设置临时动画片段的保存路径
                //AssetDatabase.CopyAsset(AssetDatabase.GUIDToAssetPath("fa7ef152f42173d4d84151dfafe38880"), newpath);//复制源动画片段到新位置
                //ani1 = ADYFX_Editor.GetOBJ(newpath) as AnimationClip;//拿到复制的新动画片段
                ani1 = new AnimationClip();
                ani1.name = go.name + assets.strs1[1];//设置animation组件中的名字为 文件名字
                ani1.legacy = true;//动画模式为传统模式
                AssetDatabase.CreateAsset(ani1, newpath);
                anima.AddClip(ani1, ani1.name);//animation组件添加此动画片段
                anima.clip = ani1;
            }
        }
    }

}
