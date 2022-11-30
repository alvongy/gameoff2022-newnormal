using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : EntityController
{
    public override EntityData Data => entity.data;

    protected EnemyCtrl entity;
    [NonSerialized] public DestinationSetter destinationSetter;

    [NonSerialized] public bool moveTrigger = false;
    [NonSerialized] public bool chargedTrigger = false;

    [SerializeField] protected float attackRange;
    [NonSerialized] public float sqrAttackRange;

    [NonSerialized] public Vector3 toCharacter;
    [NonSerialized] public float sqrDistance;

    protected virtual void TryAttack()
    {
        if (attackTimer < 0)
        {
            if (CharacterManager.Instance.character)
            {
                if (sqrDistance < sqrAttackRange)
                {
                    attackTrigger = true;
                    OnAttack.Invoke();
                    ResetAttackTimer();
                }
            }
        }
        else if (!attackTrigger)
        {
            attackTimer -= Time.deltaTime;
        }
    }

    public override void Attack()
    {
        Entity character = CharacterManager.Instance.character;
        if (character)
        {
            if (sqrDistance < sqrAttackRange)
            {
                if (BattleManager.Instance.CheckAttake(character))
                {
                    entity.OnAttack?.Invoke();
                    BattleManager.Instance.Hit(entity, character);
                }
                else
                {
                    character.Miss();
                }
            }
        }
    }
}
