using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityData_Enemy : EntityData
{
    public EnemySO enemySO;
    public int captureLevel;
    public float probability;

    protected EntityData_Enemy() { }

    static float GetCurrentDifficultyValue(float value, float roundValue, float difficultValue, int enemyLevel)
    {
        return value * enemyLevel * (1 + (enemyLevel - 1) * roundValue) * difficultValue;
    }

    public static EntityData_Enemy GetEnemyData(EnemySO so)
    {
        float roundValue = DifficultyManager.Instance.CurrentDifficulty.Round;
        float difficultValue = DifficultyManager.Instance.CurrentDifficulty.Difficult;
        int enemyLevel = DifficultyManager.Instance.GetActualLevel(so);
        EntityData_Enemy enemyData = new EntityData_Enemy();
        enemyData.enemySO = so;
        enemyData.captureLevel = enemyLevel;
        enemyData.probability = so.Probability/100f;

        enemyData.attackLowerLimit = new NumericMetadata(GetCurrentDifficultyValue(so.AttributeDic[UpgradeAttribute.AttackLowerLimit], roundValue, difficultValue, enemyLevel));
        enemyData.attackUpperLimit = new NumericMetadata(GetCurrentDifficultyValue(so.AttributeDic[UpgradeAttribute.AttackUpperLimit], roundValue, difficultValue, enemyLevel));
        enemyData.totalhp = new NumericMetadata(GetCurrentDifficultyValue(so.AttributeDic[UpgradeAttribute.TotalHp], roundValue, difficultValue, enemyLevel));
        //Debug.LogFormat("EnemyData.TotalHP: enemyId={0},enemyLevel={1},value={2},roundValue={3},difficultValue={4}|Result={5}"
        //    , so.ID, enemyLevel, so.AttributeDic[UpgradeAttribute.TotalHp], roundValue, difficultValue, enemyData.totalhp.Data);
        //Debug.Log("EnemyLog£ºLevel=" + enemyLevel);
        enemyData.hp = enemyData.TotalHP;
        enemyData.recover = new NumericMetadata(so.AttributeDic[UpgradeAttribute.Recover]);
        enemyData.moveSpeed = new NumericMetadata(so.AttributeDic[UpgradeAttribute.MoveSpeed]);
        enemyData.attackSpeed = new NumericMetadata(so.AttributeDic[UpgradeAttribute.AttackSpeed]);
        enemyData.defense = new NumericMetadata(GetCurrentDifficultyValue(so.AttributeDic[UpgradeAttribute.Defense], roundValue, difficultValue, enemyLevel));
        enemyData.dodge = new NumericMetadata(so.AttributeDic[UpgradeAttribute.Dodge]);
        enemyData.vampire = new NumericMetadata(so.AttributeDic[UpgradeAttribute.Vampire]);
        enemyData.criticalRate = new NumericMetadata(so.AttributeDic[UpgradeAttribute.CriticalRate]);
        enemyData.criticalDamage = new NumericMetadata(so.AttributeDic[UpgradeAttribute.CriticalDamage]);
        enemyData.penetrateDamage = new NumericMetadata(so.AttributeDic[UpgradeAttribute.PenetrateDamage]);
        enemyData.enemyBasicGold = new NumericMetadata(so.AttributeDic[UpgradeAttribute.EnemyBasicGold]);

        foreach (var item in EvolutionManager.Instance.evolutionTermDatabase_Learend.database)
        {
            if (item.Value.targetAttributeDic.ContainsKey(TargetObjectEnum.ENEMYS))
            {
                foreach (var kv in item.Value.targetAttributeDic[TargetObjectEnum.ENEMYS])
                {
                    enemyData[kv.Key].Reset(enemyData[kv.Key].Data + kv.Value, 1, 0);
                }
            }
        }
        enemyData.hp = enemyData.TotalHP;
        enemyData.effectDictionary = new Dictionary<CHSkillEffect, int>();
        enemyData.counterDic = new Dictionary<string, float>();

        return enemyData;
    }

    public void Reset(EnemySO so)
    {
        float roundValue = DifficultyManager.Instance.CurrentDifficulty.Round;
        float difficultValue = DifficultyManager.Instance.CurrentDifficulty.Difficult;
        int enemyLevel = DifficultyManager.Instance.GetActualLevel(so);

        enemySO = so;
        captureLevel = enemyLevel;

        attackLowerLimit.Reset(GetCurrentDifficultyValue(so.AttributeDic[UpgradeAttribute.AttackLowerLimit], roundValue, difficultValue, enemyLevel), 1, 0);
        attackUpperLimit.Reset(GetCurrentDifficultyValue(so.AttributeDic[UpgradeAttribute.AttackUpperLimit], roundValue, difficultValue, enemyLevel), 1, 0);
        totalhp.Reset(GetCurrentDifficultyValue(so.AttributeDic[UpgradeAttribute.TotalHp], roundValue, difficultValue, enemyLevel), 1, 0);
        Debug.Log("EnemyLog£ºLevel=" + enemyLevel);
        hp = TotalHP;
        recover.Reset(so.AttributeDic[UpgradeAttribute.Recover], 1, 0);
        moveSpeed.Reset(so.AttributeDic[UpgradeAttribute.MoveSpeed], 1, 0);
        attackSpeed.Reset(so.AttributeDic[UpgradeAttribute.AttackSpeed], 1, 0);
        defense.Reset(GetCurrentDifficultyValue(so.AttributeDic[UpgradeAttribute.Defense], roundValue, difficultValue, enemyLevel), 1, 0);
        dodge.Reset(so.AttributeDic[UpgradeAttribute.Dodge], 1, 0);
        vampire.Reset(so.AttributeDic[UpgradeAttribute.Vampire], 1, 0);
        criticalRate.Reset(so.AttributeDic[UpgradeAttribute.CriticalRate], 1, 0);
        criticalDamage.Reset(so.AttributeDic[UpgradeAttribute.CriticalDamage], 1, 0);
        penetrateDamage.Reset(so.AttributeDic[UpgradeAttribute.PenetrateDamage], 1, 0);

        foreach(var item in EvolutionManager.Instance.evolutionTermDatabase_Learend.database)
        {
            if (item.Value.targetAttributeDic.ContainsKey(TargetObjectEnum.ENEMYS))
            {
                foreach (var kv in item.Value.targetAttributeDic[TargetObjectEnum.ENEMYS])
                {
                    this[kv.Key].Reset(this[kv.Key].Data + kv.Value, 1, 0);
                }
            }
        }
        hp = TotalHP;
        effectDictionary.Clear();
        counterDic.Clear();
    }
}
