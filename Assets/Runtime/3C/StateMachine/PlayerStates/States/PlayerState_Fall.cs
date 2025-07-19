using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/PlayerState/Fall" , fileName = "PlayerState_Fall")]
public class PlayerState_Fall : PlayerState
{
    [SerializeField] float runspeed = 5f;
    [SerializeField] private AnimationCurve speedCurver;//控制下落速度
    public override void LogicUpdate()
    {
        if (playerCharacter.IsGrounded)
        {
            //Debug.Log("1");
            stateMachine.SwitchState(typeof(PlayerState_Land));
        }
        //下落状态按下跳跃键，要么触发二段跳，要么触发输入缓冲
        if (input.Jump)
        {
            input.SetJumpInputBufferTimer();//开启预输入计时
        }
    }

    public override void PhysicUpdate()
    {
        //落下时空中移动
        playerCharacter.Move(runspeed);
        //想实现对掉落速度的完全控制，就在这里实现。
        playerCharacter.SetVelocityY(speedCurver.Evaluate(StateDuration));//用状态持续时间来设置速度
    }
    
    /// <summary>
    /// 改变掉落末速度，近似改变重力
    /// </summary>
    /// <param name="finalFallSpeed"></param>
    public void SetFinalFallSpeed(float finalFallSpeed)
    {
        Keyframe newKeyframe = new Keyframe(0.5f, finalFallSpeed);
        speedCurver.MoveKey(1, newKeyframe);
    }
}
