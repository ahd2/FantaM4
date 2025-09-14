using Cinemachine;
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
        ToHideCursor();
        //启用动作表,在这里才是真正启用，逻辑实现在具体类中。
        input.EnableGamePlayInputs();
    }
    
    void Update()
    {
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
        Transform cam = Camera.main.transform;

        // 相机 forward/right，只保留水平分量
        Vector3 forward = cam.forward;
        forward.y = 0;
        forward.Normalize();

        Vector3 right = cam.right;
        right.y = 0;
        right.Normalize();

        // 输入
        Vector3 moveDir = forward * input.axes.y + right * input.axes.x;

        // 应用速度
        SetVelocityXZ(moveDir * speed);

        // 面朝方向
        if (moveDir.sqrMagnitude > 0.01f)
        {
            Quaternion quaDir = Quaternion.LookRotation(moveDir, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, quaDir, Time.fixedDeltaTime * 15);
            currentRotate = quaDir;
        }
    }
    
    [SerializeField] private CinemachineFreeLook freeLookCam;
    [SerializeField] private CinemachineFreeLook aimCam;

    public void EnterAimMode()
    {
        aimCam.Priority = 20;
        freeLookCam.Priority = 5;
    }

    public void ExitAimMode()
    {
        aimCam.Priority = 5;
        freeLookCam.Priority = 10;
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
}