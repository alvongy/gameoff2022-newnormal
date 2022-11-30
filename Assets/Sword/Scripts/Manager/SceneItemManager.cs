using System.Collections.Generic;
using UnityEngine;
using YK.Game.Events;
using Sirenix.OdinInspector;
using DG.Tweening;

public class SceneItemManager : SerializedMonoSingleton<SceneItemManager>
{
    [SerializeField] private Transform _hotSpringItem;
    [SerializeField] private Transform _trapItem;
    [SerializeField] private Transform _marshItem;
    [SerializeField] private Transform _tempItemParent;
    [SerializeField] private Transform _goldItemParent;
    [SerializeField] public Transform _goldItem;
    [SerializeField] public Transform startGameEntrance;

    [Title("金币预制体")]
    [SerializeField] private GameObject glodPrefab;
    [Title("金币预制体")]
    [SerializeField] private GameObject stratGamePrefab;

    private List<GameObject> hotSpringItemList = new List<GameObject>();
    private List<GameObject> trapItemList = new List<GameObject>();
    private List<GameObject> marshItemList = new List<GameObject>();

    public void Init()
    {
        DestroyAllTempItem();
        LoadSceneItems();
    }

    public void OnGameOver()
    {
        DestroyAllTempItem();

        for (int i = _goldItemParent.childCount - 1; i >= 0; i--)
        {
            ObjectPool.Destroy(_goldItemParent.GetChild(i).gameObject);
        }
    }
    public void StartGame()
    {
        Vector3 v3 = new Vector3(50f,0f,0f);
        Quaternion quaternion = Quaternion.Euler(v3);
        ObjectPool.Instantiate(stratGamePrefab, startGameEntrance.position, quaternion, startGameEntrance);
    }

    /// <summary>
    /// 加载场景中固定道具（温泉、水洼、陷阱）
    /// </summary>
    private void LoadSceneItems()
    {
        hotSpringItemList.Clear();
        trapItemList.Clear();
        marshItemList.Clear();

        for (int i = 0; i < _hotSpringItem.childCount; i++)
        {
            _hotSpringItem.GetChild(i).gameObject.SetActive(false);
            hotSpringItemList.Add(_hotSpringItem.GetChild(i).gameObject);
        }
        for (int i = 0; i < _trapItem.childCount; i++)
        {
            _trapItem.GetChild(i).gameObject.SetActive(false);
            trapItemList.Add(_trapItem.GetChild(i).gameObject);
        }
        for (int i = 0; i < _marshItem.childCount; i++)
        {
            _marshItem.GetChild(i).gameObject.SetActive(false);
            marshItemList.Add(_marshItem.GetChild(i).gameObject);
        }
    }

    /// <summary>
    /// 拿到进化卡，激活某种类型的场景道具一次
    /// </summary>
    public void ActivateRandomSceneItem(EvolutionTermSO evolution)
    {
        switch (evolution.sceneItemType)
        {
            case SceneItemTypeEnum.NONE:
                //Debug.Log("不属于场景道具类型，此次不激活道具");
                break;
            case SceneItemTypeEnum.MARSH:
                GetSceneItem(marshItemList);
                break;
            case SceneItemTypeEnum.TRAP:
                GetSceneItem(trapItemList);
                break;
            case SceneItemTypeEnum.SPRING:
                GetSceneItem(hotSpringItemList);
                break;
            case SceneItemTypeEnum.BANGALORE:
                SpawnSceneItem(evolution);
                break;
            case SceneItemTypeEnum.TREASUREBOX:
                SpawnSceneItem(evolution);
                break;
            case SceneItemTypeEnum.BLOOD:
                SpawnSceneItem(evolution);
                break;
            case SceneItemTypeEnum.PORTAL:
                SpawnSceneItem(evolution);
                break;

            default:
                Debug.LogError("未找到当前类型的道具，待处理");
                break;
        }
    }
    private void GetSceneItem(List<GameObject> list)
    {
        int r = Random.Range(0, list.Count);
        list[r].SetActive(true);
        list.RemoveAt(r);
    }

