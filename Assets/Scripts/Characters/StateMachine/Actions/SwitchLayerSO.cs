using StateMachine;
using StateMachine.ScriptableObjects;
using UnityEngine;
[CreateAssetMenu(fileName = "SwitchLayerSO", menuName = "State Machines/Actions/SwitchLayerSO")]

public class SwitchLayerSO : StateActionSO
{
	[SerializeField] private LayerMask _layerMask;

	public LayerMask LayerMask => _layerMask;

	protected override StateAction CreateAction() => new SwitchLayer();
}

internal class SwitchLayer : StateAction
{
	private SwitchLayerSO _so => (SwitchLayerSO)base.OriginSO;
	private GameObject _obj;
	private int _layer;
	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		_layer = (int)Mathf.Log(_so.LayerMask, 2);
		_obj = stateMachine.gameObject;
	}
	public override void OnStateEnter()
	{
		_obj.layer = _layer;
	}
	public override void OnUpdate()
	{

	}
}