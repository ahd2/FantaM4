using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
public class ADYFX_FindReferences : EditorWindow
{
    public Material bgmat;
    public Texture2D bgTex;
    Vector2 mBeginScrollView;
    Object sourceTex0;
    List<string> gos = new List<string>();

    [MenuItem("ADYFX/其他工具/※查找资源引用", false, 2501)]
    public static void Window1()//菜单窗口
    {
        ADYFX_FindReferences window = EditorWindow.GetWindow<ADYFX_FindReferences>();//定义窗口类
        //window.minSize = new Vector2(1200, 650);//限制窗口最小值
        //window.maxSize = new Vector2(1600, 920);//限制窗口最小值
        window.position = new Rect(50, 50, 1200, 650);
        window.titleContent = new GUIContent("查找资源引用");//标题内容
        window.Show();//创建窗口
    }
    private void OnEnable()
    {
        bgTex = new Texture2D(64, 64);
        bgmat = new Material(Shader.Find("ADYFX/Editer/BG"));
    }
    private void OnGUI()
    {
        bgTex = new Texture2D(64, 64);
        bgmat = new Material(Shader.Find("ADYFX/Editer/BG"));
        bgmat.SetFloat("_w", position.width / 30);// window.position.width/10
        bgmat.SetFloat("_h", position.height / 30);
        EditorGUI.DrawPreviewTexture(new Rect(0, 0, Screen.width, Screen.height), bgTex, bgmat);//绘制beijing
        GUILayout.Label("   拖入任意类型的资源以开始查找它被谁引用                                                                                                              结果列表（点击选中源文件）");
        var options = new[] { GUILayout.Width(250), GUILayout.Height(50) };//定义一个tex2d的宽高
        GUILayout.Space(5);
        GUILayout.Label("   如果工程巨大 整个查找过程将非常耗时 具体情况试电脑配置而异。");
        GUILayout.Space(5);
        EditorGUILayout.BeginHorizontal();//开始水平布局
        GUILayout.Space(17);
        sourceTex0 = EditorGUILayout.ObjectField(sourceTex0, typeof(Object), false, options) as Object;//然后声明这个tex2d
        if (GUILayout.Button("开始查找", GUILayout.Width(150), GUILayout.Height(50)))//特别设置按钮宽高
        {
            gos = new List<string>();
            GUI.enabled = false;
            Find1();
            GUI.enabled = true;
        }
        mBeginScrollView = GUILayout.BeginScrollView(mBeginScrollView);//开始滚动视图、列表
        {
            if (gos.Count >= 1)
            {
                for (int i = 0; i < gos.Count; i++)
                {
                    GUILayout.BeginHorizontal();//开始横向布局
                    if (GUILayout.Button(gos[i], GUILayout.Width(500), GUILayout.Height(25)))
                    {
                        ADYFX_Editor.SeleAssetsObj(gos[i]);
                    }
                    GUILayout.EndHorizontal();//结束横向布局
                }
            }
        }
        GUILayout.EndScrollView();//结束滚动视图、列表
        GUILayout.EndHorizontal();//结束横向布局
    }
    public void Find1()
    {
        EditorSettings.serializationMode = SerializationMode.ForceText;
        string path = AssetDatabase.GetAssetPath(sourceTex0);///获取选择物体的路径
        if (!string.IsNullOrEmpty(path))//判断路径是否为空
        {
            string guid = AssetDatabase.AssetPathToGUID(path);//获取guid
            List<string> withoutExtensions = new List<string>() { ".prefab", ".unity", ".mat", ".asset" };//声明一个数值 这是要判断的格式
            string[] files = Directory.GetFiles(Application.dataPath, "*.*", SearchOption.AllDirectories).Where(s => withoutExtensions.Contains(Path.GetExtension(s).ToLower())).ToArray();
            //                       读取目录中的文件   
            int startIndex = 0;

            EditorApplication.update = delegate ()
            {
                string file = files[startIndex];

                bool isCancel = EditorUtility.DisplayCancelableProgressBar("正在查找...", file, (float)startIndex / (float)files.Length);

                if (Regex.IsMatch(File.ReadAllText(file), guid))
                {
                    gos.Add(TransPath(file));
                    Debug.Log(file, AssetDatabase.LoadAssetAtPath<Object>(GetRelativeAssetsPath(file)));//打印物体名字并可以通过控制台点击框选到对应物体
                }

                startIndex++;
                if (isCancel || startIndex >= files.Length)
                {
                    EditorUtility.ClearProgressBar();
                    EditorApplication.update = null;
                    startIndex = 0;
                    Debug.Log("查找完成");
                }
            };
        }
    }
        [MenuItem("Assets/查找选中资源被谁引用了", true)]
        static private bool VFind()
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            return (!string.IsNullOrEmpty(path));
        }

        static private string GetRelativeAssetsPath(string path)
        {
        return "Assets" + Path.GetFullPath(path).Replace(Path.GetFullPath(Application.dataPath), "").Replace('\\', '/');
        }
    static private string TransPath(string path) 
    {
        string aa = "Assets";
        string[] sp = path.Split('\\');
        for (int i = 0;i<sp.Length;i++) 
        {
            if (i >=1) 
            {
                aa += "/";
                aa += sp[i];
            }
        }
        Debug.Log(aa);
            return aa;
    }
}

