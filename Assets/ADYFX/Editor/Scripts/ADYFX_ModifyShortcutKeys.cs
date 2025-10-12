using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class ADYFX_ModifyShortcutKeys : EditorWindow
{
    public Material bgmat;
    public Texture2D bgTex;
    public int nowint = 0;
    public Vector2 mBeginScrollView;
    bool Ctrl = false;
    bool Alt = false;
    bool Shift= false;
    string zimu;
    KeyCode keycode ;
    string nowstring;
    GUIStyle style20 = new GUIStyle();
    GUIStyle style16 = new GUIStyle();
    GUIStyle style15 = new GUIStyle();
    Texture2D btn;
    public string kuaijiejian = "";
    public TextAsset textAsset;//asset文件  存储了要修改快捷键的脚本的guid、字符串：拼接_头、字符串：窗口层级、字符串：拼接_尾、字符串：要在对应脚本中查找的关键字（用来确定要修改的行数）
    

    public ADYFX_Common_Assets assets;
    public string[] liebiaoname = new string[] { "1、粒子系统批量设置延迟", "2、为物体添加空粒子系统", "3、移除物体上粒子系统", "4、移除物体上未启用的粒子系统", "5、选中物体以外的设为无法选中"
    , "6、主相机跟随场景视窗单次", "7、主相机跟随场景视窗实时", "8、自选物体跟随场景视窗实时","9、当前选择物体设为显示或隐藏","10、打开工程文件夹"};
    [MenuItem("ADYFX/设置/ADYFX工具箱 快捷键设置", false, 4005)]
    public static void HelpWindow()//菜单窗口
    {
        ADYFX_ModifyShortcutKeys window = EditorWindow.GetWindow<ADYFX_ModifyShortcutKeys>();//定义窗口类
        window.minSize = new Vector2(1000, 200);//限制窗口最小值
        window.maxSize = new Vector2(1600, 900);//限制窗口最小值
        window.position = new Rect(400, 50, 1000, 500);
        window.titleContent = new GUIContent("快捷键设置");//标题内容
        window.Show();//创建窗口
    }
    private void OnEnable()
    {
        bgTex = new Texture2D(64, 64);
        bgmat = new Material(Shader.Find("ADYFX/Editer/BG"));
        assets = ADYFX_Editor.GetOBJ(AssetDatabase.GUIDToAssetPath("23a461b0e2fe7b74e93eceb965727cde")) as ADYFX_Common_Assets;
        Debug.Log(assets.strs1.Count);
        btn = ADYFX_Editor.GetTex_AssetColor("38180ab45ff112a429b6c6d7f4e14ace");
        style20.alignment = TextAnchor.MiddleCenter;//文本锚点
        style20.fontSize = 20;//文字大小
        style20.normal.textColor = new Color(1, 1, 1, 1);//文字颜色

        style16.alignment = TextAnchor.MiddleCenter;//文本锚点
        style16.fontSize = 16;//文字大小
        style16.normal.textColor = new Color(1, 1, 1, 1);//文字颜色\

        style15.normal.background = btn; //默认背景贴图
        style15.hover.background = btn;//悬停 图
        style15.active.background = btn;//点击 图
        style15.alignment = TextAnchor.MiddleCenter;//文本锚点
        style15.fontSize = 15;//文字大小
        style15.normal.textColor = new Color(1, 1, 1, 1);//文字颜色\
        nowstring = liebiaoname[0];
    }
    private void OnGUI()
    {
        bgTex = new Texture2D(64, 64);
        bgmat = new Material(Shader.Find("ADYFX/Editer/BG"));
        bgmat.SetFloat("_w", position.width / 30);// window.position.width/10
        bgmat.SetFloat("_h", position.height / 30);
        EditorGUI.DrawPreviewTexture(new Rect(0, 0, Screen.width, Screen.height), bgTex, bgmat);//绘制beijing
        GUILayout.BeginHorizontal();
        mBeginScrollView = GUILayout.BeginScrollView(mBeginScrollView,GUILayout.Width(250));//开始滚动视图、列表
        {
            GUILayout.BeginVertical("Box");
            {
                for (int i = 0; i < liebiaoname.Length; i++) 
                {
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button(liebiaoname[i], GUILayout.Width(200), GUILayout.Height(30))) 
                    {
                        nowint = i;
                        nowstring = liebiaoname[i];
                    }
                     if (nowint == i) 
                    {
                        GUILayout.Button("", GUILayout.Width(10), GUILayout.Height(30));
                    }
                    GUILayout.EndHorizontal();
                }
            }
            GUILayout.EndVertical();
        }
        GUILayout.EndScrollView();//结束滚动视图、列表


        //GUILayout.BeginHorizontal();
        //GUILayout.BeginVertical();

        GUILayout.BeginVertical("Box", GUILayout.Height(120));
        {
            GUILayout.Label("正在修改  【" + nowstring + "】  的快捷键", style20);
            string temp = "";
            GUILayout.Label("选择控制键或不需要控制键   再按下键盘其他字符以设定快捷键", style16);
            if (Ctrl)
            {
                temp += "Ctrl+";
            }
            if (Shift)
            {
                temp += "Shift+";
            }
            if (Alt)
            {
                temp += "Alt+";
            }
            GUILayout.Label("支持的按键：a-z、0-9、F1-F12 以及~，。/");
            GUILayout.Label("若因弹出窗口 或其他软件快捷键冲突 导致无法捕获你按下的键  按住Ctrl+Shift+Alt 再按下你要输入的字符");
            GUILayout.BeginHorizontal("Box");
            Ctrl = GUILayout.Toggle(Ctrl, "Ctrl", GUILayout.Width(50), GUILayout.Height(30));
            Shift = GUILayout.Toggle(Shift, "Shift", GUILayout.Width(50), GUILayout.Height(30));
            Alt = GUILayout.Toggle(Alt, "Alt", GUILayout.Width(50), GUILayout.Height(30));
            GUILayout.Space(50);
            GUILayout.Label("你把快捷键设为了：" + temp + zimu, style16);
            GUILayout.EndHorizontal();
            GUILayout.Space(25);
            GUILayout.BeginHorizontal();
            GUILayout.Space(260);
            if (GUILayout.Button("保存", style15, GUILayout.Width(225), GUILayout.Height(50)))
            {
                    win1();
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
        }
        //GUILayout.EndVertical();
        GUILayout.EndVertical();
        //GUILayout.EndHorizontal();
        GUILayout.EndHorizontal();


        if (Event.current.type == EventType.KeyUp)//检测按键抬起
        {
            if (Event.current.keyCode != KeyCode.RightControl && Event.current.keyCode != KeyCode.LeftControl &&
                Event.current.keyCode != KeyCode.RightAlt && Event.current.keyCode != KeyCode.LeftAlt &&
                Event.current.keyCode != KeyCode.RightShift && Event.current.keyCode != KeyCode.LeftShift &&
                Event.current.keyCode != KeyCode.LeftCommand && Event.current.keyCode != KeyCode.RightCommand &&
                Event.current.keyCode != KeyCode.LeftApple && Event.current.keyCode != KeyCode.RightApple)
            {
                zimu = Event.current.keyCode.ToString();//按键抬起时检测按下的是什么键
                keycode = Event.current.keyCode;
            }
        }
    }
    private void OnInspectorUpdate()
    {
        Repaint();
    }

    void win1()
    {
        kuaijiejian = "";
        if (Ctrl) 
        {
            kuaijiejian += "%";
        }
        if (Alt)
            kuaijiejian += "&";
        if (Shift)
            kuaijiejian += "#";

        if (!Ctrl&&!Alt&&!Shift) 
        {
            kuaijiejian = "_";
        }
        kuaijiejian += zimu;
        string newkey = "";
        TextAsset temp = ADYFX_Editor.GetOBJ(AssetDatabase.GUIDToAssetPath(assets.strs1[nowint])) as TextAsset;
        string wenben = temp.text;
        string[] wenben1 = wenben.Split('\n');
        BoolAndint boolAndint = ADYFX_Editor.StringFind(wenben, assets.strs5[nowint], true);
        if (boolAndint.m_Bool == true)
        {
            string ttt = "";
            newkey = assets.strs2[nowint] + kuaijiejian + assets.strs3[nowint] + assets.strs4[nowint];
            wenben1[boolAndint.m_int] = newkey;
            for (int i = 0; i < wenben1.Length; i++)
            {
                ttt += wenben1[i] + "\n";
            }
            Debug.Log(ttt);
            File.WriteAllText(AssetDatabase.GUIDToAssetPath(assets.strs1[nowint]), ttt);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        else 
        {
            Debug.LogError("无法找到对应脚本！！！，如果您在Unity之外修改过ADYFX工具箱的文件位置 请删除ADYFX/Editor/Scripts文件夹并重新导入工具箱以用原始脚本替换当前的出错脚本");
        }
    }
}
