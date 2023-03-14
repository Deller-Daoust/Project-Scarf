using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public enum FlyState
{
    Idle,Patrol,React,Chase,Attack,Death
}
[Serializable]public class FlySetting
{
    public int health;
    public float damage;
    public float moveSpeed;
    public float chaseSpeed;
    public float idleTime;
    public Transform[] PatrolRange;
    public Transform[] chaseRange;
    public Transform FindPlayer;
    public Animator animator;

}
public class FlyFSM : MonoBehaviour
{
    public FlySetting FlySetting;
    private FlyBase currentState;
    private Dictionary<FlyState, FlyBase> states = new Dictionary<FlyState, FlyBase>();
    void Start()
    {
        states.Add(FlyState.Idle, new FlyIdle());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchState(FlyState type)
    {
        if (currentState != null)
        {
            currentState.OnExit();

        }
        currentState = states[type];
        currentState.OnEnter();
    }
}
