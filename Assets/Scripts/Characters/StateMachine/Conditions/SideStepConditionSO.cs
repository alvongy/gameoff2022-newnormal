using StateMachine;
using StateMachine.ScriptableObjects;
using UnityEngine;

[CreateAssetMenu(fileName = "SideStepCDConditionSO", menuName = "State Machines/Conditions/SideStepCDConditionSO")]
public class SideStepConditionSO : StateConditionSO<SideStepCondition>
{

}
public class SideStepCondition : Condition
{
	MainCharacter Mchar;
	private SideStepConditionSO _originSO => (SideStepConditionSO)base.OriginSO; // The SO this Condition spawned from
    public override void Awake(StateMachine.StateMachine stateMachine)
    {
        Mchar = stateMachine.GetComponent<MainCharacter>(); 
    }
   
	protected override bool Statement() =>Mchar.CanSideStep;
}
