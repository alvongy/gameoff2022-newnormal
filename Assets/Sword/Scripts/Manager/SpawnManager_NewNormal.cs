using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager_NewNormal : SerializedMonoSingleton<SpawnManager_NewNormal>
{
    [SerializeField] private List<Transform> LevelWaveSpawner;

    public void Init()
    {
        for (int j = 0; j < LevelWaveSpawner[0].childCount; j++)
        {
            LevelWaveSpawner[GameLevelManager.Instance.GetCurrentLevel].GetChild(j).GetComponent<WaveSpawner_Sword>().Init();
        }
    }

    public void OnGameOver()
    {
        StopSpawnerEnemy();

    }

    public void StopSpawnerEnemy()
    {
        for (int i = 0; i < LevelWaveSpawner.Count; i++)
        {
            for (int j = 0; j < LevelWaveSpawner[i].childCount; j++)
            {
                LevelWaveSpawner[i].GetChild(j).GetComponent<WaveSpawner_Sword>().Stop();
            }
        }
    }

    public void ResumeSpawnEnemy()
    {
        StopSpawnerEnemy();

        if (GameLevelManager.Instance.GetCurrentLevel <= LevelWaveSpawner.Count - 1)
        {
            if (GameLevelManager.Instance.GetCurrentLevel <= LevelWaveSpawner.Count - 1)
            {
                for (int j = 0; j < LevelWaveSpawner[GameLevelManager.Instance.GetCurrentLevel].childCount; j++)
                {
                    LevelWaveSpawner[GameLevelManager.Instance.GetCurrentLevel].GetChild(j).GetComponent<WaveSpawner_Sword>().Init();
                }      
            }
        }
        else
        {
            Debug.Log("怪物配置已经全部刷完");
        }
    }
}
