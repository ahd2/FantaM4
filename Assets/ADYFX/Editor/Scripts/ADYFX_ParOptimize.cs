using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
public class ADYFX_ParOptimize : EditorWindow
{
    Vector2 mBeginScrollView;
    Vector2 mBeginScrollView1;
    Object sourceTex0;
    public Material bgmat;
    public Texture2D bgTex;
    [Header("是否限制粒子最大数量↓")]
    public bool isParSize = false;
    [Header("暴力设置：循环特效最大粒子数&非循环特效最大粒子数")]
    public int loopParricleMaxSize = 500;
    public int onceParricleMaxSize = 100;
    [Header("智能计算粒子峰值数(考虑原本的最大粒子，结果小于原本 则设置为峰值，大于则不)")]
    public bool isAuto = false;
    [Header("是否把粒子的ScalingMode改为Hierarchy以适应父级缩放")]
    public bool setScalingMode = false;
    //[Header("勾选√ 管理粒子发射完毕后状态：无动作、隐藏物体、销毁物体、回调")]
    //public bool isDes = false;
    //public ParticleSystemStopAction des;
    [Header("对所有粒子，NoiSe模块，统一设置其精度为2DNoise")]
    public bool isNoise = false;
    [Header("检查并关闭灯光功能")]
    public bool isLight = false;
    [Header("未使用拖尾功能的，清除trail材质引用")]
    public bool isTrailmat = false;
    [Header("是否统一设置粒子在屏幕上最大大小限制")]
    public bool isMaxParticleSize = false;
    public float maxParsize = 5;
    [Header("是否统一把所有Order in Layer还原到以下值")]
    public bool isOIL = false;
    public int oil = 0;
    [Header("是否检查并关闭所有粒子的投影和光照接收")]
    public bool isShadow = false;
    [Header("下列清单仅展示已添加的物体和获取到的粒子，无需操作")]
    GameObject[] addgo = new GameObject[0];
    int addgosize = 0;
    List<ParticleSystemRenderer> parRenders = new List<ParticleSystemRenderer>();
    List<ParticleSystem> pars = new List<ParticleSystem>();
    bool isremove = false;
    [MenuItem("ADYFX/其他工具/※批量优化粒子系统", false, 2503)]
    public static void HelpWindow()//菜单窗口
    {
        ADYFX_ParOptimize window = EditorWindow.GetWindow<ADYFX_ParOptimize>();//定义窗口类
        window.minSize = new Vector2(1200, 650);//限制窗口最小值
        window.maxSize = new Vector2(1600, 920);//限制窗口最小值
        window.position = new Rect(50, 50, 1200, 650);
        window.titleContent = new GUIContent("批量优化粒子系统");//标题内容
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
        addgosize = addgo.Length;
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
                    addgo = new GameObject[DragAndDrop.paths.Length+ addgosize];
                    for (int z = 0; z < addgosize; z++)
                    {
                        addgo[z] = temp[z];
                    }
                    for (int i = 0;i< DragAndDrop.paths.Length;i++) 
                    {
                        addgo[i+ addgosize] = ADYFX_Editor.GetGO(DragAndDrop.paths[i]);
                    }
                }
            }
        }
        GUILayout.Space(10);
        GUILayout.Label("       优化相关选项                                                      ※多选特效预制体或者场景中的特效 拖入窗口以添加※                                                            待处理列表");
        EditorGUILayout.BeginHorizontal();//开始水平布局
        GUILayout.Space(20);
        //sourceTex0 = EditorGUILayout.ObjectField(sourceTex0, typeof(Object), false, options) as Object;//然后声明这个tex2d
        mBeginScrollView = GUILayout.BeginScrollView(mBeginScrollView);//开始滚动视图、列表
        {
            GUILayout.Space(20);
            isParSize = GUILayout.Toggle(isParSize, "是否限制粒子最大数量", GUILayout.Width(150), GUILayout.Height(30));//特别设置按钮宽高
            if (isParSize)
            {
                isAuto = GUILayout.Toggle(isAuto, "智能计算粒子最大数量", GUILayout.Width(150), GUILayout.Height(30));//特别设置按钮宽高
            }
            if(isParSize&&!isAuto)
            {
                loopParricleMaxSize = EditorGUI.IntField(new Rect(250, 15, 210, 25), "→循环特效最大粒子数", loopParricleMaxSize);
                onceParricleMaxSize = EditorGUI.IntField(new Rect(250, 39, 210, 25), "→不循环特效最大粒子数", onceParricleMaxSize);
            }
            else
            {
                if (isParSize)
                    GUI.Label(new Rect(250, 20, 500, 25), "→计算粒子系统运行所需的最大粒子数量 以此设置最大粒子数限制");
            }
            setScalingMode = GUILayout.Toggle(setScalingMode, "设置ScalingMode为Hierarchy 以适应父级缩放", GUILayout.Width(500), GUILayout.Height(30));
            //EditorGUI.EnumFlagsField(new Rect(250, 15, 210, 25));
            isLight = GUILayout.Toggle(isLight, "关闭灯光功能", GUILayout.Width(500), GUILayout.Height(30));
            isTrailmat = GUILayout.Toggle(isTrailmat, "未使用拖尾功能的  清除trail材质引用", GUILayout.Width(500), GUILayout.Height(30));
            isMaxParticleSize = GUILayout.Toggle(isMaxParticleSize, "统一设置粒子在屏幕上的最大尺寸限制", GUILayout.Width(400), GUILayout.Height(30));
            if (isMaxParticleSize) 
            {
                if (!isParSize)
                {
                    maxParsize = EditorGUI.FloatField(new Rect(280, 139, 200, 25), "→粒子在屏幕上的最大尺寸", maxParsize);
                }
                else 
                {
                    maxParsize = EditorGUI.FloatField(new Rect(280, 169, 200, 25), "→粒子在屏幕上的最大尺寸", maxParsize);
                }

            }
            isOIL = GUILayout.Toggle(isOIL, "是否把Order in Layer统一设置一个值", GUILayout.Width(400), GUILayout.Height(30));
            if (isOIL)
            {
                if (!isParSize)
                {
                    oil = EditorGUI.IntField(new Rect(280, 169, 200, 25), "→Order in Layer设置为：", oil);
                }
                else
                {
                    oil = EditorGUI.IntField(new Rect(280, 199, 200, 25), "→Order in Layer设置为：", oil);
                }

            }
           
        }
        GUILayout.EndScrollView();//结束滚动视图、列表
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
        GUILayout.EndHorizontal();//结束横向布局
        isremove = GUI.Toggle(new Rect(970,28,200,30),isremove, "执行优化后清空当前列表");
        if (GUI.Button(new Rect(970, 70, 200, 35), "清空列表"))//特别设置按钮宽高
        {
            addgo = new GameObject[0];
            pars = new List<ParticleSystem>();
            parRenders = new List<ParticleSystemRenderer>();
        }
        if (GUI.Button(new Rect(970,120,200,50),"开始优化"))//特别设置按钮宽高
        {
            Add_TXGO();
            YouhuaTX();
            if (isremove) 
            {
                addgo = new GameObject[0];
                pars = new List<ParticleSystem>();
                parRenders = new List<ParticleSystemRenderer>();
            }
        }
        GUILayout.Label("待处理列表有  " + addgosize + "  项     【开始优化】会自动获取所有子级粒子系统 并执行优化");
        GUILayout.Label("上次处理了  "+ parRenders.Count + "  个粒子系统");
    }
    void Add_TXGO()//"》添加选中的物体《"
    {
        //addgo = Selection.gameObjects;
        for (int i = 0; i < addgo.Length; i++)
        {
            ParticleSystem[] linshipar = new ParticleSystem[0];
            ParticleSystemRenderer[] linshiparren = new ParticleSystemRenderer[0];
            linshipar = addgo[i].GetComponentsInChildren<ParticleSystem>(true);
            linshiparren = addgo[i].GetComponentsInChildren<ParticleSystemRenderer>(true);
            for (int a = 0; a < linshipar.Length; a++)
            {
                pars.Add(linshipar[a]);
                parRenders.Add(linshiparren[a]);
            }
        }
        //Debug.Log("本次选择了" + addgo.Length + "个物体" + "，已加入清单的粒子系统的有" + parRenders.Count + "个");
    }
    void YouhuaTX()//》开始优化《
    {
        for (int i = 0; i < pars.Count; i++)
        {
            ParticleSystem.MainModule mainmodule = pars[i].main;
            ParticleSystem.EmissionModule emimodule = pars[i].emission;
            ParticleSystem.NoiseModule noiseModule = pars[i].noise;
            ParticleSystem.LightsModule lightsModule = pars[i].lights;
            ParticleSystem.TrailModule trailModule = pars[i].trails;
            ParticleSystem.CustomDataModule customDataModule = pars[i].customData;
            if (isParSize == true)
            {
                if (isAuto == true)
                {
                    OnAuto(mainmodule, emimodule);
                }
                else
                {
                    if (pars[i].main.loop == true)
                    {
                        mainmodule.maxParticles = loopParricleMaxSize;
                    }
                    else
                    {
                        mainmodule.maxParticles = onceParricleMaxSize;
                    }
                    if (mainmodule.maxParticles <= 1)
                    {
                        mainmodule.maxParticles = 1;
                    }
                }
            }
            if (setScalingMode == true)
            {
                mainmodule.scalingMode = ParticleSystemScalingMode.Hierarchy;
            }
            //if (isDes == true)
            //{
            //    mainmodule.stopAction = des;
            //}
            if (isNoise == true)
            {
                noiseModule.quality = ParticleSystemNoiseQuality.Medium;
            }
            if (isLight == true)
            {
                lightsModule.enabled = false;
            }
            if (isTrailmat == true)
            {
                if (trailModule.enabled == false)
                {
                    parRenders[i].trailMaterial = null;
                }
            }
            if (isMaxParticleSize)
            {
                parRenders[i].maxParticleSize = maxParsize;
            }
            if (isOIL == true)
            {
                parRenders[i].sortingOrder = oil;
            }
            if (isShadow == true)
            {
                parRenders[i].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                parRenders[i].receiveShadows = false;
            }
        }
        Debug.LogError ("粒子系统优化完毕，共修改了  " + parRenders.Count + " 个");
        AssetDatabase.Refresh();//刷新资源库
    }
    public void OnAuto(ParticleSystem.MainModule mainModule, ParticleSystem.EmissionModule emissionModule)
    {
        float b = mainModule.maxParticles;
        float particleCount = 0;
        if (emissionModule.rateOverTime.constant > 0)
        {
            float size = Mathf.Max(emissionModule.rateOverTime.constantMax, emissionModule.rateOverTime.constantMin);
            particleCount += size;
        }
        if (emissionModule.rateOverTime.curveMultiplier != 0)
        {
            float[] dd = new float[emissionModule.rateOverTime.curve.keys.Length];
            for (int z = 0; z < dd.Length; z++)
            {
                dd[z] = emissionModule.rateOverTime.curve.keys[z].value;
            }
            float maxValue = Mathf.Max(dd) * emissionModule.rateOverTime.curveMultiplier;
            particleCount += maxValue;
        }
        if (emissionModule.burstCount >= 1)
        {
            for (int i = 0; i < emissionModule.burstCount; i++)
            {
                if (emissionModule.GetBurst(i).count.constant != 0)
                {
                    particleCount += Mathf.Max(emissionModule.GetBurst(i).count.constantMax, emissionModule.GetBurst(i).count.constantMin);
                    if (emissionModule.GetBurst(i).repeatInterval > 0.01f)
                    {
                        float size = Mathf.Max(emissionModule.GetBurst(i).count.constantMax, emissionModule.GetBurst(i).count.constantMin);
                        size = size / emissionModule.GetBurst(i).repeatInterval;
                        particleCount += size;
                    }
                }
                else
                {
                    if (emissionModule.GetBurst(i).count.curve == null)
                    {
                        Debug.Log("没有发射任何粒子");
                    }
                    else
                    {
                        float[] dd = new float[(int)emissionModule.GetBurst(i).count.curve.length];
                        for (int z = 0; z < dd.Length; z++)
                        {
                            dd[z] = emissionModule.GetBurst(i).count.curve.keys[z].value;
                        }
                        float maxValue = Mathf.Max(dd) * emissionModule.GetBurst(i).count.curveMultiplier;
                        particleCount += maxValue;
                        if (emissionModule.GetBurst(i).repeatInterval > 0.01f)
                        {
                            float size = Mathf.Max(emissionModule.GetBurst(i).count.constantMax, emissionModule.GetBurst(i).count.constantMin);
                            size = size / emissionModule.GetBurst(i).repeatInterval;
                            particleCount += size;
                        }
                    }
                }
            }
        }
        float sizeclamp = 0;
        sizeclamp = mainModule.startLifetime.constantMax;
        sizeclamp = Mathf.Clamp(sizeclamp, 1, 99999);
        particleCount = particleCount * sizeclamp;
        Debug.Log(sizeclamp);
        if (particleCount <= b && particleCount != 0)
        {
            float ad = Mathf.Max(emissionModule.rateOverDistance.constantMax, emissionModule.rateOverDistance.constantMin);
            if (ad == 0 && emissionModule.rateOverDistance.curve == null)
            {
                if (particleCount <= 2)
                {
                    mainModule.maxParticles = 5;
                }
                else
                {
                    mainModule.maxParticles = (int)particleCount;
                }
            }
            else
            {
                Debug.Log("当前粒子拥有距离发射量，安全起见不设置其max粒子限制，当前值：" + mainModule.maxParticles);
            }
        }
    }
}

