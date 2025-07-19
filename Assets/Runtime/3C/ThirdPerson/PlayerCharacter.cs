using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class PlayerCharacter : MonoBehaviour
{
    #region 变量声明
    //玩家输入
    private PlayerInput input;
    //public CursorManager CursorManager;//鼠标管理类
    private static bool MenuEnable;
    
    #region 操控相关
    //地面检测器
    private GroundDetector _groundDetector;
    //跳跃相关参数
    public bool IsGrounded => _groundDetector.IsGrounded;//为什么是=>？
    public bool IsFalling => rigidbody.velocity.y < 0f && !IsGrounded;//下落且不在地上，则处于下落中
        
    //视角旋转相关参数
    [SerializeField]
    private Photographer _photographer;
    [SerializeField] private Transform _followingTarget;
    //玩家物体朝向信息
    private Quaternion currentRotate;
    //玩家刚体
    Rigidbody rigidbody;

    #endregion

    #region 切换角色相关

    
    #endregion

    #endregion
    
    #region 事件函数
    private void Awake()
    {
        _photographer.InitCamera(_followingTarget);
        //获取input实例
        input = GetComponent<PlayerInput>();
        //获取刚体组件
        rigidbody = GetComponent<Rigidbody>();
        //获取地面检测器组件
        _groundDetector = GetComponentInChildren<GroundDetector>();
    }
    
    void Start()
    {
        //初始化鼠标
        //CursorManager.InitialCursor();
        //Application.targetFrameRate = 60;
        ToHideCursor();
        //启用动作表,在这里才是真正启用，逻辑实现在具体类中。
        input.EnableGamePlayInputs();
        //菜单是否开启
        MenuEnable = false;
    }
    
    void Update()
    {
        //菜单没开启的时候，才进行这些更新
        if (!MenuEnable)
        {
            UpdateCamRotate();//更新相机旋转
            //如果alt键被按下，显示鼠标
            ToHideCursor();
        }
    }
    #endregion

    #region 角色操控相关函数

    //隐藏鼠标
    void ToHideCursor()
    {
        if (input.HideCursor)//名字我命名有误导，其实为真的时候，是按下按键，是显示鼠标
        {
            _photographer.CanRotateCamera = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            _photographer.CanRotateCamera = true;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    private void UpdateCamRotate()
    {
        //实现镜头旋转的逻辑
        Quaternion rot = Quaternion.Euler(0,_photographer.Yaw,0);
    }
    /// <summary>
    /// 根据输入信号来以指定速度移动玩家
    /// </summary>
    public void Move(float speed)
    {
        //获取矫正旋转量
        Quaternion rot = Quaternion.Euler(0, _photographer.Yaw, 0);
        //矫正后正方向
        Vector3 y = rot * Vector3.forward * (input.axes.y);
        Vector3 x = rot * Vector3.right * (input.axes.x);
        SetVelocityXZ(speed * (x + y));
        if (input.Move)
        {
            //调整面朝向的部分
            Quaternion quaDir = Quaternion.LookRotation((x+y), Vector3.up);
            //缓慢转动到目标点
            transform.rotation = Quaternion.Lerp(transform.rotation, quaDir, Time.fixedDeltaTime * 15);
            currentRotate = quaDir;
        }
    }

    #endregion

    #region RigibodyRelate /*刚体相关*/
    //刚体相关函数
    /// <summary>
    /// 直接设置刚体力为输入力
    /// </summary>
    public void SetVelocity(Vector3 velocity)
    {
        rigidbody.velocity = velocity;
    }
    /// <summary>
    /// 设置XZ方向力，适合用来做移动(y方向保持不变)
    /// </summary>
    public void SetVelocityXZ(Vector3 velocity)
    {
        rigidbody.velocity = new Vector3(velocity.x ,rigidbody.velocity.y,velocity.z);
    }
    /// <summary>
    /// 设置Y方向力，适合跳跃
    /// </summary>
    public void SetVelocityY(float velocity)
    {
        rigidbody.velocity = new Vector3(rigidbody.velocity.x, velocity, rigidbody.velocity.z);
    }
    /// <summary>
    /// 设置重力是否启用
    /// </summary>
    /// <param name="value"></param>
    public void SetUseGravity(bool value)
    {
        rigidbody.useGravity = value;
    }
    
    #endregion
    /// <summary>
    /// 如果按下了返回菜单
    /// </summary>
    public void PauseMenuEnable()
    {
        MenuEnable = true;
        //鼠标显示并解锁
        _photographer.CanRotateCamera = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        //切换InputMap
        input.DisableGamePlayInputs();
        input.EnableMenuInputs();
    }
    /// <summary>
    /// 如果退出了返回菜单
    /// </summary>
    public void PauseMenuDisable()
    {
        MenuEnable = false;
        _photographer.CanRotateCamera = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        input.DisableMenuInputs();
        input.EnableGamePlayInputs();
    }
}