using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
public enum SaveGeshi
{
    JPG = 0,
    PNG = 1,
    TGA = 2,
};

public enum RGBASele
{
    此图取R通道 = 0,
    此图取G通道 = 1,
    此图取B通道 = 2,
    此图取A通道 = 3,
};
public class BoolAndint//布尔和字符串的类  用于在部分情况同时返回bool和string
{
   public bool m_Bool;
   public int m_int;
}

    public class ADYFX_Editor
{
    /// <summary>
    /// 输入一个tex2d的路径 返回一个它对应真正大小的tex2d 不受导入设置的maxsize影响
    /// </summary>
    /// <param name="Tex2dPath"></param>
    /// <returns></returns>
    static public Texture2D GetYuanTu(string Tex2dPath)//复制一份临时文件取得原尺寸颜色
    {        //bytes = File.ReadAllBytes(Tex2dPath);
        //修改meta文件强制改变tex的format为RGBA32 以让tga格式也可以被tex.EncodeToPNG();
        string metatext = File.ReadAllText(Tex2dPath + ".meta");
        string lowmetatext = metatext;
        BoolAndint cc;
        cc = ADYFX_Editor.StringFind(metatext, "textureCompression", true);
        //Debug.Log(cc.m_Bool + "" + cc.m_int);
        string[] tempformat = metatext.Split('\n');
        metatext = metatext.Replace(tempformat[cc.m_int - 1], "    textureFormat: 4");
        string lowformat = tempformat[cc.m_int - 1];
        File.WriteAllText(Tex2dPath + ".meta", metatext);
        AssetDatabase.Refresh();

        // 拿到图像的原始宽高  设置新tex的format为ARGB32以存储Alpha通道并几乎让图像无损失
        Vector2 v2 = GetTexWH(Tex2dPath);
        TextureFormat _texFormat;//贴图设置
        _texFormat = TextureFormat.RGBA32;
        Texture2D ttt = new Texture2D((int)v2.x, (int)v2.y, _texFormat, false);

        // 使用byte在原图修改TextureImporter为maxsize8192之后拿到贴图的byte数据    apply新图像并返回
        byte[] bytes;//生命一个字节文件
        Texture2D tex = ADYFX_Editor.GetTex2D(Tex2dPath);

        TextureImporter tex1 = TextureImporter.GetAtPath(Tex2dPath) as TextureImporter;
        TextureImporterNPOTScale ss = tex1.npotScale;
        tex1.isReadable = true;
        tex1.npotScale = TextureImporterNPOTScale.None;
        tex1.maxTextureSize = 8192;
        AssetDatabase.ImportAsset(Tex2dPath);//应用设置 并刷新资源库
        bytes = tex.EncodeToPNG();
        ttt.LoadImage(bytes);
        ttt.Apply();
        //还原原图的meta文件
        metatext = lowmetatext;
        File.WriteAllText(Tex2dPath + ".meta", metatext);
        AssetDatabase.Refresh();
        return ttt;
    }
    public static Texture2D ByteToTex2d(byte[] bytes, int w = 100, int h = 100)
    {
        Texture2D tex = new Texture2D(w, h);
        tex.LoadImage(bytes);
        return tex;
    }

    public static Texture2D GetFileTex(string filePath, int w = 100, int h = 100)
    {
        if (!File.Exists(filePath))
            return null;
        byte[] imgData = File.ReadAllBytes(filePath);
        return ByteToTex2d(imgData);
    }
    public static byte[] AuthGetFileData(string fileUrl)
    {
        FileStream fs = new FileStream(fileUrl, FileMode.Open, FileAccess.Read);
        byte[] buffur = new byte[fs.Length];

        fs.Read(buffur, 0, buffur.Length);
        fs.Close();
        return buffur;
    }
    public static byte[] ReadTexture(string path)
    {
        Debug.Log(" @ ! the texture path is + !!    " + path);
        FileStream fileStream = new FileStream(path, FileMode.Open, System.IO.FileAccess.Read);

        fileStream.Seek(0, SeekOrigin.Begin);

        byte[] buffer = new byte[fileStream.Length]; //创建文件长度的buffer   
        fileStream.Read(buffer, 0, (int)fileStream.Length);

        fileStream.Close();

        fileStream.Dispose();

        fileStream = null;

        return buffer;
    }

    /// <summary>
    /// 返回库中贴图真正的宽高  而不是受导入设置限制的
    /// </summary>
    /// <param name="Tex2dPath"></param>
    /// <returns></returns>
    static public Vector2 GetTexWH(string Tex2dPath)
    {
        int maxsize = 256;
        Vector2 ttt = new Vector2(512,512);
        Texture2D tex = GetTex2D(Tex2dPath);
        TextureImporter tex1 = TextureImporter.GetAtPath(Tex2dPath) as TextureImporter;
        TextureImporterNPOTScale ss = tex1.npotScale;
        maxsize = tex1.maxTextureSize;
        tex1.isReadable = true;
        tex1.npotScale = TextureImporterNPOTScale.None;
        //tex1.alphaSource = TextureImporterAlphaSource.FromInput;
        tex1.maxTextureSize = 8192;
        AssetDatabase.ImportAsset(Tex2dPath);//应用设置 并刷新资源库
        ttt = new Vector2(tex.width ,tex.height);
        //Debug.Log("贴图尺寸：" + ttt);
        tex1.maxTextureSize = maxsize;
        tex1.npotScale = ss;
        tex1.isReadable = false;
        AssetDatabase.ImportAsset(Tex2dPath);//应用设置 并刷新资源库
        AssetDatabase.Refresh();
        return ttt;
    }

