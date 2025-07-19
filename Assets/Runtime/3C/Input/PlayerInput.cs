using System.Collections;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    //InputSystem脚本
    InputActions playerInputActions;
    //处理输入逻辑
    //跳跃信号，即按下和松开按键瞬间，它们分别会为ture（这种形式是主体表达式）
    public bool Jump => playerInputActions.GamePlay.Jump.WasPressedThisFrame();
    public bool StopJump => playerInputActions.GamePlay.Jump.WasReleasedThisFrame();
    public bool HasJumpInputBuffer { get; set; }//是否有输入缓存信号
    [SerializeField]private float jumpInputBufferTime = 0.4f;//0.4秒预输入时间
    //预输入协程相关
    private WaitForSeconds waitForBufferSeconds;
    //移动信号
    //移动方向
    public Vector2 axes => playerInputActions.GamePlay.Move.ReadValue<Vector2>();
    //是否移动
    public bool Move => axes.magnitude != 0f;
    //切人输入
    public bool SwitchIce => playerInputActions.GamePlay.SwitchIce.WasPressedThisFrame();
    public bool SwitchWater => playerInputActions.GamePlay.SwitchWater.WasPressedThisFrame();
    public bool SwitchFire => playerInputActions.GamePlay.SwitchFire.WasPressedThisFrame();
    public bool SwitchWind => playerInputActions.GamePlay.SwitchWind.WasPressedThisFrame();
    //鼠标显示输入
    public bool HideCursor => playerInputActions.GamePlay.HideCursor.IsPressed();
    
    //返回菜单输入
    public bool PauseMenu => playerInputActions.GamePlay.Menu.WasPressedThisFrame();//这一帧按下了这个按键

    void Awake()
    {
        //初始化
        playerInputActions = new InputActions();
        waitForBufferSeconds = new WaitForSeconds(jumpInputBufferTime);//初始化预输入等待时间
    }
    /// <summary>
    /// 启动菜单模式输入
    /// </summary>
    public void EnableMenuInputs()
    {
        playerInputActions.Menu.Enable();
    }
    public void DisableMenuInputs()
    {
        playerInputActions.Menu.Disable();
    }
    public void EnableGamePlayInputs()
    {
        playerInputActions.GamePlay.Enable();
    }
    public void DisableGamePlayInputs()
    {
        playerInputActions.GamePlay.Disable();
    }
    /// <summary>
    /// 调用这个函数，首先停止协程，然后开启，开始计时，0.5秒后预输入清空
    /// </summary>
    public void SetJumpInputBufferTimer()
    {
        StopCoroutine(nameof(JumpInputBufferCoroutine));
        StartCoroutine(nameof(JumpInputBufferCoroutine));
    }

    IEnumerator JumpInputBufferCoroutine()
    {
        HasJumpInputBuffer = true;
        yield return waitForBufferSeconds;
        HasJumpInputBuffer = false;
    }
}
