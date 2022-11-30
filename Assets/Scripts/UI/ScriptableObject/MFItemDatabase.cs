using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif
[CreateAssetMenu]
public class MFItemDatabase : SerializedScriptableObject
{
    public string Path = "";
    public Dictionary<int, ItemSO> database = new Dictionary<int, ItemSO>();
    [Button]
    public void Refresh()
    {
#if UNITY_EDITOR

        database.Clear();

        string[] filePaths = System.IO.Directory.GetFiles(Path);

        int countFound = 0;


        if (filePaths != null && filePaths.Length > 0)
        {
            for (int i = 0; i < filePaths.Length; i++)
            {
                UnityEngine.Object obj = UnityEditor.AssetDatabase.LoadAssetAtPath(filePaths[i], typeof(ItemSO));
                if (obj is ItemSO asset)
                {
                    countFound++;
                    if (!database.ContainsKey(asset.ItemID))
                    {
                        database.Add(asset.ItemID, asset);
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
