using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
//dds转png  保持引用。1 自选多个图片拖入窗口，同时筛选掉其他格式 只留下dds在列表中，获得dds路径 获得其Guid并保存
//2 dds格式转换为png并等待Unity生成新的meta文件，
//3 列表中的路径.dds改为.png拿到新的文件的路径索引，根据路径拿到系统路径下对应的mate文件 并替换之前保存的GUID
public class ADYFX_ReplaceGUID : EditorWindow
{
    public Material bgmat;
    public Texture2D bgTex;
    public List<Texture2D> texs = new List<Texture2D>();
    public List<Texture2D> newtexs = new List<Texture2D>();
    public List<string> newtexmetas = new List<string>();
    public string[] texspaths = new string[0];
    public List<string> guids = new List<string>();
    public List<string> names = new List<string>();
    public List<string> yuanpaths = new List<string>();
    public List<string> newpaths = new List<string>();

    public Vector2 mBeginScrollView;
    public Vector2 mBeginScrollView1;

    public bool ison = true;
    [MenuItem("ADYFX/贴图工具/※dds保持资源引用转为png格式", false, 3022)]
    static void RadialblurWindowcus()//菜单窗口
    {
        ADYFX_ReplaceGUID window = EditorWindow.GetWindow<ADYFX_ReplaceGUID>();//定义窗口类
        window.titleContent = new GUIContent("dds保持资源引用转为png格式");//标题内容
        window.position = new Rect(200, 60, 700, 900);
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
        var options1 = new[] { GUILayout.Width(128), GUILayout.Height(64) };//定义一个tex2d的宽高
        GUILayout.Label("1 拖拽多个图像到窗口（自动筛选.dds）2 把列表内dds复制一份  3 使用复制出来的图像继承原GUID并删除原本的dds图像");
        if (GUILayout.Button("dds转一份png格式图像在源图旁边", GUILayout.Width(300), GUILayout.Height(30)))
        {
            ison = false;
            for (int i = 0; i < texs.Count; i++)
            {
                File.Copy(yuanpaths[i], newpaths[i], true);
                AssetDatabase.Refresh();
                string temp = newpaths[i] + ".meta";
                newtexmetas.Add(temp);
                Debug.Log(temp);
                string pp = ADYFX_Editor.GetPath(texs[i]);
                string[] sp = pp.Split('/');
                string myname = "";
                for (int z = 0; z < sp.Length; z++) 
                {
                    if (z <= sp.Length - 2)
                    {
                        if (z != 0)
                        {
                            myname = myname + "/" + sp[z];
                        }
                        else 
                        {
                            myname = myname  + sp[z];
                        }
                    }
      
                }
                myname = myname +"/"+ names[i];
                Debug.Log(myname);
                newtexs.Add(ADYFX_Editor.GetTex2D(myname) );
                AssetDatabase.Refresh();
            }
        }
        if (GUILayout.Button("使用复制出来的图像继承原GUID并删除原本的dds图像", GUILayout.Width(320), GUILayout.Height(30)))
        {
            for (int i = 0; i < newtexs.Count; i++)
            {
                AssetDatabase.Refresh();
                var content = File.ReadAllText(newtexmetas[i]);
                string newtexGuid = AssetDatabase.AssetPathToGUID(ADYFX_Editor.GetPath(newtexs[i]));
                Debug.Log("新的GUID:"+newtexGuid);
                content = content.Replace(newtexGuid, guids[i]);
                File.WriteAllText(newtexmetas[i], content);
                AssetDatabase.DeleteAsset(ADYFX_Editor.GetPath(texs[i]));
            }

        }
        if (GUILayout.Button("清空所有" ,GUILayout.Width(120), GUILayout.Height(30)))
        {
            texs = new List<Texture2D>();
            newtexs = new List<Texture2D>();
            newtexmetas = new List<string>();
            texspaths = new string[0];
            guids = new List<string>();
            names = new List<string>();
            yuanpaths = new List<string>();
            newpaths = new List<string>();
        }
        ison = GUILayout.Toggle(ison, "--允许通过拖拽资源库中的文件夹添加到列表--");
        GUILayout.Space(10);
        GUILayout.Label("以下列表仅预览和寻址   请不要改变它！        继承GUID操作执行之后 切到Unity之外的窗口 再切回Unity 等几秒 按CTRL+S保存一下");
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
                        texspaths = DragAndDrop.paths;
                        for (int i = 0; i < texspaths.Length; i++)
                        {
                            string temp = texspaths[i];

                            if (temp.Split('.')[1] == "dds")
                            {
                                texs.Add(ADYFX_Editor.GetTex2D(texspaths[i]));
                                guids.Add(AssetDatabase.AssetPathToGUID(texspaths[i]));

                                string[] split = texspaths[i].Split('/');//按反斜杠对字符串进行分割
                                string fileName = split[split.Length - 1];
                                fileName = fileName.Replace("dds","png");
                                string temp1 = fileName.Split('.')[0]+"_png"+"."+ fileName.Split('.')[1];
                                Debug.Log(temp1);
                                names.Add(temp1);

                                string yuanpath = texspaths[i].Replace("/",@"\");
                                //Debug.Log(yuanpath);
                                string targetPath = ADYFX_Editor.GetAssetsToWinPath(yuanpath);// Path.Combine(ADYFX_Editor.GetAssetsToWinPath(yuanpath), savename + "." + fengename[1]);
                                yuanpaths.Add(targetPath);
                                Debug.Log(targetPath);
                                string sourcePath = "";
                                string[] newpath = texspaths[i].Split('/');
                                string aaa = newpath[newpath.Length - 1];
                                for (int z = 0; z < newpath.Length; z++)
                                {
                                    if (z != newpath.Length - 1)
                                    {
                                        if (z <= newpath.Length - 2)
                                        {
                                            sourcePath += newpath[z] + @"\";
                                        }
                                        else
                                        {
                                            sourcePath += newpath[z];
                                        }
                                    }
                                }
                                string newpath1 = ADYFX_Editor.GetAssetsToWinPath(sourcePath + temp1) ;
                                Debug.Log("新路径"+newpath1);
                                newpaths.Add(newpath1);
                                //Debug.Log(AssetDatabase.AssetPathToGUID(texspaths[i]));
                            }
                        }
                    }
                }
            }
        }

        GUILayout.BeginHorizontal();//开始横向布局
        mBeginScrollView = GUILayout.BeginScrollView(mBeginScrollView);//开始滚动视图、列表
        {
                for (int i = 0; i < texs.Count; i++)
                {
                    GUILayout.BeginHorizontal();//开始横向布局
                    texs[i] = EditorGUILayout.ObjectField(texs[i], typeof(Texture2D), false, options1) as Texture2D;//然后声明这个tex2d
                    GUILayout.Label("GUID:", GUILayout.Width(50), GUILayout.Height(20));
                    GUILayout.Label(guids[i], GUILayout.Width(300), GUILayout.Height(20));
                    GUILayout.EndHorizontal();//结束横向布局
                }
        }
        GUILayout.EndScrollView();//结束滚动视图、列表

        mBeginScrollView1 = GUILayout.BeginScrollView(mBeginScrollView1);//开始滚动视图、列表
        {
            for (int i = 0; i < newtexs.Count; i++)
            {
                GUILayout.BeginHorizontal();//开始横向布局
                newtexs[i] = EditorGUILayout.ObjectField(newtexs[i], typeof(Texture2D), false, options1) as Texture2D;//然后声明这个tex2d
                GUILayout.Label("已生成png", GUILayout.Width(70), GUILayout.Height(20));
                //GUILayout.Label(guids[i], GUILayout.Width(300), GUILayout.Height(20));
                GUILayout.EndHorizontal();//结束横向布局
            }
        }
        GUILayout.EndScrollView();//结束滚动视图、列表
        GUILayout.EndHorizontal();//结束横向布局
    }
}
