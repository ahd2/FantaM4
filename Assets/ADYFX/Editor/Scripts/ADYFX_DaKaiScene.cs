using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
public class ADYFX_DaKaiScene : EditorWindow
{
    public Material bgmat;
    public Texture2D bgTex;
    public Vector2 mBeginScrollView;
    public ADYFX_Common_Assets assets ;
    GUIStyle style20 = new GUIStyle();
    [MenuItem("ADYFX/快速打开场景", false, 4899)]
    static void RadialblurWindowcus()//菜单窗口
    {
        ADYFX_DaKaiScene window = EditorWindow.GetWindow<ADYFX_DaKaiScene>();
        window.minSize = new Vector2(100, 100);//限制窗口最小值
        window.position = new Rect(100, 100, 600, 600);
        window.titleContent = new GUIContent("快速打开场景");//标题
        window.Show();//创建窗口
    }
    void OnFocus()//当窗口获得焦点时调用一次
    {
        style20.alignment = TextAnchor.MiddleCenter;//文本锚点
        style20.fontSize = 20;//文字大小
        style20.normal.textColor = new Color(1, 1f, 1f, 1);//文字颜色
        assets = ADYFX_Editor.GetOBJ("e3a60b1ac6ac9194289c0a6f7c29544f", true) as ADYFX_Common_Assets;
    }
    private void OnEnable()
    {
        assets = ADYFX_Editor.GetOBJ("e3a60b1ac6ac9194289c0a6f7c29544f", true) as ADYFX_Common_Assets;
        bgTex = new Texture2D(64, 64);
        bgmat = new Material(Shader.Find("ADYFX/Editer/BG"));

    }
    private void OnDestroy()
    {
        AssetDatabase.DeleteAsset(AssetDatabase.GUIDToAssetPath("e3a60b1ac6ac9194289c0a6f7c29544f")); //关闭窗口时删除旧配置（重新生成配置文件，否则关闭引擎将丢失此次打开引擎之后的修改）
        ADYFX_Common_Assets level = ScriptableObject.CreateInstance<ADYFX_Common_Assets>();//不刷新库 创建新的配置以继承旧配置的guid
        level.strs1 = assets.strs1;
        AssetDatabase.CreateAsset(level, AssetDatabase.GUIDToAssetPath("e3a60b1ac6ac9194289c0a6f7c29544f"));
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();//创建完成后刷新
    }
    private void OnGUI()
    {
        bgTex = new Texture2D(64, 64);
        bgmat = new Material(Shader.Find("ADYFX/Editer/BG"));
        bgmat.SetFloat("_w", position.width / 30);// window.position.width/10
        bgmat.SetFloat("_h", position.height / 30);
        EditorGUI.DrawPreviewTexture(new Rect(0, 0, Screen.width, Screen.height), bgTex, bgmat);//绘制beijing
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
                    assets.strs1.Add(AssetDatabase.AssetPathToGUID(DragAndDrop.paths[0]) );
                    List<string> temp = assets.strs1;
                    AssetDatabase.DeleteAsset(AssetDatabase.GUIDToAssetPath("e3a60b1ac6ac9194289c0a6f7c29544f")); //关闭窗口时删除旧配置（重新生成配置文件，否则关闭引擎将丢失此次打开引擎之后的修改）
                    ADYFX_Common_Assets level = ScriptableObject.CreateInstance<ADYFX_Common_Assets>();//不刷新库 创建新的配置以继承旧配置的guid
                    level.strs1 = temp;
                    AssetDatabase.CreateAsset(level, AssetDatabase.GUIDToAssetPath("e3a60b1ac6ac9194289c0a6f7c29544f"));
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();//创建完成后刷新
                    assets = ADYFX_Editor.GetOBJ("e3a60b1ac6ac9194289c0a6f7c29544f", true) as ADYFX_Common_Assets;
                }
            }
        }
        GUILayout.Label("拖拽场景文件到本窗口 即可添加打开对应场景的按钮", style20);
        mBeginScrollView = GUILayout.BeginScrollView(mBeginScrollView);//开始滚动视图、列表
        {
            if (assets.strs1.Count >= 1)
            {
                for (int i = 0;i< assets.strs1.Count; i++) 
                {
                    GUILayout.BeginHorizontal();
                    Object aa = ADYFX_Editor.GetOBJ(AssetDatabase.GUIDToAssetPath(assets.strs1[i]));
                    if (GUILayout.Button("打开 : " + aa .name+ "场景", GUILayout.Height(30)))
                    {
                        ADYFX_Editor.SaveScene();
                        AssetDatabase.SaveAssets();
                        ADYFX_Editor.OpenScene(AssetDatabase.GUIDToAssetPath(assets.strs1[i]));
                    }
                    if (GUILayout.Button("删除此按钮", GUILayout.Width(80), GUILayout.Height(30)))
                    {
                        assets.strs1.RemoveAt(i);
                    }
                    GUILayout.EndHorizontal();
                }
            }
            else 
            {

            }


        }
        GUILayout.EndScrollView();//结束滚动视图、列表
    }

}

