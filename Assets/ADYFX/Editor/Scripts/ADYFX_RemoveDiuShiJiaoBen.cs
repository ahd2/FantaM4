using UnityEngine;
using UnityEditor;

public class RemoveMissingScriptsRecursively : EditorWindow
{
    public Material bgmat;
    public Texture2D bgTex;
    [MenuItem("ADYFX/特效辅助/※特效丢失脚本", false, 2020)]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(RemoveMissingScriptsRecursively));
    }
    private void OnEnable()
    {
        bgTex = new Texture2D(64, 64);
        bgmat = new Material(Shader.Find("ADYFX/Editer/BG"));
    }
    public void OnGUI()
    {
        bgTex = new Texture2D(64, 64);
        bgmat = new Material(Shader.Find("ADYFX/Editer/BG"));
        bgmat.SetFloat("_w", position.width / 30);// window.position.width/10
        bgmat.SetFloat("_h", position.height / 30);
        EditorGUI.DrawPreviewTexture(new Rect(0, 0, Screen.width, Screen.height), bgTex, bgmat);//绘制beijing
        if (GUILayout.Button("先选择预制体或场景物体  再点此按钮移除丢失脚本"))
        {
            RemoveInSelected();
        }
    }
    private static void RemoveInSelected()
    {
        GameObject[] go = Selection.gameObjects;
        foreach (GameObject g in go)
        {
            RemoveRecursively(g);
        }
    }

    private static void RemoveRecursively(GameObject g)
    {
        GameObjectUtility.RemoveMonoBehavioursWithMissingScript(g);

        foreach (Transform childT in g.transform)
        {
            RemoveRecursively(childT.gameObject);
        }
    }
}
