using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class ADYFX_SaveSkinMesh : EditorWindow
{
    public GameObject go;
    public Material bgmat;
    public Texture2D bgTex;
    public GameObject fuji;

    public List<SkinnedMeshRenderer> skinneds = new List<SkinnedMeshRenderer>();//子级所有渲染
    public Vector2 mBeginScrollView;
    private int xuhao = 0;
    [MenuItem("ADYFX/特效辅助/※截取角色动画网格工具", false, 1001)]
    static void RadialblurWindowcus()//菜单窗口
    {
        ADYFX_SaveSkinMesh window = EditorWindow.GetWindow<ADYFX_SaveSkinMesh>();//定义窗口类
        window.minSize = new Vector2(500, 750);//限制窗口最小值
        window.position = new Rect(400, 50, 1000, 800);
        window.titleContent = new GUIContent("取角色动画网格");//标题内容
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
        GUILayout.Label("场景中选择一个子级包含动作的角色的物体拖进来");
        var options = new[] { GUILayout.Width(250), GUILayout.Height(50) };//定义一个tex2d的宽高

        GUILayout.BeginHorizontal();
        go = EditorGUILayout.ObjectField(go, typeof(GameObject), true, options) as GameObject;//然后声明这个tex2d
        GUILayout.Space(100);
        if (skinneds.Count >= 1) 
        {
            if (GUILayout.Button("提取当前状态的网格到场景", GUILayout.Height(50)))
            {
                if (skinneds.Count >= 1)
                {
                    save();
                }
                else 
                {
                    Debug.Log("还没有执行查找 或者没有查找到子级渲染组件");
                }
            }
            if (GUILayout.Button("导出Mesh为FBX到Assets目录",GUILayout.Height(50)))
            {
                if (fuji)
                {
                    daochumesh(fuji.name, fuji);
                }
                else 
                {
                    Debug.LogError("草，提取的网格找不到了");
                }

            }
        }
        GUILayout.EndHorizontal();
        if (GUILayout.Button("查找这个物体的子级", GUILayout.Width(250), GUILayout.Height(30))) 
        {
            if (go) 
            {
                skinneds = new List<SkinnedMeshRenderer>();
                findsonSkin();
            }
        }
        GUILayout.Space(30);
        GUILayout.Label("查找到的渲染组件（SkinnedMeshRenderer）");
        mBeginScrollView = GUILayout.BeginScrollView(mBeginScrollView);//开始滚动视图、列表
        {
            if (skinneds.Count >= 1)
            {
                for (int i = 0; i < skinneds.Count; i++)
                {
                    if (GUILayout.Button(skinneds[i].name, GUILayout.Width(200))) 
                    {
                        ADYFX_Editor.SeleHierachyObj(skinneds[i].gameObject);
                    }
                }
            }
        }
        GUILayout.EndScrollView();//结束滚动视图、列表
    }
    void findsonSkin() 
    {
        SkinnedMeshRenderer[] gos = go.GetComponentsInChildren<SkinnedMeshRenderer>();
        for (int i = 0; i < gos.Length; i++)
        {
            skinneds.Add(gos[i]);
        }
    }
    void save()
    {

        GameObject ppp = new GameObject();
        ppp.transform.localPosition = new Vector3(0,0,0);
        ppp.transform.localEulerAngles = new Vector3(0, 0, 0);
        ppp.transform.localScale = new Vector3(1, 1, 1);
        ppp.name = "提取的网格" + "_" + xuhao;
        fuji = ppp;
        ADYFX_Editor.SeleHierachyObj(fuji);//创建后选中物体
        for (int i = 0; i < skinneds.Count; i++)
        {
            GameObject tx = new GameObject();
                tx.transform.parent = fuji.transform;
            tx.name = skinneds[i].name;
            tx.transform.localPosition = skinneds[i].gameObject.transform.localPosition;
            tx.transform.localEulerAngles = skinneds[i].gameObject.transform.localEulerAngles;
            tx.transform.localScale = skinneds[i].gameObject.transform.lossyScale;
            tx.AddComponent<MeshFilter>();
            tx.AddComponent<MeshRenderer>();
            MeshRenderer render = tx.gameObject.GetComponent<MeshRenderer>();
            render.sharedMaterials = skinneds[i].sharedMaterials;
            Mesh aa = new Mesh();
            aa.name = skinneds[i].name;
            skinneds[i].BakeMesh(aa);
            MeshFilter ff = tx.gameObject.GetComponent<MeshFilter>();
            ff.sharedMesh = aa;
        }
    }
    void daochumesh(string fbxname,GameObject Meshgameobj) 
    {
        GameObject[] aa = new GameObject[1];
        aa[0] = Meshgameobj;
        FBXExporter.ExportFBX("", fbxname, aa, true);
    }
}
