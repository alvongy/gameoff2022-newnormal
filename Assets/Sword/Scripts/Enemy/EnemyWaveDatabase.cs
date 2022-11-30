using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyWaveDatabase", menuName = "Enemy/EnemyWaveDatabase")]
public class EnemyWaveDatabase : SerializedScriptableObject
{
    public string Path;
    public Dictionary<int, EnemyWaveSO> waveDic = new Dictionary<int, EnemyWaveSO>();

    [Button]
    public void Refresh()
    {
#if UNITY_EDITOR

        waveDic.Clear();

        string[] filePaths = System.IO.Directory.GetFiles(Path);

        int countFound = 0;

        if (filePaths != null && filePaths.Length > 0)
        {
            for (int i = 0; i < filePaths.Length; i++)
            {
                UnityEngine.Object obj = UnityEditor.AssetDatabase.LoadAssetAtPath(filePaths[i], typeof(EnemyWaveSO));
                if (obj is EnemyWaveSO asset)
                {
                    countFound++;
                    if (!waveDic.ContainsKey(asset.ID))
                    {
                        waveDic.Add(asset.ID, asset);
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
