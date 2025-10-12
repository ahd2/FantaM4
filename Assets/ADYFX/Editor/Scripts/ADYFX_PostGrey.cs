using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
//[ExecuteAlways]
public class ADYFX_PostGrey
{
    public bool isCamScreenCam = false;
   // [MenuItem("ADYFX/特效辅助/※查看特效灰度范围【场景中可用】 %F12", false, 2010)]
   //static void SetGrey()
   // {
   //     if (GameObject.Find("AFX_PostGrey"))
   //     {
   //         GameObject.DestroyImmediate(GameObject.Find("AFX_PostGrey"));
   //     }
   //     else
   //     {
   //         if (Shader.Find("AFX/Post/PostGrey"))
   //         {
   //             //GameObject go = new GameObject("cube1");
   //             if (GameObject.Find("AFX_PostGrey"))
   //             {
   //                 GameObject aa = GameObject.Find("AFX_PostGrey");
   //                 aa.transform.localScale = new Vector3(999, 999, 999);
   //                 aa.GetComponent<Renderer>().material = new Material(Shader.Find("AFX/Post/PostGrey"));
   //                 aa.GetComponent<Renderer>().sharedMaterial.SetFloat("_Fraction", 1);
   //             }
   //             else
   //             {
   //                 var objCube = GameObject.CreatePrimitive(PrimitiveType.Cube);//类型
   //                 objCube.name = "AFX_PostGrey";
   //                 objCube.transform.localScale = new Vector3(999, 999, 999);
   //                 objCube.GetComponent<Renderer>().material = new Material(Shader.Find("AFX/Post/PostGrey"));
   //                 objCube.GetComponent<Renderer>().sharedMaterial.SetFloat("_Fraction", 1);
   //                 if (objCube.GetComponent<Collider>()) 
   //                 {
   //                     GameObject.DestroyImmediate(objCube.GetComponent<Collider>());
   //                 }
   //                 objCube.transform.localPosition = new Vector3(0, 0, 0);
   //                 objCube.AddComponent<ParticleSystem>();
   //                 ParticleSystem par = objCube.GetComponent<ParticleSystem>();//获得组件

   //                 ADYFX_ParExpandEditor adypar = new ADYFX_ParExpandEditor();//创建粒子类
   //                 adypar.GetPar(par);//获取新创建的粒子到类里  后续修改类的值即可同步到粒子系统

   //                 adypar.main.duration = 999;
   //                 adypar.main.maxParticles = 200;
   //                 adypar.main.startSpeed = new ParticleSystem.MinMaxCurve(0, 0);
   //                 adypar.main.startLifetime = new ParticleSystem.MinMaxCurve(999);
   //                 adypar.main.scalingMode = ParticleSystemScalingMode.Hierarchy;
   //                 adypar.main.loop = true;
   //                 adypar.main.startRotation3D = false;
   //                 adypar.main.startRotation = new ParticleSystem.MinMaxCurve(0, 0);
   //                 adypar.main.startSize3D = true;
   //                 adypar.main.startSizeX = 1f;
   //                 adypar.main.startSizeY = 1f;
   //                 adypar.main.startSizeZ = 1f;
   //                 adypar.main.startRotation3D = true;
   //                 adypar.main.startRotationX = new ParticleSystem.MinMaxCurve(0, 0);
   //                 adypar.main.startRotationY = new ParticleSystem.MinMaxCurve(0, 0);
   //                 adypar.main.startRotationZ = new ParticleSystem.MinMaxCurve(0, 0);

   //                 adypar.emission.burstCount = 1;//设置单次发射量为
   //                 ParticleSystem.Burst m_burst = new ParticleSystem.Burst(0.001f, 1, 1, 1, 1.0f);//单次发射插槽
   //                 adypar.emission.SetBurst(0, m_burst);//写入插槽到单次发射量数组
   //                 adypar.emission.rateOverTime = new ParticleSystem.MinMaxCurve(0);//设置发射量为一个单值
   //                 adypar.emission.rateOverDistance = new ParticleSystem.MinMaxCurve(0);//设置发射量为一个单值

   //                 adypar.particleSystemRenderer.renderMode = ParticleSystemRenderMode.Mesh;
   //                 adypar.particleSystemRenderer.alignment = ParticleSystemRenderSpace.Local;
   //                 adypar.particleSystemRenderer.mesh = objCube.GetComponent<MeshFilter>().sharedMesh;
   //                 adypar.particleSystemRenderer.material = objCube.GetComponent<Renderer>().sharedMaterial;
   //                 adypar.particleSystemRenderer.sortingOrder = 5999;
   //                 adypar.emission.enabled = true;
   //                 adypar.shape.enabled = false;
   //                 adypar.velocityOverLifetime.enabled = false;
   //                 adypar.limitVelocityOverLifetime.enabled = false;
   //                 adypar.inheritVelocity.enabled = false;
   //                 adypar.forceOverLifetime.enabled = false;
   //                 adypar.colorOverLifetime.enabled = false;
   //                 adypar.colorBySpeed.enabled = false;
   //                 adypar.sizeOverLifetime.enabled = false;
   //                 adypar.sizeBySpeed.enabled = false;
   //                 adypar.rotationBySpeed.enabled = false;
   //                 adypar.rotationOverLifetime.enabled = false;
   //                 adypar.externalForces.enabled = false;
   //                 adypar.collision.enabled = false;
   //                 adypar.noise.enabled = false;
   //                 adypar.sub.enabled = false;
   //                 adypar.trigger.enabled = false;
   //                 adypar.lights.enabled = false;
   //                 adypar.custom.enabled = false;
   //                 adypar.textureSheetAnimation.enabled = false;
   //                 adypar.trail.enabled = false;

