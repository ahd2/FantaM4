/// <summary>
/// 状态的接口，实现为多种状态
/// </summary>
public interface IState
{
    /// <summary>
    /// 状态的进入
    /// </summary>
    void Enter();
    /// <summary>
    /// 状态的退出
    /// </summary>
    void Exit();
    /// <summary>
    /// 逻辑更新
    /// </summary>
    void LogicUpdate();
    /// <summary>
    /// 物理更新
    /// </summary>
    void PhysicUpdate();
    /// <summary>
    /// 调用某个函数才会触发更新。（碰撞体进入）
    /// </summary>
    void EnterTriggerUpdate();
    /// <summary>
    /// 调用某个函数才会触发更新。（碰撞体退出）
    /// </summary>
    void ExitTriggerUpdate();
}
