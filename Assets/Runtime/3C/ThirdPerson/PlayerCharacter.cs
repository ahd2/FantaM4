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
        
        // 缓存自身碰撞体，用于之后忽略子弹与自身碰撞
        myColliders = GetComponentsInChildren<Collider>();
    }
    
    void Start()
    {
        ToHideCursor();
        //启用动作表,在这里才是真正启用，逻辑实现在具体类中。
        input.EnableGamePlayInputs();
    }
    
    void Update()
    {
        HandleFiring();
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
    /// <summary>
    /// 根据输入信号来以指定速度移动玩家
    /// </summary>
    public void LockMove(float speed)
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
    }
    
    [SerializeField] private CinemachineFreeLook freeLookCam;
    [SerializeField] private CinemachineFreeLook aimCam;
    
    void LateUpdate()
    {
        if (aimCam == null || freeLookCam == null) return;

        // 谁的 Priority 更高，谁就是当前激活的相机
        CinemachineFreeLook activeCam, inactiveCam;

        if (aimCam.Priority > freeLookCam.Priority)
        {
            activeCam = aimCam;
            inactiveCam = freeLookCam;
        }
        else
        {
            activeCam = freeLookCam;
            inactiveCam = aimCam;
        }

        // 获取激活相机的水平朝向（Yaw）
        Vector3 forward = activeCam.transform.forward;
        forward.y = 0;
        if (forward.sqrMagnitude > 0.01f)
        {
            // 只同步非激活相机，避免写回自身（虽然无害，但更清晰）
            inactiveCam.m_XAxis.Value = activeCam.m_XAxis.Value;
            inactiveCam.m_YAxis.Value = activeCam.m_YAxis.Value;
        }
    }

    private Camera GetActiveCinemachineCamera()
    {
        foreach (var brain in FindObjectsOfType<CinemachineBrain>())
        {
            if (brain.isActiveAndEnabled && brain.OutputCamera != null)
            {
                return brain.OutputCamera;
            }
        }
        return null;
    }
    public void EnterAimMode()
    {
        //SyncFreeLookToCurrentView();
        aimCam.Priority = 20;
        freeLookCam.Priority = 5;
    }

    public void ExitAimMode()
    {
        //SyncFreeLookToCurrentView();
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
    
    [Header("Shooting")]
    public GameObject bulletPrefab;      // 在 Inspector 指向 Bullet Prefab
    public Transform aimMuzzle;          // 人物手臂 muzzle（在场景中拖骨骼或空物体到这里）
    public float bulletSpeed = 50f;
    public float fireRate = 10f;         // 每秒发射次数

// 运行时变量（不要序列化）
    private float fireCooldown = 0f;
    private bool isFiring = false;
    private Collider[] myColliders;
    
    public void StartFiring()
    {
        isFiring = true;
        fireCooldown = 0f;
    }

    public void StopFiring()
    {
        isFiring = false;
    }

    public void FireOnce()
    {
        if (bulletPrefab == null || aimMuzzle == null) return;

        var bulletGo = Instantiate(bulletPrefab, aimMuzzle.position, Quaternion.identity);
        var bulletRb = bulletGo.GetComponent<Rigidbody>();

        // 获取当前实际输出的摄像机（兼容 Cinemachine）
        Camera cam = GetActiveCinemachineCamera() ?? Camera.main;

        // 朝向屏幕中心的方向
        Vector3 dir = cam.ScreenPointToRay(new Vector2(Screen.width * 0.5f, Screen.height * 0.5f)).direction.normalized;

        Vector3 velocity = dir * bulletSpeed;

        if (bulletRb != null)
            bulletRb.velocity = velocity;

        // 忽略子弹和自身碰撞
        var bulletCollider = bulletGo.GetComponent<Collider>();
        if (bulletCollider != null && myColliders != null)
        {
            foreach (var c in myColliders)
                Physics.IgnoreCollision(bulletCollider, c);
        }

        // 如果子弹脚本有 Init，调用它（上文 Bullet.Init）
        var bulletScript = bulletGo.GetComponent<Bullet>();
        if (bulletScript != null) bulletScript.Init(velocity, this.gameObject);
    }
    
    private void HandleFiring()
    {
        if (!isFiring) return;

        fireCooldown -= Time.deltaTime;
        if (fireCooldown <= 0f)
        {
            FireOnce();
            fireCooldown = 1f / Mathf.Max(0.0001f, fireRate);
        }
    }


}