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

    void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    /// <summary>
    /// 被子弹击中时调用
    /// </summary>
    public void OnHit()
    {
        if (isDead) return;

        currentHitCount++;

        Debug.Log("hit1");
        // 播放随机受击动画
        if (hitAnimations != null && hitAnimations.Length > 0)
        {
            Debug.Log("hit");
            int index = Random.Range(0, hitAnimations.Length);
            animator.Play(hitAnimations[index], 0, 0f); // 从头播放
        }

        // 判断是否死亡
        if (currentHitCount >= maxHitCount)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        animator.Play(deathAnimation, 0, 0f); // 从头播放

        // 延迟销毁怪物（可根据死亡动画长度调整）
        StartCoroutine(DelayedDestroy(3f));
    }

    IEnumerator DelayedDestroy(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}