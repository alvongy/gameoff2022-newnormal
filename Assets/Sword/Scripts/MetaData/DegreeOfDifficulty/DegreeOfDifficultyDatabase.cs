using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "DegreeOfDifficultyDatabase", menuName = "Model/DegreeOfDifficulty/DegreeOfDifficultyDatabase")]
public class DegreeOfDifficultyDatabase : SerializedScriptableObject
{
    public string path = "";
    public Dictionary<int, DegreeOfDifficultySO> database = new Dictionary<int, DegreeOfDifficultySO>();
    [Button]
    public void Refresh()
    {
#if UNITY_EDITOR

        database.Clear();
        string[] filePaths = System.IO.Directory.GetFiles(path);
        int countFound = 0;
        if (filePaths != null && filePaths.Length > 0)
        {
            for (int i = 0; i < filePaths.Length; i++)
            {
                Object obj = AssetDatabase.LoadAssetAtPath(filePaths[i], typeof(DegreeOfDifficultySO));
                if (obj is DegreeOfDifficultySO asset)
                {
                    countFound++;
                    if (!database.ContainsKey(asset.ID))
                    {
                        database.Add(asset.ID, asset);
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