    private void SpawnSceneItem(EvolutionTermSO evolution)
    {
        Vector3 targetPos = GetRandomPosition();
        //if (targetPos)//如果此位置没检测到道具才生成，不然就再获得一个新位置用来生成
        //{
        //
        //}
        GameObject go = ObjectPool.Instantiate(evolution.sceneItemPrefab, targetPos, Quaternion.identity, _tempItemParent);
    }

    Vector3 GetRandomPosition()
    {
        Vector3 p = Vector3.zero;
        float sceneSize = 50f;

        //可根据场景实际大小赋值随机范围
        float rx = Random.Range(-sceneSize, sceneSize);
        float ry = Random.Range(-sceneSize, sceneSize);
        int i = Random.Range(0, 4);

        switch (i)
        {
            case 0:
                p = new Vector3(rx, 0, Random.Range(-sceneSize, sceneSize));
                break;
            case 1:
                p = new Vector3(-rx, 0, Random.Range(-sceneSize, sceneSize));
                break;
            case 2:
                p = new Vector3(Random.Range(-sceneSize, sceneSize), 0, ry);
                break;
            case 3:
                p = new Vector3(Random.Range(-sceneSize, sceneSize), 0, -ry);
                break;
            default:
                break;
        }
        return (p);
    }

    public void OnCurrentLevelEnd()
    {
        DestroyAllTempItem();
    }

    public void DestroyAllTempItem()
    {
        for (int i = _tempItemParent.childCount - 1; i >= 0; i--)
        {
            ObjectPool.Destroy(_tempItemParent.GetChild(i).gameObject);
        }
        for (int i = startGameEntrance.childCount - 1; i >= 0; i--)
        {
            ObjectPool.Destroy(startGameEntrance.GetChild(i).gameObject);
        }
    }

    public void SpawnGoldObject(EnemyCtrl enemyCtrl)
    {
        if (EvolutionManager.Instance.survivalDifficultyUnlockDic[2])
        {
            float r = Random.Range(0,100);
            if (r <= 3)
            {
                Debug.Log("金币双倍掉落");
                for (int i = 0; i < 2; i++)
                {
                    GameObject gold = ObjectPool.Instantiate(glodPrefab, enemyCtrl.transform.position+new Vector3(i,i,i), Quaternion.identity, _goldItemParent);
                    gold.GetComponent<CollectableItem_Prop>().InitGold((int)enemyCtrl.Data[UpgradeAttribute.EnemyBasicGold].GetData());
                    gold.transform.DOMoveY(6, 0.5f, false);
                }
            }
            else
            {
                GameObject gold = ObjectPool.Instantiate(glodPrefab, enemyCtrl.transform.position, Quaternion.identity, _goldItemParent);
                gold.GetComponent<CollectableItem_Prop>().InitGold((int)enemyCtrl.Data[UpgradeAttribute.EnemyBasicGold].GetData());
                gold.transform.DOMoveY(6, 0.5f, false);
            }
        }
        else
        {
            GameObject gold = ObjectPool.Instantiate(glodPrefab, enemyCtrl.transform.position, Quaternion.identity, _goldItemParent);
            gold.GetComponent<CollectableItem_Prop>().InitGold((int)enemyCtrl.Data[UpgradeAttribute.EnemyBasicGold].GetData());
            gold.transform.DOMoveY(6, 0.5f, false);
        }
        

        //gold.GetComponent<Rigidbody>().AddExplosionForce(50f, gold.transform.position-Vector3.up*0.1f, 1f);
        //gold.transform.DOMoveY(20f, 0.5f,false).OnComplete(() => 
        //{
        //    gold.GetComponent<CollectableItem_Prop>().isGoldActive = true;
        //});

    }
}
