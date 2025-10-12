using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
public class ADYFX_EditorHelp : EditorWindow
{
    public Material bgmat;
    public Texture2D bgTex;
    ADYFX_EditorHelp window;
    //public enum Yemian//枚举
    //{
    //    Window01,
    //    Window02,
    //}
    public float time = 0;
    public int yemian = 0;//记录当前打开的是哪个页面
    public ADYFX_Common_Assets assets;//str1记录列表名称 str2记录对应功能介绍 str3记录对应gif的GUID  str4记录gif帧数
    //public int liebiao = 0;
    public Vector2 mBeginScrollView = new Vector2(500,640);
    public Vector2 mBeginScrollView1 = new Vector2(100,100);
    GUIStyle style = new GUIStyle();
    GUIStyle style20 = new GUIStyle();
    List<string> liebiaotext = new List<string>();
    List<string> jieshaotext = new List<string>();
    List<string> videoguids = new List<string>();
    //ADYFX_Color_Assets ca ;//存储序列的文件
    //List<ADYFX_EditorColors> gifcolors = new List<ADYFX_EditorColors>();
    List<Texture2D> texs = new List<Texture2D>();
    public int zhenshu;
    public Texture2D tempTex0;
    [MenuItem("ADYFX/帮助", false, 5000)]
    public static void HelpWindow()//菜单窗口
    {
        ADYFX_EditorHelp window = EditorWindow.GetWindow<ADYFX_EditorHelp>();//定义窗口类
        window.minSize = new Vector2(1250, 800);//限制窗口最小值
        //window.maxSize = new Vector2(1600, 920);//限制窗口最小值
        window.position = new Rect(200, 100, 1550, 800);
        window.titleContent = new GUIContent("ADYFX工具 使用帮助");//标题内容
        window.Show();//创建窗口
    }
    void OnFocus()//当窗口获得焦点时调用一次
    {
        window = EditorWindow.GetWindow<ADYFX_EditorHelp>();//定义窗口类
        bgTex = new Texture2D(64, 64);
        bgmat = new Material(Shader.Find("ADYFX/Editer/BG"));
        //ca = ADYFX_Editor.GetOBJ("220042d4162f41e49ae464a6d47025c4",true) as ADYFX_Color_Assets;
        assets = ADYFX_Editor.GetOBJ("a71804a7f1ea75043aa468384bd321d1", true) as ADYFX_Common_Assets;
        liebiaotext = assets.strs1;
        jieshaotext = assets.strs2;
        videoguids = assets.strs3;
        ////zhenshu = ca.gifDuration-1;
        //for (int i = 0;i < ca.gifColors.Count; i++) 
        //{
        //    //texs.Add(new Texture2D((int)ca.gifWH.x, (int)ca.gifWH.y));
        //    //texs[i].SetPixels(ca.gifColors[i].colors);
        //    texs[i].Apply();//应用写入
        //}
        ADYFX_EditorTime.ReTime();
        StartWindow();//初始化窗口 获取所需资源
    }
    private void OnEnable()
    {
        window = EditorWindow.GetWindow<ADYFX_EditorHelp>();//定义窗口类
        bgTex = new Texture2D(64, 64);
        bgmat = new Material(Shader.Find("ADYFX/Editer/BG"));
        //ca = ADYFX_Editor.GetOBJ("220042d4162f41e49ae464a6d47025c4",true) as ADYFX_Color_Assets;
        assets = ADYFX_Editor.GetOBJ("a71804a7f1ea75043aa468384bd321d1", true) as ADYFX_Common_Assets;
        liebiaotext = assets.strs1;
        jieshaotext = assets.strs2;
        videoguids = assets.strs3;
        ////zhenshu = ca.gifDuration-1;
        //for (int i = 0;i < ca.gifColors.Count; i++) 
        //{
        //    //texs.Add(new Texture2D((int)ca.gifWH.x, (int)ca.gifWH.y));
        //    //texs[i].SetPixels(ca.gifColors[i].colors);
        //    texs[i].Apply();//应用写入
        //}
        ADYFX_EditorTime.ReTime();
        StartWindow();//初始化窗口 获取所需资源
    }
    private void Update()
    {
        time = ADYFX_EditorTime.time;
        Repaint();
        if (time >=zhenshu) 
        {
            ADYFX_EditorTime.ReTime();
        }
        //Debug.Log(Time.realtimeSinceStartup);
    }
    //void OnFocus()//当窗口获得焦点时调用一次
    //{
    //    StartWindow();//初始化窗口 获取所需资源
    //}
    void StartWindow() //初始化窗口 获取所需资源
    {
        style.alignment = TextAnchor.MiddleCenter;//文本锚点
        style.fontSize = 24;//文字大小
        style.normal.textColor = new Color(1, 1, 1, 1);//文字颜色

        style20.alignment = TextAnchor.MiddleCenter;//文本锚点
        style20.fontSize = 20;//文字大小
        style20.normal.textColor = new Color(0.65f, 0.65f, 0.65f, 1);//文字颜色
        style20.wordWrap = true;//自动换行
    }
    private void OnGUI()
    {
        bgTex = new Texture2D(64, 64);
        bgmat = new Material(Shader.Find("ADYFX/Editer/BG"));
        bgmat.SetFloat("_w", window.position.width / 30);// window.position.width/10
        bgmat.SetFloat("_h", window.position.height / 30);
        EditorGUI.DrawPreviewTexture(new Rect(0, 0, 2560, 1440), bgTex, bgmat);//绘制beijing

        {
            GUILayout.BeginHorizontal();
            GUILayout.Box("功能列表", style, GUILayout.Width(200), GUILayout.Height(35));
            GUILayout.Space(45);
            if (GUILayout.Button("去看视频使用教程", GUILayout.Width(200), GUILayout.Height(30)))
            {
                Application.OpenURL("https://space.bilibili.com/7234711/channel/seriesdetail?sid=613948");
            }
            GUILayout.Space(500);
            GUILayout.Label("版本：ADYFX工具箱  1.0正式版", GUILayout.Width(200), GUILayout.Height(35));
            GUILayout.EndHorizontal();
        }


        GUILayout.BeginHorizontal();
        mBeginScrollView = GUILayout.BeginScrollView(mBeginScrollView, GUILayout.Width(250));//开始滚动视图、列表
        {
            GUILayout.BeginVertical("Box");
            {
                // yemian = (Yemian)GUILayout.SelectionGrid((int)yemian, liebiaotext.ToArray(), 1, GUILayout.Width(250), GUILayout.Height(liebiaotext.Count*30));//根据toober单选 改变枚举值 从而影响下面判断  运行不同的功能函数
                //yemian = GUILayout.SelectionGrid((int)yemian, liebiaotext.ToArray(), 1, GUILayout.Width(250), GUILayout.Height(liebiaotext.Count * 30));//根据toober单选 改变枚举值 从而影响下面判断  运行不同的功能函数
                for (int i = 0; i < liebiaotext.Count; i++)
                {
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button(liebiaotext[i], GUILayout.Width(200), GUILayout.Height(35)))
                    {
                        yemian = i;
                    }
                    if (yemian == i)
                    {
                        GUILayout.Button("", GUILayout.Width(10), GUILayout.Height(35));
                    }
                    GUILayout.Space(15);
                    GUILayout.EndHorizontal();
                }
            }
            GUILayout.EndVertical();
        }
        GUILayout.EndScrollView();//结束滚动视图、列表
        GUILayout.BeginVertical("Box");//对应功能的文字介绍
        {
            GUILayout.Label(jieshaotext[yemian], style20);
        }
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        //EditorGUI.DrawPreviewTexture(new Rect(300, 50, texs[0].width, texs[0].height), texs[(int)time ]);//绘制使用教程GIF
        //GUILayout.Label(time.ToString());
        //Debug.Log(Time.realtimeSinceStartup);
        //if (GUI.Button(new Rect(20, 40, 150, 25), "查看图文教程"))
        //{
        //    Application.OpenURL("https://space.bilibili.com/7234711/article");
        //}
        //if (GUI.Button(new Rect(20, 70, 150, 25), "查看视频教程"))
        //{
        //    Application.OpenURL("https://space.bilibili.com/7234711/channel/seriesdetail?sid=613948");
        //}
        //if (GUI.Button(new Rect(20, 100, 150, 25), "常见问题解决&提交Bug"))
        //{
        //    Application.OpenURL("https://space.bilibili.com/7234711/article");
        //}
        //if (GUI.Button(new Rect(20, 130, 150, 25), "检查更新"))
        //{
        //    Application.OpenURL("https://space.bilibili.com/7234711/article");
        //}
        //if (GUI.Button(new Rect(20, 170, 150, 25), "更多特效教程"))
        //{
        //    Application.OpenURL("https://space.bilibili.com/7234711");
        //}
        //if (GUI.Button(new Rect(20, 310, 150, 30), "一键加群"))
        //{
        //    Application.OpenURL("https://jq.qq.com/?_wv=1027&k=DRzfUJ86");
        //}
        //GUI.Label(new Rect(20, 220, 200, 20), "当前版本 ver 0.1");
        //GUI.Label(new Rect(20, 240, 200, 20), "发布时间 2021.12.17");
        //GUI.Label(new Rect(250, 220, 200, 20), "");
        //GUI.Label(new Rect(230, 5, 100, 20), "Bilibili@ADY521");
        //GUI.Label(new Rect(0, 250, 500, 20), "·································································································································································································");
        //GUI.Label(new Rect(20, 270, 500, 20), "上海【米哈游】 特效内推 加群 704939661 联系管理员 WaiCokl");
        //GUI.Label(new Rect(20, 290, 500, 20), "上海&成都【波克城市】 内推 加群 704939661 联系群主ADY521");

    }
    public void Window01()
    {
        //mBeginScrollView1 = GUILayout.BeginScrollView(mBeginScrollView1);//开始滚动视图、列表
        {
            tempTex0 = ADYFX_Editor.GetTex2D_GUID("17009c2e62392b642bcbbf2f0dfa1e2d");
            GUILayout.BeginVertical("Box");
            {
                GUILayout.Label("【新建粒子系统】\n在Hierarchy列表内右键即可看到 ADYFX_新建粒子系统 菜单展开即可按需快速创建粒子系统" +
                    "，在这里\n新建的粒子系统已是最优设置,无需scaling Mode等繁琐调整。", style20);
            }
        }
        EditorGUI.DrawPreviewTexture(new Rect(260, 150, tempTex0.width, tempTex0.height), tempTex0);
        //GUILayout.EndScrollView();//结束滚动视图、列表
    }
    public void Window02()
    {
        tempTex0 = ADYFX_Editor.GetTex2D_GUID("7d9e80754c2f83744a47e180dbd87795");
        GUILayout.BeginVertical("Box");
        {
            GUILayout.Label("【粒子系统组件快速设置】\n在粒子系统组件上右键即可看到  一键设置自定义顶点数据相关选项（CustomData1+UV2）\n" +
                "菜单点击即可按需快速设置粒子系统将开启粒子系统的自定义顶点数据流 开启CustomData模块1  \n" +
                "在render选项卡中开启Custom Vertex Streams选项 并添加'UV2'、Custom1.xyzw标签。", style20);
        }
    EditorGUI.DrawPreviewTexture(new Rect(260, 150, tempTex0.width, tempTex0.height), tempTex0);
    }
    public void Window03()
    {
        tempTex0 = ADYFX_Editor.GetTex2D_GUID("38fda9e2271eb304ea77b740402520f6");
        GUILayout.BeginVertical("Box");
        {
            GUILayout.Label("【查找资源引用】\n你可以拖入一个任意类型的文件  点开始查找 以得知它被谁使用了 可以点击列表中的按钮以选中它\n", style20);
        }
        EditorGUI.DrawPreviewTexture(new Rect(260, 150, tempTex0.width, tempTex0.height), tempTex0);
    }
    public void Window04()
    {
        tempTex0 = ADYFX_Editor.GetTex2D_GUID("b2b11d6645875dd4d816caf3fdc79aa2");
        GUILayout.BeginVertical("Box");
        {
            GUILayout.Label("【移除丢失脚本】\n多选预制体或场景中的物体  然后点按钮 即可移除所选物体的miss脚本（自动包含所选物体的子级）\n", style20);
        }
        EditorGUI.DrawPreviewTexture(new Rect(260, 150, tempTex0.width, tempTex0.height), tempTex0);
    }
    public void Window05()
    {
        tempTex0 = ADYFX_Editor.GetTex2D_GUID("5ce27012e3959f14cb91ef506483328f");
        GUILayout.BeginVertical("Box");
        {
            GUILayout.Label("【批量移除碰撞体】\n有时候你的特效预制体中可能有遗漏的未移除的碰撞盒 这些组件会占用不必要的性能 多选预制体\n或场景中的物体  然后执行删除操作（自动包含所选物体的子级）\n" +
                "可以通过勾选“只移除含有Mesh组件的物体”的碰撞体来防止误删", style20);
        }
        EditorGUI.DrawPreviewTexture(new Rect(260, 150, tempTex0.width, tempTex0.height), tempTex0);
    }
    public void Window06()
    {
        tempTex0 = ADYFX_Editor.GetTex2D_GUID("e7db0ae3c83abfe4aa0b1afb3a566a50");
        EditorGUI.DrawPreviewTexture(new Rect(260, 160, tempTex0.width, tempTex0.height), tempTex0);
        GUILayout.BeginVertical("Box");
        {
            GUILayout.Label("【拼特效工具】\n有时候你想从旧特效中找找灵感或者找到一些特效 你可以打开拼特效窗口 然后在库中多选你的预制体 \n" +
                "把它添加进列表  然后运行Unity  在列表中点击名称即可把对应特效调出到场景中播放 看到合适的特效点击\n“收藏”" +
                "按钮 它就会暂存到场景中的“shoucang”这个空物体下  等你挑选完毕 你就可以复制这些特效\n 退出运行然后粘贴出来。", style20);
        }
    }
    public void Window07()
    {
        tempTex0 = ADYFX_Editor.GetTex2D_GUID("c0ded3db030de8e4b8e44bb86bee515e");
        GUILayout.BeginVertical("Box");
        {
            GUILayout.Label("【查看特效灰度】\n按下Ctrl+F12 把场景视图置灰 以在场景中查看特效配色、明暗搭配。", style20);
        }
        EditorGUI.DrawPreviewTexture(new Rect(260, 150, tempTex0.width, tempTex0.height), tempTex0);
    }
    public void Window08()
    {
        tempTex0 = ADYFX_Editor.GetTex2D_GUID("7c795eae84800c9498c56a7cd69ce12f");
        GUILayout.BeginVertical("Box");
        {
            GUILayout.Label("【显示或隐藏当前物体】\n按下F11 把场景中当前在选中的物体设为显示或隐藏（根据当前的显示状态），允许多选。", style20);
        }
        EditorGUI.DrawPreviewTexture(new Rect(260, 150, tempTex0.width, tempTex0.height), tempTex0);
    }
    public void Window09()
    {
        tempTex0 = ADYFX_Editor.GetTex2D_GUID("605a53f0904822e49aeed81901811c3a");
        GUILayout.BeginVertical("Box");
        {
            GUILayout.Label("【主相机跟随场景视窗】\n按下Ctrl+F1实时同步主相机视角为场景视窗的角度  按下Alt+F1同步一次 \n" +
                "需要注意的是  如果你的场景中相机有多个，则不能自动选择相机  你需要手动选择相机并按下Ctrl+Alt+F1\n以实时同步，这个物体并不一定是相机 选择其他物体也可以跟随窗口。", style20);
        }
        EditorGUI.DrawPreviewTexture(new Rect(260, 150, tempTex0.width, tempTex0.height), tempTex0);
    }
    public void Window10()
    {
        tempTex0 = ADYFX_Editor.GetTex2D_GUID("7642b16f240738145b5536df981fc887");
        GUILayout.BeginVertical("Box");
        {
            GUILayout.Label("【屏蔽物体在场景中的可选状态】\n按下F10 即可屏蔽除当前选中物体之外的所有物体（其他物体仍然在场景中显示但不可选中\n 且不在列表显示） " +
                "", style20);
        }
        EditorGUI.DrawPreviewTexture(new Rect(260, 150, tempTex0.width, tempTex0.height), tempTex0);
    }
    public Texture2D help11Tex1;
    public Texture2D help11Tex2;
    public Texture2D help11Tex3;
    public Texture2D help11Tex4;
    private int help11int = 1;
    public void Window11()
    {
        //tempTex0 = ADYFX_Editor.GetTex2D("Assets/ADYFX/Elements/HelpTex/help11_1.png");
        help11Tex1 = ADYFX_Editor.GetTex2D_GUID("f0ba9562f8df7c4478e394579bfd7c06");
        help11Tex2 = ADYFX_Editor.GetTex2D_GUID("92708ef2524f59445a1382d548adfbcc");
        help11Tex3 = ADYFX_Editor.GetTex2D_GUID("9564f996c92d7a64692d1ecf85441165");
        help11Tex4 = ADYFX_Editor.GetTex2D_GUID("68f57a6e745d7b746be433a1d4a13e39");
        if(help11int ==1)
            EditorGUI.DrawPreviewTexture(new Rect(260, 150, help11Tex1.width, help11Tex1.height), help11Tex1);
        if (help11int == 2)
            EditorGUI.DrawPreviewTexture(new Rect(260, 150, help11Tex2.width, help11Tex2.height), help11Tex2);
        if (help11int == 3)
            EditorGUI.DrawPreviewTexture(new Rect(260, 150, help11Tex3.width, help11Tex3.height), help11Tex3);
        if (help11int == 4)
            EditorGUI.DrawPreviewTexture(new Rect(260, 150, help11Tex4.width, help11Tex4.height), help11Tex4);
        if (GUI.Button(new Rect(1060, 400, 120, 60), "下一张图"))//特别设置按钮宽高
        {
            help11int += 1;
            if (help11int > 4)
            {
                help11int = 1;
            }
        }
        GUILayout.BeginVertical("Box");
        {
            GUILayout.Label("【截取角色动画的当前帧存为单个模型】\n配合Unity自带的导出FBX插件 快速提取角色动作的一帧成为单个模型，在窗口中Skinned一栏挂上角色的挂\n材质的那个物体 然后新建一个Cube" +
                "，在Filter一栏挂上这个Cube  打开角色的动画 \n 挑选一帧合适的  回到窗口点“截取”按钮 即可把角色动作的当前帧的模型写入到Cube \n 此时选中这个物体使用Unity的FBX Export插件导出即可", style20);
        }
    }
    public void Window12()
    {
        tempTex0 = ADYFX_Editor.GetTex2D_GUID("32eafb9939de43c47955b9b1f179ec5e");
        GUILayout.BeginVertical("Box");
        {
            GUILayout.Label("【批量修改文件命名】\n你可以用它为工程中的资源进行重命名 比如贴图，先设置命名规则 可添加一个前缀 也可以勾选后缀选项\n 同时添加后缀序号  不需要下方替换字符功能的话可以勾选跳过替换\n" +
                "直接按前缀+后缀和序号的形式进行批量重命名 " +
                "", style20);
        }
        EditorGUI.DrawPreviewTexture(new Rect(260, 150, tempTex0.width, tempTex0.height), tempTex0);
    }
}
