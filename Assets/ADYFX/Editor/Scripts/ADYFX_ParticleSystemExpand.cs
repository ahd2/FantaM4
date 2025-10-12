using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
[CustomEditor(typeof(ParticleSystem))]
public class ADYFX_ParticleSystemExpand : DecoratorEditor//: Editor
{
    public ADYFX_Common_Assets assets;
    public bool isbtn = true;
//    private string ButtonStyleName
//#if UNITY_2018
//                 = "ScriptText";
//#else
//                  = "AnimClipToolbarPopup";
//#endif
    private ParticleSystem thisParticleSystem;//自身的粒子系统
    public ADYFX_ParticleSystemExpand() : base("ParticleSystemInspector") { }
    private void OnEnable()
    {
        thisParticleSystem = target as ParticleSystem;
        assets = ADYFX_Editor.GetOBJ("fa5defd4fa471364180b027e3d2b8c80", true) as ADYFX_Common_Assets;
        if (assets.strs1[0] == "1")
        {
            isbtn = true;
        }
        else 
        {
            isbtn = false;
        }
    }

    public override void OnInspectorGUI()
    {
        ///重置Transform
        GUIUpdate_ParticleTransform();
        base.OnInspectorGUI();

        serializedObject.Update();
        //......//
        serializedObject.ApplyModifiedProperties();
    }
    private void GUIUpdate_ParticleTransform()
    {
        //using (new GUILayout.HorizontalScope("box"))
        //{
            if (isbtn) 
            {
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
                GUILayout.Label("重置:", GUILayout.Width(32), GUILayout.Height(20));
                if (GUILayout.Button("位", GUILayout.Width(25), GUILayout.Height(20)))
                {
                    thisParticleSystem.transform.localPosition = Vector3.zero;
                }
                if (GUILayout.Button("旋", GUILayout.Width(25), GUILayout.Height(20)))
                {
                    thisParticleSystem.transform.localEulerAngles = new Vector3(0, 0, 0);
                }
                if (GUILayout.Button("缩", GUILayout.Width(25), GUILayout.Height(20)))
                {
                    thisParticleSystem.transform.localScale = Vector3.one;
                }
                GUILayout.Space(5);
            GUILayout.Label("清空:", GUILayout.Width(32), GUILayout.Height(20));
            if (GUILayout.Button("粒子材质", GUILayout.Width(60), GUILayout.Height(20)))
                {
                    ParticleSystemRenderer parrender = thisParticleSystem.gameObject.GetComponent<ParticleSystemRenderer>();
                    parrender.material = null;
                }
                if (GUILayout.Button("拖尾材质", GUILayout.Width(60), GUILayout.Height(20)))
                {
                    ParticleSystemRenderer parrender = thisParticleSystem.gameObject.GetComponent<ParticleSystemRenderer>();
                    parrender.trailMaterial = null;
                }
                if (GUILayout.Button("Mesh", GUILayout.Width(44), GUILayout.Height(20)))
                {
                    ParticleSystemRenderer parrender = thisParticleSystem.gameObject.GetComponent<ParticleSystemRenderer>();
                    parrender.mesh = null;
                }
            GUILayout.Space(20);
            if (GUILayout.Button("优化", GUILayout.Width(40), GUILayout.Height(20)))
                {
                    ParticleSystem.MainModule main = thisParticleSystem.main;
                    main.scalingMode = ParticleSystemScalingMode.Hierarchy;
                    Debug.Log("当前粒子系统 scalingMode =" + ParticleSystemScalingMode.Hierarchy);
                    main.maxParticles = 50;
                    Debug.Log("当前粒子系统 maxParticles =50");
                    main.playOnAwake = true;
                    Debug.Log("当前粒子系统 playOnAwake = √");

                }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("开启:", GUILayout.Width(34), GUILayout.Height(20));
            if (GUILayout.Button("拖尾", GUILayout.Width(40), GUILayout.Height(20)))
            {
                KaiTrail();
            }
            if (GUILayout.Button("星链", GUILayout.Width(40), GUILayout.Height(20)))
            {
                KaiRibbnon();
            }
            GUILayout.Label("翻转UV:", GUILayout.Width(48), GUILayout.Height(20));
            if (GUILayout.Button("横", GUILayout.Width(25), GUILayout.Height(20)))
            {
                ParticleSystemRenderer parrender = thisParticleSystem.gameObject.GetComponent<ParticleSystemRenderer>();
                float tt = parrender.flip.x;
                if (tt == 0)
                {
                    tt = 1;
                }
                else 
                {
                    tt = 0;
                }
                parrender.flip = new Vector3(tt, parrender.flip.y, parrender.flip.z);
            }
            if (GUILayout.Button("竖", GUILayout.Width(25), GUILayout.Height(20)))
            {
                ParticleSystemRenderer parrender = thisParticleSystem.gameObject.GetComponent<ParticleSystemRenderer>();
                float tt = parrender.flip.y;
                if (tt == 0)
                {
                    tt = 1;
                }
                else
                {
                    tt = 0;
                }
                parrender.flip = new Vector3(parrender.flip.x,tt , parrender.flip.z);
            }
            GUILayout.Space(10);
            if (GUILayout.Button("横  随机", GUILayout.Width(55), GUILayout.Height(20)))
            {
                ParticleSystemRenderer parrender = thisParticleSystem.gameObject.GetComponent<ParticleSystemRenderer>();
                parrender.flip = new Vector3(0.5f, parrender.flip.y, parrender.flip.z);
            }
            if (GUILayout.Button("竖  随机", GUILayout.Width(55), GUILayout.Height(20)))
            {
                ParticleSystemRenderer parrender = thisParticleSystem.gameObject.GetComponent<ParticleSystemRenderer>();
                parrender.flip = new Vector3(parrender.flip.x, 0.5f, parrender.flip.z);
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("自身粒子排序:", GUILayout.Width(80), GUILayout.Height(20));
            if (GUILayout.Button("新的在前", GUILayout.Width(62), GUILayout.Height(20)))
            {
                ParticleSystemRenderer parrender = thisParticleSystem.gameObject.GetComponent<ParticleSystemRenderer>();
                parrender.sortMode = ParticleSystemSortMode.YoungestInFront;
            }
            if (GUILayout.Button("老的在前", GUILayout.Width(62), GUILayout.Height(20)))
            {
                ParticleSystemRenderer parrender = thisParticleSystem.gameObject.GetComponent<ParticleSystemRenderer>();
                parrender.sortMode = ParticleSystemSortMode.OldestInFront;
            }
            if (GUILayout.Button("距离排序", GUILayout.Width(62), GUILayout.Height(20)))
            {
                ParticleSystemRenderer parrender = thisParticleSystem.gameObject.GetComponent<ParticleSystemRenderer>();
                parrender.sortMode = ParticleSystemSortMode.Distance;
            }

            GUILayout.EndHorizontal();

            GUILayout.Space(-24);
            GUILayout.EndVertical();
        }
        //}
    }
    [MenuItem("CONTEXT/ParticleSystem/启用或关闭粒子系统扩展 (点选不同物体后刷新面板)")]
    static public void CustomData001(MenuCommand cmd)//在其他组件上扩展，MenuCommand是获取其他组件，这个值是unity传的 根据组件类型获取对应组件
    {
        bool isbtn;
        ADYFX_Common_Assets assets = ADYFX_Editor.GetOBJ("fa5defd4fa471364180b027e3d2b8c80", true) as ADYFX_Common_Assets;
        if (assets.strs1[0] == "1")
        {
            isbtn = true;
        }
        else
        {
            isbtn = false;
        }
        isbtn = !isbtn;
        AssetDatabase.DeleteAsset(AssetDatabase.GUIDToAssetPath("fa5defd4fa471364180b027e3d2b8c80")); //关闭窗口时删除旧配置（重新生成配置文件，否则关闭引擎将丢失此次打开引擎之后的修改）
        ADYFX_Common_Assets level = ScriptableObject.CreateInstance<ADYFX_Common_Assets>();//不刷新库 创建新的配置以继承旧配置的guid
        if (isbtn)
        {
            level.strs1 = new List<string>() { "1" };
        }
        else
        {
            level.strs1 = new List<string>() { "0" };
        }
        AssetDatabase.CreateAsset(level, AssetDatabase.GUIDToAssetPath("fa5defd4fa471364180b027e3d2b8c80"));
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();//创建完成后刷新
    }
    void KaiTrail() 
    {
        ParticleSystem particle = thisParticleSystem.gameObject.GetComponent<ParticleSystem>();
        ParticleSystem.MainModule main = particle.main;
        ParticleSystem.TrailModule trail = particle.trails;
        trail.enabled = true;
        trail.mode = ParticleSystemTrailMode.PerParticle;
        trail.worldSpace = true;
        trail.dieWithParticles = false;
        trail.sizeAffectsWidth = false;
        trail.inheritParticleColor = false;
        ParticleSystem.MinMaxGradient minMaxGradient = new ParticleSystem.MinMaxGradient();
        minMaxGradient.mode = ParticleSystemGradientMode.Gradient;
        trail.colorOverLifetime = minMaxGradient;
        trail.colorOverTrail = minMaxGradient;
        trail.ratio = 1;
        trail.lifetime = (1 / main.startLifetime.constantMax)*0.25f;
    }
    void KaiRibbnon()
    {
        ParticleSystem particle = thisParticleSystem.gameObject.GetComponent<ParticleSystem>();
        ParticleSystem.MainModule main = particle.main;
        ParticleSystem.TrailModule trail = particle.trails;
        trail.enabled = true;
        trail.mode = ParticleSystemTrailMode.Ribbon;
        trail.worldSpace = true;
        trail.dieWithParticles = false;
        trail.sizeAffectsWidth = false;
        trail.inheritParticleColor = false;
        ParticleSystem.MinMaxGradient minMaxGradient = new ParticleSystem.MinMaxGradient();
        minMaxGradient.mode = ParticleSystemGradientMode.Gradient;
        trail.colorOverLifetime = minMaxGradient;
        trail.colorOverTrail = minMaxGradient;
        trail.ratio = 1;
        trail.lifetime = (1 / main.startLifetime.constantMax) * 0.25f;
        trail.widthOverTrail = 0.1f;
    }
    [MenuItem("CONTEXT/ParticleSystem/！！！将本粒子切换为Mesh渲染（继承粒子系统的材质和模型 并移除粒子系统）！！！")]
    static public void MeshRender1(MenuCommand cmd)//在其他组件上扩展，MenuCommand是获取其他组件，这个值是unity传的 根据组件类型获取对应组件
    {
        ParticleSystem par = (ParticleSystem)cmd.context;//强制转型从MenuCommand.context获得组件
        ParticleSystemRenderer render = par.GetComponent<ParticleSystemRenderer>();
        GameObject go = par.gameObject;
        if (!go.GetComponent<MeshFilter>())
            go.AddComponent<MeshFilter>();
        if (!go.GetComponent<MeshRenderer>())
            go.AddComponent <MeshRenderer > ();
        go.GetComponent<MeshFilter>().sharedMesh = render.mesh;
        go.GetComponent<MeshRenderer>().sharedMaterial = render.sharedMaterial;
        go.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        go.GetComponent<MeshRenderer>().receiveShadows = false;
        //Undo.RecordObject(go.GetComponent<ParticleSystem>(), "del ParticleSystem");
        Undo.DestroyObjectImmediate(go.GetComponent<ParticleSystem>());
        //DestroyImmediate(go.GetComponent<ParticleSystem>());
    }
}


