using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 检测角色脚部是否和地面重叠的检测器
/// </summary>
public class GroundDetector : MonoBehaviour
{
    //检测半径
    [SerializeField]private float detectionRadius = 0.1f;
    private Collider[] _colliders = new Collider[1];
    [SerializeField]private LayerMask groundLayer;
    
    public bool IsGrounded => //转变为主体表达式了
        //返回一个int，即重叠的碰撞体个数。输入参数为检测圆心,半径,存检测结果碰撞体的数组,检测层级
        //注意最后的!=0的判定，让其变为了bool值。ture是就是在地面上。
        Physics.OverlapSphereNonAlloc(transform.position, detectionRadius, _colliders, groundLayer)!=0;
    
    /// <summary>
    /// 在绘制选中物体的gizmos时，顺带绘制我这个的。这个球体是刚好拟合检测器的，但也可以不拟合。
    /// </summary>
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
