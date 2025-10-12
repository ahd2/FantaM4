using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ADYFX_Pintexiao : EditorWindow
{
    public Material bgmat;
    public Texture2D bgTex;
    public Vector2 mBeginScrollView;
    //public List<GameObject> gos = new List<GameObject>();
    public GameObject[] gos = new GameObject[0];
    public string[] paths = new string[0];
    public GameObject shoucang;
    public int value = 0;
    [MenuItem("ADYFX/特效辅助/※拼特效", false, 2100)]
    static void RadialblurWindowcus()//菜单窗口
    {
        ADYFX_Pintexiao window = EditorWindow.GetWindow<ADYFX_Pintexiao>();//定义窗口类
        //window.minSize = new Vector2(1200, 650);//限制窗口最小值
        //window.maxSize = new Vector2(1200, 650);//限制窗口最小值
        window.titleContent = new GUIContent("拼特效");//标题内容
        //window.position = new Rect(200, 60, 1200, 650);
        window.Show();//创建窗口
        //ADYFX_Editor.SetColorSpaceValue();
        //Debug.Log("当前色彩空间为" + PlayerSettings.colorSpace);
    }
    private void OnEnable()
    {
        bgTex = new Texture2D(64, 64);
        bgmat = new Material(Shader.Find("ADYFX/Editer/BG"));
    }
    void OnFocus()//当窗口获得焦点时调用一次
    {
        //StartWindow();//初始化窗口 获取所需资源
        if (GameObject.Find("shoucang"))
        {
            shoucang = GameObject.Find("shoucang");
        }
        else
        {
            shoucang = GameObject.CreatePrimitive(PrimitiveType.Cube);//类型
            shoucang.name = "shoucang";
            shoucang.transform.localScale = new Vector3(1, 1, 1);
            shoucang.transform.position = new Vector3(1, 1, 1);
            if (shoucang.GetComponent<Collider>())
            {
                GameObject.DestroyImmediate(shoucang.GetComponent<Collider>());
            }
            if (shoucang.GetComponent<Renderer>())
            {
                GameObject.DestroyImmediate(shoucang.GetComponent<Renderer>());
            }
            if (shoucang.GetComponent<MeshFilter>())
            {
                GameObject.DestroyImmediate(shoucang.GetComponent<MeshFilter>());
            }
        }

    }
    private void OnGUI()
    {
        bgTex = new Texture2D(64, 64);
        bgmat = new Material(Shader.Find("ADYFX/Editer/BG"));
        bgmat.SetFloat("_w", position.width / 30);// window.position.width/10
        bgmat.SetFloat("_h", position.height / 30);
        EditorGUI.DrawPreviewTexture(new Rect(0, 0, Screen.width, Screen.height), bgTex, bgmat);//绘制beijing
        GUILayout.Space(5);
        GUILayout.Label("先在库中多选特效预制体  再↓");
        GUILayout.Space(5);
        if (GUILayout.Button("添加当前选中的预制体、物体到列表", GUILayout.Width(200), GUILayout.Height(35)))
        {
            paths = new string[0];
            gos = Selection.gameObjects;
            paths = new string[gos.Length];
            Debug.Log("添加选中预制体："+gos.Length);
            for (int i = 0;i<gos.Length;i ++)
            {
                paths[i] = ADYFX_Editor.GetPath(gos[i]); 
            }
        }
        GUILayout.Space(10);
        if (GUILayout.Button("清空列表", GUILayout.Width(100), GUILayout.Height(25)))
        {
            paths = new string[0];
            gos = new GameObject[0];
            Debug.Log("清空列表");
        }
        GUILayout.Space(10);
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
        GUILayout.Label("运行后  点击按钮以查看特效");
        GUILayout.Space(5);

        mBeginScrollView = GUILayout.BeginScrollView(mBeginScrollView);//开始滚动视图、列表
        {
            if (gos.Length >= 1)
        {
            for (int i = 0; i < gos.Length; i++)
            {
                GUILayout.BeginHorizontal();//开始横向布局
                if (GUILayout.Button("选中源", GUILayout.Width(50), GUILayout.Height(25)))
                {
                    ADYFX_Editor.SeleAssetsObj(paths[i]);
                }
                if (GUILayout.Button(gos[i].name, GUILayout.Width(250), GUILayout.Height(25)))
                {
                        value = i;
                    if (GameObject.Find("AFX_Root"))
                    {
                        GameObject.DestroyImmediate(GameObject.Find("AFX_Root"));
                        var objCube = GameObject.CreatePrimitive(PrimitiveType.Cube);//类型
                        objCube.name = "AFX_Root";
                        objCube.transform.localScale = new Vector3(1, 1, 1);
                        objCube.transform.position = new Vector3(0, 0, 0);
                        if (objCube.GetComponent<Collider>())
                        {
                            GameObject.DestroyImmediate(objCube.GetComponent<Collider>());
                        }
                        if (objCube.GetComponent<Renderer>())
                        {
                            GameObject.DestroyImmediate(objCube.GetComponent<Renderer>());
                        }
                        if (objCube.GetComponent<MeshFilter>())
                        {
                            GameObject.DestroyImmediate(objCube.GetComponent<MeshFilter>());
                        }
                        GameObject ady = Instantiate(gos[i], new Vector3(0,0,0), Quaternion.identity);//实例化物体
                        ady.transform.parent = objCube.transform;
                        ady.transform.localPosition = new Vector3(0,0,0);
                            ady.SetActive(true);

                        }
                    else
                    {
                        var objCube = GameObject.CreatePrimitive(PrimitiveType.Cube);//类型
                        objCube.name = "AFX_Root";
                        objCube.transform.localScale = new Vector3(1, 1, 1);
                        objCube.transform.position = new Vector3(0, 0, 0);
                        if (objCube.GetComponent<Collider>())
                        {
                            GameObject.DestroyImmediate(objCube.GetComponent<Collider>());
                        }
                        if (objCube.GetComponent<Renderer>())
                        {
                            GameObject.DestroyImmediate(objCube.GetComponent<Renderer>());
                        }
                        if (objCube.GetComponent<MeshFilter>())
                        {
                            GameObject.DestroyImmediate(objCube.GetComponent<MeshFilter>());
                        }
                        GameObject ady = Instantiate(gos[i]);//实例化物体
                        ady.transform.parent = objCube.transform;
                        ady.transform.localPosition = new Vector3(0, 0, 0);
                            ady.SetActive(true);
                    }
                        Debug.Log("已实例化预制体");
                }
                if (GUILayout.Button("收藏", GUILayout.Width(50), GUILayout.Height(25)))
                {
                    GameObject ady = Instantiate(gos[i]);//实例化物体
                    ady.transform.parent = shoucang.transform;
                    ady.transform.localPosition = new Vector3(0, 0, 0);
                    Debug.Log("收藏");
                }
                    if (value == i)
                    {
                        GUILayout.Button("NOW", GUILayout.Width(40), GUILayout.Height(25));
                    }
                GUILayout.EndHorizontal();//结束横向布局
            }
        }
        }
        GUILayout.EndScrollView();//结束滚动视图、列表
    }
}
