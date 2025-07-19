using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 基础状态类的实现，用来做玩家状态类的父类。
/// </summary>
public class PlayerState : ScriptableObject,IState
{
    //交叉淡化相关
    [SerializeField]private string stateName;//要在data资源中自己赋值，输入动画名即可
    private int stateHash;
    [SerializeField ,Range(0f,1f)]private float transitionDuration = 0.1f;//交叉淡化时间
    
    //玩家控制器
    protected PlayerCharacter playerCharacter;
    //玩家输入
    protected PlayerInput input;
    //
    protected Animator animator;
    protected PlayerStateMachine stateMachine;
    //动画是否播放完毕（通过进入状态的持续时间是否大于等于动画长度来判断）
    protected bool IsAnimationFinished => StateDuration >= animator.GetCurrentAnimatorStateInfo(0).length;
    protected float StateDuration => Time.time - StateStartTime;//用当前时间-状态开始时间获取状态持续时间
    private float StateStartTime;//记录状态开始时间

    private void OnEnable()
    {
        stateHash = Animator.StringToHash(stateName);
    }

    //初始化动画和状态机
    public void Initialize(Animator animator,PlayerInput input, PlayerStateMachine stateMachine,PlayerCharacter playerCharacter)
    {
        this.animator = animator;
        this.stateMachine = stateMachine;
        this.input = input;
        this.playerCharacter = playerCharacter;
    }
    
    //接口的实现,virtual修饰让其可被子类重写。
    public virtual void Enter()
    {
        //所有子类都要播放动画，在这里统一实现。且加上交叉淡化。
        animator.CrossFade(stateHash,transitionDuration);
        StateStartTime = Time.time;
    }

    public virtual void Exit()
    {
    }

    public virtual void LogicUpdate()
    {
    }

    public virtual void PhysicUpdate()
    {
    }
    public virtual void EnterTriggerUpdate()
    {
    }
    public virtual void ExitTriggerUpdate()
    {
        
    }
}