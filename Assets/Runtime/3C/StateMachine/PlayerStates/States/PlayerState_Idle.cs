using UnityEngine;
[CreateAssetMenu(menuName = "StateMachine/PlayerState/Idle" , fileName = "PlayerState_Idle")]
public class PlayerState_Idle : PlayerState
{
    public override void Enter()
    {
        //进入此状态后播放idle动画
        base.Enter();
        playerCharacter.SetVelocityXZ(Vector3.zero);
        //Debug.Log("开始idle");
    }

    public override void LogicUpdate()
    {
        //执行逻辑更新，用什么按键时，进入跑步状态
        if (input.Move)
        {
            //Debug.Log("开始退出idle");
            stateMachine.SwitchState(typeof(PlayerState_Run));
        }
        //进入跳起状态
        if (input.Jump)
        {
            stateMachine.SwitchState(typeof(PlayerState_JumpUp));
        }
        //切换掉落状态（脚下没地面）
        if (!playerCharacter.IsGrounded)
        {
            stateMachine.SwitchState(typeof(PlayerState_Fall));
        }
    }

    public override void PhysicUpdate()
    {
        //锁定朝向
        //playerCharacter.LockRotate();
    }
}