using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/PlayerState/Land" , fileName = "PlayerState_Land")]
public class PlayerState_Land : PlayerState
{
    [SerializeField]private float stiffTime = 0.2f;//硬直时间
    [SerializeField] float runspeed = 2.5f;//落地移动速度
    public override void LogicUpdate()
    {
        //进入跳起状态（补，存在跳跃缓冲时也进入跳跃）
        if (input.Jump || input.HasJumpInputBuffer)
        {
            stateMachine.SwitchState(typeof(PlayerState_JumpUp));
        }
        //只有跳跃能打断硬直
        if (StateDuration < stiffTime)
        {
            return;
        }
        //执行逻辑更新，用什么按键时，进入跑步状态
        if (input.Move)
        {
            //Debug.Log("开始退出idle");
            stateMachine.SwitchState(typeof(PlayerState_Run));
        }
        //落地动画播完，进入空闲
        if (IsAnimationFinished)
        {
            stateMachine.SwitchState(typeof(PlayerState_Idle));
        }
    }

    public override void PhysicUpdate()
    {
        playerCharacter.Move(runspeed);
    }
}
