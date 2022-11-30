using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EntityData
{
    public UnityAction OnDataChange;

    public static List<UpgradeAttribute> allAttributes = new List<UpgradeAttribute>()
    {
        UpgradeAttribute.TotalHp,
        UpgradeAttribute.Recover,
        UpgradeAttribute.AttackLowerLimit,
        UpgradeAttribute.AttackUpperLimit,
        UpgradeAttribute.MoveSpeed,
        UpgradeAttribute.AttackSpeed,
        UpgradeAttribute.Defense,
        UpgradeAttribute.Dodge,
        UpgradeAttribute.Vampire,
        UpgradeAttribute.CriticalRate,
        UpgradeAttribute.CriticalDamage,
        UpgradeAttribute.PenetrateDamage,
        UpgradeAttribute.EnemyBasicGold,
    };

    public NumericMetadata this[UpgradeAttribute attribute]
    {
        get
        {
            switch (attribute)
            {
                case UpgradeAttribute.TotalHp:
                    return totalhp;
                case UpgradeAttribute.Recover:
                    return recover;
                case UpgradeAttribute.AttackLowerLimit:
                    return attackLowerLimit;
                case UpgradeAttribute.AttackUpperLimit:
                    return attackUpperLimit;
                case UpgradeAttribute.MoveSpeed:
                    return moveSpeed;
                case UpgradeAttribute.AttackSpeed:
                    return attackSpeed;
                case UpgradeAttribute.Defense:
                    return defense;
                case UpgradeAttribute.Dodge:
                    return dodge;
                case UpgradeAttribute.Vampire:
                    return vampire;
                case UpgradeAttribute.CriticalRate:
                    return criticalRate;
                case UpgradeAttribute.CriticalDamage:
                    return criticalDamage;
                case UpgradeAttribute.PenetrateDamage:
                    return penetrateDamage;
                case UpgradeAttribute.EnemyBasicGold:
                    return enemyBasicGold;
                default:
                    return null;
            }
        }
        private set
        {
            switch (attribute)
            {
                case UpgradeAttribute.TotalHp:
                    totalhp = value;break;
                case UpgradeAttribute.Recover:
                    recover = value; break;
                case UpgradeAttribute.AttackLowerLimit:
                    attackLowerLimit = value; break;
                case UpgradeAttribute.AttackUpperLimit:
                    attackUpperLimit = value; break;
                case UpgradeAttribute.MoveSpeed:
                    moveSpeed = value; break;
                case UpgradeAttribute.AttackSpeed:
                    attackSpeed = value; break;
                case UpgradeAttribute.Defense:
                    defense = value; break;
                case UpgradeAttribute.Dodge:
                    dodge = value; break;
                case UpgradeAttribute.Vampire:
                    vampire = value; break;
                case UpgradeAttribute.CriticalRate:
                    criticalRate = value; break;
                case UpgradeAttribute.CriticalDamage:
                    criticalDamage = value; break;
                case UpgradeAttribute.PenetrateDamage:
                    penetrateDamage = value; break;
            }
        }
    }

    public Dictionary<string, float> counterDic;
    public Dictionary<CHSkillEffect, int> effectDictionary;

    public int TotalHP
    {
        get => (int)totalhp.GetData();
    }
    public NumericMetadata totalhp;

    public int hp;

    protected float recoverBuffer;
    public float Recover
    {
        get => recover.GetData();
    }
    public NumericMetadata recover;

    public float AttackLowerLimit
    {
        get => attackLowerLimit.GetData();
    }
    public NumericMetadata attackLowerLimit;

    public float AttackUpperLimit
    {
        get => attackUpperLimit.GetData();
    }
    public NumericMetadata attackUpperLimit;

    public float MoveSpeed
    {
        get => moveSpeed.GetData();
    }
    public NumericMetadata moveSpeed;

    public float AttackSpeed
    {
        get => attackSpeed.GetData();
    }
    public NumericMetadata attackSpeed;

    public float Defense                                    //·ÀÓù
    {
        get => defense.GetData();
    }
    public NumericMetadata defense;

    public float Dodge                                      //ÉÁ±Ü
    {
        get => dodge.GetData();
    }
    public NumericMetadata dodge;

    public float Vampire                                    //ÎüÑª
    {
        get => vampire.GetData();
    }
    public NumericMetadata vampire;

    public float CriticalRate                               //±©»÷ÂÊ
    {
        get => criticalRate.GetData();
    }
    public NumericMetadata criticalRate;

    public float CriticalDamage                             //±©»÷ÉËº¦
    {
        get => criticalDamage.GetData();
    }
    public NumericMetadata criticalDamage;
    
    public float PenetrateDamage                             //´©Í¸ÉËº¦
    {
        get => penetrateDamage.GetData();
    }
    public NumericMetadata penetrateDamage;

    public float EnemyBasicGold                             //µôÂä½ð±Ò
    {
        get => enemyBasicGold.GetData();
    }

    public NumericMetadata enemyBasicGold;

    protected EntityData() { }

    public void OnTotalHPChange(int value)
    {
        hp = Mathf.Max(1, hp + value);
    }

    public float GetAttackInterval()
    {
        if (AttackSpeed > 0)
        {
            return 100 / AttackSpeed;
        }
        else
        {
            return float.MaxValue;
        }
    }

    public void OnUpdate()
    {
        recoverBuffer += Recover / 10 * Time.deltaTime;
        if (recoverBuffer > 1)
        {
            hp = Mathf.Min(hp + (int)recoverBuffer, TotalHP);
            recoverBuffer -= (int)recoverBuffer;
        }
    }

    public EntityData Copy()
    {
        EntityData entityData = new EntityData();
        foreach(var item in allAttributes)
        {
            entityData[item] = this[item].Copy();
        }
        return entityData;
    }
}