   //                 adypar.particleSystemRenderer.enabled = true;
   //                 ADYFX_Editor.SeleHierachyObj(objCube);//创建后选中物体
   //                 objCube.AddComponent<AFX_SeleNull>();
   //                 objCube.hideFlags = HideFlags.HideInHierarchy;//物体不出现在场景列表中
   //                 //objCube.hideFlags = HideFlags.HideInInspector;//物体出现在场景列表中，但不可见组件
   //             }
   //         }
   //         else
   //         {
   //             Debug.LogError("【ADYFX/特效辅助/查看特效灰度范围】所需着色器未找到");
   //         }
   //     }
   // }
    //[MenuItem("ADYFX/特效辅助/查看特效灰度范围(取消查看) #&Z", false, 1)]
    //static void RemGrey()
    //{
    //    if (GameObject.Find("AFX_PostGrey(可以移动但不要修改名称)")) 
    //    {
    //        GameObject.DestroyImmediate(GameObject.Find("AFX_PostGrey(可以移动但不要修改名称)"));
    //    }
    //}
    [MenuItem("ADYFX/特效辅助/※主相机跟随场景视角（单次  相机Tag必须是MainCamera） &F1", false, 2011)]
    static void SetMainCam()
    {

        //Camera.main.transform.position = SceneView.lastActiveSceneView.camera.transform.position;
        //Camera.main.transform.rotation = SceneView.lastActiveSceneView.camera.transform.rotation;
        int size = 0;
        foreach (AFX_MainCamFollowSceneCam obj in Object.FindObjectsOfType(typeof(AFX_MainCamFollowSceneCam)))//判断场景中有没有跟随脚本
        {
            size += 1;
        }
        if (size >= 1) //有则拿到所有相机 清除脚本
        {
            foreach (AFX_MainCamFollowSceneCam obj in Object.FindObjectsOfType(typeof(AFX_MainCamFollowSceneCam)))//拿到所有相机 清除脚本
            {
                GameObject.DestroyImmediate(obj.GetComponent<AFX_MainCamFollowSceneCam>());
            }
        }
        else  //没有则可以开始添加
        {
            if (size == 0)
            {
                List<Camera> cams = new List<Camera>();
                List<Camera> maincams = new List<Camera>();
                List<Camera> othercams = new List<Camera>();
                foreach (Camera obj in Object.FindObjectsOfType(typeof(Camera)))//拿到所有相机
                {
                    cams.Add(obj);
                }
                if (cams.Count < 1)
                {
                    Debug.LogError("场景中没有相机！");
                }
                else
                {
                    for (int i = 0; i < cams.Count; i++) //区分相机tag
                    {
                        if (cams[i].gameObject.tag == "MainCamera")
                        {
                            maincams.Add(cams[i]);
                        }
                        else
                        {
                            othercams.Add(cams[i]);
                        }
                    }
                    if (maincams.Count == 1) //有主相机时
                    {
                        GameObject aa = maincams[0].gameObject;
                            aa.transform.position = SceneView.lastActiveSceneView.camera.transform.position;
                        aa.transform.rotation = SceneView.lastActiveSceneView.camera.transform.rotation;
                        Debug.Log(string.Format("<color=#A6A6A6>{0}</color>", "为Tag是MainCamera的相机执行了同步场景视角"));
                    }
                    else if (maincams.Count > 1)  //有多个主相机时
                    {
                        GameObject aa = maincams[0].gameObject;
                            aa.transform.position = SceneView.lastActiveSceneView.camera.transform.position;
                        aa.transform.rotation = SceneView.lastActiveSceneView.camera.transform.rotation;
                        Debug.Log(string.Format("<color=#FF7878>{0}</color>", "找到了多个Tag为MainCamera的相机  自动为第一个找到的相机执行同步场景视角"));
                    }
                    else  //无主相机时
                    {
                        GameObject aa = othercams[0].gameObject;
                            aa.transform.position = SceneView.lastActiveSceneView.camera.transform.position;
                        aa.transform.rotation = SceneView.lastActiveSceneView.camera.transform.rotation;
                        Debug.Log(string.Format("<color=#FF4343>{0}</color>", "没有找到Tag为MainCamera的相机  默认为第一个遍历到的非MainCamera的Tag的相机执行同步场景视角"));
                    
                    }
                }
            }
        }
    }
    [MenuItem("ADYFX/特效辅助/※主相机跟随场景视角（实时  要取消再按一次即可,相机Tag必须是MainCamera） %F1", false, 2012)]
    static void SetMainCam1()
    {
        int size = 0;
        foreach (AFX_MainCamFollowSceneCam obj in Object.FindObjectsOfType(typeof(AFX_MainCamFollowSceneCam)))//判断场景中有没有跟随脚本
        {
            size += 1;
        }
        if(size >= 1) //有则拿到所有相机 清除脚本
        {
            foreach (AFX_MainCamFollowSceneCam obj in Object.FindObjectsOfType(typeof(AFX_MainCamFollowSceneCam)))//拿到所有相机 清除脚本
            {
                GameObject.DestroyImmediate(obj.GetComponent<AFX_MainCamFollowSceneCam>());
            }
        }
        else  //没有则可以开始添加
        {
            if (size == 0) 
            {
                List<Camera> cams = new List<Camera>();
                List<Camera> maincams = new List<Camera>();
                List<Camera> othercams = new List<Camera>();
                foreach (Camera obj in Object.FindObjectsOfType(typeof(Camera)))//拿到所有相机
                {
                    cams.Add(obj);
                }
                if (cams.Count < 1)
                {
                    Debug.LogError("场景中没有相机！");
                }
                else
                {
                    for (int i = 0; i < cams.Count; i++) //区分相机tag
                    {
                        if (cams[i].gameObject.tag == "MainCamera")
                        {
                            maincams.Add(cams[i]);
                        }
                        else
                        {
                            othercams.Add(cams[i]);
                        }
                    }
                    if (maincams.Count == 1) //有主相机时
                    {
                        GameObject aa = maincams[0].gameObject;
                        bool cc = aa.GetComponent<AFX_MainCamFollowSceneCam>();
                        if (cc)
                        {
                            GameObject.DestroyImmediate(aa.GetComponent<AFX_MainCamFollowSceneCam>());
                        }
                        else
                        {
                            aa.AddComponent<AFX_MainCamFollowSceneCam>();
                            Debug.Log(string.Format("<color=#A6A6A6>{0}</color>", "你的场景中有多个相机，当前为Tag是MainCamera的相机执行了同步场景视角，这可能不是你想要的，如果这个相机不对，请把你要设置跟随的相机 Tag设为MainCamera 且场景中只有一个这种Tag的相机。"));
                        }
                    }
                    else if (maincams.Count > 1)  //有多个主相机时
                    {
                        GameObject aa = maincams[0].gameObject;
                        bool cc = aa.GetComponent<AFX_MainCamFollowSceneCam>();
                        if (cc)
                        {
                            GameObject.DestroyImmediate(aa.GetComponent<AFX_MainCamFollowSceneCam>());
                        }
                        else
                        {
                            aa.AddComponent<AFX_MainCamFollowSceneCam>();
                            Debug.Log(string.Format("<color=#FF7878>{0}</color>", "找到了多个Tag为MainCamera的相机  自动为第一个找到的相机执行同步场景视角"));
                        }
                    }
                    else  //无主相机时
                    {
                        GameObject aa = othercams[0].gameObject;
                        bool cc = aa.GetComponent<AFX_MainCamFollowSceneCam>();
                        if (cc)
                        {
                            GameObject.DestroyImmediate(aa.GetComponent<AFX_MainCamFollowSceneCam>());
                        }
                        else
                        {
                            aa.AddComponent<AFX_MainCamFollowSceneCam>();
                            Debug.Log(string.Format("<color=#FF4343>{0}</color>", "没有找到Tag为MainCamera的相机  默认为第一个遍历到的非MainCamera的Tag的相机执行同步场景视角"));
                        }
                    }
                }
            }
        }
    }


