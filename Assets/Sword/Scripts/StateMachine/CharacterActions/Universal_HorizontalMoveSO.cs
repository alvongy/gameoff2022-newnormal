using StateMachine;
using StateMachine.ScriptableObjects;
using UnityEngine;

[CreateAssetMenu(fileName = "Universal_HorizontalMove", menuName = "State Machines/Actions/Universal_Horizontal Move")]
public class Universal_HorizontalMoveSO : StateActionSO
{
    protected override StateAction CreateAction() => new Universal_HorizontalMove();
}

public class Universal_HorizontalMove : StateAction
{
    protected new Universal_HorizontalMoveSO OriginSO => (Universal_HorizontalMoveSO)base.OriginSO;
    private Entity entity;
    private EntityController controller;

    public override void Awake(StateMachine.StateMachine stateMachine)
    {
        entity = stateMachine.GetComponent<Entity>();
        controller = stateMachine.GetComponent<EntityController>();
    }

    public override void OnStateEnter()
    {
        controller.animator.SetBool("Move", true);
    }
    public override void OnStateExit()
    {
        controller.animator.SetBool("Move", false);
    }

    public override void OnUpdate()
    {
        controller.movementVector.x = controller.movementInput.x * entity.Data.MoveSpeed * controller.moveSpeedMultiply;
        controller.movementVector.z = controller.movementInput.z * entity.Data.MoveSpeed * controller.moveSpeedMultiply;
    }
}