    /// <summary>
    /// 返回一张贴图  需要输入ADYFX_Color_Assets的GUID
    /// </summary>
    /// <param name="guid"></param>
    /// <returns></returns>
    static public Texture2D GetTex_AssetColor(string guid)
    {
        ADYFX_Color_Assets assets = ADYFX_Editor.GetOBJ(guid,true) as ADYFX_Color_Assets;
        Texture2D tex = new Texture2D((int)assets.gifWH.x, (int)assets.gifWH.y);
        //tex.minimumMipmapLevel = 0;
        //tex.anisoLevel = 16;
        //tex.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);
        tex.SetPixels( assets.gifColors[0].colors);
        tex.Apply();
        //EditorSceneManager.OpenScene(scenePath);
        return tex;
    }

    /// <summary>
    /// 打开一个场景
    /// </summary>
    /// <param name="scenePath"></param>
    static public void OpenScene(string scenePath)
    {
        EditorSceneManager.OpenScene(scenePath);
    }
    /// <summary>
    /// 保存当前打开的场景
    /// </summary>
    static public void SaveScene()
    {
        Scene nowScene;
        nowScene = EditorSceneManager.GetActiveScene();
        EditorSceneManager.SaveScene(nowScene);
    }


    /// <summary>
    /// 返回当前引擎打开的累计时间
    /// </summary>
    /// <returns></returns>
    static public float NowEngineTime() 
    {
        return UnityEngine.Time.realtimeSinceStartup;
    }

    /// <summary>
    /// 输入要查找的字符串(通常是脚本或.txt)  再输入要查找的关键字    单独返回bool   或 返回BoolAndString类 同时包含bool和行数
    /// </summary>
    /// <param name="str"></param>
    /// <param name="cankao"></param>
    /// <param name="fanhuiint"></param>
    /// <returns></returns>
    static public BoolAndint StringFind(string str, string cankao, bool fanhuiint) 
    {
        BoolAndint aa = new BoolAndint( );
        aa.m_Bool = false;
        aa.m_int = 0;
        string[] temp = str.Split('\n');
        //List<string> texts = new List<string>();
        for (int i = 0; i < temp.Length; i++)
        {
            if (temp[i].Length >= cankao.Length)
            {
                for (int j = 0; j < temp[i].Length; j++)
                {
                    if ((temp[i].Length - j) < cankao.Length)
                    {
                        //break;
                    }
                    else
                    {
                        if (temp[i].Substring(j, cankao.Length) == cankao)
                        {
                            //Debug.Log(i);
                            aa.m_Bool = true;
                            aa.m_int = i;
                            return aa;
                            //findindx[i] = true;
                            //Debug.Log("第" + i + "个元素 正确");
                        }
                    }
                }
            }
        }
        return aa;
    }
    /// <summary>
    /// 输入要查找的字符串(通常是脚本或.txt)  再输入要查找的关键字(这里按回车分割 每行查找  返回bool,可先判断是否查找成功 再次执行3形参函数以返回查找到的行数)
    /// </summary>
    /// <param name="str"></param>
    /// <param name="cankao"></param>
    /// <returns></returns>
    static public bool  StringFind( string str,string cankao) 
    {
        
        string[] temp = str.Split('\n');
        //List<string> texts = new List<string>();
        for (int i = 0; i < temp.Length; i++)
        {
            if (temp[i].Length >= cankao.Length)
            {
                for (int j = 0; j < temp[i].Length; j++)
                {
                    if ((temp[i].Length - j) < cankao.Length)
                    {
                        //break;
                    }
                    else
                    {
                        if (temp[i].Substring(j, cankao.Length) == cankao)
                        {
                            //Debug.Log(i);
                            return true;
                            //findindx[i] = true;
                            //Debug.Log("第" + i + "个元素 正确");
                        }
                    }
                }
            }
        }
        return false;
    }
    //static public int StringFind(string str, string cankao,bool fanhuiint)
    //{

    //    string[] temp = str.Split('\n');
    //    //List<string> texts = new List<string>();
    //    for (int i = 0; i < temp.Length; i++)
    //    {
    //        if (temp[i].Length >= cankao.Length)
    //        {
    //            for (int j = 0; j < temp[i].Length; j++)
    //            {
    //                if ((temp[i].Length - j) < cankao.Length)
    //                {
    //                    //break;
    //                }
    //                else
    //                {
    //                    if (temp[i].Substring(j, cankao.Length) == cankao)
    //                    {
    //                        //Debug.Log(i);
    //                        return i;
    //                        //findindx[i] = true;
    //                        //Debug.Log("第" + i + "个元素 正确");
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    return 0;
    //}


    ///// <summary>
    ///// unity中的路径转换为Win完整路径  可直接保存的
    ///// </summary>
    ///// <param name="path"></param>
    ///// <returns></returns>
    //static public string PathChange_UnityToWin(string path)
    //{
    //    string zz = "";

    //    return zz;
    //}
    ///// <summary>
    ///// win的路径转换为Unity的Assets起始路径
    ///// </summary>
    ///// <param name="path"></param>
    ///// <returns></returns>
    //static public string PathChange_WinToUnity(string path) 
    //{
    //    string zz = "";

    //    return zz;
    //}

