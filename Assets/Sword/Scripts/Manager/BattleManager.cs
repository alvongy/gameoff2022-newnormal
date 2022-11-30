using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoSingleton<BattleManager>
{
    [Title("闪避上限%")]
    [SerializeField] float dodgeMax;
    protected override void OnAwake()
    {
        
    }

    public void OnGameOver()
    {
        for(int i = transform.childCount - 1; i >= 0; i--)
        {
            transform.GetChild(i).GetComponent<SWProjectile>()?.Over();

            Debug.Log("销毁场未销毁的远程怪炮弹");
        }
    }

    public bool CheckAttake(Entity anther)
    {
        if (!anther.gameObject.activeInHierarchy) { return false; }
        return !(Random.Range(0f, 100f) < Mathf.Min(dodgeMax, anther.Data.Dodge));
    }

 

    public bool Hit(Entity attaker, Entity anther)
    {
        float damageValue = Random.Range(attaker.Data.AttackLowerLimit, attaker.Data.AttackLowerLimit + attaker.Data.AttackUpperLimit);
        damageValue = Mathf.Max(damageValue - anther.Data.Defense, 0) + 2 * attaker.Data.PenetrateDamage;
        if(Random.Range(0f, 100f) < attaker.Data.CriticalRate)
        {
            damageValue = damageValue * (1.5f + attaker.Data.CriticalDamage / 100f);
        }
        int hp = anther.HP;
        anther.Damaged((int)damageValue);
        float vampireValue = Mathf.Max(0, (hp - anther.HP)) * (attaker.Data.Vampire / 100f);
        attaker.Recover((int)vampireValue);
        //attaker.OnAttack?.Invoke();
        if (anther.HP > 0)
        {
            return true;
        }
        else
        {
            anther.Death();
            return false;
        }
    }

    public bool Hit(EntityData data, Entity anther)
    {
        float damageValue = Random.Range(data.AttackLowerLimit, data.AttackLowerLimit + data.AttackUpperLimit);
        damageValue = Mathf.Max(damageValue - anther.Data.Defense, 0) + 2 * data.PenetrateDamage;
        if (Random.Range(0f, 100f) < data.CriticalRate)
        {
            damageValue = damageValue * (1.5f + data.CriticalDamage / 100f);
        }
        anther.Damaged((int)damageValue);
        if (anther.HP > 0)
        {
            return true;
        }
        else
        {
            anther.Death();
            return false;
        }
    }

    public bool Hit(float damageValue, Entity anther)
    {
        anther.Damaged((int)damageValue);
        if (anther.HP > 0)
        {
            return true;
        }
        else
        {
            anther.Death();
            return false;
        }
    }
}
