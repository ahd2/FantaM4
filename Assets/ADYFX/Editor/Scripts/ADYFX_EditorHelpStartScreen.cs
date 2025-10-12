using UnityEditor;
using UnityEngine;

	//[InitializeOnLoad]
	public class ADYFX_EditorHelpStartScreen
	{
		static ADYFX_EditorHelpStartScreen()
		{
			EditorApplication.update += Update;//
		    //Debug.Log("自定义Update委托");
		}
		static void Update()
		{
			EditorApplication.update -= Update;
			if (!EditorApplication.isPlayingOrWillChangePlaymode)
			{
			if (Time.realtimeSinceStartup<10) 
			{
				ADYFXHelpStartScreen();
			}
	     	//Debug.Log("我唤起了起始页");
			}
		}
		static void ADYFXHelpStartScreen ()
        {
		ADYFX_EditorHelp.HelpWindow();
         }
}