    /// <summary>
    /// 判断是否更换贴图 并在换图时修改上一张贴图的isalpha和次幂设置，输入变量 一个路径存储 一个图 一个布尔值 一个用于存储low次幂的 一个用于存储lowisalpha的bool
    /// </summary>
    /// <param name="lowpath1"></param>
    /// <param name="texture"></param>
    /// <param name="isImporter"></param>
    /// <param name="lowTextureImporterNPOTScale"></param>
    /// <param name="lowTextureImporterIsalpha"></param>
    static public void SetTexNpotScale(string lowpath1,Texture2D texture, bool isImporter, TextureImporterNPOTScale lowTextureImporterNPOTScale, bool lowTextureImporterIsalpha ) 
    {
        if (lowpath1 == ADYFX_Editor.GetPath(texture))
        {//资源未变动时 什么都不做 如果是导入新资源了 则修改它为npotScale.None
            if (isImporter && texture != null)
            {
                TextureImporter tex1 = TextureImporter.GetAtPath(ADYFX_Editor.GetPath(texture)) as TextureImporter;
                if (tex1)
                {
                    tex1.npotScale = TextureImporterNPOTScale.None;
                    tex1.alphaIsTransparency = false;
                    AssetDatabase.ImportAsset(ADYFX_Editor.GetPath(texture));//应用设置 并刷新资源库
                }
                isImporter = false;
            }
        }
        else
        {//资源变动时 记录当前图像设置 在下次换图的时候给它修改回去 
            if (lowpath1 != "")//上一个图像不是空的话 则改回旧图像原本的npotScale
            {
                TextureImporter tex1 = TextureImporter.GetAtPath(lowpath1) as TextureImporter;
                if (tex1)
                {
                    tex1.npotScale = lowTextureImporterNPOTScale;
                    tex1.alphaIsTransparency = lowTextureImporterIsalpha;
                    AssetDatabase.ImportAsset(lowpath1);//应用设置 并刷新资源库
                }
            }
            //先记录新图像的导入设置  再开启isImporter 在下一针改变新贴图的npotScale为none以确保能获取正确的原图尺寸
            if (texture != null)
            {
                TextureImporter tex = TextureImporter.GetAtPath(ADYFX_Editor.GetPath(texture)) as TextureImporter;//low获取当前的贴图导入设置
                if (tex) 
                {
                    lowTextureImporterNPOTScale = tex.npotScale;
                    lowTextureImporterIsalpha = tex.alphaIsTransparency;
                }
                lowpath1 = ADYFX_Editor.GetPath(texture);//low路径记录为当前路径
                isImporter = true;
            }
            else
            {
                lowpath1 = "";
            }
        }
    }
    /// <summary>
    /// 判断路径是否是assets路径  通常输入一个保存时的路径 这个路径是win绝对路径 返回一个bool值 如果是包含assets的路径返回true
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    static public bool PathAssetsPanDuan(string path)
    {
        bool aa = false;
       string[] bb =  path.Split('/');
        for(int i = 0;i< bb.Length;i++)
        {
            if (bb[i] == "Assets")
            {
                aa = true;
            }
        }
        return aa;
    }
    /// <summary>
    /// 设置一个贴图的导入设置为  允许读写 不调整次幂 输入一个参数2 布尔值 以控制是否开启贴图的alphaIsTransparency
    /// </summary>
    /// <param name="path"></param>
    /// <param name="noalpha"></param>
    static public void SetTexImport(string path ,bool noalpha)
    {
        //TextureImporter tex1 = TextureImporter.GetAtPath(ADYFX_Editor.GetPath(yuanTex)) as TextureImporter;
        TextureImporter tex1 = TextureImporter.GetAtPath(path) as TextureImporter;
        tex1.isReadable = true;
        tex1.npotScale = TextureImporterNPOTScale.None;
        tex1.alphaSource = TextureImporterAlphaSource.FromInput;
        if (noalpha == false)
        {
            tex1.alphaIsTransparency = false;
        }
        else
        {
            tex1.alphaIsTransparency = true;
        }

        AssetDatabase.ImportAsset(path);//应用设置 并刷新资源库
    }
    /// <summary>
    /// 设置一个贴图的导入设置为  允许读写 不调整次幂 导入alpha
    /// </summary>
    /// <param name="path"></param>
    static public void SetTexImport(string path) 
    {
        //TextureImporter tex1 = TextureImporter.GetAtPath(ADYFX_Editor.GetPath(yuanTex)) as TextureImporter;
        TextureImporter tex1 = TextureImporter.GetAtPath(path) as TextureImporter;
        tex1.isReadable = true;
        tex1.npotScale = TextureImporterNPOTScale.None;
        tex1.alphaSource = TextureImporterAlphaSource.FromInput;
        tex1.alphaIsTransparency = true;
        AssetDatabase.ImportAsset(path);//应用设置 并刷新资源库
    }
    /// <summary>
    /// 从windows中选择一个贴图导入到adyfx临时目录或自定义目录中  传入一个新的命名  并返回这个贴图在assets中的路径
    /// </summary>
    /// <param name="savename"></param>
    /// <returns></returns>
    static public string ImportSystemTex(string savename)
    {
        string re = "";
        string sourcePath = "";
        string pathsele = EditorUtility.OpenFilePanelWithFilters("选择Windows中的图片 （在【打开】按钮上方切换筛选格式） ", "", new string[] { "png", "png", "tga", "tga", "jpg", "jpg", "dds", "dds" });
        string[] split = pathsele.Split('/');//按反斜杠对字符串进行分割
        if (split.Length >= 2)
        {
            string fileName = split[split.Length - 1];
            for (int i = 0; i < split.Length; i++)
            {
                if (i != split.Length - 1)
                {
                    if (i <= split.Length - 2)
                    {
                        sourcePath += split[i] + @"\";
                    }
                    else
                    {
                        sourcePath += split[i];
                    }
                }
            }
            string sourceFile = Path.Combine(sourcePath, fileName);
            string[] fengename = fileName.Split('.');//按.对字符串进行分割
            if (fengename[1]=="dds")//如果选择了dds的图片 则改成png格式复制进unity  因为System.IO无法正确生成dds格式
            {
                fengename[1] = "png";
            }
            string targetPath = Path.Combine(ADYFX_Editor.GetAssetsToWinPath(@"Assets\ADYFX\Elements\Temp"), savename + "." + fengename[1]);
            string istargetpath = ADYFX_Editor.GetAssetsToWinPath(@"Assets\ADYFX\Elements\Temp");
            if (!Directory.Exists(istargetpath))//判断有没有temp文件夹 如果没有就创建
            {
                Directory.CreateDirectory(istargetpath);
            }
            // 复制文件，TRUE为如果目标目录已存在该文件，则覆盖；FALSE已存在该文件 则取消复制
            File.Copy(sourceFile, targetPath, true);
            AssetDatabase.Refresh();
            re = "Assets/ADYFX/Elements/Temp" + "/" + savename + "." + fengename[1];
            //AssetDatabase.RenameAsset();
            // 移动文件
            //File.Move(sourceFile, destFile);
            //Debug.Log("导入了临时图片  路径为："+ re);
        }
        return re;
    }
    /// <summary>
    /// 从windows中选择一个贴图导入到任意assets目录中  传入一个新的命名
    /// </summary>
    /// <param name="assetsPath"></param>
    /// <param name="savename"></param>
    static public void ImportSystemTex(string assetsPath ,string savename)
    {
        string sourcePath = "";
        string pathsele = EditorUtility.OpenFilePanelWithFilters("选择Windows中的图片 ", "", new string[] { "png", "png", "tga", "tga", "jpg", "jpg", "dds", "dds" });
        string[] split = pathsele.Split('/');//按反斜杠对字符串进行分割
        string fileName = split[split.Length - 1];
        for (int i = 0; i < split.Length; i++)
        {
            if (i != split.Length - 1)
            {
                if (i <= split.Length - 2)
                {
                    sourcePath += split[i] + @"\";
                }
                else
                {
                    sourcePath += split[i];
                }
            }
        }
        string[] apath = assetsPath.Split('/');
        string assetspathtemp = "";
        for (int j = 0;j< apath.Length;j++)
        {
            if (j < apath.Length - 1)
            {
                assetspathtemp += apath[j] + @"\";
            }
            else
            {
                assetspathtemp += apath[j];
            }

        }
        string sourceFile = Path.Combine(sourcePath, fileName);
        string[] fengename = fileName.Split('.');//按.对字符串进行分割
        string targetPath = Path.Combine(assetspathtemp, savename + "." + fengename[1]);
        string istargetpath = ADYFX_Editor.GetAssetsToWinPath(@"Assets\ADYFX\Elements\Temp");
        if (!Directory.Exists(istargetpath))//判断有没有temp文件夹 如果没有就创建
        {
            Directory.CreateDirectory(istargetpath);
        }
        // 复制文件，TRUE为如果目标目录已存在该文件，则覆盖；FALSE已存在该文件 则取消复制
        File.Copy(sourceFile, targetPath, true);
        AssetDatabase.Refresh();
        //AssetDatabase.RenameAsset();
        // 移动文件
        //File.Move(sourceFile, destFile);
    }

