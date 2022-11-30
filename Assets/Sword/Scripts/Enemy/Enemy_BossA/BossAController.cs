using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YK.Game.Events;

public class BossAController : EnemyController
{
    //◊Ó–°æ‡¿Î°¢”Œµ¥æ‡¿Î°¢◊∑ª˜æ‡¿Î
    [SerializeField] float minDistance;
    [SerializeField] Vector2 wanderRange;
    [SerializeField] float pursuitDistance;
    [NonSerialized] public float sqrMinDistance;
    [NonSerialized] public Vector2 sqrWanderRange;
    [NonSerialized] public float sqrPursuitDistance;

    [SerializeField] float impactCooldown;
    [NonSerialized] public bool impactSkill;
    [NonSerialized] public bool impacting;
    [NonSerialized] public float impactSkillTimer;

    [SerializeField] float bulletSpeed;
    [SerializeField] float shootCooldown;
    [NonSerialized] public bool shootSkill;
    [NonSerialized] public bool shooting;
    [NonSerialized] public float shootSkillTimer;

    [NonSerialized] public bool canhit;
    [NonSerialized] public new Collider collider;
    public GameObject warningAreaParNode_Impact;
    public SpriteRenderer warningArea_Impact;
    public GameObject warningAreaParNode_Shoot;
    //public SpriteRenderer warningArea_Shoot;

    private void Awake()
    {
        entity = GetComponent<Enemy_BossA>();
        destinationSetter = GetComponent<DestinationSetter>();
        collider = GetComponent<Collider>();
        canhit = false;
        sqrAttackRange = attackRange * attackRange;
        sqrMinDistance = minDistance * minDistance;
        sqrWanderRange = new Vector2(wanderRange.x * wanderRange.x, wanderRange.y * wanderRange.y);
        sqrPursuitDistance = pursuitDistance * pursuitDistance;
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
        if (!impactSkill && !impacting && !chargedTrigger)
        {
            impactSkillTimer -= Time.deltaTime;
            if (impactSkillTimer < 0)
            {
                impactSkill = true;
                impactSkillTimer = impactCooldown;
            }
        }
        if (!shootSkill && !shooting)
        {
            shootSkillTimer -= Time.deltaTime;
            if (shootSkillTimer < 0)
            {
                shootSkill = true;
                shootSkillTimer = shootCooldown;
            }
        }
    }

    public void Shoot()
    {
        Vector3 p = warningAreaParNode_Shoot.transform.right;
        for (int i = 0; i < 8; i++)
        {
            GameObject go = ObjectPool.Instantiate(((EntityData_Enemy)Data).enemySO.projectilePrefab, transform.position + Vector3.up * 3, Quaternion.identity);
            go.transform.parent = BattleManager.Instance.transform;
            SWProjectile projectile = go.GetComponent<SWProjectile>();
            int angel = i * 45;
            Quaternion q;
            q = Quaternion.Euler(Vector3.up * angel);
            projectile.Init(entity, bulletSpeed, (q * p).normalized, LayerMask.GetMask("Characters", "Obstacle"));
            projectile.onBrforeDestroy += () => {

            };
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (impacting && canhit)
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
            else if (other.tag.Equals("Border"))
            {
                impacting = false;
            }
        }
    }

    private void OnEnable()
    {
        impactSkill = true;
        impacting = false;
        impactSkillTimer = impactCooldown;
        shootSkill = true;
        shooting = false;
        shootSkillTimer = shootCooldown;
    }

    private void OnDisable()
    {
        impacting = false;
        shooting = false;
        chargedTrigger = false;
        attackTrigger = false;
        warningArea_Impact.enabled = false;
        warningAreaParNode_Shoot.SetActive(false);
        collider.isTrigger = false;
    }
}
