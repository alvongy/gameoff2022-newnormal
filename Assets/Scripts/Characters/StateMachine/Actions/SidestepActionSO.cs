using StateMachine;
using StateMachine.ScriptableObjects;
using UnityEngine;

[CreateAssetMenu(fileName = "Sidestep", menuName = "State Machines/Actions/Sidestep")]
public class SidestepActionSO : StateActionSO<SidestepAction>
{
	[Tooltip("Horizontal XZ plane speed multiplier")]
	public float speed = 8f;
}

public class SidestepAction : StateAction
{
	private MainCharacter _character;
	Vector3 _movement;
	private SidestepActionSO _originSO => (SidestepActionSO)base.OriginSO;

	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		_character = stateMachine.GetComponent<MainCharacter>();
	}

	public override void OnStateEnter()
	{
		_movement.x = Mathf.Sin(_character.transform.rotation.eulerAngles.y * Mathf.Deg2Rad);
		_movement.y = 0f;
		_movement.z = Mathf.Cos(_character.transform.rotation.eulerAngles.y * Mathf.Deg2Rad);
		if (_character.RuntimeAdditionalDic.ContainsKey(CharacterDataType.SideStepDistance))
		{
			_movement *= _originSO.speed + _character.RuntimeAdditionalDic[CharacterDataType.SideStepDistance].GetData();
        }
        else
        {
			_movement *= _originSO.speed;
		}
		_character.RaiseSideStep();
	}


	public override void OnUpdate()
	{
		_character.movementVector.x = _movement.x;
		_character.movementVector.z = _movement.z;
	}
}