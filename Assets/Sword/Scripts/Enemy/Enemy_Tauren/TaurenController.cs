using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaurenController : EnemyController
{
    [SerializeField] float minDistance;
    protected float sqrMinDistance;

    [HideInInspector] public bool canhit;
    [HideInInspector] public new Collider collider;
    public GameObject warningAreaParNode;
    public SpriteRenderer warningArea;
    public SpriteRenderer readySign;
    [HideInInspector] public Vector3 direction;

    private void Awake()
    {
        entity = GetComponent<Enemy_Tauren>();
        destinationSetter = GetComponent<DestinationSetter>();
        collider = GetComponent<Collider>();
        canhit = false;
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

            if (sqrDistance > sqrMinDistance)
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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (attackTrigger && canhit)
        {
            if (other.tag.Equals("Player"))
            {
                Character_Sword character = other.GetComponent<Character_Sword>();
                if (character && BattleManager.Instance.CheckAttake(character))
                {
                    BattleManager.Instance.Hit(entity, character);
                    canhit = false;
                }
            }
            else if(other.tag.Equals("Border"))
            {
                attackTrigger = false;
            }
        }
    }

    private void OnDisable()
    {
        chargedTrigger = false;
        attackTrigger = false;
        warningArea.enabled = false;
        readySign.enabled = false;
        collider.isTrigger = false;
    }

    protected override void TryAttack()
    {
        if (!attackTrigger && !chargedTrigger)
        {
            if (attackTimer < 0)
            {
                if (sqrDistance < sqrAttackRange)
                {
                    direction = toCharacter.normalized;
                    chargedTrigger = true;
                    ResetAttackTimer();
                }
            }
            else
            {
                attackTimer -= Time.deltaTime;
            }
        }
    }
}
