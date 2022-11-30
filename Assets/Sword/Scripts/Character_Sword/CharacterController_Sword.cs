using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class CharacterController_Sword : EntityController
{
    public override EntityData Data => CharacterManager.Instance.characterData;

    public bool CanMove { get => canMove; set => canMove = value; }

    [SerializeField] float radiu;
    [SerializeField] float damageRadiu;
    bool canMove;
    public UnityEvent OnRoll;
   
    public enum State
    {
        None,
        Idle,
        Walking,
        Sprinting,
        Attacking,
        Rolling,
    }

    public State state;                   //µ±Ç°×´Ì¬

    private Character_Sword character;
    [SerializeField] SpriteRenderer readySign;

    private void Awake()
    {
        canMove = true;
           character = GetComponent<Character_Sword>();
        character.OnDamaged += (a) => {
            animator.SetTrigger("Hit");
        };
        //animator = GetComponent<Animator>();
        arr = new Collider[30];
        enemies = new List<EnemyCtrl>(8);
    }

    private void OnEnable()
    {
        InputManager.Instance.MoveEvent += SetInputVector;
        InputManager.Instance.SpaceDownEvent += SetRollTrigger;
    }

    private void OnDisable()
    {
        if (InputManager.Instance) 
        {
            InputManager.Instance.MoveEvent -= SetInputVector;
            InputManager.Instance.SpaceDownEvent -= SetRollTrigger;
        }
    }

    private void Update()
    {
        if (canMove) { RecalculateMovement(); }
   
        if(attackTimer < 0)
        {
            //readySign.enabled = true;
            readySign.gameObject.SetActive(true);
            if (Time.frameCount % 5 == 0 && GetOverlap(radiu) &&!rollTrigger&&canMove)
            {
                string[] randomAttack = { "Attack01", "Attack02", "Attack03", "Attack04" };
                animator.SetTrigger(randomAttack[UnityEngine.Random.Range(0, randomAttack.Length)]);
                ResetAttackTimer();
            }
        }
        else
        {
            readySign.gameObject.SetActive(false);
            //readySign.enabled = false;
            attackTimer -= Time.deltaTime;
        }
    }

    public void SetRollTrigger()
    {
        if (!canMove) { return; }
        if (state == State.Idle || state == State.Walking)
        {
            if (CharacterManager.Instance.RollAmount > 0)
            {
                rollTrigger = true;
                OnRoll?.Invoke();
            }
        }
    }

    Collider[] arr;
    List<EnemyCtrl> enemies;

    bool GetOverlap(float r)
    {
        enemies.Clear();
        int count = Physics.OverlapSphereNonAlloc(transform.position, r, arr, LayerMask.GetMask("Enemy"));
        for (int i = 0; i < count; i++)
        {
            EnemyCtrl enemyCtrl = arr[i].GetComponent<EnemyCtrl>();
            if (enemyCtrl)
            {
                enemies.Add(enemyCtrl);
            }
        }
        return enemies.Count > 0;
    }

    public override void Attack()
    {
        if (GetOverlap(damageRadiu))
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if (BattleManager.Instance.CheckAttake(enemies[i]))
                {
                    OnAttack?.Invoke();
                    BattleManager.Instance.Hit(character, enemies[i]);
                }
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radiu);
    }
#endif
}
