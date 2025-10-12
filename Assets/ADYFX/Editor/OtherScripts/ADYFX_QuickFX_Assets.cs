using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADYFX_QuickFX_Assets : ScriptableObject
{
    public List<GameObject> gos = new List<GameObject>();
    public List<string> texts = new List<string>();
    public List<Texture2D> texs = new List<Texture2D>();
    public ADYFX_QuickFX_Assets()
    {
        gos.Add(null);
        texts.Add(null);
        texs.Add(null);
    }
}