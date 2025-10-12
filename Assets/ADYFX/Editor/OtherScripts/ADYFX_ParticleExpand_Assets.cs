using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADYFX_ParticleExpand_Assets : ScriptableObject
{
    public List<GameObject> gos = new List<GameObject>();
    public List<string> texts = new List<string>();
    public List<string> names = new List<string>();
    public ADYFX_ParticleExpand_Assets()
    {
        gos.Add(null);
        texts.Add(null);
        names.Add(null);
    }
}