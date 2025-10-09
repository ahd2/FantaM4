using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject Owner { get; private set; }
    public float lifeTime = 3f;

    Rigidbody rb;
    [Tooltip("贴花预制体（需包含 Decal Projector 组件）")]
    public GameObject decalPrefab; // 拖入你的 Decal 预制体

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // 初始化方法：设置速度与持有者
    public void Init(Vector3 velocity, GameObject owner = null)
    {
        Owner = owner;
        if (rb == null) rb = GetComponent<Rigidbody>();
        rb.velocity = velocity;
    }

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        var other = collision.gameObject;
        // if (other == Owner) return; // 忽略打到自己
        //
        // 获取第一个碰撞点
        ContactPoint contact = collision.contacts[0];
        Vector3 hitPoint = contact.point;
        Vector3 hitNormal = contact.normal;

        // === 1. 生成贴花 ===
        if (decalPrefab != null)
        {
            // 贴花位置：稍微向表面内偏移，避免 Z-fighting
            Vector3 decalPosition = hitPoint + hitNormal * 0.001f;

            // 贴花旋转：使投影方向垂直于表面（Decal Projector 朝 -forward 方向投影）
            Quaternion decalRotation = Quaternion.LookRotation(-hitNormal);

            Instantiate(decalPrefab, decalPosition, decalRotation);
        }

        // 其它扩展：发送消息或调用接口（IColorable / IDamageable）
        Destroy(gameObject);
    }
}
