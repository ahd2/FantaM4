using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

[CreateAssetMenu(menuName = "StateMachine/PlayerState/Run" , fileName = "PlayerState_Run")]
public class PlayerState_Run : PlayerState
{
    //奔跑速度
    [SerializeField] float runspeed = 5f;
    public override void Enter()
    {
        base.Enter();
        //Debug.Log("开始run");
    }

    public override void LogicUpdate()
    {
        //松开移动键，切换Idle
        if (!input.Move)
        {
            stateMachine.SwitchState(typeof(PlayerState_Idle));
        }
        //切换跳起状态
        if (input.Jump)
        {
            stateMachine.SwitchState(typeof(PlayerState_JumpUp));
        }
        //切换掉落状态（脚下没地面）
        if (!playerCharacter.IsGrounded)
        {
            stateMachine.SwitchState(typeof(PlayerState_CoyoteTime));
        }
    }
    public override void PhysicUpdate()
    {
        playerCharacter.Move(runspeed);
    }
}
