using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YK.Game.Events;

public class MonsterManager : SerializedMonoSingleton<MonsterManager>
{
    [Title("敌人数量上限"), SerializeField]
    int totalEnemyAmount = 15;

    [SerializeField] EnemyDatabase enemyDatabase;
    private List<EntityData_Enemy> entityDataList;
    private int enemyCounter;

    public bool CanSpawn => enemyCounter < totalEnemyAmount;
    public bool Cleaned => enemyCounter == 0;

    protected override void OnAwake()
    {
        entityDataList = new List<EntityData_Enemy>(32);
    }

    public void Init()
    {
        enemyCounter = 0;
    }

    EntityData_Enemy GetEnemyData(int id)
    {
        EntityData_Enemy data;
        if (entityDataList.Count > 0)
        {
            data = entityDataList[entityDataList.Count - 1];
            entityDataList.RemoveAt(entityDataList.Count - 1);
            data.Reset(enemyDatabase.database[id]);
        }
        else
        {
            data = EntityData_Enemy.GetEnemyData(enemyDatabase.database[id]);
        }
        return data;
    }

    public void SpawnEnemy(int id, Vector3 pos)
    {
        if (enemyCounter >= totalEnemyAmount)
        {
            return;
        }
        var go = ObjectPool.Instantiate(enemyDatabase.database[id].Prefab, pos, Quaternion.identity);
        go.transform.parent = transform;
        EnemyCtrl enemyCtrl = go.GetComponent<EnemyCtrl>();
        enemyCtrl.data = GetEnemyData(id);
        enemyCtrl.Init();
        enemyCounter++;
    }

    public void EnemyDestroy(EnemyCtrl enemyCtrl)
    {
        if (CaptureManager.Instance.CheckCapture(enemyCtrl.data))
        {
            int captureLevel = Random.Range(enemyCtrl.data.captureLevel - 1, enemyCtrl.data.captureLevel + 2);
            captureLevel = Mathf.Clamp(captureLevel, 1, 20);
            UI_Manager.Instance.captureEquipPanel.AddEquip(CaptureManager.Instance.GetCaptureEquip(captureLevel));
        }

        SceneItemManager.Instance.SpawnGoldObject(enemyCtrl);
        entityDataList.Add(enemyCtrl.data);
        enemyCtrl.data = null;
        ObjectPool.Destroy(enemyCtrl.gameObject);
        enemyCounter--;

        if (SpawnManager.Instance.Completed && Cleaned)
        {
            Debug.Log("本关卡配置的怪都全部刷完且全部杀死了，执行下一关的方法。当前关卡存在的敌人数量："+ enemyCounter);
            GameLevelManager.Instance.CurrentLevelEnd();
        }
    }

    public void OnGameOver()
    {
        DestroyAllEnemy();
    }

    public void DestroyAllEnemy()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            EnemyCtrl enemyCtrl = transform.GetChild(i).GetComponent<EnemyCtrl>();
            if (enemyCtrl)
            {
                EnemyDestroy(enemyCtrl);
            }
        }
        enemyCounter = 0;
    }


    public void OnCurrentLevelEnd()
    {
        DestroyAllEnemy();
    }


}
