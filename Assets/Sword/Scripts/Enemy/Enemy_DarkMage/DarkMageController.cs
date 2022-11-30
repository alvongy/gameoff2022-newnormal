using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YK.Game.Events;

public class DarkMageController : EnemyController
{
    [SerializeField] float minDistance;
    [SerializeField] float bulletSpeed;
    protected float sqrMinDistance;

    public GameObject warningAreaParNode;
    public SpriteRenderer warningArea;
    [SerializeField] SpriteRenderer readySign;
    private void Awake()
    {
        entity = GetComponent<Enemy_DarkMage>();
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

            float sqrTemp = sqrDistance - sqrMinDistance;
            if (Mathf.Abs(sqrTemp) > 0.64)
            {
                if(sqrTemp > 0)
                {
                    destinationSetter.SetDestination(character.transform.position);
                }
                else
                {
                    //·´·½Ïò
                    destinationSetter.SetDestination(transform.position + (-v3).normalized);
                }
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

    private void OnDisable()
    {
        attackTrigger = false;
        warningArea.enabled = false;
        readySign.enabled = false;
    }

    public override void Attack()
    {
        GameObject go = ObjectPool.Instantiate(((EntityData_Enemy)Data).enemySO.projectilePrefab, transform.position + Vector3.up * 3, Quaternion.identity);
        go.transform.parent = BattleManager.Instance.transform;
        SWProjectile projectile = go.GetComponent<SWProjectile>();
        projectile.Init(entity, bulletSpeed, warningAreaParNode.transform.right, LayerMask.GetMask("Characters", "Obstacle"));
        entity.OnAttack?.Invoke();
    }

    protected override void TryAttack()
    {
        if (!attackTrigger)
        {
            if (attackTimer < 0)
            {
                attackTrigger = true;
                ResetAttackTimer();
                OnAttack.Invoke();
            }
            else
            {
                attackTimer -= Time.deltaTime;
            }
        }
    }

    public override void OnAttackEnter()
    {
        warningArea.enabled = true;
        readySign.enabled = true;
        warningAreaParNode.transform.right = toCharacter.normalized;
    }

    public override void OnAttackExit()
    {
        warningArea.enabled = false;
        readySign.enabled = false;
    }
}
