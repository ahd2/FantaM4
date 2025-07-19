using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "StateMachine/PlayerState/CoyoteTime" , fileName = "PlayerState_CoyoteTime")]
public class PlayerState_CoyoteTime : PlayerState
{
    //土狼时间速度
    [SerializeField] float runspeed = 5f;
    //土狼时间长度
    [SerializeField]private float coyoteTime = 0.1f;
    public override void Enter()
    {
        base.Enter();
        //Debug.Log("开始run");
        //禁用重力防止往下掉
        playerCharacter.SetUseGravity(false);
        playerCharacter.SetVelocityY(0);//不这样设置它还是会往下掉，奇怪（虽然掉得慢）。
    }

    public override void Exit()
    {
        //退出时启用
        playerCharacter.SetUseGravity(true);
    }

    public override void LogicUpdate()
    {
        //切换跳起状态
        if (input.Jump)
        {
            stateMachine.SwitchState(typeof(PlayerState_JumpUp));
        }
        //超过土狼时间或没进行移动，切换掉落状态
        if (StateDuration > coyoteTime || !input.Move)
        {
            stateMachine.SwitchState(typeof(PlayerState_Fall));
        }
    }
    public override void PhysicUpdate()
    {
        playerCharacter.Move(runspeed);
    }
}
