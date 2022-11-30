using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityData_Character : EntityData
{
    public CharacterSO characterSO;
    public Dictionary<EquipPart, List<Equip>> equipDictionary;

    protected EntityData_Character() { }

    public static EntityData_Character GetCharacterData(CharacterSO charaSO)
    {
        EntityData_Character characterData = new EntityData_Character();
        characterData.characterSO = charaSO;
        characterData.totalhp = new NumericMetadata(charaSO.AttributeDic[UpgradeAttribute.TotalHp]);
        characterData.hp = characterData.TotalHP;
        characterData.recover = new NumericMetadata(charaSO.AttributeDic[UpgradeAttribute.Recover]);
        characterData.attackLowerLimit = new NumericMetadata(charaSO.AttributeDic[UpgradeAttribute.AttackLowerLimit]);
        characterData.attackUpperLimit = new NumericMetadata(charaSO.AttributeDic[UpgradeAttribute.AttackUpperLimit]);
        characterData.moveSpeed = new NumericMetadata(charaSO.AttributeDic[UpgradeAttribute.MoveSpeed]);
        characterData.attackSpeed = new NumericMetadata(charaSO.AttributeDic[UpgradeAttribute.AttackSpeed]);
        characterData.defense = new NumericMetadata(charaSO.AttributeDic[UpgradeAttribute.Defense]);
        characterData.dodge = new NumericMetadata(charaSO.AttributeDic[UpgradeAttribute.Dodge]);
        characterData.vampire = new NumericMetadata(charaSO.AttributeDic[UpgradeAttribute.Vampire]);
        characterData.criticalRate = new NumericMetadata(charaSO.AttributeDic[UpgradeAttribute.CriticalRate]);
        characterData.criticalDamage = new NumericMetadata(charaSO.AttributeDic[UpgradeAttribute.CriticalDamage]);
        characterData.penetrateDamage = new NumericMetadata(charaSO.AttributeDic[UpgradeAttribute.PenetrateDamage]);
        characterData.enemyBasicGold = new NumericMetadata(0);

        characterData.effectDictionary = new Dictionary<CHSkillEffect, int>();
        characterData.counterDic = new Dictionary<string, float>();
        characterData.equipDictionary = new Dictionary<EquipPart, List<Equip>>();

        return characterData;
    }

    public void Reset(CharacterSO charaSO)
    {
        OnDataChange = delegate { };
        characterSO = charaSO;
        totalhp.Reset(charaSO.AttributeDic[UpgradeAttribute.TotalHp], 1, 0);
        hp = TotalHP;
        recover.Reset(charaSO.AttributeDic[UpgradeAttribute.Recover], 1, 0);
        attackLowerLimit.Reset(charaSO.AttributeDic[UpgradeAttribute.AttackLowerLimit], 1, 0);
        attackUpperLimit.Reset(charaSO.AttributeDic[UpgradeAttribute.AttackUpperLimit], 1, 0);
        moveSpeed.Reset(charaSO.AttributeDic[UpgradeAttribute.MoveSpeed], 1, 0);
        attackSpeed.Reset(charaSO.AttributeDic[UpgradeAttribute.AttackSpeed], 1, 0);
        defense.Reset(charaSO.AttributeDic[UpgradeAttribute.Defense], 1, 0);
        dodge.Reset(charaSO.AttributeDic[UpgradeAttribute.Dodge], 1, 0);
        vampire.Reset(charaSO.AttributeDic[UpgradeAttribute.Vampire], 1, 0);
        criticalRate.Reset(charaSO.AttributeDic[UpgradeAttribute.CriticalRate], 1, 0);
        criticalDamage.Reset(charaSO.AttributeDic[UpgradeAttribute.CriticalDamage], 1, 0);
        penetrateDamage.Reset(charaSO.AttributeDic[UpgradeAttribute.PenetrateDamage], 1, 0);
        effectDictionary.Clear();
        counterDic.Clear();
        equipDictionary.Clear();
    }

    public void ArmEquip(Equip equip, Equip replaced = null)
    {
        int totalHP = TotalHP;
        if (!equipDictionary.ContainsKey(equip.data.PartCode))
        {
            equipDictionary.Add(equip.data.PartCode, new List<Equip>());
        }
        if (replaced != null)
        {
            equip.MetaKey = replaced.MetaKey;
            foreach (var key in allAttributes)
            {
                this[key].RemoveEffect(replaced.MetaKey);
            }
            equipDictionary[replaced.data.PartCode].Remove(replaced);
        }
        else
        {
            equip.MetaKey = string.Format("Equip_{0}_{1}", equip.data.Part, equipDictionary[equip.data.PartCode].Count);
        }
        int total = characterSO.EquipPartDic[equip.data.PartCode];
        if (total > 0 && total > equipDictionary[equip.data.PartCode].Count)
        {
            foreach (var kv in equip.resultAttributeDic)
            {
                if (allAttributes.Contains(kv.Key)) 
                {
                    this[kv.Key].SetExtraValueDic(equip.MetaKey, kv.Value);
                }
            }
            equipDictionary[equip.data.PartCode].Add(equip);
        }
        OnTotalHPChange(TotalHP - totalHP);
        UI_Manager.Instance.captureEquipPanel.ShowCaptureMessage(equip.data.PartCode);
        OnDataChange.Invoke();
    }

    public void EvolutionAddition(EvolutionTermSO evolutionTerm)
    {
        evolutionTerm.MetaKey = string.Format("EvolutionTerm_{0}_{1}", evolutionTerm.evolutionType, evolutionTerm.ID);

        foreach (var d in evolutionTerm.targetAttributeDic[TargetObjectEnum.PLAYER])
        {
            if (allAttributes.Contains(d.Key))
            {
                this[d.Key].SetExtraValueDic(evolutionTerm.MetaKey, d.Value);
                if (d.Key== UpgradeAttribute.TotalHp)
                {
                    CharacterManager.Instance.character.Recover((int)d.Value);
                }  
            }
        }
    }
}