    [MenuItem("ADYFX/特效辅助/※自选相机跟随场景视角（实时 ,无论你当前选中的物体是不是相机 都会跟随场景视窗！） %&F1", false, 2013)]
    static void SetMainCam2()
    {
        int size = 0;
        foreach (AFX_MainCamFollowSceneCam obj in Object.FindObjectsOfType(typeof(AFX_MainCamFollowSceneCam)))//判断场景中有没有跟随脚本
        {
            size += 1;
        }
        if (size >= 1) //有则拿到所有相机 清除脚本
        {
            foreach (AFX_MainCamFollowSceneCam obj in Object.FindObjectsOfType(typeof(AFX_MainCamFollowSceneCam)))//拿到所有相机 清除脚本
            {
                GameObject.DestroyImmediate(obj.GetComponent<AFX_MainCamFollowSceneCam>());
            }
        }
        else  //没有则可以开始添加
        {
            GameObject[] aa = Selection.gameObjects;
            bool cc = aa[0].GetComponent<AFX_MainCamFollowSceneCam>();
            if (cc)
            {
                GameObject.DestroyImmediate(aa[0].GetComponent<AFX_MainCamFollowSceneCam>());
            }
            else
            {
                aa[0].AddComponent<AFX_MainCamFollowSceneCam>();
                Debug.Log(string.Format("<color=#A6A6A6>{0}</color>", "你指定了一个相机（或物体）在跟随场景窗口视角  再次按下Ctrl+Alt+F1 即可解除跟随"));
            }
        }
    }
}