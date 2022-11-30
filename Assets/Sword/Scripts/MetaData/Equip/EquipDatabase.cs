using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "EquipDatabase", menuName = "Model/Equip/EquipDatabase")]
public class EquipDatabase : SerializedScriptableObject
{
    public string path = "";
    public Dictionary<int, EquipSO> database = new Dictionary<int, EquipSO>();
    public Dictionary<EquipPart, List<EquipSO>> partDic = new Dictionary<EquipPart, List<EquipSO>>();

    [Button]
    public void Refresh()
    {
#if UNITY_EDITOR

        database.Clear();
        partDic.Clear();
        string[] filePaths = System.IO.Directory.GetFiles(path);
        int countFound = 0;
        if (filePaths != null && filePaths.Length > 0)
        {
            for (int i = 0; i < filePaths.Length; i++)
            {
                Object obj = AssetDatabase.LoadAssetAtPath(filePaths[i], typeof(EquipSO));
                if (obj is EquipSO asset)
                {
                    countFound++;
                    if (!database.ContainsKey(asset.ID))
                    {
                        database.Add(asset.ID, asset);
                        if (partDic.ContainsKey(asset.PartCode))
                        {
                            partDic[asset.PartCode].Add(asset);
                        }
                        else 
                        {
                            partDic.Add(asset.PartCode, new List<EquipSO>(4));
                            partDic[asset.PartCode].Add(asset);
                        }
                    }
                }
            }
        }

        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
#endif
    }
}
