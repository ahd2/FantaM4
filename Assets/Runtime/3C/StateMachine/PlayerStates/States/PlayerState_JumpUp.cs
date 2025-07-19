using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "StateMachine/PlayerState/JumpUp" , fileName = "PlayerState_JumpUp")]
public class PlayerState_JumpUp : PlayerState
{
    [SerializeField]private float jumpForce = 15.0f;
    [SerializeField] float runspeed = 5f;
    public override void Enter()
    {
        base.Enter();
        playerCharacter.SetVelocityY(jumpForce);
        input.HasJumpInputBuffer = false;//关闭跳跃输入缓冲
    }

    public override void LogicUpdate()
    {
        //往下掉后切换落下状态
        if (/*input.StopJump||*/playerCharacter.IsFalling)
        {
            stateMachine.SwitchState(typeof(PlayerState_Fall));
        }
        //有时候跳起被卡住，就不会进入下落状态，加一个落地则切换落地状态（动画播放完后，不然就会一跳跃就落地）
        if (IsAnimationFinished&&playerCharacter.IsGrounded)
        {
            //Debug.Log("1");
            stateMachine.SwitchState(typeof(PlayerState_Land));
        }
    }

    public override void PhysicUpdate()
    {
        playerCharacter.Move(runspeed);
    }
    /// <summary>
    /// 改变弹跳力函数
    /// </summary>
    public void SetJumpForce(float jumpForce)
    {
        this.jumpForce = jumpForce;
    }
}
