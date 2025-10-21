using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [Header("动画控制")]
    public Animator animator;

    [Header("受击配置")]
    public int maxHitCount = 3;
    private int currentHitCount = 0;

    [Tooltip("受击动画名称列表")]
    public string[] hitAnimations = { "Hit1", "Hit2", "Hit3" };

    [Tooltip("死亡动画名称")]
    public string deathAnimation = "Die";

    private bool isDead = false;

    [Header("移动配置")]
    [Tooltip("移动速度（单位：米/秒）")]
    public float moveSpeed = 2f;

    // 是否暂停移动（受击时）
    private bool isHitStunned = false;

    // 记录受击前的位置（第一次受击时记录）
    private Vector3 hitStartPos;

    void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    void Update()
    {
        // 死亡或受击时不移动
        if (isDead || isHitStunned) return;

        // 沿自身Z轴（正方向）持续移动
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }

    /// <summary>
    /// 被子弹击中时调用
    /// </summary>
    public void OnHit()
    {
        if (isDead) return;

        currentHitCount++;

        if (hitAnimations != null && hitAnimations.Length > 0)
        {
            int index = Random.Range(0, hitAnimations.Length);
            string anim = hitAnimations[index];

            // 只有在第一次进入受击状态时才下沉并记录位置
            if (!isHitStunned)
            {
                isHitStunned = true;
                hitStartPos = transform.position; // 记录受击前位置
                transform.position = hitStartPos + Vector3.down * 1f; // 下沉一次

                StartCoroutine(PlayHitAnimation(anim));
            }
            else
            {
                // 仍在受击中，只刷新动画
                animator.Play(anim, 0, 0f);
            }
        }

        if (currentHitCount >= maxHitCount)
        {
            Die();
        }
    }

    IEnumerator PlayHitAnimation(string animName)
    {
        animator.Play(animName, 0, 0f);

        float hitDuration = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(hitDuration);

        // 动画结束后恢复原位（若尚未死亡）
        if (!isDead)
            transform.position = hitStartPos;

        isHitStunned = false;
    }

    void Die()
    {
        // 如果在受击中，下沉中 → 恢复到原位
        if (isHitStunned)
        {
            transform.position = hitStartPos;
            isHitStunned = false;
        }

        isDead = true;
        animator.Play(deathAnimation, 0, 0f);

        // 延迟销毁
        StartCoroutine(DelayedDestroy(3f));
    }

    IEnumerator DelayedDestroy(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
