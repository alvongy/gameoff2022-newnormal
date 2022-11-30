using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDatabase", menuName = "Model/Enemy/EnemyDatabase")]
public class EnemyDatabase : SerializedScriptableObject
{
    public string path = "";
    public Dictionary<int, EnemySO> database = new Dictionary<int, EnemySO>();
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
                Object obj = AssetDatabase.LoadAssetAtPath(filePaths[i], typeof(EnemySO));
                if (obj is EnemySO asset)
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
