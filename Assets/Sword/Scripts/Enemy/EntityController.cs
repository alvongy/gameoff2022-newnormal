using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class EntityController : MonoBehaviour
{

    public UnityEvent OnAttack;

    public abstract EntityData Data { get; }

    public Vector2 InputVector => _inputVector; 
    protected Vector2 _inputVector;

    [NonSerialized] public Vector3 movementInput;
    [NonSerialized] public Vector3 movementVector;

    [NonSerialized] public bool attackTrigger = false;
    [NonSerialized] public bool rollTrigger = false;
    [NonSerialized] public float moveSpeedMultiply = 1;

    protected float attackTimer;

    public new SpriteRenderer renderer;

    public Animator animator;

    public virtual void Init()
    {
        ResetAttackTimer();
    }

    protected void ResetAttackTimer()
    {
        attackTimer = Data.GetAttackInterval();
    }

    public virtual void RecalculateMovement()
    {
        Vector3 adjustedMovement = new Vector3(_inputVector.x, 0f, _inputVector.y);

        movementInput = adjustedMovement.normalized;
    }

    protected virtual void SetInputVector(Vector2 inputVector)
    {
        _inputVector = inputVector;
    }

    public virtual void Attack() { }
    public virtual void OnAttackEnter() { }
    public virtual void OnAttackExit() { }

    public virtual void OnDamage() { }
}
