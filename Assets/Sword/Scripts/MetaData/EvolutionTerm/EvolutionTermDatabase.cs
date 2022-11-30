using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "EvolutionTermDatabase", menuName = "Model/Evolution/EvolutionTermDatabase")]
public class EvolutionTermDatabase : SerializedScriptableObject
{
    public string path = "";
    public Dictionary<int, EvolutionTermSO> database = new Dictionary<int, EvolutionTermSO>();
    public Dictionary<EvolutionTypeEnum, List<EvolutionTermSO>> partDic = new Dictionary<EvolutionTypeEnum, List<EvolutionTermSO>>();

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
                Object obj = AssetDatabase.LoadAssetAtPath(filePaths[i], typeof(EvolutionTermSO));
                if (obj is EvolutionTermSO asset)
                {
                    countFound++;
                    if (!database.ContainsKey(asset.ID))
                    {
                        database.Add(asset.ID, asset);
                        if (partDic.ContainsKey(asset.evolutionType))
                        {
                            partDic[asset.evolutionType].Add(asset);
                        }
                        else
                        {
                            partDic.Add(asset.evolutionType, new List<EvolutionTermSO>(4));
                            partDic[asset.evolutionType].Add(asset);
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
