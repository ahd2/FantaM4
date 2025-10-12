using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class ADYFX_QuickFX : EditorWindow
{
    public Material bgmat;
    public Texture2D bgTex;
    public Vector2 mBeginScrollView;
    //public GameObject[] gos = new GameObject[0];
    public List<GameObject> gos = new List<GameObject>();
    public string[] paths = new string[0];
    public int value = 0;
    public string targetPath = "";
    public bool ison = false;
    [MenuItem("ADYFX/特效辅助/※快速创建特效", false, 2200)]
    static void RadialblurWindowcus()//菜单窗口
    {
        ADYFX_QuickFX window = EditorWindow.GetWindow<ADYFX_QuickFX>();//定义窗口类
        window.titleContent = new GUIContent("快速创建特效");//标题内容
        window.Show();//创建窗口
    }
    void OnFocus()//当窗口获得焦点时调用一次
    {

    }
    private void OnEnable()
    {
        bgTex = new Texture2D(64, 64);
        bgmat = new Material(Shader.Find("ADYFX/Editer/BG"));
    }
    [MenuItem("Assets/复制路径", false, 115)]
    static void FXCopyPath()
    {
        string bb = AssetDatabase.GetAssetPath(Selection.objects[0]);
        Debug.Log(AssetDatabase.GetAssetPath(Selection.objects[0]));
        UnityEngine.GUIUtility.systemCopyBuffer = bb;
    }
    private void OnGUI()
    {
        bgTex = new Texture2D(64, 64);
        bgmat = new Material(Shader.Find("ADYFX/Editer/BG"));
        bgmat.SetFloat("_w", position.width / 30);// window.position.width/10
        bgmat.SetFloat("_h", position.height / 30);
        EditorGUI.DrawPreviewTexture(new Rect(0, 0, Screen.width, Screen.height), bgTex, bgmat);//绘制beijing
        ison = GUILayout.Toggle(ison, "--允许通过拖拽资源库中的文件夹添加到列表--");
        //GUILayout.Label("拖拽一个资源库中的文件夹到窗口以添加其内部的预制体到列表");
        if (ison) 
        {
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
                        targetPath = DragAndDrop.paths[0];
                        gos = GetAllPrefabs();
                        paths = new string[gos.Count];
                        for (int i = 0; i < gos.Count; i++)
                        {
                            paths[i] = AssetDatabase.GetAssetPath(gos[i]);
                        }
                    }
                }
            }
        }
        GUILayout.Label("资源库右键一个文件夹“Copy Path”再复制到此处   点击【载入路径中的特效到列表】或↑↑↑");
        //GUILayout.Label("当前使用的路径"+targetPath);
        targetPath = EditorGUILayout.TextField(targetPath, GUILayout.Height(22));
        if (GUILayout.Button("载入路径中的特效到列表", GUILayout.Width(300), GUILayout.Height(30)))
        {
            //targetPath = GUIUtility.systemCopyBuffer;
            gos = GetAllPrefabs();
            paths = new string[gos.Count];
            for (int i = 0; i < gos.Count; i++) 
            {
                paths[i] = AssetDatabase.GetAssetPath(gos[i]);
            }
        }
        //GUILayout.Space(20);
        //if (GUILayout.Button("添加当前选中到列表", GUILayout.Width(200), GUILayout.Height(35)))
        //{
        //    paths = new string[0];
        //    gos = Selection.gameObjects;
        //    paths = new string[gos.Length];
        //    Debug.Log("添加选中预制体："+gos.Length);
        //    for (int i = 0;i<gos.Length;i ++)
        //    {
        //        paths[i] = ADYFX_Editor.GetPath(gos[i]); 
        //    }
        //}
        if (GUI.Button(new Rect(400,65,100,30),"清空列表"))
        {
            paths = new string[0];
            //gos = new GameObject[0];
            gos = new List<GameObject>();
            Debug.Log("清空列表");
        }
        //if (GUILayout.Button("清空列表并删除其源预制体", GUILayout.Width(200), GUILayout.Height(25)))
        //{
        //    for (int i = 0; i < gos.Length; i++)
        //    {
        //        AssetDatabase.DeleteAsset(paths[i]);
        //    }
        //    paths = new string[0];
        //    gos = new GameObject[0];
        //    Debug.Log("删除列表中的源预制体");
        //}
        GUILayout.Space(10);
        GUILayout.Label("----------------------点击生成  将对应特效生成到当前所选物体的子级-----------------------");
        mBeginScrollView = GUILayout.BeginScrollView(mBeginScrollView);//开始滚动视图、列表
        {
            if (gos.Count >= 1)
        {
            for (int i = 0; i < gos.Count; i++)
            {
                GUILayout.BeginHorizontal();//开始横向布局
                if (GUILayout.Button("-选中源-", GUILayout.Width(80), GUILayout.Height(25)))
                {
                        Object obj = AssetDatabase.LoadMainAssetAtPath(paths[i]);
                        Selection.activeObject = obj;
                        EditorGUIUtility.PingObject(obj);
                    }
                if (GUILayout.Button(("生成 "+gos[i].name), GUILayout.Width(400), GUILayout.Height(25)))
                {
                        value = i;
                        string nname = gos[i].name;
                        GameObject ady = Instantiate(gos[i]);//实例化物体
                        if (Selection.gameObjects.Length >= 1) 
                        {
                            ady.transform.parent = Selection.gameObjects[0].transform;
                        }
                        ady.transform.localPosition = new Vector3(0, 0, 0);
                        ady.name = nname;
                        Selection.activeObject = ady;
                    }
                    if (value == i)
                    {
                        GUILayout.Button("上次生成", GUILayout.Width(80), GUILayout.Height(25));
                    }
                GUILayout.EndHorizontal();//结束横向布局
            }
        }
        }
        GUILayout.EndScrollView();//结束滚动视图、列表
    }

    private List<GameObject> GetAllPrefabs()
    {
        List<GameObject> prefabs = new List<GameObject>();//预制体go数组
        var resourcesPath = Application.dataPath;
        string[] temp = targetPath.Split('/');
        string pp = resourcesPath;
        for (int i = 0;i< temp.Length;i++) 
        {
            if (temp[i]=="Assets")
            {

            }
            else 
            {
                pp = pp+ "/" + temp[i];
            }
        }
        Debug.Log(pp);
        var absolutePaths = System.IO.Directory.GetFiles(pp, "*.prefab", System.IO.SearchOption.AllDirectories);
        for (int i = 0; i < absolutePaths.Length; i++)
        {
            EditorUtility.DisplayProgressBar("获取预制体……", "获取预制体中……", (float)i / absolutePaths.Length);
            string path = "Assets" + absolutePaths[i].Remove(0, resourcesPath.Length);
            path = path.Replace("\\", "/");
            //Debug.LogError(path);
            GameObject prefab = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
            if (prefab != null)
            {
                prefabs.Add(prefab);
            }
            else 
            {
                            Debug.Log("预制体不存在！ " + path);
            }
        }
        EditorUtility.ClearProgressBar();
        return prefabs;
    }
}
