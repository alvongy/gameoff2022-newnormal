using StateMachine;
using StateMachine.ScriptableObjects;
using UnityEngine;
[CreateAssetMenu(fileName = "RotateAction", menuName = "State Machines/Actions/Rotate")]
public class RotateActionSO : StateActionSO
{
	[SerializeField] private float _rotateSpeed = 10f;

	public float RotateSpeed => _rotateSpeed;

	protected override StateAction CreateAction() => new RotateAction();
}
public class RotateAction : StateAction
{
	MainCharacter _mainCharacter;
	Transform _transform;
	RotateActionSO _originSO => (RotateActionSO)base.OriginSO;
	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		_mainCharacter = stateMachine.GetComponent<MainCharacter>();
		_transform = stateMachine.transform;
	}

	public override void OnUpdate()
	{
		//_transform.eulerAngles = Vector3.up * Mathf.Atan2(_mainCharacter.movementVector.x, _mainCharacter.movementVector.z) * Mathf.Rad2Deg;
		if (_mainCharacter.movementVector.sqrMagnitude > 0.1f)
		{
			_transform.rotation = Quaternion.Lerp(_transform.rotation, Quaternion.LookRotation(new Vector3(_mainCharacter.movementVector.x, 0f, _mainCharacter.movementVector.z)), _originSO.RotateSpeed * Time.deltaTime);
		}
	}
}
