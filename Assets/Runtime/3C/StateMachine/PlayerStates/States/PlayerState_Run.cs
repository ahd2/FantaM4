using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

[CreateAssetMenu(menuName = "StateMachine/PlayerState/Run" , fileName = "PlayerState_Run")]
public class PlayerState_Run : PlayerState
{
    [SerializeField] float runspeed = 10f; // 作为飞行速度使用

    public override void Enter()
    {
        base.Enter();
        // 进入“Run(飞行)”时关闭重力
        playerCharacter.SetUseGravity(false);   // 你已有封装:contentReference[oaicite:3]{index=3}
        playerCharacter.SetVelocity(Vector3.zero);
    }

    public override void Exit()
    {
        base.Exit();
        // 离开“Run(飞行)”时恢复重力
        playerCharacter.SetUseGravity(true);
    }

    public override void LogicUpdate()
    {
        // 没有移动输入就回 Idle（保持你原有逻辑）
        if (!input.Move)
        {
            stateMachine.SwitchState(typeof(PlayerState_Idle));
        }
        // 如仍需要跳跃键切换其它状态，可保留你的逻辑
        if (input.Jump)
        {
            stateMachine.SwitchState(typeof(PlayerState_JumpUp));
        }
    }

    public override void PhysicUpdate()
    {
        // 直接调用 PlayerCharacter 的飞行函数
        playerCharacter.Fly(runspeed);
    }
}