using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class ADYFX_RemovePar : ScriptableWizard
{
    [MenuItem("ADYFX/特效辅助/※移除所选物体上的粒子系统 %&F8", false, 2030)]
    static void RemovePar()
    {
        GameObject[] gos = Selection.gameObjects;
        for(int i = 0;i<gos.Length;i++)
        {
            if (gos[i].gameObject.GetComponent<ParticleSystem>()) 
            {
                DestroyImmediate(gos[i].gameObject.GetComponent<ParticleSystemRenderer>(), true);
                DestroyImmediate(gos[i].gameObject.GetComponent<ParticleSystem>(), true);
                Debug.LogError("你移除了游戏物体： "+ gos[i] .name+ " 的粒子系统组件！");
            }
        }
    }
    [MenuItem("ADYFX/特效辅助/※移除所选物体上发射数为0或没有开启渲染的粒子系统 %F8", false, 2031)]
    static void RemovePar1()
    {
        GameObject[] gos = Selection.gameObjects;
        for (int i = 0; i < gos.Length; i++)
        {
            if (gos[i].gameObject.GetComponent<ParticleSystem>())
            {
                ParticleSystem.EmissionModule emission = gos[i].gameObject.GetComponent<ParticleSystem>().emission;
                ParticleSystemRenderer particleSystemRenderer = gos[i].gameObject.GetComponent<ParticleSystemRenderer>();
                if (emission.enabled == false|| particleSystemRenderer.enabled == false) 
                {
                    DestroyImmediate(gos[i].gameObject.GetComponent<ParticleSystemRenderer>(), true);
                    DestroyImmediate(gos[i].gameObject.GetComponent<ParticleSystem>(), true);
                    Debug.LogError("你移除了游戏物体： " + gos[i].name + " 的粒子系统组件！");
                }
            }
        }
    }
    [MenuItem("ADYFX/特效辅助/※为所选物体添加空粒子系统 %F7", false, 2032)]
    static void RemovePar2()
    {
        GameObject[] gos = Selection.gameObjects;
        for (int i = 0; i < gos.Length; i++)
        {
            if (!gos[i].gameObject.GetComponent<ParticleSystem>())
            {
                gos[i].AddComponent<ParticleSystem>();
                ParticleSystem.EmissionModule emission = gos[i].gameObject.GetComponent<ParticleSystem>().emission;
                ParticleSystemRenderer particleSystemRenderer = gos[i].gameObject.GetComponent<ParticleSystemRenderer>();
                ParticleSystem.ShapeModule shape = gos[i].gameObject.GetComponent<ParticleSystem>().shape;
                ParticleSystem.MainModule main = gos[i].gameObject.GetComponent<ParticleSystem>().main;
                particleSystemRenderer.enabled = false;
                emission.enabled = false;
                shape.enabled = false;
                main.loop = false;
            }
        }
    }
}
