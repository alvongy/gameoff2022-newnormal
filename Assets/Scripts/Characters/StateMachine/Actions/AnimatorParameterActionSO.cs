using StateMachine;
using StateMachine.ScriptableObjects;
using UnityEngine;
using Moment = StateMachine.StateAction.SpecificMoment;

[CreateAssetMenu(fileName = "AnimatorParameterAction", menuName = "State Machines/Actions/Set Animator Parameter")]
public class AnimatorParameterActionSO : StateActionSO
{
	public enum ParameterType
	{
		BOOL,
		INT,
		FLOAT,
		TRIGGER,
	}
	public ParameterType parameterType = default;
	public string parameterName = default;

	public bool boolValue = default;
	public int intValue = default;
	public float floatValue = default;

	public Moment timeToRun = default;
	protected override StateAction CreateAction()
	{
		return new AnimatorParameterAction(Animator.StringToHash(parameterName));
	}
}

public class AnimatorParameterAction : StateAction
{
	Animator _animator;
	AnimatorParameterActionSO _originSO => (AnimatorParameterActionSO)base.OriginSO;
	int _parameterIndex;

	public AnimatorParameterAction(int parameterHash)
	{
		_parameterIndex = parameterHash;
	}


	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		_animator = stateMachine.GetComponent<Animator>();
	}

	public override void OnStateEnter()
	{
		if (_originSO.timeToRun == Moment.OnStateEnter)
		{
			SetParameter();
		}
	}

	public override void OnUpdate()
	{
	}

	public override void OnStateExit()
	{
		if (_originSO.timeToRun == Moment.OnStateExit)
		{
			SetParameter();
		}
	}
	private void SetParameter()
	{
		switch (_originSO.parameterType)
		{
			case AnimatorParameterActionSO.ParameterType.BOOL:
				_animator.SetBool(_parameterIndex, _originSO.boolValue);
				break;
			case AnimatorParameterActionSO.ParameterType.INT:
				_animator.SetInteger(_parameterIndex, _originSO.intValue);
				break;
			case AnimatorParameterActionSO.ParameterType.FLOAT:
				_animator.SetFloat(_parameterIndex, _originSO.floatValue);
				break;
			case AnimatorParameterActionSO.ParameterType.TRIGGER:
				_animator.SetTrigger(_parameterIndex);
				break;
		}
	}
}
