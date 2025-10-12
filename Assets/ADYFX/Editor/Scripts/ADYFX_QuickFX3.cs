using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;


public class ADYFX_QuickFX3 : EditorWindow
{
    public Material bgmat;
    public Texture2D bgTex;
    public Vector2 mBeginScrollView;
    public List<GameObject> prefabs = new List<GameObject>();
    public List<string> texts = new List<string>();
    public List<Texture2D> gifs = new List<Texture2D>();
    public string prefabsPathText = "2365975703bd1e2479bfdf56fc838a8a";//通过GUID找到 存储快捷预制体的 路径的文档
    public string temp1;
    public static ADYFX_QuickFX_Assets quickFX_Assets;
    string path_Create;
[MenuItem("ADYFX/一键特效/配置窗口", false, 1051)]
    static void RadialblurWindowcus()//菜单窗口
    {
        ADYFX_QuickFX3 window = EditorWindow.GetWindow<ADYFX_QuickFX3>();
        window.minSize = new Vector2(1200, 600);//限制窗口最小值
        window.position = new Rect(100, 100, 1200, 600);
        window.titleContent = new GUIContent("一键特效>配置");//标题
        window.Show();//创建窗口
    }
    void OnFocus()//当窗口获得焦点时调用一次
    {
        bgTex = new Texture2D(64, 64);
        bgmat = new Material(Shader.Find("ADYFX/Editer/BG"));
        path_Create = AssetDatabase.GUIDToAssetPath(prefabsPathText);
        quickFX_Assets = (Object)AssetDatabase.LoadAssetAtPath(path_Create, typeof(Object)) as ADYFX_QuickFX_Assets;//从库中获取配置

        //prefabs = new List<GameObject>();
        //texts = new List<string>();
        //gifs = new List<Texture2D>();
        //for (int i = 0; i < quickFX_Assets.gos.Count; i++) //窗口数组拿取配置
        //{
        //    prefabs.Add(quickFX_Assets.gos[i]);
        //    texts.Add(quickFX_Assets.texts[i]);
        //    gifs.Add(quickFX_Assets.texs[i]);
        //}
    }
    private void OnEnable()
    {
        bgTex = new Texture2D(64, 64);
        bgmat = new Material(Shader.Find("ADYFX/Editer/BG"));
        path_Create = AssetDatabase.GUIDToAssetPath(prefabsPathText);
        quickFX_Assets = (Object)AssetDatabase.LoadAssetAtPath(path_Create, typeof(Object)) as ADYFX_QuickFX_Assets;//从库中获取配置

        prefabs = new List<GameObject>(); 
        texts = new List<string>(); 
        gifs = new List<Texture2D>();
        for (int i = 0; i < quickFX_Assets.gos.Count; i++) //窗口数组拿取配置
        {
            prefabs.Add(quickFX_Assets.gos[i]);
            texts.Add(quickFX_Assets.texts[i]);
            gifs.Add(quickFX_Assets.texs[i]);
         }
    }
    private void OnDestroy()
    {
        bgTex = new Texture2D(64, 64);
        bgmat = new Material(Shader.Find("ADYFX/Editer/BG"));
        AssetDatabase.DeleteAsset(path_Create); //关闭窗口时删除旧配置（重新生成配置文件，否则关闭引擎将丢失此次打开引擎之后的修改）
        ADYFX_QuickFX_Assets level = ScriptableObject.CreateInstance<ADYFX_QuickFX_Assets>();//不刷新库 创建新的配置以继承旧配置的guid
        level.gos = new List<GameObject>();
        level.texts = new List<string>();
        level.texs = new List<Texture2D>();
        for (int i = 0; i < prefabs.Count; i++)
        {

            level.gos.Add(prefabs[i]);
            level.texts.Add(texts[i]);
            level.texs.Add(gifs[i]);
        }
        AssetDatabase.CreateAsset(level, path_Create);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();//创建完成后刷新
    }
    private void OnGUI()
    {
        bgmat.SetFloat("_w", position.width / 30);// window.position.width/10
        bgmat.SetFloat("_h", position.height / 30);
        EditorGUI.DrawPreviewTexture(new Rect(0, 0, Screen.width, Screen.height), bgTex, bgmat);//绘制beijing
        var options1 = new[] { GUILayout.Width(250), GUILayout.Height(40) };//定义一个tex2d的宽高

        GUILayout.Label("自定义元素      添加特效预制体  并撰写简介  添加预览图       （注意，元素最低两个，否则主窗口报错）");
        if (prefabs.Count == 0)
        {
            if (GUILayout.Button("添加元素", GUILayout.Width(100), GUILayout.Height(40)))
            {
                prefabs.Add(null);
                texts.Add(null);
                gifs.Add(null);
            }
        }
        mBeginScrollView = GUILayout.BeginScrollView(mBeginScrollView);//开始滚动视图、列表
        {
            for (int i = 0; i < prefabs.Count; i++)
            {
                using (new GUILayout.HorizontalScope())
                {
                    EditorGUI.BeginChangeCheck();
                    GUILayout.Label("第" + (i + 1) + "个", GUILayout.Width(40), GUILayout.Height(40));
                    prefabs[i] = EditorGUILayout.ObjectField(prefabs[i], typeof(GameObject), false, options1) as GameObject;
                    texts[i] = EditorGUILayout.TextField(texts[i], GUILayout.Height(40));
                    gifs[i] = (Texture2D)EditorGUILayout.ObjectField(gifs[i], typeof(Texture2D), false, GUILayout.Width(70), GUILayout.Height(40));
                    if (GUILayout.Button("添加元素", GUILayout.Width(100), GUILayout.Height(40)))
                    {
                        prefabs.Add(null);
                        texts.Add(null);
                        gifs.Add(null);
                    }
                    if (GUILayout.Button("删除此元素", GUILayout.Width(100), GUILayout.Height(40)))
                    {
                        prefabs.RemoveAt(i);
                        texts.RemoveAt(i);
                        gifs.RemoveAt(i);
                    }

                }
            }
        }
        GUILayout.EndScrollView();//结束滚动视图、列表

    }
    //private void Update()
    //{
    //    Repaint();
    //}
}