    /// <summary>
    /// 取消所有输入框的聚焦
    /// </summary>
    static public void QuXiaoJujiao()
    {
        GUI.FocusControl(null);
    }


    /// <summary>
    /// 用于判断按键状态  输入一个要判断的按键  和判断类型  0 按住 1按下 2抬起
    /// </summary>
    /// <param name="keyCode"></param>
    /// <param name="zhuangtai"></param>
    /// <returns></returns>
    static public bool Anjian(KeyCode keyCode ,int zhuangtai)
    {
        bool aa = false;
        if (zhuangtai == 0)
        {
            if (Event.current.keyCode == keyCode)//按住按键
            {
                aa = true;
            }
        }
        else if (zhuangtai == 1)
        {
            if (Event.current.type == EventType.KeyDown)//按下按键
            {
                if (Event.current.keyCode == keyCode)
                {
                    aa = true;
                }
            }
        }
        else
        {
            if (Event.current.type == EventType.KeyUp)//按键抬起
            {
                if (Event.current.keyCode == keyCode)
                {
                    aa = true;
                }
            }
        }
        return aa;
    } 


    /// <summary>
    /// 判断色彩空间 并set所有shader的全局变量“_ColorSpaceValue”以矫正guieditor的显示效果
    /// </summary>
    static public void SetColorSpaceValue() 
    {
        if (PlayerSettings.colorSpace == ColorSpace.Linear) //判断色彩空间
        {
            Shader.SetGlobalFloat("_ColorSpaceValue", 1 / 2.2f);
        }
        else
        {
            Shader.SetGlobalFloat("_ColorSpaceValue", 1);
        }
    }
    /// <summary>
    /// 输入一个游戏物体以选中它
    /// </summary>
    /// <param name="go"></param>
    public static void SeleHierachyObj(GameObject go)
    {
        Selection.activeObject = go;
    }
    /// <summary>
    /// 输入一个go以选中它
    /// </summary>
    /// <param name="go"></param>
    public static void SeleHierachyObj(Object go)
    {
        Selection.activeObject = go;
    }
    /// <summary>
    /// 在库中框选 并选中一个物体 且打开属性面板
    /// </summary>
    /// <param name="path"></param>
    public static void SeleAssetsObj(string path) 
    {
        Object obj = AssetDatabase.LoadMainAssetAtPath(path);
        Selection.activeObject = obj;
        //var assetObj = AssetDatabase.LoadAssetAtPath<GameObject>(FXpath);//在库中框选当前特效
        EditorGUIUtility.PingObject(obj);
    }
    /// <summary>
    /// 改变目标图片的导入设置为 导入Alpha
    /// </summary>
    /// <param name="path"></param>
    public static void SetTex2DisAlpha(string path) 
    {
        TextureImporter tex1 = TextureImporter.GetAtPath(path) as TextureImporter;
        tex1.isReadable = true;//开启贴图资源的允许读写，准备进行alpha判断
        AssetDatabase.ImportAsset(path);//应用设置 并刷新资源库
        tex1.alphaSource = TextureImporterAlphaSource.FromInput;//设置为导入Alpha
        tex1.alphaIsTransparency = true;//设置为以png的透明像素为Alpha
        tex1.isReadable = false;
        AssetDatabase.ImportAsset(path);//应用设置 并刷新资源库
        //AssetDatabase.Refresh();//刷新资源库
    }
    /// <summary>
    /// 获取原图颜色  返回一个tex2d  （开启贴图的允许读写，获取颜色后 再关闭）
    /// </summary>
    /// <param name="YuanTu"></param>
    /// <returns></returns>
    public static Texture2D GetTexColor_Texture2D(Texture2D YuanTu) 
    {
        Texture2D aa = new Texture2D(YuanTu.width, YuanTu.height, TextureFormat.RGBA32, true);//新建图像的TextureFormat要用新建的 不要用原图的 否则可能不能正常获取YuanTu.GetPixels()
        TextureImporter ti = (TextureImporter)TextureImporter.GetAtPath(AssetDatabase.GetAssetPath(YuanTu));
        ti.isReadable = true;
        //ti.npotScale = TextureImporterNPOTScale.None;//设置2次幂选项 关闭则不设置  开启则有3个选项 取最近、取大、取小
        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(YuanTu));
        aa.SetPixels(YuanTu.GetPixels());
        ti.isReadable = false;
        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(YuanTu));
        return aa;
    }
    /// <summary>
    /// 获取原图颜色  返回颜色数组 （开启贴图的允许读写，获取颜色后 再关闭）
    /// </summary>
    /// <param name="YuanTu"></param>
    /// <returns></returns>
    public static Color[] GetTexColor_Color (Texture2D YuanTu)
        {
        Color[] cc = new Color[YuanTu.width* YuanTu.height];
        TextureImporter ti = (TextureImporter)TextureImporter.GetAtPath(AssetDatabase.GetAssetPath(YuanTu));
        ti.isReadable = true;
        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(YuanTu));
        cc = YuanTu.GetPixels();
        ti.isReadable = false;
        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(YuanTu));
        return cc;
        }
    /// <summary>
    /// 修改一张tex2d的大小，注意 这个方法不能直接修改库中图  要修改 先HuHuoQuTexColor() 拿取库的图片到内存
    /// </summary>
    /// <param name="YuanTu"></param>
    /// <param name="targetWidth"></param>
    /// <param name="targetHeight"></param>
    /// <returns></returns>
    public static Texture2D TextureScale(Texture2D YuanTu, float targetWidth, float targetHeight)
    {
        Texture2D result = new Texture2D((int)targetWidth, (int)targetHeight, YuanTu.format, false);
        for (int i = 0; i < result.height; ++i)
        {
            for (int j = 0; j < result.width; ++j)
            {
                Color newColor = YuanTu.GetPixelBilinear((float)j / (float)result.width, (float)i / (float)result.height);
                result.SetPixel(j, i, newColor);
            }
        }
        result.Apply();
        return result;
    }
    /// <summary>
    /// 输入一个vector4 返回布尔值，XY输入鼠标在X轴上的生效区间（例如鼠标位置大于100小于200像素的位置）ZW输入Y轴区间
    /// </summary>
    /// <param name="要判断的鼠标所在区间"></param>
    /// <returns></returns>
    public static bool IsShuBiaoPos(Vector4 要判断的鼠标所在区间,Vector2 鼠标位置) 
    {
        bool aa = false;
        if (鼠标位置.x > 要判断的鼠标所在区间.x && 鼠标位置.x < 要判断的鼠标所在区间.y&& 鼠标位置.y > 要判断的鼠标所在区间.z&& 鼠标位置.y < 要判断的鼠标所在区间.w) 
        {
             aa = true;
        }
        return aa;
    }
