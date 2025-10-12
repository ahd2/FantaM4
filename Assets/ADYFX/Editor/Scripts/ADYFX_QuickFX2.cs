using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Reflection;
//using System;

public class ADYFX_QuickFX2 : EditorWindow
{
    private Material bgmat;
    private Texture2D bgTex;
    private Texture2D morenyulantu;
    private List<GameObject> gos = new List<GameObject>();//元素
    private List<Texture2D> gifs = new List<Texture2D>();//元素的预览图
    private List<string> texts = new List<string>();//元素的介绍
    private List<GUIStyle> styles = new List<GUIStyle>();//元素的介绍
    private List<bool> findindx = new List<bool>();//元素的介绍
    private string find;
    private float yulanWH = 160;//预览图宽高 
    private Material viewmat;
    private Texture2D tex;
    private Vector2 mBeginScrollView;
    private Vector2 mBeginScrollView1;
    private Vector2 mBeginScrollView2;
    GUIStyle style20 = new GUIStyle();
    ADYFX_QuickFX_Assets assets;
    bool isfind = false;
    public string assetspath = "2365975703bd1e2479bfdf56fc838a8a";
    [MenuItem("ADYFX/一键特效/主窗口", false, 1050)]
    static void RadialblurWindowcus()//菜单窗口
    {
        ADYFX_QuickFX2 window = EditorWindow.GetWindow<ADYFX_QuickFX2>();
        window.titleContent = new GUIContent("一键特效");//标题
        window.Show();//创建窗口
    }
    void OnFocus()//当窗口获得焦点时调用一次
    {

        bgTex = new Texture2D(64, 64);
        bgmat = new Material(Shader.Find("ADYFX/Editer/BG"));
        morenyulantu = ADYFX_Editor.GetTex2D_GUID("e1c31f748dee5db468c06a3f41f15342");
        assets = (Object)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(assetspath), typeof(Object)) as ADYFX_QuickFX_Assets;
        style20.alignment = TextAnchor.MiddleCenter;//文本锚点
        style20.fontSize = 20;//文字大小
        style20.normal.textColor = new Color(1, 1f, 1f, 1);//文字颜色
        shauxin();
    }
    private void OnEnable()
    {
        bgTex = new Texture2D(64, 64);
        bgmat = new Material(Shader.Find("ADYFX/Editer/BG"));
        morenyulantu = ADYFX_Editor.GetTex2D_GUID("e1c31f748dee5db468c06a3f41f15342");
        assets  = (Object)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(assetspath), typeof(Object)) as ADYFX_QuickFX_Assets;
        style20.alignment = TextAnchor.MiddleCenter;//文本锚点
        style20.fontSize = 20;//文字大小
        style20.normal.textColor = new Color(1, 1f,1f, 1);//文字颜色
        shauxin();
    }
    private void OnGUI()
    {
        bgTex = new Texture2D(64, 64);
        bgmat = new Material(Shader.Find("ADYFX/Editer/BG"));
        bgmat.SetFloat("_w", position.width / 30);// window.position.width/10
        bgmat.SetFloat("_h", position.height / 30);
        EditorGUI.DrawPreviewTexture(new Rect(0, 0, Screen.width, Screen.height), bgTex, bgmat);//绘制beijing
        if (gos.Count >= 2)
        {
            if (GUILayout.Button("刷新", GUILayout.Height(25)))
            {
                shauxin();
            }
            GUILayout.Label("库中共 " + gos.Count + " 个特效元素", style20);
            GUILayout.Label("可以通过 ADYFX>一键特效>设置窗口 添加新的元素");
            GUILayout.Label("点击预览图 生成对应特效元素到当前选中的物体子级");
            GUILayout.BeginHorizontal();//开始横向布局
            find = GUILayout.TextField(find);
            if (GUILayout.Button("清空输入", GUILayout.Width(80), GUILayout.Height(20)))
            {
                find = "";
            }
            GUILayout.EndHorizontal();//结束横向布局
            GUILayout.BeginHorizontal();//开始横向布局
            if (GUILayout.Button("关键字查找", GUILayout.Width(150), GUILayout.Height(25)))
            {
                isfind = true;
                findgo();
            }
            GUILayout.Space(20);
            if (GUILayout.Button("结束查找", GUILayout.Width(120), GUILayout.Height(25)))
            {
                isfind = false;
            }
            GUILayout.Space(20);
            GUILayout.EndHorizontal();//结束横向布局

            if (isfind)
            {
                mBeginScrollView2 = GUILayout.BeginScrollView(mBeginScrollView2);//开始滚动视图、列表
                {
                    for (int i = 0; i < gos.Count; i++)
                    {
                        if (findindx[i] == true)
                        {
                            if (gos[i])
                            {
                                GUILayout.Space(5);
                                GUILayout.BeginHorizontal();//开始横向布局
                                GUILayout.Button("", GUILayout.Width(5), GUILayout.Height(80));
                                if (GUILayout.Button(texts[i], styles[i], GUILayout.Width(160), GUILayout.Height(90)))
                                {
                                    string nname = gos[i].name;
                                    GameObject ady = Instantiate(gos[i]);//实例化物体
                                    if (Selection.gameObjects.Length >= 1)
                                    {
                                        ady.transform.parent = Selection.gameObjects[0].transform;
                                    }
                                    //ady.transform.localPosition = new Vector3(0, 0, 0);
                                    ady.name = nname;
                                    Selection.activeObject = ady;
                                    EditorGUIUtility.PingObject(ady);
                                }
                                GUILayout.Button("", GUILayout.Width(5), GUILayout.Height(80));
                                //GUILayout.Label("");
                                GUILayout.EndHorizontal();//结束横向布局
                            }
                        }
                    }
                }
                GUILayout.EndScrollView();//结束滚动视图、列表
            }
            else
            {
                GUILayout.BeginHorizontal();//开始横向布局
                mBeginScrollView = GUILayout.BeginScrollView(mBeginScrollView);//开始滚动视图、列表
                {
                    for (int i = 0; i < gos.Count / 2; i++)
                    {
                        if (gos[i])
                        {
                            GUILayout.Space(5);
                            GUILayout.BeginHorizontal();//开始横向布局
                            GUILayout.Button("", GUILayout.Width(5), GUILayout.Height(80));
                            if (GUILayout.Button(texts[i], styles[i], GUILayout.Width(160), GUILayout.Height(90)))
                            {

                                string nname = gos[i].name;
                                GameObject ady = Instantiate(gos[i]);//实例化物体
                                if (Selection.gameObjects.Length >= 1)
                                {
                                    ady.transform.parent = Selection.gameObjects[0].transform;
                                }
                                //ady.transform.localPosition = new Vector3(0, 0, 0);
                                ady.name = nname;
                                Selection.activeObject = ady;
                                EditorGUIUtility.PingObject(ady);
                            }
                            GUILayout.Button("", GUILayout.Width(5), GUILayout.Height(80));
                            //GUILayout.Label("");
                            GUILayout.EndHorizontal();//结束横向布局
                        }

                    }
                }
                GUILayout.EndScrollView();//结束滚动视图、列表
                mBeginScrollView1 = GUILayout.BeginScrollView(mBeginScrollView1);//开始滚动视图、列表
                {
                    for (int i = (gos.Count / 2) - 1; i < gos.Count; i++)
                    {
                        if (gos[i])
                        {
                            GUILayout.Space(5);
                            GUILayout.BeginHorizontal();//开始横向布局
                            GUILayout.Button("", GUILayout.Width(5), GUILayout.Height(80));
                            if (GUILayout.Button(texts[i], styles[i], GUILayout.Width(160), GUILayout.Height(90)))
                            {
                                string nname = gos[i].name;
                                GameObject ady = Instantiate(gos[i]);//实例化物体
                                if (Selection.gameObjects.Length >= 1)
                                {
                                    ady.transform.parent = Selection.gameObjects[0].transform;
                                }
                                //ady.transform.localPosition = new Vector3(0, 0, 0);
                                ady.name = nname;
                                Selection.activeObject = ady;
                                EditorGUIUtility.PingObject(ady);

                            }
                            GUILayout.Button("", GUILayout.Width(5), GUILayout.Height(80));
                            //GUILayout.Label("");
                            GUILayout.EndHorizontal();//结束横向布局
                        }

                    }
                }
                GUILayout.EndScrollView();//结束滚动视图、列表
                GUILayout.EndHorizontal();//结束横向布局
            }
        }
        else 
        {
            GUILayout.Label("你还没有预设特效元素 或元素小于2个    无法运行");
            GUILayout.Label("请通过 ADYFX>一键特效>设置窗口 添加新的元素");
        }
        
    }
    void findgo() //按字符串查找所有预制体string.Substring
    {
        findindx = new List<bool>();
        for (int j = 0; j < gos.Count; j++)
        {
            findindx.Add(false);
        }
        for (int i = 0 ; i < texts.Count; i++)
        {
            if (texts[i].Length >= find.Length)
            {
                for (int j = 0; j < texts[i].Length; j++)
                {
                    if ((texts[i].Length - j) < find.Length)
                    {
                        //break;
                    }
                    else
                    {
                        if (texts[i].Substring(j, find.Length) == find)
                        {
                            findindx[i] = true;
                            //Debug.Log("第" + i + "个元素 正确");
                        }
                    }
                }
            }
        }
    }
    private void shauxin()
    {
        isfind = false;
      gos = new List<GameObject>();//元素
     gifs = new List<Texture2D>();//元素的预览图
     texts = new List<string>();//元素的介绍
        findindx = new List<bool>();
        styles = new List<GUIStyle>();
        gos = assets.gos;
        gifs = assets.texs;
        texts = assets.texts;

        for (int j = 0; j < gos.Count; j++) 
        {
            findindx.Add(false);
        }
        for (int i = 0;i<gifs.Count;i++) 
        {
            styles.Add(new GUIStyle());
            styles[i].alignment = TextAnchor.UpperLeft;//文本锚点
            styles[i].fontSize = 15;//文字大小
            styles[i].normal.textColor = new Color(1, 1, 1, 1f);//文字颜色
            if (gifs[i] == null)
            {
                styles[i].normal.background = morenyulantu;
            }
            else
            {
                styles[i].normal.background = gifs[i]; //默认背景贴图
            }
            styles[i].hover.background = gifs[i];//悬停 图
            styles[i].hover.textColor = new Color(1, 1, 1, 0.6f); //悬停 字
            styles[i].active.background = gifs[i];//点击 图
            styles[i].active.textColor = new Color(1, 1, 1, 0.6f);
        }
    }
    private void OnInspectorUpdate()
    {
        Repaint();
    }

}
