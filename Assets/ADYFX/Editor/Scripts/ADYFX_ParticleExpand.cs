using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Presets;//预设文件的命名空间

public class ADYFX_ParticleExpand
{
    [MenuItem("CONTEXT/ParticleSystem/一键设置自定义顶点数据相关选项（CustomData1+UV2）")]//第一个CONTEXT 是固定的 第二个是组件 第三个是方法命名
    static public void CustomData001(MenuCommand cmd)//在其他组件上扩展，MenuCommand是获取其他组件，这个值是unity传的 根据组件类型获取对应组件
    {
        ParticleSystem par = (ParticleSystem)cmd.context;//强制转型从MenuCommand.context获得组件
        ParticleSystemRenderer particleSystemRenderer = par.gameObject.GetComponent<ParticleSystemRenderer>();
        //ParticleSystem.MainModule main = par.main;//主模块
        //ParticleSystem.EmissionModule emission = par.emission;//发射量模块
        //ParticleSystem.ShapeModule shape = par.shape;//发射器形状模块
        //ParticleSystem.VelocityOverLifetimeModule velocityOverLifetime = par.velocityOverLifetime;//生命方向力模块
        //ParticleSystem.LimitVelocityOverLifetimeModule limitVelocityOverLifetime = par.limitVelocityOverLifetime;//生命阻力模块
        //ParticleSystem.InheritVelocityModule inheritVelocity = par.inheritVelocity;//跟随力模块
        //ParticleSystem.ColorOverLifetimeModule colorOverLifetime = par.colorOverLifetime;//生命颜色模块
        //ParticleSystem.SizeOverLifetimeModule sizeOverLifetime = par.sizeOverLifetime;//生命大小模块
        //ParticleSystem.RotationOverLifetimeModule rotationOverLifetime = par.rotationOverLifetime;//生命旋转模块
        //ParticleSystem.NoiseModule noise = par.noise;//运动噪波模块
        //ParticleSystem.SubEmittersModule subEmitters = par.subEmitters;//子级挂载模块
        //ParticleSystem.TextureSheetAnimationModule textureSheetAnimation = par.textureSheetAnimation;//序列图模块
        //ParticleSystem.TrailModule trail = par.trails;//拖尾模块
        ParticleSystem.CustomDataModule customData = par.customData;//自定义顶点数据流模块

        //main.duration = 1;
        //main.maxParticles = 5;
        //main.startSpeed = 0;
        //main.startLifetime = 1f;
        //main.scalingMode = ParticleSystemScalingMode.Hierarchy;
        //main.loop = false;
        //main.startRotation3D = false;
        //main.startRotation = 0f;
        //main.startSize3D = true;
        //main.startSizeX = 1f;
        //main.startSizeY = 1f;
        //main.startSizeZ = 1f;
        //main.startRotation3D = true;
        //main.startRotationX = 0f;
        //main.startRotationY = 0f;
        //main.startRotationZ = 0f;
        ////main.startRotationX =  new ParticleSystem.MinMaxCurve(1,new animacurve)

        //emission.enabled = true;
        //emission.burstCount = 1;
        //ParticleSystem.Burst m_burst = new ParticleSystem.Burst(0.001f, 1, 1, 1, 1.0f);//单次发射插槽
        //emission.SetBurst(0, m_burst);
        //emission.rateOverTimeMultiplier = 0;
        //emission.rateOverDistanceMultiplier = 0;
        //emission.rateOverTime = new ParticleSystem.MinMaxCurve(0);//设置发射量为一个单值
        //emission.rateOverDistance = new ParticleSystem.MinMaxCurve(0);//设置发射量为一个单值
        ////emission.rateOverTime = new ParticleSystem.MinMaxCurve(5.0f);//设置发射量为一个单值
        ////emission.rateOverTime = new ParticleSystem.MinMaxCurve(1, curve);//设置发射量为曲线

        ////parem.rateOverTime = new ParticleSystem.MinMaxCurve(1,new AnimationCurve(), new AnimationCurve());
        ////public MinMaxCurve(float multiplier, AnimationCurve min, AnimationCurve max);
        //shape.enabled = false;
        //velocityOverLifetime.enabled = false;
        //limitVelocityOverLifetime.enabled = false;
        //inheritVelocity.enabled = false;
        //noise.enabled = false;
        //subEmitters.enabled = false;
        //textureSheetAnimation.enabled = false;
        //trail.enabled = false;

        //sizeOverLifetime.enabled = true;
        //sizeOverLifetime.separateAxes = false;
        //Keyframe[] keyframes = new Keyframe[3];
        //keyframes[0] = new Keyframe(0f, 0f, 2f, 2f);
        //keyframes[1] = new Keyframe(0.242007f, 0.7655697f, 0.9918784f, 0.9918784f);
        //keyframes[2] = new Keyframe(1f, 1f, 0f, 0f);
        //sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1, new AnimationCurve(keyframes));//设置发射量为曲线
        customData.enabled = true;
        customData.SetMode(0, ParticleSystemCustomDataMode.Vector);
        //particleSystemRenderer.renderMode = ParticleSystemRenderMode.Billboard;
        //particleSystemRenderer.alignment = ParticleSystemRenderSpace.View;
        List<ParticleSystemVertexStream> vertexStream = new List<ParticleSystemVertexStream>();
        vertexStream.Add(ParticleSystemVertexStream.Position);
        vertexStream.Add(ParticleSystemVertexStream.Normal);
        vertexStream.Add(ParticleSystemVertexStream.Color);
        vertexStream.Add(ParticleSystemVertexStream.UV);
        vertexStream.Add(ParticleSystemVertexStream.UV2);
        vertexStream.Add(ParticleSystemVertexStream.Custom1XYZW);
        particleSystemRenderer.SetActiveVertexStreams(vertexStream);
    }
    [MenuItem("CONTEXT/ParticleSystem/一键设置自定义顶点数据相关选项（CustomData1、2+UV2+UV3）")]//第一个CONTEXT 是固定的 第二个是组件 第三个是方法命名
    static public void CustomData002(MenuCommand cmd)//在其他组件上扩展，MenuCommand是获取其他组件，这个值是unity传的 根据组件类型获取对应组件
    {
        ParticleSystem par = (ParticleSystem)cmd.context;//强制转型从MenuCommand.context获得组件
        ParticleSystemRenderer particleSystemRenderer = par.gameObject.GetComponent<ParticleSystemRenderer>();
        ParticleSystem.CustomDataModule customData = par.customData;//自定义顶点数据流模块
        customData.enabled = true;
        customData.SetMode(ParticleSystemCustomData.Custom1, ParticleSystemCustomDataMode.Vector);
        customData.SetMode(ParticleSystemCustomData.Custom2, ParticleSystemCustomDataMode.Vector);
        List<ParticleSystemVertexStream> vertexStream = new List<ParticleSystemVertexStream>();
        vertexStream.Add(ParticleSystemVertexStream.Position);
        vertexStream.Add(ParticleSystemVertexStream.Normal);
        vertexStream.Add(ParticleSystemVertexStream.Color);
        vertexStream.Add(ParticleSystemVertexStream.UV);
        vertexStream.Add(ParticleSystemVertexStream.UV2);
        vertexStream.Add(ParticleSystemVertexStream.Custom1XYZW);
        vertexStream.Add(ParticleSystemVertexStream.Custom2XYZW);
        particleSystemRenderer.SetActiveVertexStreams(vertexStream);
    }


