using UnityEngine;

public class CameraCrossDoor : MonoBehaviour
{
    public Material targetMaterial; // 要修改的材质
    public string parameterName = "_Ref"; // 材质参数名称，比如 "_Color", "_TintColor", "_Metallic" 等
    public int insideColor = 0; // 进入门内时的颜色
    public int outsideColor = 2; // 门外面时的颜色

    private bool isInDoor = false;

    private void OnTriggerEnter(Collider other)
    {
        // 判断是否是主相机
        if (other.CompareTag("MainCamera"))
        {
            int currentColor = targetMaterial.GetInt(parameterName);
            ChangeMaterialParameter(2 - currentColor);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
    }

    private void ChangeMaterialParameter(int newColor)
    {
        if (targetMaterial != null)
        {
            targetMaterial.SetInt(parameterName, newColor);
        }
        else
        {
            Debug.LogWarning("目标材质未设置！");
        }
    }
}