using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class ADYFX_ParNew : EditorWindow
{
    public Material bgmat;
    public Texture2D bgTex;
    public Vector2 mBeginScrollView;
    public List<GameObject> prefabs = new List<GameObject>();
    public List<string> texts = new List<string>();
    public List<string> names = new List<string>();
    public string prefabsPathText = "779170988afd9c240bfb22fa19585a04";//通过GUID找到 存储快捷预制体的 路径的文档
    public string jiaobenpath = "e587b008eb8dbda43b996f7498310eda";//通过GUID找到 脚本的路径
    public static ADYFX_ParticleExpand_Assets quickFX_Assets;
    string path_Create;
    string path_jiaoben;//脚本的路径

    string[] jiaobentext1 = new string[0];//要生成的脚本的头
    public TextAsset daimaduan1;
    string[] jiaobentext2 = new string[0];//要生成的脚本的中间代码段
    public TextAsset daimaduan2;
    string[] jiaobentext3 = new string[0];//要生成的脚本的尾
    public TextAsset daimaduan3;
    string wei = "\n}"; 
    GUIStyle style20 = new GUIStyle();
    [MenuItem("ADYFX/设置/新建粒子系统设置", false, 4001)]
    static void RadialblurWindowcus()//菜单窗口
    {
        ADYFX_ParNew window = EditorWindow.GetWindow<ADYFX_ParNew>();
        window.minSize = new Vector2(1500, 600);//限制窗口最小值
        window.position = new Rect(100, 100, 1500, 600);
        window.titleContent = new GUIContent("新建粒子系统>设置");//标题
        window.Show();//创建窗口
    }
    void OnFocus()//当窗口获得焦点时调用一次
    {
        style20.alignment = TextAnchor.MiddleCenter;//文本锚点
        style20.fontSize = 20;//文字大小
        style20.normal.textColor = new Color(1, 1f, 1f, 1);//文字颜色
        bgTex = new Texture2D(64, 64);
        bgmat = new Material(Shader.Find("ADYFX/Editer/BG"));
        daimaduan1 = ADYFX_Editor.GetOBJ("d0ce90f5fc575584d9f338380559bcde", true) as TextAsset;
        daimaduan2 = ADYFX_Editor.GetOBJ("2f2e7c527d8d177408bc209cf42b3407", true) as TextAsset;
        daimaduan3 = ADYFX_Editor.GetOBJ("82edf1f71724d6c4493bf758697bb115", true) as TextAsset;
        jiaobentext1 = daimaduan1.text.Split('\n');
        jiaobentext2 = daimaduan2.text.Split('\n');
        jiaobentext3 = daimaduan3.text.Split('\n');

        //path_jiaoben = AssetDatabase.GUIDToAssetPath(jiaobenpath);
        //path_Create = AssetDatabase.GUIDToAssetPath(prefabsPathText);
        //quickFX_Assets = (Object)AssetDatabase.LoadAssetAtPath(path_Create, typeof(Object)) as ADYFX_ParticleExpand_Assets;//从库中获取配置
        //Debug.Log(quickFX_Assets.gos.Count);
        //prefabs = new List<GameObject>();
        //texts = new List<string>();
        //names = new List<string>();
        //if (quickFX_Assets.gos.Count >= 1)
        //{
        //    for (int i = 0; i < quickFX_Assets.gos.Count; i++) //窗口数组拿取配置
        //    {
        //        prefabs.Add(quickFX_Assets.gos[i]);
        //        texts.Add(quickFX_Assets.texts[i]);
        //        names.Add(quickFX_Assets.names[i]);
        //    }
        //}
    }
    private void OnEnable()
    {
        bgTex = new Texture2D(64, 64);
        bgmat = new Material(Shader.Find("ADYFX/Editer/BG"));
        daimaduan1 = ADYFX_Editor.GetOBJ("d0ce90f5fc575584d9f338380559bcde", true) as TextAsset;
        daimaduan2 = ADYFX_Editor.GetOBJ("2f2e7c527d8d177408bc209cf42b3407", true) as TextAsset;
        daimaduan3 = ADYFX_Editor.GetOBJ("82edf1f71724d6c4493bf758697bb115", true) as TextAsset;
        jiaobentext1 = daimaduan1.text.Split('\n');
        jiaobentext2 = daimaduan2.text.Split('\n');
        jiaobentext3 = daimaduan3.text.Split('\n');

        path_jiaoben = AssetDatabase.GUIDToAssetPath(jiaobenpath);
        path_Create = AssetDatabase.GUIDToAssetPath(prefabsPathText);
        quickFX_Assets = (Object)AssetDatabase.LoadAssetAtPath(path_Create, typeof(Object)) as ADYFX_ParticleExpand_Assets;//从库中获取配置
        Debug.Log(quickFX_Assets.gos.Count);
        prefabs = new List<GameObject>();
        texts = new List<string>();
        names = new List<string>();
        if (quickFX_Assets.gos.Count>=1)
        {
            for (int i = 0; i < quickFX_Assets.gos.Count; i++) //窗口数组拿取配置
            {
                prefabs.Add(quickFX_Assets.gos[i]);
                texts.Add(quickFX_Assets.texts[i]);
                names.Add(quickFX_Assets.names[i]);
            }
        }
    }
    private void OnDestroy()
    {
        AssetDatabase.DeleteAsset(path_Create); //关闭窗口时删除旧配置（重新生成配置文件，否则关闭引擎将丢失此次打开引擎之后的修改）
        ADYFX_ParticleExpand_Assets level = ScriptableObject.CreateInstance<ADYFX_ParticleExpand_Assets>();//不刷新库 创建新的配置以继承旧配置的guid
        level.gos = new List<GameObject>();
        level.texts = new List<string>();
        level.names = new List<string>();
        for (int i = 0; i < prefabs.Count; i++)
        {

            level.gos.Add(prefabs[i]);
            level.texts.Add(texts[i]);
            level.names.Add(names[i]);
        }
        AssetDatabase.CreateAsset(level, path_Create);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();//创建完成后刷新
        Shengcheng();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();//创建完成后刷新
    }
    private void OnGUI()
    {
        bgmat.SetFloat("_w", position.width / 30);// window.position.width/10
        bgmat.SetFloat("_h", position.height / 30);
        EditorGUI.DrawPreviewTexture(new Rect(0, 0, Screen.width, Screen.height), bgTex, bgmat);//绘制beijing
        var options1 = new[] { GUILayout.Width(250), GUILayout.Height(40) };//定义一个tex2d的宽高
        GUILayout.Label("在本页面添加元素，即可按这些元素生成脚本，以在场景右键菜单 添加快速创建对应元素的按钮  (关闭本窗口即按配置生成脚本)", style20);
       GUILayout.Label("1、添加预制体 2、输入它在右键【快速创建】中的按钮命名 3、输入创建后的命名  ※（注意  命名只支持中文、英文和下划线  不支持其他任何符号  否则会报错）", style20);
        if (prefabs.Count == 0)
        {
            if (GUILayout.Button("添加元素", GUILayout.Width(100), GUILayout.Height(40)))
            {
                prefabs.Add(null);
                texts.Add(null);
                names.Add(null);
            }
        }
        mBeginScrollView = GUILayout.BeginScrollView(mBeginScrollView);//开始滚动视图、列表
        {
            if (prefabs.Count>=1) 
            {
                for (int i = 0; i < prefabs.Count; i++)
                {
                    using (new GUILayout.HorizontalScope())
                    {
                        EditorGUI.BeginChangeCheck();
                        GUILayout.Label("第" + (i + 1) + "个", GUILayout.Width(40), GUILayout.Height(40));
                        prefabs[i] = EditorGUILayout.ObjectField(prefabs[i], typeof(GameObject), false, options1) as GameObject;
                        GUILayout.Space(10);
                        texts[i] = EditorGUILayout.TextField(texts[i], GUILayout.Height(40));
                        GUILayout.Space(10);
                        names[i] = EditorGUILayout.TextField(names[i], GUILayout.Height(40));
                        if (GUILayout.Button("添加元素", GUILayout.Width(100), GUILayout.Height(40)))
                        {
                            prefabs.Add(null);
                            texts.Add(null);
                            names.Add(null);
                        }
                        if (GUILayout.Button("删除此元素", GUILayout.Width(100), GUILayout.Height(40)))
                        {
                            prefabs.RemoveAt(i);
                            texts.RemoveAt(i);
                            names.RemoveAt(i);
                        }
                    }
                }
            }
        }
        GUILayout.EndScrollView();//结束滚动视图、列表
    }
    void Shengcheng() 
    {
        string temp = "";
        temp += daimaduan1.text;
        for (int i = 0;i< prefabs.Count;i++)
        {
            string[] temp1 = new string[jiaobentext2.Length];
            for (int z = 0; z < temp1.Length; z++) 
            {
                temp1[z] = jiaobentext2[z];
            }
            temp1[0] = temp1[0].Replace("菜单名字", texts[i]);
            temp1[1] =  temp1[1].Replace("随机数",""+i);
            temp1[3] = temp1[3].Replace("物体名字", names[i]);
            string tempguid = AssetDatabase.AssetPathToGUID(ADYFX_Editor.GetPath(prefabs[i]));
            temp1[4] = temp1[4].Replace("GUID", tempguid);
            for (int j = 0; j < temp1.Length; j++) 
            {
                temp += "\n";
                temp += temp1[j];
                temp += "\n";
            }
        }
        temp = temp + "\n"+daimaduan3.text;
        Debug.Log(temp);
        File.WriteAllText(AssetDatabase.GUIDToAssetPath(jiaobenpath), temp);
        //Debug.Log(aa);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}

