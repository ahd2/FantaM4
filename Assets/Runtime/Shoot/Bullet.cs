using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject Owner { get; private set; }
    public float lifeTime = 3f;

    Rigidbody rb;

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
        // var other = collision.gameObject;
        // if (other == Owner) return; // 忽略打到自己
        //
        // // 示例：若目标被标记为 BlueBox，则将它变红
        // if (other.CompareTag("BlueBox"))
        // {
        //     var rend = other.GetComponent<Renderer>();
        //     if (rend != null)
        //     {
        //         // 这里使用 rend.material 会在运行时实例化材质，适合原型/演示
        //         rend.material.color = Color.red;
        //     }
        // }

        // 其它扩展：发送消息或调用接口（IColorable / IDamageable）
        Destroy(gameObject);
    }
}
