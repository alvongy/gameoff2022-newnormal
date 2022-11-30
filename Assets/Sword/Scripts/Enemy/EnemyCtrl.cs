using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyCtrl : Entity
{
    [SerializeField] MMFeedbacks feedbacks;
    protected bool alive;
    public override EntityData Data { get => data; }
    public EntityData_Enemy data;

    public virtual void Init() 
    { 
        alive = true; 
    }

    public override void Miss()
    {
        alive = true;
        feedbacks.PlayFeedbacks(this.transform.position, 0);
    }

    public override void Damaged(int damage)
    {
        int hp = Data.hp;
        Data.hp = Mathf.Max(0, Data.hp - damage);
        int dmgV = Mathf.Max(hp - Data.hp, 0);
        OnDamaged.Invoke(dmgV);
        feedbacks?.PlayFeedbacks(this.transform.position, damage);
    }

    public override void Death()
    {
        alive = false;
        OnDeath.Invoke();
    }
}