/// <summary>
/// 输入一个图片路径和一个格式或者格式组 进行判断 返回布尔值
/// </summary>
/// <param name="path"></param>
/// <param name="geshi"></param>
/// <returns></returns>
    public static bool JianChaTexGeShi (string path,string geshi)
        {
        bool aa = false;
        string[] split = path.Split('.');//按反斜杠对字符串进行分割
        string[] cc = new string[split.Length];
        for (int i = 0; i < split.Length; i++)
        {
            cc[i] = split[i];
            //Debug.Log(cc[i]);
        }
        if (cc[cc.Length-1] == geshi) 
        {
            aa = true;
        }
        else 
        {
            aa = false;
        }

        return aa;
      }
    public static bool JianChaTexGeShi(string path, string[] geshi)
    {
        bool aa = false;
        string[] split = path.Split('.');//按反斜杠对字符串进行分割
        string[] cc = new string[split.Length];
        for (int i = 0; i < split.Length; i++)
        {
            cc[i] = split[i];
        }
        for (int j = 0; j < geshi.Length; j++) 
        {
            if (cc[cc.Length - 1] == geshi[j]) 
            {
                aa = true;
            }
        }
        return aa;
    }
    /// <summary>
    /// 返回目标路径的格式，这个路径需是assets起始的路径
    /// </summary>
    /// <param name="path"></param>
    /// <param name="geshi"></param>
    /// <returns></returns>
    public static string ReturnTexGeShi(string path, string[] geshi)
    {
        string shuchu = "";
        string[] split = path.Split('.');//按反斜杠对字符串进行分割
        string[] cc = new string[split.Length];
        for (int i = 0; i < split.Length; i++)
        {
            cc[i] = split[i];
        }
        for (int j = 0; j < geshi.Length; j++)
        {
            if (cc[cc.Length - 1] == geshi[j])
            {
                shuchu = geshi[j];
                return shuchu;
            }
        }
        return shuchu;
    }
    /// <summary>
    /// Unity 可保存的格式 （JPG PNG TGA）
    /// </summary>
    /// <returns></returns>
    public static string[] TexSaveGeShi() 
    {
        string[] aa = new string[6];
        aa[0] = "png";
        aa[1] = "PNG";
        aa[2] = "jpg";
        aa[3] = "JPG";
        aa[4] = "TGA";
        aa[5] = "tga";
        return aa;
    }
    /// <summary>
    /// 获取常用图片格式 使用string承接
    /// </summary>
    public static string[] TexGeShis() 
    {
        string[] aa = new string[40];
        aa[0] = "bmp";
        aa[1] = "BMP";
        aa[2] = "jpg";
        aa[3] = "JPG";
        aa[4] = "psd";
        aa[5] = "PSD";
        aa[6] = "psb";
        aa[7] = "PSB";
        aa[8] = "jpeg";
        aa[9] = "JPEG";
        aa[10] = "jpe";
        aa[11] = "JPE";
        aa[12] = "png";
        aa[13] = "PNG";
        aa[14] = "TGA";
        aa[15] = "tga";
        aa[16] = "tif";
        aa[17] = "TIF";
        aa[18] = "gif";
        aa[19] = "GIF";
        aa[20] = "pcx";
        aa[21] = "PCX";
        aa[22] = "exif";
        aa[23] = "EXIF";
        aa[24] = "fpx";
        aa[25] = "FPX";
        aa[26] = "SVG";
        aa[27] = "svg";
        aa[28] = "webp";
        aa[29] = "WEBP";
        aa[30] = "WMF";
        aa[31] = "wmf";
        aa[32] = "iff";
        aa[33] = "IFF";
        aa[34] = "PXR";
        aa[35] = "pxr";
        aa[36] = "dds";
        aa[37] = "DDS";
        aa[38] = "exif";
        aa[39] = "EXIF";
        return aa;
    }
    /// <summary>
    /// 输入路径  返回Object
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static Object GetOBJ(string path)
    {
        Object obj = (Object)AssetDatabase.LoadAssetAtPath(path, typeof(Object));
        return obj;
    }
    /// <summary>
    /// 输入路径  返回Object  通过GUID
    /// </summary>
    /// <param name="guid"></param>
    /// <param name="isGuid"></param>
    /// <returns></returns>
    public static Object GetOBJ(string guid,bool isGuid)
    {
        Object obj = (Object)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guid), typeof(Object));
        return obj;
    }

    /// <summary>
    ///  输入一个Asset起始的路径 获取预制体（通常这个路径由右键文件CopyPath得来）如果拿到了 也可以AssetDatabase.GetAssetPath(obj);获取库中路径
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static GameObject GetGO(string path)
    {
        GameObject texture2D = (GameObject)AssetDatabase.LoadAssetAtPath(path, typeof(GameObject));
        return texture2D;
    }

    /// <summary>
    /// 获取打开WIN10资源管理器的路径 输出string
    /// </summary>
    /// <returns></returns>
    public static string GetSystemPath()
    {
        string path = EditorUtility.OpenFolderPanel("", "", ""); ;
        return path;
    }
    /// <summary>
    /// 输入一个Asset起始的路径 获取贴图（通常这个路径由右键文件CopyPath得来）如果拿到了贴图 也可以AssetDatabase.GetAssetPath(obj);获取库中路径
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static Texture2D GetTex2D(string path)
    {
        Texture2D texture2D = (Texture2D)AssetDatabase.LoadAssetAtPath(path, typeof(Texture2D));
        return texture2D;
    }
    /// <summary>
    /// 使用贴图的GUID获取贴图
    /// </summary>
    /// <param name="guid"></param>
    /// <returns></returns>
    public static Texture2D GetTex2D_GUID(string guid)
    {
        //Texture2D texture2D = (Texture2D)AssetDatabase.LoadAssetAtPath(path, typeof(Texture2D));
        string path = AssetDatabase.GUIDToAssetPath(guid);
        Texture2D texture2D = (Texture2D)AssetDatabase.LoadAssetAtPath(path, typeof(Texture2D));
        return texture2D;
    }
    /// <summary>
    /// 获取物体在资源库中的路径
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static string GetPath(Object obj) 
    {
        string pathAssets = AssetDatabase.GetAssetPath(obj);//获取库中路径
        return pathAssets;
    }
    /// <summary>
    /// 使用一个string承接此结果 获取输入物体（贴图、fbx等）的Win10内绝对路径,重载可以不去除文件名和格式
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static string GetFullPath( Object obj)//使用一个string承接此结果 获取原文件的绝对路径
    {
        string pathAssets = AssetDatabase.GetAssetPath(obj);//获取库中路径
        string path0 = AssetsToSystemPath();//获取系统绝对路径  并去除路径中/后的“Assets”字符
        string path1 = AssetsPath(pathAssets);//原本文件路径去除文件名 再和绝对路径相加 组成完整路径
        string path = path0 + path1;
        return path;
    }

    public static string GetFullPath(Object obj,bool fudaigeshi)//使用一个string承接此结果 获取原文件的绝对路径
    {
        string pathAssets = AssetDatabase.GetAssetPath(obj);//获取库中路径
        string path0 = AssetsToSystemPath();//获取系统绝对路径  并去除路径中/后的“Assets”字符
        //string path1 = AssetsPath(pathAssets);//原本文件路径去除文件名 再和绝对路径相加 组成完整路径
        string path = path0 + pathAssets;
        return path;
    }
    /// <summary>
    /// 保存的同时 获取保存路径 
    /// </summary>
    /// <param name="tex"></param>
    /// <param name="path"></param>
    /// <param name="name1"></param>
    /// <param name="houzhui"></param>
    /// <param name="xuhao"></param>
    /// <param name="saveGeshi"></param>
    /// <param name="debugColor"></param>
    /// <param name="debugstring"></param>
    /// <returns></returns>
    public static string SaveTex_returnPath(Texture2D tex, string path, string name1, string houzhui, int xuhao, SaveGeshi saveGeshi, Color debugColor, string debugstring)
    {
        string aaaa = "";
        byte[] bytes;
        string geshi;
        if (saveGeshi == SaveGeshi.PNG)
        {
            bytes = tex.EncodeToPNG();
            geshi = ".png";
        }
        else if (saveGeshi == SaveGeshi.JPG)
        {
            bytes = tex.EncodeToJPG();
            geshi = ".jpg";
        }
        else
        {
            bytes = tex.EncodeToTGA();
            geshi = ".tga";
        }
        string endname = name1 + houzhui + xuhao;
        File.WriteAllBytes(path + "/" + endname + geshi, bytes);
        AssetDatabase.Refresh();
        string color16jinzhi = ColorUtility.ToHtmlStringRGB(debugColor);
        aaaa = path + "/" + endname + geshi;
        Debug.Log(string.Format("<color=#" + color16jinzhi + ">{0}</color>", debugstring + "  本次输出新文件：" + endname + geshi));
        return aaaa;
    }

    /// <summary>
    /// 输入一个已有完整颜色组的tex2d图，以保存到指定位置。texture2D.ReadPixels获取rt内容、texture2D.GetPixels颜色数组获取一张图的内容
    /// </summary>
    /// <param name="tex"></param>
    /// <param name="path"></param>
    /// <param name="name1"></param>
    /// <param name="houzhui"></param>
    /// <param name="xuhao"></param>
    /// <param name="saveGeshi"></param>
    /// <param name="debugColor"></param>
    /// <param name="debugstring"></param>
    public static void SaveTex(Texture2D tex, string path,string name1,string houzhui,int xuhao, SaveGeshi saveGeshi, Color debugColor,string debugstring)
    {
        string aaaa = "";
        byte[] bytes;
        string geshi;
        if(saveGeshi == SaveGeshi.PNG)
        {
            bytes = tex.EncodeToPNG();
            geshi = ".png";
        }
        else if (saveGeshi == SaveGeshi.JPG)
        {
            bytes = tex.EncodeToJPG();
            geshi = ".jpg";
        }
        else
        {
            bytes = tex.EncodeToTGA();
            geshi = ".tga";
        }
        string endname = name1 + houzhui + xuhao;
        File.WriteAllBytes(path + "/" + endname + geshi, bytes);
        AssetDatabase.Refresh();
        string color16jinzhi = ColorUtility.ToHtmlStringRGB(debugColor);
        aaaa = path + "/" + endname + geshi;
        Debug.Log(string.Format("<color=#"+color16jinzhi+">{0}</color>", debugstring + "  本次输出新文件：" + endname + geshi));
        return ;
    }
    public static void SaveTex(Texture2D tex, string path, string name1,  string saveGeshi, Color debugColor, string debugstring)
    {
        string aaaa = "";
        byte[] bytes;
        string geshi = saveGeshi;
        if (saveGeshi == "png")
        {
            bytes = tex.EncodeToPNG();
            geshi = ".png";
        }
        else if (saveGeshi == "jpg")
        {
            bytes = tex.EncodeToJPG();
            geshi = ".jpg";
        }
        else
        {
            bytes = tex.EncodeToTGA();
            geshi = ".tga";
        }
        string endname = name1;
        File.WriteAllBytes(path + "/" + endname + geshi, bytes);
        AssetDatabase.Refresh();
        string color16jinzhi = ColorUtility.ToHtmlStringRGB(debugColor);
        aaaa = path + "/" + endname + geshi;
        Debug.Log(string.Format("<color=#" + color16jinzhi + ">{0}</color>", debugstring + "  本次输出新文件：" + endname + geshi));
        return;
    }
    public static void SaveTex(Texture2D tex, string path, string name1, string houzhui, SaveGeshi saveGeshi, Color debugColor, string debugstring)
    {
        string aaaa = "";
        byte[] bytes;
        string geshi;
        if (saveGeshi == SaveGeshi.PNG)
        {
            bytes = tex.EncodeToPNG();
            geshi = ".png";
        }
        else if (saveGeshi == SaveGeshi.JPG)
        {
            bytes = tex.EncodeToJPG();
            geshi = ".jpg";
        }
        else
        {
            bytes = tex.EncodeToTGA();
            geshi = ".tga";
        }
        string endname = name1 + houzhui;
        File.WriteAllBytes(path + "/" + endname + geshi, bytes);
        AssetDatabase.Refresh();
        string color16jinzhi = ColorUtility.ToHtmlStringRGB(debugColor);
        aaaa = path + "/" + endname + geshi;
        Debug.Log(string.Format("<color=#" + color16jinzhi + ">{0}</color>", debugstring + "  本次输出新文件：" + endname + geshi));
        return;
    }
    public static void SaveTex(Texture2D tex, string path, string name1, string houzhui, int xuhao, SaveGeshi saveGeshi)
    {
        byte[] bytes;
        string geshi;
        if (saveGeshi == SaveGeshi.PNG)
        {
            bytes = tex.EncodeToPNG();
            geshi = ".png";
        }
        else if (saveGeshi == SaveGeshi.JPG)
        {
            bytes = tex.EncodeToJPG();
            geshi = ".jpg";
        }
        else
        {
            bytes = tex.EncodeToTGA();
            geshi = ".tga";
        }
        string endname = name1 + houzhui + xuhao;
        File.WriteAllBytes(path + "/" + endname + geshi, bytes);
        AssetDatabase.Refresh();
        return;
    }
    /// <summary>
    /// 获取Asset目录在当前系统中的路径
    /// </summary>
    /// <returns></returns>
    static public string AssetsToSystemPath()//获取系统绝对路径
    {
        string patht = Application.dataPath;
        string[] split = patht.Split('/');//按反斜杠对字符串进行分割
        string[] cc = new string[split.Length];
        string newpath = "";
        for (int i = 0; i < split.Length; i++)
        {
            cc[i] = split[i];
            if (i != split.Length - 1)
            {
                if (i != 0)
                {
                    newpath = newpath + "/" + cc[i];
                }
                if (i == 0)
                {
                    newpath = newpath + cc[i];
                }
            }
        }
        newpath = newpath + "/";
        return newpath;
    }
    /// <summary>
    /// 输入一个Asset内的路径 得到一个去除Assets字符的字符串路径
    /// </summary>
    /// <param name="patht"></param>
    /// <returns></returns>
    static string AssetsPath(string patht)//得到文件在资源库中的路径
    {
        string[] split = patht.Split('/');//按反斜杠对字符串进行分割
        string[] cc = new string[split.Length];
        string newpath = "";
        for (int i = 0; i < split.Length; i++)
        {
            cc[i] = split[i];
            if (i != split.Length - 1)
            {
                if (i != 0)
                {
                    newpath = newpath + "/" + cc[i];
                }
                if (i == 0)
                {
                    //newpath = "/" + cc[i];
                    newpath = "" + cc[i];
                }
            }
            //Debug.Log("其一："+cc[i]);
        }
        return newpath;
    }
    /// <summary>
    /// 输入一个win系统的完整路径下的unity目录  输出一个unity的 Assets起始的路径
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
   public  static string WinPathToAssetsPath(string path) 
    {
        bool isxiegang = false;
        bool isassets = false;
        string[] split = path.Split('/');//按反斜杠对字符串进行分割
        string newpath = "";
        for (int i = 0; i < split.Length; i++)
        {
            if (split[i] == "Assets") 
            {
                isassets = true;
            }
            if (isassets == true) 
            {
                if (isxiegang == false)
                {
                    newpath = newpath + split[i];
                    isxiegang = true;
                }
                else 
                {
                    newpath = newpath + "/" + split[i];
                }

            }
        }
        return newpath;
    }

    /// <summary>
    /// 获取当前工程在系统中的目录  这是为System.IO所准备的方法 路径中的斜杠是win中使用的正斜杠【 \ 】参数输入一个Assets起始的文件夹路径（ \ 正斜杠）
    /// </summary>
    /// <param name="assetsPath"></param>
    /// <returns></returns>
    public static string GetAssetsToWinPath(string assetsPath)
    {
        string patht = Application.dataPath;
        string[] split = patht.Split('/');//按反斜杠对字符串进行分割
        string[] cc = new string[split.Length];
        string newpath = "";
        for (int i = 0; i < split.Length; i++)
        {
            cc[i] = split[i];
            if (i != split.Length - 1)
            {
                if (i != 0)
                {
                    newpath = newpath + @"\" + cc[i];
                }
                if (i == 0)
                {
                    newpath = newpath + cc[i];
                }
            }
        }
        newpath = newpath + @"\";
        return newpath+ assetsPath;
    }
}
