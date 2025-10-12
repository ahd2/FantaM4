using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class ADYFX_ParDelaySet : EditorWindow
{
    public Material bgmat;
    public Texture2D bgTex;
    public float delayoffset = 0;
    [MenuItem("ADYFX/特效辅助/※粒子系统批量设置Delay  _F8", false, 2002)]
    public static void ShowWindow()
    {
        ADYFX_ParDelaySet window = EditorWindow.GetWindow<ADYFX_ParDelaySet>();//定义窗口类
        window.minSize = new Vector2(250, 150);//限制窗口最小值
        //window.maxSize = new Vector2(1600, 920);//限制窗口最小值
        window.position = new Rect(1500, 300, 350, 300);
        window.titleContent = new GUIContent("粒子系统批量设置Delay增减");//标题内容
        window.Show();//创建窗口
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
        GUILayout.Label("将当前多选的粒子系统“StartDelay”在原基础上添加以下值\n（允许负值  但请注意粒子系统最低延迟为0）");
        delayoffset = EditorGUILayout.FloatField("StartDelay增量", delayoffset);
        if (GUILayout.Button("执行"))
        {
            GameObject[] go = Selection.gameObjects;
            List<ParticleSystem> pars = new List<ParticleSystem>();
            for (int i = 0; i < go.Length; i++)
            {
                if (go[i].GetComponent<ParticleSystem>())
                {
                    pars.Add(go[i].GetComponent<ParticleSystem>());

                }
            }
            for (int j = 0; j < pars.Count; j++)
            {
                //parmin[j].startDelay  =  delayoffset;
                     ParticleSystem.MainModule main;
                main = pars[j].main;
                main.startDelay =   new ParticleSystem.MinMaxCurve(main.startDelay.constant+ delayoffset);
            }
        }
    }
}
