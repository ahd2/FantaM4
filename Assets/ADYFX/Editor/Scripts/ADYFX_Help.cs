using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class ADYFX_Help : EditorWindow
{
    ADYFX_Help window;
    GUIStyle Style01 = new GUIStyle();//新建按钮样式
    Texture2D btntex;

    //[MenuItem("ADYFX/测试/※测试 %M")]
    static public void Help()//菜单窗口
    {
        ADYFX_Help window = EditorWindow.GetWindow<ADYFX_Help>();//定义窗口类
        window.minSize = new Vector2(1280, 720);//限制窗口最小值
        window.maxSize = new Vector2(1280, 720);//限制窗口最小值
        float sw = Screen.currentResolution.width;
        float sh = Screen.currentResolution.height;
        Rect pos = new Rect((sw-1280)/2, (sh - 720) / 2, 1280, 720);
        window.position = pos;
        window.titleContent = new GUIContent("测试");//标题内容
        window.Show();//创建窗口
    }
    private void OnEnable()
    {
        //window = EditorWindow.GetWindow<ADYFX_Help>();//定义窗口类
        //                                              //Debug.Log(window.position);
        //Style01.alignment = TextAnchor.MiddleCenter;//文本锚点
        //Style01.fontSize = 50;//文字大小
        //Style01.normal.textColor = new Color(0, 0, 0, 0);//文字颜色
        //Style01.normal.background = btn01tex0; //默认背景贴图
        //Style01.hover.background = btn01tex1;//悬停 图
        //Style01.hover.textColor = new Color(0, 0, 0, 0); //悬停 字
        //Style01.active.background = btn01tex0;//点击 图
        //Style01.active.textColor = new Color(0, 0, 0, 0);
    }
    private void OnGUI()
    {

    }
}
