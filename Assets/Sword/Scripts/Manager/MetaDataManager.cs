using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using UnityEngine.Events;
using System.IO;
using Excel;
using Sirenix.OdinInspector;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MetaDataManager : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] CharacterDatabase characterDatabase;
    [SerializeField] EnemyDatabase enemyDatabase;
    [SerializeField] EquipDatabase equipDatabase;
    [SerializeField] CaptureDataSO captureData;
    [SerializeField] DegreeOfDifficultyDatabase difficultyData;

    [Button]
    public void Load()
    {
        Import("/Excel/character.xlsx", "character", ImportCharacter);
        Import("/Excel/monster.xlsx", "base_attribute", ImportEnemy);
        Import("/Excel/equipment.xlsx", "equipment", ImportEquipAttrParticulars);
        Import("/Excel/equipment.xlsx", "equipment_attribute_ratio", ImportEquipAttrWeight);
        Import("/Excel/attributesetting.xlsx", "level_major_attribute", ImportFirstAttributeValue);
        Import("/Excel/attributesetting.xlsx", "level_vice_attribute", ImportSecondAttributeValue);
        Import("/Excel/attributesetting.xlsx", "level_major_attribute_error", ImportFirstAttributeRange);
        Import("/Excel/attributesetting.xlsx", "level_vice_attribute_error", ImportSecondAttributeRange);
        Import("/Excel/attributesetting.xlsx", "level_quality", ImportProductQualityDistribution);
        Import("/Excel/attributesetting.xlsx", "attribute_score", ImportAttributeScore);
        Import("/Excel/levelcoefficient.xlsx", "level_coefficient", ImportLevelCoefficient);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    int GetIndex(string s) { return s[0] - 'A'; }

    public void Import(string path, string page, UnityAction<DataTable> action)
    {
        IExcelDataReader excelDataReader;
        using (FileStream fileStream = File.Open(Application.streamingAssetsPath + path, FileMode.Open, FileAccess.Read))
        {
            excelDataReader = ExcelReaderFactory.CreateOpenXmlReader(fileStream);
        }
        DataSet result = excelDataReader.AsDataSet();
        action.Invoke(result.Tables[page]);
    }

    /// <summary>
    /// 导入角色
    /// </summary>
    /// <param name="dataTable"></param>
    public void ImportCharacter(DataTable dataTable)
    {
        DataRow row = null;
        for (int i = 2; i < dataTable.Rows.Count; i++)
        {
            row = dataTable.Rows[i];
            int id = int.Parse(dataTable.Rows[i][GetIndex("A")].ToString());
            if (characterDatabase.database.ContainsKey(id))
            {
                CharacterSO so = characterDatabase.database[id];
                so.AttributeDic = new Dictionary<UpgradeAttribute, float>()
                    {
                        { UpgradeAttribute.TotalHp, float.Parse(dataTable.Rows[i][GetIndex("C")].ToString()) },
                        { UpgradeAttribute.Recover, float.Parse(dataTable.Rows[i][GetIndex("D")].ToString()) },
                        { UpgradeAttribute.AttackLowerLimit, float.Parse(dataTable.Rows[i][GetIndex("E")].ToString()) },
                        { UpgradeAttribute.AttackUpperLimit, float.Parse(dataTable.Rows[i][GetIndex("F")].ToString()) },
                        { UpgradeAttribute.MoveSpeed, float.Parse(dataTable.Rows[i][GetIndex("G")].ToString()) },
                        { UpgradeAttribute.AttackSpeed, float.Parse(dataTable.Rows[i][GetIndex("H")].ToString()) },
                        { UpgradeAttribute.Defense, float.Parse(dataTable.Rows[i][GetIndex("I")].ToString()) },
                        { UpgradeAttribute.Dodge, float.Parse(dataTable.Rows[i][GetIndex("J")].ToString()) },
                        { UpgradeAttribute.Vampire, float.Parse(dataTable.Rows[i][GetIndex("K")].ToString()) },
                        { UpgradeAttribute.CriticalRate, float.Parse(dataTable.Rows[i][GetIndex("L")].ToString()) },
                        { UpgradeAttribute.CriticalDamage, float.Parse(dataTable.Rows[i][GetIndex("M")].ToString()) },
                        { UpgradeAttribute.PenetrateDamage, float.Parse(dataTable.Rows[i][GetIndex("N")].ToString()) },
                    };
                string partsStr = row[GetIndex("O")].ToString();
                string[] parts = partsStr.Split(';');
                so.EquipPartDic.Clear();
                for (int j = 0; j < parts.Length; j++)
                {
                    EquipPart part = (EquipPart)int.Parse(parts[j]);
                    if (so.EquipPartDic.ContainsKey(part))
                    {
                        so.EquipPartDic[part]++;
                    }
                    else
                    {
                        so.EquipPartDic.Add(part, 1);
                    }
                }
                EditorUtility.SetDirty(so);
            }
        }
    }

    /// <summary>
    /// 导入敌人
    /// </summary>
    /// <param name="dataTable"></param>
    public void ImportEnemy(DataTable dataTable)
    {
        DataRow row = null;
        for (int i = 2; i < dataTable.Rows.Count; i++)
        {
            row = dataTable.Rows[i];
            int id = int.Parse(dataTable.Rows[i][GetIndex("A")].ToString());
            if (enemyDatabase.database.ContainsKey(id))
            {
                EnemySO so = enemyDatabase.database[id];
                so.AttributeDic = new Dictionary<UpgradeAttribute, float>()
                    {
                        { UpgradeAttribute.TotalHp, float.Parse(dataTable.Rows[i][GetIndex("C")].ToString()) },
                        { UpgradeAttribute.Recover, float.Parse(dataTable.Rows[i][GetIndex("D")].ToString()) },
                        { UpgradeAttribute.AttackLowerLimit, float.Parse(dataTable.Rows[i][GetIndex("E")].ToString()) },
                        { UpgradeAttribute.AttackUpperLimit, float.Parse(dataTable.Rows[i][GetIndex("F")].ToString()) },
                        { UpgradeAttribute.MoveSpeed, float.Parse(dataTable.Rows[i][GetIndex("G")].ToString()) },
                        { UpgradeAttribute.AttackSpeed, float.Parse(dataTable.Rows[i][GetIndex("H")].ToString()) },
                        { UpgradeAttribute.Defense, float.Parse(dataTable.Rows[i][GetIndex("I")].ToString()) },
                        { UpgradeAttribute.Dodge, float.Parse(dataTable.Rows[i][GetIndex("J")].ToString()) },
                        { UpgradeAttribute.Vampire, float.Parse(dataTable.Rows[i][GetIndex("K")].ToString()) },
                        { UpgradeAttribute.CriticalRate, float.Parse(dataTable.Rows[i][GetIndex("L")].ToString()) },
                        { UpgradeAttribute.CriticalDamage, float.Parse(dataTable.Rows[i][GetIndex("M")].ToString()) },
                        { UpgradeAttribute.PenetrateDamage, float.Parse(dataTable.Rows[i][GetIndex("N")].ToString()) },
                        { UpgradeAttribute.EnemyBasicGold, float.Parse(dataTable.Rows[i][GetIndex("P")].ToString()) },
                    };
                so.Probability = int.Parse(dataTable.Rows[i][GetIndex("O")].ToString());
                EditorUtility.SetDirty(so);
            }
        }
    }

    /// <summary>
    /// 导入各类装备属性槽
    /// </summary>
    /// <param name="dataTable"></param>
    public void ImportEquipAttrParticulars(DataTable dataTable)
    {
        DataRow row = null;
        for (int i = 2; i < dataTable.Rows.Count; i++)
        {
            row = dataTable.Rows[i];
            int id = int.Parse(dataTable.Rows[i][GetIndex("A")].ToString());
            if (equipDatabase.database.ContainsKey(id))
            {
                equipDatabase.database[id].PartCode = (EquipPart)int.Parse(dataTable.Rows[i][GetIndex("B")].ToString());
                equipDatabase.database[id].Part = CaptureManager.Instance.LocalizedMetaName_EquipPart[(int)equipDatabase.database[id].PartCode];
                string s = dataTable.Rows[i][GetIndex("C")].ToString();
                string[] arr = s.Split(';');
                equipDatabase.database[id].FirstAttributeList = new List<UpgradeAttribute>();
                for (int j = 0; j < arr.Length; j++)
                {
                    UpgradeAttribute attribute = (UpgradeAttribute)int.Parse(arr[j]);
                    if (attribute != UpgradeAttribute.None)
                    {
                        equipDatabase.database[id].FirstAttributeList.Add(attribute);
                    }
                }
                s = dataTable.Rows[i][GetIndex("D")].ToString();
                arr = s.Split(';');
                equipDatabase.database[id].SecondAttributeList = new List<UpgradeAttribute>();
                for (int j = 0; j < arr.Length; j++)
                {
                    UpgradeAttribute attribute = (UpgradeAttribute)int.Parse(arr[j]);
                    if (attribute != UpgradeAttribute.None && !equipDatabase.database[id].SecondAttributeList.Contains(attribute))
                    {
                        equipDatabase.database[id].SecondAttributeList.Add(attribute);
                    }
                }
                EditorUtility.SetDirty(equipDatabase.database[id]);
            }
        }
        foreach (var kv in equipDatabase.partDic)
        {
            if (kv.Value != null)
            {
                kv.Value.Clear();
            }
        }
        foreach (var kv in equipDatabase.database)
        {
            if (!equipDatabase.partDic.ContainsKey(kv.Value.PartCode))
            {
                equipDatabase.partDic.Add(kv.Value.PartCode, new List<EquipSO>());
            }
            equipDatabase.partDic[kv.Value.PartCode].Add(kv.Value);
        }
        EditorUtility.SetDirty(equipDatabase);
    }

    /// <summary>
    /// 设置装备属性权重
    /// </summary>
    /// <param name="dataTable"></param>
    public void ImportEquipAttrWeight(DataTable dataTable)
    {
        DataRow row = null;
        for (int i = 2; i < dataTable.Rows.Count; i++)
        {
            row = dataTable.Rows[i];
            int id = int.Parse(dataTable.Rows[i][GetIndex("A")].ToString());
            if (equipDatabase.database.ContainsKey(id))
            {
                equipDatabase.database[id].WeightDic = new Dictionary<UpgradeAttribute, float>()
                    {
                        { UpgradeAttribute.TotalHp, float.Parse(dataTable.Rows[i][GetIndex("B")].ToString()) },
                        { UpgradeAttribute.Recover, float.Parse(dataTable.Rows[i][GetIndex("C")].ToString()) },
                        { UpgradeAttribute.AttackLowerLimit, float.Parse(dataTable.Rows[i][GetIndex("D")].ToString()) },
                        { UpgradeAttribute.AttackUpperLimit, float.Parse(dataTable.Rows[i][GetIndex("E")].ToString()) },
                        { UpgradeAttribute.MoveSpeed, float.Parse(dataTable.Rows[i][GetIndex("F")].ToString()) },
                        { UpgradeAttribute.AttackSpeed, float.Parse(dataTable.Rows[i][GetIndex("G")].ToString()) },
                        { UpgradeAttribute.Defense, float.Parse(dataTable.Rows[i][GetIndex("H")].ToString()) },
                        { UpgradeAttribute.Dodge, float.Parse(dataTable.Rows[i][GetIndex("I")].ToString()) },
                        { UpgradeAttribute.Vampire, float.Parse(dataTable.Rows[i][GetIndex("J")].ToString()) },
                        { UpgradeAttribute.CriticalRate, float.Parse(dataTable.Rows[i][GetIndex("K")].ToString()) },
                        { UpgradeAttribute.CriticalDamage, float.Parse(dataTable.Rows[i][GetIndex("L")].ToString()) },
                        { UpgradeAttribute.PenetrateDamage, float.Parse(dataTable.Rows[i][GetIndex("M")].ToString()) },
                    };
                EditorUtility.SetDirty(equipDatabase.database[id]);
            }
        }
    }

    /// <summary>
    /// 导入主属性基准值
    /// </summary>
    /// <param name="dataTable"></param>
    public void ImportFirstAttributeValue(DataTable dataTable)
    {
        DataRow row = null;
        captureData.firstAttributeValueOfLevelDic = new Dictionary<UpgradeAttribute, float>[100];
        for (int i = 2; i < dataTable.Rows.Count; i++)
        {
            row = dataTable.Rows[i];
            int level = int.Parse(dataTable.Rows[i][GetIndex("A")].ToString());
            captureData.firstAttributeValueOfLevelDic[level - 1] = new Dictionary<UpgradeAttribute, float>()
                {
                    { UpgradeAttribute.TotalHp, float.Parse(dataTable.Rows[i][GetIndex("B")].ToString()) },
                    { UpgradeAttribute.Recover, float.Parse(dataTable.Rows[i][GetIndex("C")].ToString()) },
                    { UpgradeAttribute.AttackLowerLimit, float.Parse(dataTable.Rows[i][GetIndex("D")].ToString()) },
                    { UpgradeAttribute.AttackUpperLimit, float.Parse(dataTable.Rows[i][GetIndex("E")].ToString()) },
                    { UpgradeAttribute.MoveSpeed, float.Parse(dataTable.Rows[i][GetIndex("F")].ToString()) },
                    { UpgradeAttribute.AttackSpeed, float.Parse(dataTable.Rows[i][GetIndex("G")].ToString()) },
                    { UpgradeAttribute.Defense, float.Parse(dataTable.Rows[i][GetIndex("H")].ToString()) },
                    { UpgradeAttribute.Dodge, float.Parse(dataTable.Rows[i][GetIndex("I")].ToString()) },
                    { UpgradeAttribute.Vampire, float.Parse(dataTable.Rows[i][GetIndex("J")].ToString()) },
                    { UpgradeAttribute.CriticalRate, float.Parse(dataTable.Rows[i][GetIndex("K")].ToString()) },
                    { UpgradeAttribute.CriticalDamage, float.Parse(dataTable.Rows[i][GetIndex("L")].ToString()) },
                    { UpgradeAttribute.PenetrateDamage, float.Parse(dataTable.Rows[i][GetIndex("M")].ToString()) },
                };
        }
        EditorUtility.SetDirty(captureData);
    }

    /// <summary>
    /// 导入副属性基准值
    /// </summary>
    /// <param name="dataTable"></param>
    public void ImportSecondAttributeValue(DataTable dataTable)
    {
        DataRow row = null;
        captureData.secondAttributeValueOfLevelDic = new Dictionary<UpgradeAttribute, float>[100];
        for (int i = 2; i < dataTable.Rows.Count; i++)
        {
            row = dataTable.Rows[i];
            int level = int.Parse(dataTable.Rows[i][GetIndex("A")].ToString());
            captureData.secondAttributeValueOfLevelDic[level - 1] = new Dictionary<UpgradeAttribute, float>()
                {
                    { UpgradeAttribute.TotalHp, float.Parse(dataTable.Rows[i][GetIndex("B")].ToString()) },
                    { UpgradeAttribute.Recover, float.Parse(dataTable.Rows[i][GetIndex("C")].ToString()) },
                    { UpgradeAttribute.AttackLowerLimit, float.Parse(dataTable.Rows[i][GetIndex("D")].ToString()) },
                    { UpgradeAttribute.AttackUpperLimit, float.Parse(dataTable.Rows[i][GetIndex("E")].ToString()) },
                    { UpgradeAttribute.MoveSpeed, float.Parse(dataTable.Rows[i][GetIndex("F")].ToString()) },
                    { UpgradeAttribute.AttackSpeed, float.Parse(dataTable.Rows[i][GetIndex("G")].ToString()) },
                    { UpgradeAttribute.Defense, float.Parse(dataTable.Rows[i][GetIndex("H")].ToString()) },
                    { UpgradeAttribute.Dodge, float.Parse(dataTable.Rows[i][GetIndex("I")].ToString()) },
                    { UpgradeAttribute.Vampire, float.Parse(dataTable.Rows[i][GetIndex("J")].ToString()) },
                    { UpgradeAttribute.CriticalRate, float.Parse(dataTable.Rows[i][GetIndex("K")].ToString()) },
                    { UpgradeAttribute.CriticalDamage, float.Parse(dataTable.Rows[i][GetIndex("L")].ToString()) },
                    { UpgradeAttribute.PenetrateDamage, float.Parse(dataTable.Rows[i][GetIndex("M")].ToString()) },
                };
        }
        EditorUtility.SetDirty(captureData);
    }

    /// <summary>
    /// 导入主属性波动值
    /// </summary>
    /// <param name="dataTable"></param>
    public void ImportFirstAttributeRange(DataTable dataTable)
    {
        DataRow row = null;
        captureData.firstAttributeRangeDic = new Dictionary<UpgradeAttribute, float>[100];
        for (int i = 2; i < dataTable.Rows.Count; i++)
        {
            row = dataTable.Rows[i];
            int level = int.Parse(dataTable.Rows[i][GetIndex("A")].ToString());
            captureData.firstAttributeRangeDic[level - 1] = new Dictionary<UpgradeAttribute, float>()
                {
                    { UpgradeAttribute.TotalHp, float.Parse(dataTable.Rows[i][GetIndex("B")].ToString()) },
                    { UpgradeAttribute.Recover, float.Parse(dataTable.Rows[i][GetIndex("C")].ToString()) },
                    { UpgradeAttribute.AttackLowerLimit, float.Parse(dataTable.Rows[i][GetIndex("D")].ToString()) },
                    { UpgradeAttribute.AttackUpperLimit, float.Parse(dataTable.Rows[i][GetIndex("E")].ToString()) },
                    { UpgradeAttribute.MoveSpeed, float.Parse(dataTable.Rows[i][GetIndex("F")].ToString()) },
                    { UpgradeAttribute.AttackSpeed, float.Parse(dataTable.Rows[i][GetIndex("G")].ToString()) },
                    { UpgradeAttribute.Defense, float.Parse(dataTable.Rows[i][GetIndex("H")].ToString()) },
                    { UpgradeAttribute.Dodge, float.Parse(dataTable.Rows[i][GetIndex("I")].ToString()) },
                    { UpgradeAttribute.Vampire, float.Parse(dataTable.Rows[i][GetIndex("J")].ToString()) },
                    { UpgradeAttribute.CriticalRate, float.Parse(dataTable.Rows[i][GetIndex("K")].ToString()) },
                    { UpgradeAttribute.CriticalDamage, float.Parse(dataTable.Rows[i][GetIndex("L")].ToString()) },
                    { UpgradeAttribute.PenetrateDamage, float.Parse(dataTable.Rows[i][GetIndex("M")].ToString()) },
                };
        }
        EditorUtility.SetDirty(captureData);
    }

    /// <summary>
    /// 导入副属性波动值
    /// </summary>
    /// <param name="dataTable"></param>
    public void ImportSecondAttributeRange(DataTable dataTable)
    {
        DataRow row = null;
        captureData.secondAttributeRangeDic = new Dictionary<UpgradeAttribute, float>[100];
        for (int i = 2; i < dataTable.Rows.Count; i++)
        {
            row = dataTable.Rows[i];
            int level = int.Parse(dataTable.Rows[i][GetIndex("A")].ToString());
            captureData.secondAttributeRangeDic[level - 1] = new Dictionary<UpgradeAttribute, float>()
                {
                    { UpgradeAttribute.TotalHp, float.Parse(dataTable.Rows[i][GetIndex("B")].ToString()) },
                    { UpgradeAttribute.Recover, float.Parse(dataTable.Rows[i][GetIndex("C")].ToString()) },
                    { UpgradeAttribute.AttackLowerLimit, float.Parse(dataTable.Rows[i][GetIndex("D")].ToString()) },
                    { UpgradeAttribute.AttackUpperLimit, float.Parse(dataTable.Rows[i][GetIndex("E")].ToString()) },
                    { UpgradeAttribute.MoveSpeed, float.Parse(dataTable.Rows[i][GetIndex("F")].ToString()) },
                    { UpgradeAttribute.AttackSpeed, float.Parse(dataTable.Rows[i][GetIndex("G")].ToString()) },
                    { UpgradeAttribute.Defense, float.Parse(dataTable.Rows[i][GetIndex("H")].ToString()) },
                    { UpgradeAttribute.Dodge, float.Parse(dataTable.Rows[i][GetIndex("I")].ToString()) },
                    { UpgradeAttribute.Vampire, float.Parse(dataTable.Rows[i][GetIndex("J")].ToString()) },
                    { UpgradeAttribute.CriticalRate, float.Parse(dataTable.Rows[i][GetIndex("K")].ToString()) },
                    { UpgradeAttribute.CriticalDamage, float.Parse(dataTable.Rows[i][GetIndex("L")].ToString()) },
                    { UpgradeAttribute.PenetrateDamage, float.Parse(dataTable.Rows[i][GetIndex("M")].ToString()) },
                };
        }
        EditorUtility.SetDirty(captureData);
    }

    /// <summary>
    /// 导入装备品质爆率
    /// </summary>
    /// <param name="dataTable"></param>
    public void ImportProductQualityDistribution(DataTable dataTable)
    {
        DataRow row = null;
        captureData.productQualityDistribution = new Dictionary<EquipQuality, float>[100];
        for (int i = 2; i < dataTable.Rows.Count; i++)
        {
            row = dataTable.Rows[i];
            int level = int.Parse(dataTable.Rows[i][GetIndex("A")].ToString());
            captureData.productQualityDistribution[level - 1] = new Dictionary<EquipQuality, float>();
            for (int j = 1; j <= 4; j++)
            {
                captureData.productQualityDistribution[level - 1].Add((EquipQuality)j, float.Parse(dataTable.Rows[i][j].ToString()));
            }
        }
        EditorUtility.SetDirty(captureData);
    }

    /// <summary>
    /// 导入装备评分标准
    /// </summary>
    public void ImportAttributeScore(DataTable dataTable)
    {
        DataRow row = null;
        captureData.scoreWeightDic = new Dictionary<UpgradeAttribute, float>[100];
        for (int i = 2; i < dataTable.Rows.Count; i++)
        {
            row = dataTable.Rows[i];
            int level = int.Parse(dataTable.Rows[i][GetIndex("A")].ToString());
            captureData.scoreWeightDic[level - 1] = new Dictionary<UpgradeAttribute, float>();
            for (int j = 1; j <= 12; j++)
            {
                captureData.scoreWeightDic[level - 1].Add((UpgradeAttribute)j, float.Parse(dataTable.Rows[i][j].ToString()));
            }
        }
        EditorUtility.SetDirty(captureData);
    }

    /// <summary>
    /// 导入难度参数
    /// </summary>
    /// <param name="dataTable"></param>
    public void ImportLevelCoefficient(DataTable dataTable)
    {
        DataRow row = null;
        for (int i = 2; i < dataTable.Rows.Count; i++)
        {
            row = dataTable.Rows[i];
            int id = int.Parse(dataTable.Rows[i][GetIndex("A")].ToString());
            if (difficultyData.database.ContainsKey(id))
            {
                difficultyData.database[id].Round = float.Parse(dataTable.Rows[i][GetIndex("B")].ToString());
                difficultyData.database[id].Difficult = float.Parse(dataTable.Rows[i][GetIndex("C")].ToString());
                difficultyData.database[id].StrengthenCaptureCycleTime = float.Parse(dataTable.Rows[i][GetIndex("D")].ToString());
                difficultyData.database[id].StrengthenEnemyCycleTime = float.Parse(dataTable.Rows[i][GetIndex("E")].ToString());
                EditorUtility.SetDirty(difficultyData.database[id]);
            }
        }
    }
#endif
}