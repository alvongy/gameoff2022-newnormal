using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatController : EnemyController
{
    [SerializeField] float minDistance;
    protected float sqrMinDistance;

    [SerializeField] SpriteRenderer fx;
    [SerializeField] SpriteRenderer warningArea;
    [SerializeField] SpriteRenderer readySign;

    private void Awake()
    {
        entity = GetComponent<Enemy_Bat>();
        destinationSetter = GetComponent<DestinationSetter>();
        sqrAttackRange = attackRange * attackRange;
        sqrMinDistance = minDistance * minDistance;
    }

    private void Update()
    {
        Character_Sword character = CharacterManager.Instance.character;
        if (character)
        {
            Vector3 v3 = character.transform.position - transform.position;
            v3.y = 0;
            toCharacter = v3;
            sqrDistance = toCharacter.sqrMagnitude;
        }
        if (character && sqrDistance > sqrMinDistance)
        {
            destinationSetter.SetDestination(character.transform.position);
            moveTrigger = true;
            destinationSetter.speed = Data.MoveSpeed * moveSpeedMultiply;
        }
        else
        {
            moveTrigger = false;
        }
        TryAttack();
    }

    public void OnDisable()
    {
        moveTrigger = false;
        attackTrigger = false;
        rollTrigger = false;
        fx.enabled = false;
        warningArea.enabled = false;
        readySign.enabled = false;
    }

    public override void OnAttackEnter()
    {
        warningArea.enabled = true;
        readySign.enabled = true;
    }

    public override void OnAttackExit()
    {
        warningArea.enabled = false;
        readySign.enabled = false;
    }

    public override void OnDamage()
    {
        attackTrigger = false;
    }
}
