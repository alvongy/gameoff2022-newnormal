using StateMachine;
using StateMachine.ScriptableObjects;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyFaceCharacter", menuName = "State Machines/Actions/Enemy Face Character")]
public class EnemyFaceCharacterSO : StateActionSO
{
	public TransformAnchor playerAnchor;

	protected override StateAction CreateAction()
	{
		return new EnemyFaceCharacter();
	}
}
public class EnemyFaceCharacter : StateAction
{
	TransformAnchor _character;
	Transform _actor;
	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		_actor = stateMachine.transform;
		_character = (OriginSO as EnemyFaceCharacterSO).playerAnchor;
	}

	public override void OnUpdate()
	{
		if (_character.isSet)
		{
			Vector3 relativePos = _character.Value.position - _actor.position;
			if (Mathf.Abs(relativePos.sqrMagnitude) < 0.02f)
			{
				return;
			}

			relativePos.y = 0f;
			_actor.rotation = Quaternion.LookRotation(relativePos);
		}
	}
}
