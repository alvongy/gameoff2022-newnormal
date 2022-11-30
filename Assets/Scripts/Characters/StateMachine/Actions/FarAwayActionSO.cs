using StateMachine;
using StateMachine.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using YK.Game.Events;

[CreateAssetMenu(fileName = "FarAwayAction", menuName = "State Machines/Actions/FarAway")]
public class FarAwayActionSO : StateActionSO
{
    protected override StateAction CreateAction() => new FarAwayAction();

}
public class FarAwayAction : StateAction
{
    //private FarAwayActionSO _config;
    private GameObject _obj;
    public override void OnUpdate()
    {       
    }
    public override void Awake(StateMachine.StateMachine stateMachine)
    {
        //_config = (FarAwayActionSO)OriginSO;
        _obj = stateMachine.gameObject;
 
    }
    public override void OnStateEnter()
    {
        ObjectPool.Destroy(_obj);
    }
}