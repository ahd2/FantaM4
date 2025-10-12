using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ADYFX_ImportSettings : EditorWindow
{
    public Material bgmat;
    public Texture2D bgTex;
    public enum ImportYemian//枚举
    {
        Window01,
        Window02
    }
    public ImportYemian yemian;
    public enum Clamp//枚举
    {
        isrepeat,
        isclamp,
        none
    }
    public Clamp clamp = Clamp.none;
    public enum Filter//枚举
    {
        point,
        bili,
        pr,
        none
    }
    public Filter filter = Filter.none;
    Vector2 mBeginScrollView;
    Vector2 mBeginScrollView1;
    GameObject[] addgo = new GameObject[0];
    Texture2D[] addgo1 = new Texture2D[0];
    int addgosize = 0;
    int addgosize1 = 0;
    bool isremove = false;
    Object[] texs = new Object[0];
    Object[]  gos  = new Object[0];
    GUIStyle style20 = new GUIStyle();

    bool isblendShapes = false;
    bool isvisbility = false;
    bool iscam = true;
    bool islights = true;
    bool animatype = false;
    bool isanima = false;
    bool ismat = false;
    bool isduxie = false;
    List<GameObject> models = new List<GameObject>();
    List<ModelImporter> modelImporters = new List<ModelImporter>();
    List<string> modelStrs = new List<string>();
    bool isalphatotra = false;
    bool ispower2 = true;
    bool isminmap = true;
    List<Texture2D> texture2Ds = new List<Texture2D>();

    List<TextureImporter> textureImporters = new List<TextureImporter>();
    List<string> texStrs = new List<string>();

    [MenuItem("ADYFX/其他工具/※批量优化贴图、模型的导入设置", false, 2503)]
    public static void HelpWindow()//菜单窗口
    {
        ADYFX_ImportSettings window = EditorWindow.GetWindow<ADYFX_ImportSettings>();//定义窗口类
        window.minSize = new Vector2(1200, 650);//限制窗口最小值
        window.maxSize = new Vector2(1600, 920);//限制窗口最小值
        window.position = new Rect(50, 50, 1200, 650);
        window.titleContent = new GUIContent("优化贴图、模型的导入设置");//标题内容
        window.Show();//创建窗口
    }
    private void OnEnable()
    {
        bgTex = new Texture2D(64, 64);
        bgmat = new Material(Shader.Find("ADYFX/Editer/BG"));
    }
    public void Window01()
    {
        bgTex = new Texture2D(64, 64);
        bgmat = new Material(Shader.Find("ADYFX/Editer/BG"));
        bgmat.SetFloat("_w", position.width / 30);// window.position.width/10
        bgmat.SetFloat("_h", position.height / 30);
        EditorGUI.DrawPreviewTexture(new Rect(0, 0, Screen.width, Screen.height), bgTex, bgmat);//绘制beijing
        addgosize = addgo.Length;
        GUI.Label(new Rect(500, 10, 400, 50), "待处理列表", style20);
        if (mouseOverWindow == this)
        {//鼠标位于当前窗口
            if (Event.current.type == EventType.DragUpdated)
            {//拖入窗口未松开鼠标
                DragAndDrop.visualMode = DragAndDropVisualMode.Generic;//改变鼠标外观
            }
            else if (Event.current.type == EventType.DragExited)
            {//拖入窗口并松开鼠标
                Focus();//获取焦点，使unity置顶(在其他窗口的前面)
                if (DragAndDrop.paths != null)
                {
                    GameObject[] temp = addgo;
                    addgo = new GameObject[DragAndDrop.paths.Length + addgosize];
                    for (int z = 0; z < addgosize; z++)
                    {
                        addgo[z] = temp[z];
                    }
                    for (int i = 0; i < DragAndDrop.paths.Length; i++)
                    {
                        addgo[i + addgosize] = ADYFX_Editor.GetGO(DragAndDrop.paths[i]);
                    }
                }
            }
        }
        //EditorGUILayout.BeginHorizontal();//开始水平布局
        GUILayout.Space(20);
        //sourceTex0 = EditorGUILayout.ObjectField(sourceTex0, typeof(Object), false, options) as Object;//然后声明这个tex2d
        mBeginScrollView = GUILayout.BeginScrollView(mBeginScrollView);//开始滚动视图、列表
        {
            GUILayout.Space(20);
            isblendShapes = GUILayout.Toggle(isblendShapes, "不导入骨骼、变形器信息", GUILayout.Width(500), GUILayout.Height(30));//特别设置按钮宽高
            isvisbility = GUILayout.Toggle(isvisbility, "不导入可见性设置", GUILayout.Width(500), GUILayout.Height(30));//特别设置按钮宽高
            iscam = GUILayout.Toggle(iscam, "不导入摄像机", GUILayout.Width(500), GUILayout.Height(30));//特别设置按钮宽高
            islights = GUILayout.Toggle(islights, "不导入灯光", GUILayout.Width(500), GUILayout.Height(30));//特别设置按钮宽高
            animatype = GUILayout.Toggle(animatype, "设置Rig>动画标签为“None”", GUILayout.Width(500), GUILayout.Height(30));//特别设置按钮宽高
            isanima = GUILayout.Toggle(isanima, "不导入动画", GUILayout.Width(500), GUILayout.Height(30));//特别设置按钮宽高
            ismat = GUILayout.Toggle(ismat, "不导入材质", GUILayout.Width(500), GUILayout.Height(30));//特别设置按钮宽高
            isduxie = GUILayout.Toggle(isduxie, "关闭模型的“允许读写”", GUILayout.Width(500), GUILayout.Height(30));//特别设置按钮宽高
            GUILayout.Label("-----------以上内容未勾选的会保持原本的设置-----------");
           
        }
        GUILayout.EndScrollView();//结束滚动视图、列表

        if (addgo.Length < 1)
        {
            GUI.Label(new Rect(500, 55, 400, 50), "※ 拖拽模型到窗口以添加 ※\n-- 不要添加模型以外的资源  否则无法运行！--", style20);
        }
        else
        {
            EditorGUILayout.BeginHorizontal();//开始水平布局
            GUILayout.Space(10);
            mBeginScrollView1 = GUILayout.BeginScrollView(mBeginScrollView1);//开始滚动视图、列表
            {
                if (addgo.Length >= 1)
                {
                    for (int i = 0; i < addgo.Length; i++)
                    {
                        GUILayout.BeginHorizontal();//开始横向布局
                        if (GUILayout.Button(addgo[i].name, GUILayout.Width(300), GUILayout.Height(25)))
                        {
                            ADYFX_Editor.SeleAssetsObj(ADYFX_Editor.GetPath(addgo[i]));
                        }
                        GUILayout.EndHorizontal();//结束横向布局
                    }
                }
            }
            GUILayout.EndScrollView();//结束滚动视图、列表
            GUILayout.EndHorizontal();//结束横向布局
        }

        isremove = GUI.Toggle(new Rect(970, 28, 200, 30), isremove, "执行优化后清空当前列表");
        if (GUI.Button(new Rect(970, 70, 200, 35), "清空列表"))//特别设置按钮宽高
        {
            addgo = new GameObject[0];
            modelStrs = new List<string>();
            modelImporters = new List<ModelImporter>();
        }
        if (GUI.Button(new Rect(970, 120, 200, 50), "开始优化"))//特别设置按钮宽高
        {
            Addmesh();
            Setmesh();
            if (isremove)
            {
                addgo = new GameObject[0];
                modelStrs = new List<string>();
                modelImporters = new List<ModelImporter>();
            }
        }
    }
    public void Window02()
    {
        addgosize1 = addgo1.Length;
        GUI.Label(new Rect(500, 10, 400, 50), "待处理列表", style20);
        if (mouseOverWindow == this)
        {//鼠标位于当前窗口
            if (Event.current.type == EventType.DragUpdated)
            {//拖入窗口未松开鼠标
                DragAndDrop.visualMode = DragAndDropVisualMode.Generic;//改变鼠标外观
            }
            else if (Event.current.type == EventType.DragExited)
            {//拖入窗口并松开鼠标
                Focus();//获取焦点，使unity置顶(在其他窗口的前面)
                if (DragAndDrop.paths != null)
                {
                    Texture2D[] temp = addgo1;
                    addgo1 = new Texture2D[DragAndDrop.paths.Length + addgosize1];
                    for (int z = 0; z < addgosize1; z++)
                    {
                        addgo1[z] = temp[z];
                    }
                    for (int i = 0; i < DragAndDrop.paths.Length; i++)
                    {
                        addgo1[i + addgosize1] = ADYFX_Editor.GetTex2D(DragAndDrop.paths[i]);
                    }
                }
            }
        }

        //EditorGUILayout.BeginHorizontal();//开始水平布局
        GUILayout.Space(20);
        //sourceTex0 = EditorGUILayout.ObjectField(sourceTex0, typeof(Object), false, options) as Object;//然后声明这个tex2d
        mBeginScrollView = GUILayout.BeginScrollView(mBeginScrollView);//开始滚动视图、列表
        {
            GUILayout.Label("-----------将Png图像的透明像素设为Alpha通道-----------\n           不勾选则使用原本设置");
            isalphatotra = GUILayout.Toggle(isalphatotra, "打开“透明像素转换为Alpha通道”（Alpha is Transparency）", GUILayout.Width(500), GUILayout.Height(20));//特别设置按钮宽高
            GUILayout.Space(30);
            GUILayout.Label("-----------关闭Unity对非2次幂尺寸的图像的缩放校正-----------\n           不勾选则使用原本设置");
            ispower2 = GUILayout.Toggle(ispower2, "关闭“非2次幂图像缩放”（Non-Power of2设为None）", GUILayout.Width(500), GUILayout.Height(20));//特别设置按钮宽高
            GUILayout.Space(30);
            GUILayout.Label("----如果不关闭Generate Mip Maps选项Unity会生成3张低清晰度的小图----\n用以在此贴图距离摄像机较远时使用  但这会增加贴图的空间占用\n           不勾选则使用原本设置");
            isminmap = GUILayout.Toggle(isminmap, "关闭Generate Mip Maps", GUILayout.Width(500), GUILayout.Height(20));//特别设置按钮宽高
            GUILayout.Space(30);
            GUILayout.Label("-----------贴图重铺设置 Repeat是重铺  Clamp是不重铺-----------\n（具体体现在材质的贴图的tiling值超过1时是继续重铺还是重复边缘像素）");
            clamp = (Clamp)GUILayout.SelectionGrid((int)clamp, new string[] { "Repeat", "Clamp" ,"忽略"}, 3, GUILayout.Width(350), GUILayout.Height(25));
            GUILayout.Space(30);
            GUILayout.Label("-----------贴图像素的插值  默认是Bilinear（临近像素插值）-----------\n Point（完全不插值 适合像素风或者极小的规则的形状贴图使用）");
            filter = (Filter)GUILayout.SelectionGrid((int)filter, new string[] { "Point", "Bilinear", "Trilinear","忽略" }, 4, GUILayout.Width(350), GUILayout.Height(25));

        }
        GUILayout.EndScrollView();//结束滚动视图、列表

        if (addgo1.Length < 1)
        {
            GUI.Label(new Rect(500, 55, 400, 50), "※ 拖拽贴图到窗口以添加 ※\n-- 不要添加贴图以外的资源  否则无法运行！--", style20);
        }
        else
        {
            EditorGUILayout.BeginHorizontal();//开始水平布局
            GUILayout.Space(10);
            mBeginScrollView1 = GUILayout.BeginScrollView(mBeginScrollView1);//开始滚动视图、列表
            {
                if (addgo1.Length >= 1)
                {
                    for (int i = 0; i < addgo1.Length; i++)
                    {
                        GUILayout.BeginHorizontal();//开始横向布局
                        if (GUILayout.Button(addgo1[i].name, GUILayout.Width(300), GUILayout.Height(25)))
                        {
                            ADYFX_Editor.SeleAssetsObj(ADYFX_Editor.GetPath(addgo1[i]));
                        }
                        GUILayout.EndHorizontal();//结束横向布局
                    }
                }
            }
            GUILayout.EndScrollView();//结束滚动视图、列表
            GUILayout.EndHorizontal();//结束横向布局
        }

        isremove = GUI.Toggle(new Rect(970, 28, 200, 30), isremove, "执行优化后清空当前列表");
        if (GUI.Button(new Rect(970, 70, 200, 35), "清空列表"))//特别设置按钮宽高
        {
            addgo1 = new Texture2D[0];
            texStrs = new List<string>();
            textureImporters = new List<TextureImporter>();
        }
        if (GUI.Button(new Rect(970, 120, 200, 50), "开始优化"))//特别设置按钮宽高
        {
            Addtex();
            SetTex();
            if (isremove)
            {
                addgo1 = new Texture2D[0];
                texStrs = new List<string>();
                textureImporters = new List<TextureImporter>();
            }
        }
    }
    void Addmesh()//添加选中的物体
    {
        for (int i = 0; i < addgo.Length; i++)
        {
            GameObject mod = addgo[i] as GameObject;
            string path = AssetDatabase.GetAssetPath(mod);
            modelStrs.Add(path);
            ModelImporter meshsh = ModelImporter.GetAtPath(path) as ModelImporter;
            modelImporters.Add(meshsh);
        }
    }
    void Setmesh() 
    {
        for (int i = 0; i < modelImporters.Count; i++)
        {
            if (isblendShapes)
            {
                modelImporters[i].importBlendShapes = false;
            }

            if (isvisbility)
            {
                modelImporters[i].importVisibility = false;
            }

            if (iscam)
            {
                modelImporters[i].importCameras = false;
            }

            if (islights)
            {
                modelImporters[i].importLights = false;
            }

            if (animatype)
            {
                modelImporters[i].animationType = ModelImporterAnimationType.None;
            }

            if (isanima)
            {
                modelImporters[i].importAnimation = false;
            }

            if (ismat)
            {
                modelImporters[i].materialImportMode = ModelImporterMaterialImportMode.None;
            }

            if (isduxie) 
            {
                modelImporters[i].isReadable = false;
            }
            //modelImporters[i].swapUVChannels = false;
            AssetDatabase.ImportAsset(modelStrs[i]);
        }
        AssetDatabase.Refresh();
    }
    void Addtex() 
    {
        foreach (Texture2D texture in texs)
        {
            texture2Ds.Add(texture);
        }
        for (int i = 0; i < addgo1.Length; i++)
        {
            Texture2D mod = addgo1[i] as Texture2D;
            string path = AssetDatabase.GetAssetPath(mod);
            texStrs.Add(path);
            TextureImporter tex = ModelImporter.GetAtPath(path) as TextureImporter;
            textureImporters.Add(tex);
        }
    }
    void SetTex() 
    {
        for (int i = 0; i < textureImporters.Count; i++)
        {
           string [] aa = texStrs[i].Split('.');
            if (aa[aa.Length-1] == "dds"|| aa[aa.Length - 1] == "DDS")
            {
                Debug.Log(texStrs[i]+"是dds格式的贴图  不存在对应导入设置，已跳过它。");
            }
            else 
            {
                textureImporters[i].isReadable = true;
                if (isalphatotra)
                {
                    textureImporters[i].alphaIsTransparency = true;
                }
                if (ispower2)
                {
                    textureImporters[i].npotScale = TextureImporterNPOTScale.None;
                }
                if (isminmap)
                {
                    textureImporters[i].mipmapEnabled = false;
                }

                if (clamp == Clamp.isrepeat)
                {
                    textureImporters[i].wrapMode = TextureWrapMode.Repeat;
                }
                if (clamp == Clamp.isclamp)
                {
                    textureImporters[i].wrapMode = TextureWrapMode.Clamp;
                }

                if (filter == Filter.point)
                {
                    textureImporters[i].filterMode = FilterMode.Point;
                }
                if (filter == Filter.bili)
                {
                    textureImporters[i].filterMode = FilterMode.Bilinear;
                }
                if (filter == Filter.pr)
                {
                    textureImporters[i].filterMode = FilterMode.Trilinear;
                }
                textureImporters[i].isReadable = false;
                AssetDatabase.ImportAsset(texStrs[i]);
            }
        }
        AssetDatabase.Refresh();
    }
    private void OnGUI()
    {
        bgmat.SetFloat("_w", position.width / 30);// window.position.width/10
        bgmat.SetFloat("_h", position.height / 30);
        EditorGUI.DrawPreviewTexture(new Rect(0, 0, Screen.width, Screen.height), bgTex, bgmat);//绘制beijing
        style20.alignment = TextAnchor.MiddleCenter;//文本锚点
        style20.fontSize = 20;//文字大小
        style20.normal.textColor = new Color(0.65f, 0.65f, 0.65f, 1);//文字颜色
        GUILayout.Space(15);
        yemian = (ImportYemian)GUILayout.SelectionGrid((int)yemian, new string[] { "批量优化模型导入设置", "批量优化贴图导入设置", }, 2, GUILayout.Width(300), GUILayout.Height(40));
        if (yemian == ImportYemian.Window01)
        {
            Window01();
        }
        else if ((yemian == ImportYemian.Window02))
        {
            Window02();
        }
    }
    //private Object[] GetSelectedTextures()
    //{
    //    return Selection.GetFiltered(typeof(Texture2D), SelectionMode.DeepAssets);
    //}
    //private Object[] GetSelectedGameObj()
    //{
    //    return Selection.GetFiltered(typeof(GameObject), SelectionMode.DeepAssets);
    //}
}