    //Hierarchy窗口右键菜单
    [MenuItem("GameObject/ADYFX_新建粒子系统/1、面对摄像机的单个片", false, 11)]//第三个值如果在10左右，且在gameobgect下，就可以放到右键菜单里了
    static public void CreatePar01()
    {
        bool isparent = false;//判断有没有选中物体  以决定实例化时是否需要父级
        if (Selection.gameObjects.Length > 0)
        {
            isparent = true;
        }
        GameObject tx = new GameObject();
        if (isparent) 
        {
            GameObject parent = Selection.gameObjects[0];
            tx.transform.parent = parent.transform;
        }
        tx.name = "FX_474";
        tx.transform.localPosition = new Vector3(0, 0, 0);
        tx.transform.localScale = new Vector3(1, 1, 1);
        tx.AddComponent<ParticleSystem>();
        ADYFX_Editor.SeleHierachyObj(tx);//创建后选中物体
        ParticleSystem par = tx.GetComponent<ParticleSystem>();//获得组件

        ADYFX_ParExpandEditor adypar = new ADYFX_ParExpandEditor();//创建粒子类
        adypar.GetPar(par);//获取新创建的粒子到类里  后续修改类的值即可同步到粒子系统

        adypar.main.duration = 1;
        adypar.main.maxParticles = 5;
        adypar.main.startSpeed = 0;
        adypar.main.startLifetime = 1f;
        adypar.main.scalingMode = ParticleSystemScalingMode.Hierarchy;
        adypar.main.loop = false;
        adypar.main.startRotation3D = false;
        adypar.main.startRotation = 0f;
        adypar.main.startSize3D = true;
        adypar.main.startSizeX = 1f;
        adypar.main.startSizeY = 1f;
        adypar.main.startSizeZ = 1f;
        adypar.main.startRotation3D = true;
        adypar.main.startRotationX = 0f;
        adypar.main.startRotationY = 0f;
        adypar.main.startRotationZ = 0f;
        //main.startRotationX =  new ParticleSystem.MinMaxCurve(1,new animacurve)


        adypar.emission.burstCount = 1;//设置单次发射量为
        ParticleSystem.Burst m_burst = new ParticleSystem.Burst(0.001f, 1, 1, 1, 1.0f);//单次发射插槽
        adypar.emission.SetBurst(0, m_burst);//写入插槽到单次发射量数组
        adypar.emission.rateOverTimeMultiplier = 0;//持续发射量乘0
        adypar.emission.rateOverDistanceMultiplier = 0;
        adypar.emission.rateOverTime = new ParticleSystem.MinMaxCurve(0);//设置发射量为一个单值
        adypar.emission.rateOverDistance = new ParticleSystem.MinMaxCurve(0);//设置发射量为一个单值
                                                                             //emission.rateOverTime = new ParticleSystem.MinMaxCurve(5.0f);//设置发射量为一个单值
                                                                             //emission.rateOverTime = new ParticleSystem.MinMaxCurve(1, curve);//设置发射量为曲线

        //parem.rateOverTime = new ParticleSystem.MinMaxCurve(1,new AnimationCurve(), new AnimationCurve());
        //public MinMaxCurve(float multiplier, AnimationCurve min, AnimationCurve max);
        Gradient gradient = new Gradient();
        gradient.mode = GradientMode.Blend;
        gradient.colorKeys = new GradientColorKey[5] { new GradientColorKey(new Color(0.8160377f, 0.9782589f, 1f, 1f), 0), new GradientColorKey(new Color(0.5424528f, 1f, 0.9951187f, 1f), 0.1222705f), new GradientColorKey(new Color(0.3160377f, 0.7540266f, 1f, 1f), 0.5427176f), new GradientColorKey(new Color(0.1241545f, 0.3197895f, 0.5849056f, 1f), 0.8291447f), new GradientColorKey(new Color(0f, 0f, 0f, 1f), 1f) };
        gradient.alphaKeys = new GradientAlphaKey[4] {new GradientAlphaKey(0,0),new GradientAlphaKey(1,0.023f),new GradientAlphaKey(1,0.68f),new GradientAlphaKey(0,0) };
        adypar.colorOverLifetime.color = new ParticleSystem.MinMaxGradient(new Gradient());


        adypar.sizeOverLifetime.separateAxes = false;//关闭3轴缩放
        Keyframe[] keyframes = new Keyframe[3];//新建key数组
        keyframes[0] = new Keyframe(0f, 0f, 2f, 2f);//写入key数组
        keyframes[1] = new Keyframe(0.242007f, 0.7655697f, 0.9918784f, 0.9918784f);
        keyframes[2] = new Keyframe(1f, 1f, 0f, 0f);
        adypar.sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1, new AnimationCurve(keyframes));//设置发射量为曲线
        adypar.particleSystemRenderer.renderMode = ParticleSystemRenderMode.Billboard;
        adypar.particleSystemRenderer.alignment = ParticleSystemRenderSpace.View;

