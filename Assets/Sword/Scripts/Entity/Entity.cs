using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Entity : MonoBehaviour
{
    public UnityAction<int> OnDamaged = delegate { };
    public UnityEvent OnDeath ;
    public UnityEvent OnAttack ;

    public abstract EntityData Data { get; }

    public abstract int HP { get; set; }

    public virtual void Recover(int value)
    {
        if (Data != null)
        {
            HP = Mathf.Min(HP + value, Data.TotalHP);
        }
        else
        {
            HP += value;
        }
        if (HP <= 0)
        {
            HP = 0;
            Death();
        }
    }

    public virtual void Miss() { }

    public abstract void Damaged(int damage);

    public abstract void Death();
}
