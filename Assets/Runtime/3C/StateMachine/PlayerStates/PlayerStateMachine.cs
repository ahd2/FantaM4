using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : StateMachine
{
    //动画组件，后面各个状态类中对动画组件的操控也是通过这里传过去的动画组件
    //本质上还是在控制同一个动画组件。
    Animator animator;
    //被控制的所有类
    [SerializeField] PlayerState[] States;
    //玩家输入
    PlayerInput input;
    PlayerCharacter playerCharacter;
    void Awake()
    {
        animator = GetComponent<Animator>();
        input = GetComponent<PlayerInput>();
        playerCharacter = GetComponent<PlayerCharacter>();
        //初始化字典，长度就是状态类数量即可
        stateTable = new Dictionary<Type, IState>(States.Length);
        //初始化所有被控制的类
        foreach (PlayerState state in States)
        {
            state.Initialize(animator,input,this,playerCharacter);
            //拿到该状态的Type，存进字典的键
            stateTable.Add(state.GetType(),state);
        }
    }

    void Start()
    {
        //让本状态机以idle状态启动
        SwitchOn(stateTable[typeof(PlayerState_Idle)]);
    }

    
    /// <summary>  
    /// 根据类型返回具体的状态类对象  
    /// </summary>  
    public Dictionary<Type, IState> GetStateObjectDictionary()
    {
        return stateTable;
    }
}