        adypar.emission.enabled = true;
        adypar.shape.enabled = false;
        adypar.velocityOverLifetime.enabled = false;
        adypar.limitVelocityOverLifetime.enabled = false;
        adypar.inheritVelocity.enabled = false;
        adypar.forceOverLifetime.enabled = false;
        adypar.colorOverLifetime.enabled = true;
        adypar.colorBySpeed.enabled = false;
        adypar.sizeOverLifetime.enabled = true;
        adypar.sizeBySpeed.enabled = false;
        adypar.rotationBySpeed.enabled = false;
        adypar.rotationOverLifetime.enabled = false;
        adypar.externalForces.enabled = false;
        adypar.collision.enabled = false;
        adypar.noise.enabled = false;
        adypar.sub.enabled = false;
        adypar.trigger.enabled = false;
        adypar.lights.enabled = false;
        adypar.custom.enabled = false;
        adypar.textureSheetAnimation.enabled = false;
        adypar.trail.enabled = false;

        adypar.particleSystemRenderer.enabled = true;
    }
    [MenuItem("GameObject/ADYFX_新建粒子系统/2、面对摄像机的单个Mesh", false, 11)]//第三个值如果在10左右，且在gameobgect下，就可以放到右键菜单里了
    static public void CreatePar02()
    {
        bool isparent = false;//判断有没有选中物体  以决定实例化时是否需要父级
        if (Selection.gameObjects.Length > 0)
        {
            isparent = true;
        }
        GameObject tx = new GameObject();
        if (isparent)
        {
            GameObject parent = Selection.gameObjects[0];
            tx.transform.parent = parent.transform;
        }
        tx.name = "FX_ViewMesh01";
        tx.transform.localPosition = new Vector3(0, 0, 0);
        tx.transform.localScale = new Vector3(1, 1, 1);
        tx.AddComponent<ParticleSystem>();
        ADYFX_Editor.SeleHierachyObj(tx);//创建后选中物体
        ParticleSystem par = tx.GetComponent<ParticleSystem>();//获得组件

        ADYFX_ParExpandEditor adypar = new ADYFX_ParExpandEditor();//创建粒子类
        adypar.GetPar(par);//获取新创建的粒子到类里  后续修改类的值即可同步到粒子系统

        adypar.main.duration = 1;
        adypar.main.maxParticles = 5;
        adypar.main.startSpeed = 0;
        adypar.main.startLifetime = 1f;
        adypar.main.scalingMode = ParticleSystemScalingMode.Hierarchy;
        adypar.main.loop = false;
        adypar.main.startRotation3D = false;
        adypar.main.startRotation = 0f;
        adypar.main.startSize3D = true;
        adypar.main.startSizeX = 1f;
        adypar.main.startSizeY = 1f;
        adypar.main.startSizeZ = 1f;
        adypar.main.startRotation3D = true;
        adypar.main.startRotationX = 0f;
        adypar.main.startRotationY = 0f;
        adypar.main.startRotationZ = 0f;
        //main.startRotationX =  new ParticleSystem.MinMaxCurve(1,new animacurve)


        adypar.emission.burstCount = 1;//设置单次发射量为
        ParticleSystem.Burst m_burst = new ParticleSystem.Burst(0.001f, 1, 1, 1, 1.0f);//单次发射插槽
        adypar.emission.SetBurst(0, m_burst);//写入插槽到单次发射量数组
        adypar.emission.rateOverTimeMultiplier = 0;//持续发射量乘0
        adypar.emission.rateOverDistanceMultiplier = 0;
        adypar.emission.rateOverTime = new ParticleSystem.MinMaxCurve(0);//设置发射量为一个单值
        adypar.emission.rateOverDistance = new ParticleSystem.MinMaxCurve(0);//设置发射量为一个单值
                                                                             //emission.rateOverTime = new ParticleSystem.MinMaxCurve(5.0f);//设置发射量为一个单值
                                                                             //emission.rateOverTime = new ParticleSystem.MinMaxCurve(1, curve);//设置发射量为曲线

        //parem.rateOverTime = new ParticleSystem.MinMaxCurve(1,new AnimationCurve(), new AnimationCurve());
        //public MinMaxCurve(float multiplier, AnimationCurve min, AnimationCurve max);
        Gradient gradient = new Gradient();
        gradient.mode = GradientMode.Blend;
        gradient.colorKeys = new GradientColorKey[2] { new GradientColorKey(new Color(1, 1, 1f, 1f), 0), new GradientColorKey(new Color(1, 1f, 1, 1f), 1) };
        gradient.alphaKeys = new GradientAlphaKey[4] { new GradientAlphaKey(0, 0), new GradientAlphaKey(1, 0.1f), new GradientAlphaKey(1, 0.6f), new GradientAlphaKey(0, 0) };
        adypar.colorOverLifetime.color = new ParticleSystem.MinMaxGradient(gradient);


        adypar.sizeOverLifetime.separateAxes = false;//关闭3轴缩放
        Keyframe[] keyframes = new Keyframe[3];//新建key数组
        keyframes[0] = new Keyframe(0f, 0f, 2f, 2f);//写入key数组
        keyframes[1] = new Keyframe(0.242007f, 0.7655697f, 0.9918784f, 0.9918784f);
        keyframes[2] = new Keyframe(1f, 1f, 0f, 0f);
        adypar.sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1, new AnimationCurve(keyframes));//设置发射量为曲线
        adypar.particleSystemRenderer.renderMode = ParticleSystemRenderMode.Billboard;
        adypar.particleSystemRenderer.alignment = ParticleSystemRenderSpace.View;

        adypar.particleSystemRenderer.renderMode = ParticleSystemRenderMode.Mesh;
        adypar.particleSystemRenderer.alignment = ParticleSystemRenderSpace.View;



        adypar.emission.enabled = true;
        adypar.shape.enabled = false;
        adypar.velocityOverLifetime.enabled = false;
        adypar.limitVelocityOverLifetime.enabled = false;
        adypar.inheritVelocity.enabled = false;
        adypar.forceOverLifetime.enabled = false;
        adypar.colorOverLifetime.enabled = true;
        adypar.colorBySpeed.enabled = false;
        adypar.sizeOverLifetime.enabled = true;
        adypar.sizeBySpeed.enabled = false;
        adypar.rotationBySpeed.enabled = false;
        adypar.rotationOverLifetime.enabled = false;
        adypar.externalForces.enabled = false;
        adypar.collision.enabled = false;
        adypar.noise.enabled = false;
        adypar.sub.enabled = false;
        adypar.trigger.enabled = false;
        adypar.lights.enabled = false;
        adypar.custom.enabled = false;
        adypar.textureSheetAnimation.enabled = false;
        adypar.trail.enabled = false;

        adypar.particleSystemRenderer.enabled = true;
    }
    [MenuItem("GameObject/ADYFX_新建粒子系统/3、锁定轴向的单个Mesh", false, 11)]//第三个值如果在10左右，且在gameobgect下，就可以放到右键菜单里了
    static public void CreatePar03()
    {
        bool isparent = false;//判断有没有选中物体  以决定实例化时是否需要父级
        if (Selection.gameObjects.Length > 0)
        {
            isparent = true;
        }
        GameObject tx = new GameObject();
        if (isparent)
        {
            GameObject parent = Selection.gameObjects[0];
            tx.transform.parent = parent.transform;
        }
        tx.name = "FX_LocalSpaceMesh01";
        tx.transform.localPosition = new Vector3(0, 0, 0);
        tx.transform.localScale = new Vector3(1, 1, 1);
        tx.AddComponent<ParticleSystem>();
        ADYFX_Editor.SeleHierachyObj(tx);//创建后选中物体
        ParticleSystem par = tx.GetComponent<ParticleSystem>();//获得组件

        ADYFX_ParExpandEditor adypar = new ADYFX_ParExpandEditor();//创建粒子类
        adypar.GetPar(par);//获取新创建的粒子到类里  后续修改类的值即可同步到粒子系统

        adypar.main.duration = 1;
        adypar.main.maxParticles = 5;
        adypar.main.startSpeed = 0;
        adypar.main.startLifetime = 1f;
        adypar.main.scalingMode = ParticleSystemScalingMode.Hierarchy;
        adypar.main.loop = false;
        adypar.main.startRotation3D = false;
        adypar.main.startRotation = 0f;
        adypar.main.startSize3D = true;
        adypar.main.startSizeX = 1f;
        adypar.main.startSizeY = 1f;
        adypar.main.startSizeZ = 1f;
        adypar.main.startRotation3D = true;
        adypar.main.startRotationX = 0f;
        adypar.main.startRotationY = 0f;
        adypar.main.startRotationZ = 0f;
        //main.startRotationX =  new ParticleSystem.MinMaxCurve(1,new animacurve)


        adypar.emission.burstCount = 1;//设置单次发射量为
        ParticleSystem.Burst m_burst = new ParticleSystem.Burst(0.001f, 1, 1, 1, 1.0f);//单次发射插槽
        adypar.emission.SetBurst(0, m_burst);//写入插槽到单次发射量数组
        adypar.emission.rateOverTimeMultiplier = 0;//持续发射量乘0
        adypar.emission.rateOverDistanceMultiplier = 0;
        adypar.emission.rateOverTime = new ParticleSystem.MinMaxCurve(0);//设置发射量为一个单值
        adypar.emission.rateOverDistance = new ParticleSystem.MinMaxCurve(0);//设置发射量为一个单值
                                                                             //emission.rateOverTime = new ParticleSystem.MinMaxCurve(5.0f);//设置发射量为一个单值
                                                                             //emission.rateOverTime = new ParticleSystem.MinMaxCurve(1, curve);//设置发射量为曲线

        //parem.rateOverTime = new ParticleSystem.MinMaxCurve(1,new AnimationCurve(), new AnimationCurve());
        //public MinMaxCurve(float multiplier, AnimationCurve min, AnimationCurve max);
        Gradient gradient = new Gradient();
        gradient.mode = GradientMode.Blend;
        gradient.colorKeys = new GradientColorKey[2] { new GradientColorKey(new Color(1, 1, 1f, 1f), 0), new GradientColorKey(new Color(1, 1f, 1, 1f), 1) };
        gradient.alphaKeys = new GradientAlphaKey[4] { new GradientAlphaKey(0, 0), new GradientAlphaKey(1, 0.1f), new GradientAlphaKey(1, 0.6f), new GradientAlphaKey(0, 0) };
        adypar.colorOverLifetime.color = new ParticleSystem.MinMaxGradient(gradient);


        adypar.sizeOverLifetime.separateAxes = false;//关闭3轴缩放
        Keyframe[] keyframes = new Keyframe[3];//新建key数组
        keyframes[0] = new Keyframe(0f, 0f, 2f, 2f);//写入key数组
        keyframes[1] = new Keyframe(0.242007f, 0.7655697f, 0.9918784f, 0.9918784f);
        keyframes[2] = new Keyframe(1f, 1f, 0f, 0f);
        adypar.sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1, new AnimationCurve(keyframes));//设置发射量为曲线
        adypar.particleSystemRenderer.renderMode = ParticleSystemRenderMode.Billboard;
        adypar.particleSystemRenderer.alignment = ParticleSystemRenderSpace.View;

        adypar.particleSystemRenderer.renderMode = ParticleSystemRenderMode.Mesh;
        adypar.particleSystemRenderer.alignment = ParticleSystemRenderSpace.Local;


        adypar.emission.enabled = true;
        adypar.shape.enabled = false;
        adypar.velocityOverLifetime.enabled = false;
        adypar.limitVelocityOverLifetime.enabled = false;
        adypar.inheritVelocity.enabled = false;
        adypar.forceOverLifetime.enabled = false;
        adypar.colorOverLifetime.enabled = true;
        adypar.colorBySpeed.enabled = false;
        adypar.sizeOverLifetime.enabled = true;
        adypar.sizeBySpeed.enabled = false;
        adypar.rotationBySpeed.enabled = false;
        adypar.rotationOverLifetime.enabled = false;
        adypar.externalForces.enabled = false;
        adypar.collision.enabled = false;
        adypar.noise.enabled = false;
        adypar.sub.enabled = false;
        adypar.trigger.enabled = false;
        adypar.lights.enabled = false;
        adypar.custom.enabled = false;
        adypar.textureSheetAnimation.enabled = false;
        adypar.trail.enabled = false;

        adypar.particleSystemRenderer.enabled = true;
    }
    [MenuItem("GameObject/ADYFX_新建粒子系统/4、环形爆发粒子 面对摄像机的片", false, 11)]//第三个值如果在10左右，且在gameobgect下，就可以放到右键菜单里了
    static public void CreatePar04()
    {
        bool isparent = false;//判断有没有选中物体  以决定实例化时是否需要父级
        if (Selection.gameObjects.Length > 0)
        {
            isparent = true;
        }
        GameObject tx = new GameObject();
        if (isparent)
        {
            GameObject parent = Selection.gameObjects[0];
            tx.transform.parent = parent.transform;
        }
        tx.name = "FX_LocalSpaceMesh01";
        tx.transform.localPosition = new Vector3(0, 0, 0);
        tx.transform.localScale = new Vector3(1, 1, 1);
        tx.transform.localEulerAngles = new Vector3(-90f,0,0);
        tx.AddComponent<ParticleSystem>();
        ADYFX_Editor.SeleHierachyObj(tx);//创建后选中物体
        ParticleSystem par = tx.GetComponent<ParticleSystem>();//获得组件

        ADYFX_ParExpandEditor adypar = new ADYFX_ParExpandEditor();//创建粒子类
        adypar.GetPar(par);//获取新创建的粒子到类里  后续修改类的值即可同步到粒子系统

        adypar.main.duration = 2;
        adypar.main.maxParticles = 50;
        adypar.main.startSpeed =new ParticleSystem.MinMaxCurve(5,20);
        adypar.main.startLifetime = new ParticleSystem.MinMaxCurve(0.7f, 2f);
        adypar.main.scalingMode = ParticleSystemScalingMode.Hierarchy;
        adypar.main.loop = false;
        adypar.main.startRotation3D = false;
        adypar.main.startRotation = new ParticleSystem.MinMaxCurve(0,6.3f);
        adypar.main.startSize3D = false;
        //adypar.main.startSizeX = 1f;
        //adypar.main.startSizeY = 1f;
        //adypar.main.startSizeZ = 1f;
        adypar.main.startRotation3D = false;
        //adypar.main.startRotationX = 0f;
        //adypar.main.startRotationY = 0f;
        //adypar.main.startRotationZ = 0f;
        //main.startRotationX =  new ParticleSystem.MinMaxCurve(1,new animacurve)


        adypar.emission.burstCount = 1;//设置单次发射量为
        ParticleSystem.Burst m_burst = new ParticleSystem.Burst(0.001f, 16, 24, 1, 1.0f);//单次发射插槽
        adypar.emission.SetBurst(0, m_burst);//写入插槽到单次发射量数组
        adypar.emission.rateOverTimeMultiplier = 0;//持续发射量乘0
        adypar.emission.rateOverDistanceMultiplier = 0;
        adypar.emission.rateOverTime = new ParticleSystem.MinMaxCurve(0);//设置发射量为一个单值
        adypar.emission.rateOverDistance = new ParticleSystem.MinMaxCurve(0);//设置发射量为一个单值
                                                                             //emission.rateOverTime = new ParticleSystem.MinMaxCurve(5.0f);//设置发射量为一个单值
                                                                             //emission.rateOverTime = new ParticleSystem.MinMaxCurve(1, curve);//设置发射量为曲线

        //parem.rateOverTime = new ParticleSystem.MinMaxCurve(1,new AnimationCurve(), new AnimationCurve());
        //public MinMaxCurve(float multiplier, AnimationCurve min, AnimationCurve max);
        Gradient gradient = new Gradient();
        gradient.mode = GradientMode.Blend;
        gradient.colorKeys = new GradientColorKey[2] { new GradientColorKey(new Color(1, 1, 1f, 1f), 0), new GradientColorKey(new Color(1, 1f, 1, 1f), 1) };
        gradient.alphaKeys = new GradientAlphaKey[4] { new GradientAlphaKey(0, 0), new GradientAlphaKey(1, 0.1f), new GradientAlphaKey(1, 0.6f), new GradientAlphaKey(0, 0) };
        adypar.colorOverLifetime.color = new ParticleSystem.MinMaxGradient(gradient);
        adypar.noise.quality = ParticleSystemNoiseQuality.Medium;
        adypar.noise.frequency = 0.5f;
        adypar.noise.strength = 0.5f;
        adypar.noise.scrollSpeed = 0f;

        adypar.shape.shapeType = ParticleSystemShapeType.Cone;
        adypar.shape.angle = 90;
        adypar.shape.radius = 1;
        adypar.shape.radiusThickness = 0.12f;
        adypar.shape.arc = 360;
        adypar.shape.arcMode = ParticleSystemShapeMultiModeValue.Random;
        adypar.shape.position = new Vector3(0,0,0.2f);
        adypar.shape.scale = new Vector3(1,1,1);

        adypar.limitVelocityOverLifetime.dampen = 0.3f;
        adypar.limitVelocityOverLifetime.drag = 0f;
        adypar.limitVelocityOverLifetime.separateAxes = false;
        adypar.limitVelocityOverLifetime.limit = new ParticleSystem.MinMaxCurve(0.5f,1f);

        adypar.rotationOverLifetime.separateAxes = false;
        adypar.rotationOverLifetime.x = new ParticleSystem.MinMaxCurve(0,0);
        adypar.rotationOverLifetime.y = new ParticleSystem.MinMaxCurve(0, 0);
        adypar.rotationOverLifetime.z = new ParticleSystem.MinMaxCurve(-1f,1f);

        adypar.sizeOverLifetime.separateAxes = false;//关闭3轴缩放
        Keyframe[] keyframes = new Keyframe[2];//新建key数组
        keyframes[0] =new Keyframe(0f, 0.5f, 1.393405f, 1.393405f);//写入key数组
        keyframes[1] = new Keyframe(1f, 1f, 0f, 0f);
        adypar.sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1, new AnimationCurve(keyframes));//设置发射量为曲线
        adypar.particleSystemRenderer.renderMode = ParticleSystemRenderMode.Billboard;
        adypar.particleSystemRenderer.alignment = ParticleSystemRenderSpace.View;

        adypar.emission.enabled = true;
        adypar.shape.enabled = true;
        adypar.velocityOverLifetime.enabled = false;
        adypar.limitVelocityOverLifetime.enabled = true;
        adypar.inheritVelocity.enabled = false;
        adypar.forceOverLifetime.enabled = false;
        adypar.colorOverLifetime.enabled = true;
        adypar.colorBySpeed.enabled = false;
        adypar.sizeOverLifetime.enabled = true;
        adypar.sizeBySpeed.enabled = false;
        adypar.rotationBySpeed.enabled = false;
        adypar.rotationOverLifetime.enabled = true;
        adypar.externalForces.enabled = false;
        adypar.collision.enabled = false;
        adypar.noise.enabled = true;
        adypar.sub.enabled = false;
        adypar.trigger.enabled = false;
        adypar.lights.enabled = false;
        adypar.custom.enabled = false;
        adypar.textureSheetAnimation.enabled = false;
        adypar.trail.enabled = false;

        adypar.particleSystemRenderer.enabled = true;
    }
    [MenuItem("GameObject/ADYFX_新建粒子系统/5、Box持续发射粒子 面对摄像机的片", false, 11)]//第三个值如果在10左右，且在gameobgect下，就可以放到右键菜单里了
    static public void CreatePar05()
    {
        bool isparent = false;//判断有没有选中物体  以决定实例化时是否需要父级
        if (Selection.gameObjects.Length > 0)
        {
            isparent = true;
        }
        GameObject tx = new GameObject();
        if (isparent)
        {
            GameObject parent = Selection.gameObjects[0];
            tx.transform.parent = parent.transform;
        }
        tx.name = "FX_LocalSpaceMesh01";
        tx.transform.localPosition = new Vector3(0, 0, 0);
        tx.transform.localScale = new Vector3(1, 1, 1);
        tx.transform.localEulerAngles = new Vector3(-90f, 0, 0);
        tx.AddComponent<ParticleSystem>();
        ADYFX_Editor.SeleHierachyObj(tx);//创建后选中物体
        ParticleSystem par = tx.GetComponent<ParticleSystem>();//获得组件

        ADYFX_ParExpandEditor adypar = new ADYFX_ParExpandEditor();//创建粒子类
        adypar.GetPar(par);//获取新创建的粒子到类里  后续修改类的值即可同步到粒子系统

        adypar.main.duration = 999;
        adypar.main.maxParticles = 200;
        adypar.main.startSpeed = new ParticleSystem.MinMaxCurve(0, 0);
        adypar.main.startLifetime = new ParticleSystem.MinMaxCurve(2f, 3.5f);
        adypar.main.scalingMode = ParticleSystemScalingMode.Hierarchy;
        adypar.main.loop = true;
        adypar.main.startRotation3D = false;
        adypar.main.startRotation = new ParticleSystem.MinMaxCurve(0, 6.3f);
        adypar.main.startSize3D = false;
        //adypar.main.startSizeX = 1f;
        //adypar.main.startSizeY = 1f;
        //adypar.main.startSizeZ = 1f;
        adypar.main.startRotation3D = false;
        //adypar.main.startRotationX = 0f;
        //adypar.main.startRotationY = 0f;
        //adypar.main.startRotationZ = 0f;
        //main.startRotationX =  new ParticleSystem.MinMaxCurve(1,new animacurve)


        //adypar.emission.burstCount = 0;//设置单次发射量为
        //ParticleSystem.Burst m_burst = new ParticleSystem.Burst(0.001f, 16, 24, 1, 1.0f);//单次发射插槽
        //adypar.emission.SetBurst(0, m_burst);//写入插槽到单次发射量数组
        adypar.emission.rateOverTimeMultiplier = 1;//持续发射量乘0
        adypar.emission.rateOverDistanceMultiplier = 1;
        adypar.emission.rateOverTime = new ParticleSystem.MinMaxCurve(10,22);//设置发射量为一个单值
        adypar.emission.rateOverDistance = new ParticleSystem.MinMaxCurve(0);//设置发射量为一个单值
                                                                             //emission.rateOverTime = new ParticleSystem.MinMaxCurve(5.0f);//设置发射量为一个单值
                                                                             //emission.rateOverTime = new ParticleSystem.MinMaxCurve(1, curve);//设置发射量为曲线

        //parem.rateOverTime = new ParticleSystem.MinMaxCurve(1,new AnimationCurve(), new AnimationCurve());
        //public MinMaxCurve(float multiplier, AnimationCurve min, AnimationCurve max);
        Gradient gradient = new Gradient();
        gradient.mode = GradientMode.Blend;
        gradient.colorKeys = new GradientColorKey[2] { new GradientColorKey(new Color(1, 1, 1f, 1f), 0), new GradientColorKey(new Color(1, 1f, 1, 1f), 1) };
        gradient.alphaKeys = new GradientAlphaKey[4] { new GradientAlphaKey(0, 0), new GradientAlphaKey(1, 0.1f), new GradientAlphaKey(1, 0.6f), new GradientAlphaKey(0, 0) };
        adypar.colorOverLifetime.color = new ParticleSystem.MinMaxGradient(gradient);
        adypar.noise.quality = ParticleSystemNoiseQuality.Medium;
        adypar.noise.frequency = 0.5f;
        adypar.noise.strength = 0.5f;
        adypar.noise.scrollSpeed = 0f;

        adypar.shape.shapeType = ParticleSystemShapeType.Box;
        //adypar.shape.angle = 90;
        //adypar.shape.radius = 1;
        //adypar.shape.radiusThickness = 0.12f;
        //adypar.shape.arc = 360;
        //adypar.shape.arcMode = ParticleSystemShapeMultiModeValue.Random;
        adypar.shape.position = new Vector3(0, 0, 0);
        adypar.shape.scale = new Vector3(2, 2, 2);

        adypar.velocityOverLifetime.x = new ParticleSystem.MinMaxCurve(1,0.2f);
        adypar.velocityOverLifetime.y = new ParticleSystem.MinMaxCurve(0, 0);
        adypar.velocityOverLifetime.z = new ParticleSystem.MinMaxCurve(-0.5f, 0.5f);
        adypar.velocityOverLifetime.speedModifier = 1f;
        adypar.velocityOverLifetime.radial = 0f;
        adypar.velocityOverLifetime.orbitalOffsetX = new ParticleSystem.MinMaxCurve(0);
        adypar.velocityOverLifetime.orbitalOffsetY = new ParticleSystem.MinMaxCurve(0);
        adypar.velocityOverLifetime.orbitalOffsetZ = new ParticleSystem.MinMaxCurve(0);

        //adypar.limitVelocityOverLifetime.dampen = 0.3f;
        //adypar.limitVelocityOverLifetime.drag = 0f;
        //adypar.limitVelocityOverLifetime.separateAxes = false;
        //adypar.limitVelocityOverLifetime.limit = new ParticleSystem.MinMaxCurve(0.5f, 1f);

        adypar.rotationOverLifetime.separateAxes = false;
        adypar.rotationOverLifetime.x = new ParticleSystem.MinMaxCurve(0, 0);
        adypar.rotationOverLifetime.y = new ParticleSystem.MinMaxCurve(0, 0);
        adypar.rotationOverLifetime.z = new ParticleSystem.MinMaxCurve(-1f, 1f);

        adypar.sizeOverLifetime.separateAxes = false;//关闭3轴缩放
        Keyframe[] keyframes = new Keyframe[2];//新建key数组
        keyframes[0] = new Keyframe(0f, 0.5f, 1.393405f, 1.393405f);//写入key数组
        keyframes[1] = new Keyframe(1f, 1f, 0f, 0f);
        adypar.sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1, new AnimationCurve(keyframes));//设置发射量为曲线
        adypar.particleSystemRenderer.renderMode = ParticleSystemRenderMode.Billboard;
        adypar.particleSystemRenderer.alignment = ParticleSystemRenderSpace.View;

        adypar.emission.enabled = true;
        adypar.shape.enabled = true;
        adypar.velocityOverLifetime.enabled = true;
        adypar.limitVelocityOverLifetime.enabled = false;
        adypar.inheritVelocity.enabled = false;
        adypar.forceOverLifetime.enabled = false;
        adypar.colorOverLifetime.enabled = true;
        adypar.colorBySpeed.enabled = false;
        adypar.sizeOverLifetime.enabled = true;
        adypar.sizeBySpeed.enabled = false;
        adypar.rotationBySpeed.enabled = false;
        adypar.rotationOverLifetime.enabled = true;
        adypar.externalForces.enabled = false;
        adypar.collision.enabled = false;
        adypar.noise.enabled = false;
        adypar.sub.enabled = false;
        adypar.trigger.enabled = false;
        adypar.lights.enabled = false;
        adypar.custom.enabled = false;
        adypar.textureSheetAnimation.enabled = false;
        adypar.trail.enabled = false;

        adypar.particleSystemRenderer.enabled = true;
    }
    [MenuItem("GameObject/ADYFX_新建粒子系统/6、Box持续发射粒子 锁定轴向的Mesh", false, 11)]//第三个值如果在10左右，且在gameobgect下，就可以放到右键菜单里了
    static public void CreatePar06()
    {
        bool isparent = false;//判断有没有选中物体  以决定实例化时是否需要父级
        if (Selection.gameObjects.Length > 0)
        {
            isparent = true;
        }
        GameObject tx = new GameObject();
        if (isparent)
        {
            GameObject parent = Selection.gameObjects[0];
            tx.transform.parent = parent.transform;
        }
        tx.name = "FX_LocalSpaceMesh01";
        tx.transform.localPosition = new Vector3(0, 0, 0);
        tx.transform.localScale = new Vector3(1, 1, 1);
        tx.transform.localEulerAngles = new Vector3(-90f, 0, 0);
        tx.AddComponent<ParticleSystem>();
        ADYFX_Editor.SeleHierachyObj(tx);//创建后选中物体
        ParticleSystem par = tx.GetComponent<ParticleSystem>();//获得组件

        ADYFX_ParExpandEditor adypar = new ADYFX_ParExpandEditor();//创建粒子类
        adypar.GetPar(par);//获取新创建的粒子到类里  后续修改类的值即可同步到粒子系统

        adypar.main.duration = 999;
        adypar.main.maxParticles = 200;
        adypar.main.startSpeed = new ParticleSystem.MinMaxCurve(0, 0);
        adypar.main.startLifetime = new ParticleSystem.MinMaxCurve(2f, 3.5f);
        adypar.main.scalingMode = ParticleSystemScalingMode.Hierarchy;
        adypar.main.loop = true;
        adypar.main.startRotation3D = false;
        adypar.main.startRotation = new ParticleSystem.MinMaxCurve(0, 6.3f);
        adypar.main.startSize3D = true;
        adypar.main.startSizeX = 1f;
        adypar.main.startSizeY = 1f;
        adypar.main.startSizeZ = 1f;
        adypar.main.startRotation3D = true;
        adypar.main.startRotationX = new ParticleSystem.MinMaxCurve(6.3f, 0);
        adypar.main.startRotationY = new ParticleSystem.MinMaxCurve(6.3f, 0);
        adypar.main.startRotationZ = new ParticleSystem.MinMaxCurve(6.3f, 0);
        //main.startRotationX =  new ParticleSystem.MinMaxCurve(1,new animacurve)


        //adypar.emission.burstCount = 0;//设置单次发射量为
        //ParticleSystem.Burst m_burst = new ParticleSystem.Burst(0.001f, 16, 24, 1, 1.0f);//单次发射插槽
        //adypar.emission.SetBurst(0, m_burst);//写入插槽到单次发射量数组
        adypar.emission.rateOverTimeMultiplier = 1;//持续发射量乘0
        adypar.emission.rateOverDistanceMultiplier = 1;
        adypar.emission.rateOverTime = new ParticleSystem.MinMaxCurve(10, 22);//设置发射量为一个单值
        adypar.emission.rateOverDistance = new ParticleSystem.MinMaxCurve(0);//设置发射量为一个单值
                                                                             //emission.rateOverTime = new ParticleSystem.MinMaxCurve(5.0f);//设置发射量为一个单值
                                                                             //emission.rateOverTime = new ParticleSystem.MinMaxCurve(1, curve);//设置发射量为曲线

        //parem.rateOverTime = new ParticleSystem.MinMaxCurve(1,new AnimationCurve(), new AnimationCurve());
        //public MinMaxCurve(float multiplier, AnimationCurve min, AnimationCurve max);
        Gradient gradient = new Gradient();
        gradient.mode = GradientMode.Blend;
        gradient.colorKeys = new GradientColorKey[2] { new GradientColorKey(new Color(1, 1, 1f, 1f), 0), new GradientColorKey(new Color(1, 1f, 1, 1f), 1) };
        gradient.alphaKeys = new GradientAlphaKey[4] { new GradientAlphaKey(0, 0), new GradientAlphaKey(1, 0.1f), new GradientAlphaKey(1, 0.6f), new GradientAlphaKey(0, 0) };
        adypar.colorOverLifetime.color = new ParticleSystem.MinMaxGradient(gradient);
        adypar.noise.quality = ParticleSystemNoiseQuality.Medium;
        adypar.noise.frequency = 0.5f;
        adypar.noise.strength = 0.5f;
        adypar.noise.scrollSpeed = 0f;

        adypar.shape.shapeType = ParticleSystemShapeType.Box;
        //adypar.shape.angle = 90;
        //adypar.shape.radius = 1;
        //adypar.shape.radiusThickness = 0.12f;
        //adypar.shape.arc = 360;
        //adypar.shape.arcMode = ParticleSystemShapeMultiModeValue.Random;
        adypar.shape.position = new Vector3(0, 0, 0);
        adypar.shape.scale = new Vector3(2, 2, 2);

        adypar.velocityOverLifetime.x = new ParticleSystem.MinMaxCurve(1, 0.2f);
        adypar.velocityOverLifetime.y = new ParticleSystem.MinMaxCurve(0, 0);
        adypar.velocityOverLifetime.z = new ParticleSystem.MinMaxCurve(-0.5f, 0.5f);
        adypar.velocityOverLifetime.speedModifier = 1f;
        adypar.velocityOverLifetime.radial = 0f;
        adypar.velocityOverLifetime.orbitalOffsetX = new ParticleSystem.MinMaxCurve(0);
        adypar.velocityOverLifetime.orbitalOffsetY = new ParticleSystem.MinMaxCurve(0);
        adypar.velocityOverLifetime.orbitalOffsetZ = new ParticleSystem.MinMaxCurve(0);

        //adypar.limitVelocityOverLifetime.dampen = 0.3f;
        //adypar.limitVelocityOverLifetime.drag = 0f;
        //adypar.limitVelocityOverLifetime.separateAxes = false;
        //adypar.limitVelocityOverLifetime.limit = new ParticleSystem.MinMaxCurve(0.5f, 1f);

        adypar.rotationOverLifetime.separateAxes = true;
        adypar.rotationOverLifetime.x = new ParticleSystem.MinMaxCurve(-1f, 1f);
        adypar.rotationOverLifetime.y = new ParticleSystem.MinMaxCurve(-1f, 1f);
        adypar.rotationOverLifetime.z = new ParticleSystem.MinMaxCurve(-1f, 1f);

        adypar.sizeOverLifetime.separateAxes = false;//关闭3轴缩放
        Keyframe[] keyframes = new Keyframe[2];//新建key数组
        keyframes[0] = new Keyframe(0f, 0.5f, 1.393405f, 1.393405f);//写入key数组
        keyframes[1] = new Keyframe(1f, 1f, 0f, 0f);
        adypar.sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1, new AnimationCurve(keyframes));//设置发射量为曲线
        adypar.particleSystemRenderer.renderMode = ParticleSystemRenderMode.Mesh;
        adypar.particleSystemRenderer.alignment = ParticleSystemRenderSpace.Local;

        adypar.emission.enabled = true;
        adypar.shape.enabled = true;
        adypar.velocityOverLifetime.enabled = true;
        adypar.limitVelocityOverLifetime.enabled = false;
        adypar.inheritVelocity.enabled = false;
        adypar.forceOverLifetime.enabled = false;
        adypar.colorOverLifetime.enabled = true;
        adypar.colorBySpeed.enabled = false;
        adypar.sizeOverLifetime.enabled = true;
        adypar.sizeBySpeed.enabled = false;
        adypar.rotationBySpeed.enabled = false;
        adypar.rotationOverLifetime.enabled = true;
        adypar.externalForces.enabled = false;
        adypar.collision.enabled = false;
        adypar.noise.enabled = false;
        adypar.sub.enabled = false;
        adypar.trigger.enabled = false;
        adypar.lights.enabled = false;
        adypar.custom.enabled = false;
        adypar.textureSheetAnimation.enabled = false;
        adypar.trail.enabled = false;

        adypar.particleSystemRenderer.enabled = true;
    }
}
