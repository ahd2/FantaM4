using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 状态机类持有所有状态类，并对它们进行管理以及更新
/// </summary>
public class StateMachine : MonoBehaviour
{
    IState currentState;
    //用状态类的名字对应其类本身，方便获取状态类
    protected Dictionary<System.Type, IState> stateTable;
    
    /// <summary>
    /// 状态逻辑更新
    /// </summary>
    void Update()
    {
        currentState.LogicUpdate();
        Debug.Log(currentState.GetType());
    }
    /// <summary>
    /// 物理更新
    /// </summary>
    void FixedUpdate()
    {
        currentState.PhysicUpdate();
    }
    /// <summary>
    /// 状态机调用此函数，才会更新逻辑(在碰撞体进入时)
    /// </summary>
    public void EnterTriggerUpdate()
    {
        currentState.EnterTriggerUpdate();
    }
    /// <summary>
    /// 状态机调用此函数，才会更新逻辑(在碰撞体进入时)
    /// </summary>
    public void ExitTriggerUpdate()
    {
        currentState.ExitTriggerUpdate();
    }
    /// <summary>
    /// 状态机进入一个新状态（注意主语）
    /// </summary>
    protected void SwitchOn(IState newState){
        currentState = newState;
        currentState.Enter();
    }
    /// <summary>
    /// 状态机切换一个新状态
    /// </summary>
    protected internal void SwitchState(IState newState){
        //先退出当前状态
        currentState.Exit();
        //进入新状态
        SwitchOn(newState);
    }
    /// <summary>
    /// 重载函数：切换新状态，传入type
    /// </summary>
    protected internal void SwitchState(System.Type newStateType){
        //先退出当前状态
        currentState.Exit();
        //进入新状态
        SwitchOn(stateTable[newStateType]);
    }
}