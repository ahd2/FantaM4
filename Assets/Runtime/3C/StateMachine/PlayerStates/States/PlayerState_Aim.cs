using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/PlayerState/Aim", fileName = "PlayerState_Aim")]
public class PlayerState_Aim : PlayerState
{
    public override void Enter()
    {
        base.Enter();
        // 切换到过肩相机
        playerCharacter.EnterAimMode();
        //playerCharacter.fireRate = fireRate; // 可选：同步速率
        playerCharacter.StartFiring();      // 开始持续开火
    }

    public override void LogicUpdate()
    {
        // 松开鼠标右键 → 回 Idle
        if (!input.Aim)
        {
            stateMachine.SwitchState(typeof(PlayerState_Idle));
        }
    }

    public override void PhysicUpdate()
    {
        // 瞄准时禁止移动
        //playerCharacter.SetVelocityXZ(Vector3.zero);

        playerCharacter.LockMove(5.0f);//要改成锁定朝向的run
        // 让角色朝向相机
        Transform cam = Camera.main.transform;
        Vector3 forward = cam.forward;
        forward.y = 0;
        forward.Normalize();
        if (forward.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(forward);
            playerCharacter.transform.rotation =
                Quaternion.Lerp(playerCharacter.transform.rotation, targetRot, Time.fixedDeltaTime * 15f);
        }
    }

    public override void Exit()
    {
        base.Exit();
        playerCharacter.StopFiring();
        // 切回正常相机
        playerCharacter.ExitAimMode();
    }
}