using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class ADYFX_SetMatQueue : EditorWindow
{
    public Material bgmat;
    public Texture2D bgTex;
    public enum Iftype//editorgui框架枚举
    {
        大于,
        大于等于,
        等于,
        小于等于,
        小于
    }
    public string[] iftypes = new string[] { "大于", "大于等于", "等于", "小于等于", "小于"};
    public Iftype iftype;

    public enum Yunsuan//editorgui框架枚举
    {
        加,
        减,
        等于
    }
    public string[] yunshuanstr = new string[] { "+", "-", "=" };
    public Yunsuan yunshuan;

    private string shaderpath = "";
    public float qishishengxiaoceng = 3000;
    public float value = 1;
    public Shader shader;
    GUIStyle style = new GUIStyle();
    GUIStyle style20 = new GUIStyle();
    List<Material> mats = new List<Material>();
    public string[] targetPath = new string[0];
    public Vector2 mBeginScrollView;
    List<string> xunzhi = new List<string>();
    private void OnEnable()
    {
        bgTex = new Texture2D(64, 64);
        bgmat = new Material(Shader.Find("ADYFX/Editer/BG"));

        style.alignment = TextAnchor.MiddleCenter;//文本锚点
        style.fontSize = 16;//文字大小
        style.normal.textColor = new Color(0.65f, 0.65f, 0.65f, 1);//文字颜色

        style20.alignment = TextAnchor.MiddleCenter;//文本锚点
        style20.fontSize = 20;//文字大小
        style20.normal.textColor = new Color(1, 0.2f, 0.2f, 1);//文字颜色
    }
    void OnFocus()//当窗口获得焦点时调用一次
    {
        bgTex = new Texture2D(64, 64);
        bgmat = new Material(Shader.Find("ADYFX/Editer/BG"));

        style.alignment = TextAnchor.MiddleCenter;//文本锚点
        style.fontSize = 16;//文字大小
        style.normal.textColor = new Color(0.65f, 0.65f, 0.65f, 1);//文字颜色

        style20.alignment = TextAnchor.MiddleCenter;//文本锚点
        style20.fontSize = 20;//文字大小
        style20.normal.textColor = new Color(1, 0.2f, 0.2f, 1);//文字颜色
    }
    [MenuItem("ADYFX/特效辅助/※批量修改材质渲染队列", false, 2110)]
    static void RadialblurWindowcus()//菜单窗口
    {
        ADYFX_SetMatQueue window = EditorWindow.GetWindow<ADYFX_SetMatQueue>();//定义窗口类
        window.minSize = new Vector2(500, 750);//限制窗口最小值
        window.maxSize = new Vector2(500, 1350);//限制窗口最小值
        window.position = new Rect(400, 50, 500, 800);
        window.titleContent = new GUIContent("批量修改材质渲染队列");//标题内容
        window.Show();//创建窗口
    }
    private void OnGUI()
    {
        bgTex = new Texture2D(64, 64);
        bgmat = new Material(Shader.Find("ADYFX/Editer/BG"));
        bgmat.SetFloat("_w", position.width / 30);// window.position.width/10
        bgmat.SetFloat("_h", position.height / 30);
        EditorGUI.DrawPreviewTexture(new Rect(0, 0, Screen.width, Screen.height), bgTex, bgmat);//绘制beijing
        GUILayout.Label("按判断规则来修改材质Queue", style);
        GUILayout.Label("如果指定一个Shader 那只有这个Shader所属的材质才能添加到列表，否则所有材质都可以");
        var options1 = new[] { GUILayout.Width(200), GUILayout.Height(25) };//定义一个tex2d的宽高
        var options2 = new[] { GUILayout.Width(480), GUILayout.Height(20) };//定义一个tex2d的宽高
        shader = EditorGUILayout.ObjectField(shader, typeof(Shader), false, options1) as Shader;//然后声明这个tex2d
        GUILayout.Space(5);//旧版的空格，2018.3以下
                           //shaderpath = EditorGUILayout.TextField(shaderpath, GUILayout.Height(22));
        GUILayout.Label("指定一个队列值");
        qishishengxiaoceng = EditorGUILayout.FloatField(qishishengxiaoceng);
        GUILayout.Label("然后与材质队列值比较  如果材质的队列值：");
        iftype = (Iftype)GUILayout.Toolbar((int)iftype, new string[] { ">"+ qishishengxiaoceng, ">=" + qishishengxiaoceng, "=" + qishishengxiaoceng, "<=" + qishishengxiaoceng, "<" + qishishengxiaoceng }, GUILayout.Width(500));//根据toober单选 改变枚举值 从而影响下面判断  运行不同的功能函数
        GUILayout.Label("则");
        GUILayout.Label("与此值");
        value = EditorGUILayout.FloatField(value);
        yunshuan = (Yunsuan)GUILayout.Toolbar((int)yunshuan, new string[] { "渲染队列"+"+" + value, "渲染队列" + "-" + value, "渲染队列" + "=" + value }, GUILayout.Width(300));//根据toober单选 改变枚举值 从而影响下面判断  运行不同的功能函数
        GUILayout.Space(10);//旧版的空格，2018.3以下

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button(" 执行修改 ", GUILayout.Width(200), GUILayout.Height(30))) 
        {
            ZhiXing();
        }
        GUILayout.Space(95);//旧版的空格，2018.3以下
        if (GUILayout.Button(" 清空列表 ", GUILayout.Width(200), GUILayout.Height(30)))
        {
            mats = new List<Material>();
            xunzhi = new List<string>();
            //if (Event.current.alt) 
            //{
            //    Debug.Log("aaa" );
            //}
        }
        EditorGUILayout.EndHorizontal();
        GUILayout.Label("库中多选材质球 拖拽到窗口以添加到待处理列表", style);
        if (shader != null)
        {
            GUILayout.Label("待处理列表 \n(仅能添加指定Shader所属材质)", style20);
        }
        else 
        {
            GUILayout.Label("待处理列表", style20);
        }


        if (DragAndDrop.paths.Length > 0)
        {
            if (mouseOverWindow == this)
            {//鼠标位于当前窗口
                if (Event.current.type == EventType.DragUpdated)
                {//拖入窗口未松开鼠标
                    DragAndDrop.visualMode = DragAndDropVisualMode.Generic;//改变鼠标外观
                }
                else if (Event.current.type == EventType.DragExited)
                {//拖入窗口并松开鼠标
                    targetPath = new string[0];
                    targetPath = DragAndDrop.paths;
                    for (int i = 0; i < targetPath.Length; i++)
                    {
                        string[] aa = targetPath[i].Split('.');
                        if (aa[aa.Length - 1]== "mat") 
                        {
                            //Debug.Log(shader.name);
                            Material mat = (Material)AssetDatabase.LoadAssetAtPath(targetPath[i], typeof(Material));
                            if (shader != null)
                            {
                                if (mat.shader == shader)
                                {
                                    mats.Add(mat);
                                    xunzhi.Add(targetPath[i]);
                                }
                            }
                            else 
                            {
                                mats.Add(mat);
                                xunzhi.Add(targetPath[i]);
                            }
                        }
                    }
                }
            }
        }

        mBeginScrollView = GUILayout.BeginScrollView(mBeginScrollView);//开始滚动视图、列表
        {
            for (int j = 0; j < mats.Count; j++)
            {
                //mats[j] = EditorGUILayout.ObjectField(mats[j], typeof(Material), false, options2) as Material;//然后声明这个tex2d

                if (GUILayout.Button(mats[j].name, GUILayout.Width(480), GUILayout.Height(18)))
                {
                    Object obj = AssetDatabase.LoadMainAssetAtPath(xunzhi[j]);
                    Selection.activeObject = obj;
                }
            }
        }
        GUILayout.EndScrollView();//结束滚动视图、列表
    }

    void ZhiXing() 
    {
        for (int i = 0; i < mats.Count; i++) 
        {
            if (iftype == Iftype.大于) 
            {
                if (mats[i].renderQueue > qishishengxiaoceng) 
                {
                    if (yunshuan == Yunsuan.加) 
                    {
                        mats[i].renderQueue += (int)value;
                    }
                    if (yunshuan == Yunsuan.减)
                    {
                        mats[i].renderQueue -= (int)value;
                    }
                    if (yunshuan == Yunsuan.等于)
                    {
                        mats[i].renderQueue = (int)value;
                    }
                }
            }
            if (iftype == Iftype.大于等于)
            {
                if (mats[i].renderQueue >= qishishengxiaoceng)
                {
                    if (yunshuan == Yunsuan.加)
                    {
                        mats[i].renderQueue += (int)value;
                    }
                    if (yunshuan == Yunsuan.减)
                    {
                        mats[i].renderQueue -= (int)value;
                    }
                    if (yunshuan == Yunsuan.等于)
                    {
                        mats[i].renderQueue = (int)value;
                    }
                }
            }
            if (iftype == Iftype.等于)
            {
                if (mats[i].renderQueue == qishishengxiaoceng)
                {
                    if (yunshuan == Yunsuan.加)
                    {
                        mats[i].renderQueue += (int)value;
                    }
                    if (yunshuan == Yunsuan.减)
                    {
                        mats[i].renderQueue -= (int)value;
                    }
                    if (yunshuan == Yunsuan.等于)
                    {
                        mats[i].renderQueue = (int)value;
                    }
                }
            }
            if (iftype == Iftype.小于等于)
            {
                if (mats[i].renderQueue <= qishishengxiaoceng)
                {
                    if (yunshuan == Yunsuan.加)
                    {
                        mats[i].renderQueue += (int)value;
                    }
                    if (yunshuan == Yunsuan.减)
                    {
                        mats[i].renderQueue -= (int)value;
                    }
                    if (yunshuan == Yunsuan.等于)
                    {
                        mats[i].renderQueue = (int)value;
                    }
                }
            }
            if (iftype == Iftype.小于)
            {
                if (mats[i].renderQueue < qishishengxiaoceng)
                {
                    if (yunshuan == Yunsuan.加)
                    {
                        mats[i].renderQueue += (int)value;
                    }
                    if (yunshuan == Yunsuan.减)
                    {
                        mats[i].renderQueue -= (int)value;
                    }
                    if (yunshuan == Yunsuan.等于)
                    {
                        mats[i].renderQueue = (int)value;
                    }
                }
            }
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
