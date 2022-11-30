using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EvolutionManager : SerializedMonoSingleton<EvolutionManager>
{
    [Title("进化词条选择库")]
    public EvolutionTermDatabase evolutionTermDatabase;
    [Title("选择过的进化词条库")]
    public EvolutionTermDatabase evolutionTermDatabase_Learend;

    [Title("各质量出品率随等级对照表")]
    public Dictionary<EquipQuality, float>[] productQualityDistribution;
    [Title("进化词条当前生存难度")]
    public int survivalDifficulty;
    [Title("生存难度解锁")]
    public Dictionary<int,bool> survivalDifficultyUnlockDic;

    //public bool survivalAddRefresh;
    //public bool survivalAddGoldDrop;
    //public bool survivalShopDepreciate;
    //public bool survivalPlayerRevive;

    public Dictionary<EquipQuality, List<EvolutionTermSO>> benignPoolDic = new Dictionary<EquipQuality, List<EvolutionTermSO>>();
    public Dictionary<EquipQuality, List<EvolutionTermSO>> neutralPoolDic = new Dictionary<EquipQuality, List<EvolutionTermSO>>();
    public Dictionary<EquipQuality, List<EvolutionTermSO>> malignancyPoolDic = new Dictionary<EquipQuality, List<EvolutionTermSO>>();

    [HideInInspector] public EvolutionTermSO currentClickEvolutionTerm;

    public Dictionary<EvolutionTypeEnum, List<EvolutionTermSO>> RunTimeEvolutionTermDatabase = new Dictionary<EvolutionTypeEnum, List<EvolutionTermSO>>();
    //实装过的进化效果记录
    private Dictionary<int, EvolutionTermSO> realizedEvolutionTermDatabase = new Dictionary<int, EvolutionTermSO>();

    public UnityAction OnLearendEvolutionChange;

    public void Init()
    {
        evolutionTermDatabase_Learend.database.Clear();
        realizedEvolutionTermDatabase.Clear();
        benignPoolDic.Clear();
        neutralPoolDic.Clear();
        malignancyPoolDic.Clear();
        RunTimeEvolutionTermDatabase.Clear();
        survivalDifficulty = 0;

        foreach (var p in evolutionTermDatabase.partDic)
        {
            RunTimeEvolutionTermDatabase.Add(p.Key,p.Value);
            if (p.Key == EvolutionTypeEnum.BENIGN)
            {
                foreach (var e in p.Value)
                {
                    for (int i = 0; i < e.termNum; i++)
                    {
                        if (!benignPoolDic.ContainsKey(e.evolutionQuality))
                        {
                            benignPoolDic.Add(e.evolutionQuality, new List<EvolutionTermSO>() { e });
                        }
                        else
                        {
                            benignPoolDic[e.evolutionQuality].Add(e);
                        }  
                    } 
                }
            }
            else if (p.Key == EvolutionTypeEnum.NEUTRAL)
            {
                foreach (var e in p.Value)
                {
                    for (int i = 0; i < e.termNum; i++)
                    {
                        if (!neutralPoolDic.ContainsKey(e.evolutionQuality))
                        {
                            neutralPoolDic.Add(e.evolutionQuality, new List<EvolutionTermSO>() { e }) ;
                        }
                        else
                        {
                            neutralPoolDic[e.evolutionQuality].Add(e);
                        }
                    }
                }
            }
            else if (p.Key == EvolutionTypeEnum.MALIGNANCY)
            {
                foreach (var e in p.Value)
                {
                    for (int i = 0; i < e.termNum; i++)
                    {
                        if (!malignancyPoolDic.ContainsKey(e.evolutionQuality))
                        {
                            malignancyPoolDic.Add(e.evolutionQuality, new List<EvolutionTermSO>() { e }) ;
                        }
                        else
                        {
                            malignancyPoolDic[e.evolutionQuality].Add(e);
                        }   
                    }
                }
            }
        }
    }

    public void OnGameOver()
    {
        evolutionTermDatabase_Learend.database.Clear();
        realizedEvolutionTermDatabase.Clear();
        survivalDifficulty = 0;
    }

    /// <summary>
    /// 当前关卡结束后，弹窗生成进化词条等
    /// </summary>
    public void OnCurrentLevelEnd()
    {
        //UI_Manager.Instance.selectEvolutionTerm.ShowEvolutionTermChoiceUI();
        UI_Manager.Instance.selectEvolutionTerm.CaptureEvolutionTerm();
    }
    public void OnGameResume()
    {
        
    }

    /// <summary>
    /// 获取进化牌品性
    /// </summary>
    /// <returns></returns>
    public EvolutionTypeEnum GetRandomEvolutionType()
    {
        int begin = 0;
        int neutral = 0;
        int malignancy = 0;

        foreach (var b in benignPoolDic)
        {
            foreach (var e in b.Value)
            {
                begin ++;
            }
        }
        foreach (var n in neutralPoolDic)
        {
            foreach (var e in n.Value)
            {
                neutral++;
            }
        }
        foreach (var m in malignancyPoolDic)
        {
            foreach (var e in m.Value)
            {
                malignancy++;
            }
        }

        int s = begin + neutral + malignancy;
        int r = Random.Range(0, begin + neutral + malignancy);


        Debug.Log($"r:{r}。良：{begin}，中：{neutral}，恶：{malignancy}");
        if (r <= begin)
        {
            return EvolutionTypeEnum.BENIGN;
        } 
        else if (r > begin && r <= (begin + neutral))
        {
            return EvolutionTypeEnum.NEUTRAL;
        }
        else
        {
            return EvolutionTypeEnum.MALIGNANCY;
        }
    }

    /// <summary>
    /// 选择完进化词条后的事件处理
    /// </summary>
    public void EndSelectcEvolutionTerm()
    {
        if (!evolutionTermDatabase_Learend.database.ContainsKey(currentClickEvolutionTerm.ID))
        {
            evolutionTermDatabase_Learend.database.Add(currentClickEvolutionTerm.ID, currentClickEvolutionTerm);
            //将生存难度数值更新
        }
       if (!EntranceManager.Instance.InGame){ return; }
        EvolutionAddition();
        SurvivalDifficultyAddition();
        ShopManager.Instance.ShownShopPanel();
        OnLearendEvolutionChange.Invoke();
    }

    /// <summary>
    /// 进化效果实装
    /// </summary>
    public void EvolutionAddition()
    {
        foreach (var evolution in evolutionTermDatabase_Learend.database)
        {
            //加属性类型：从EntityData_Character中添加属性
            //加特殊效果型
            //场景道具类型：从SceneItemManager中激活某个场景道具
            //道具分为两种：一直存在与每关重新生成，一直存在的则进入下面的判断，不重新生效；重新生成的则不进入

            if (evolution.Value.isRebirth)
            {
                SceneItemManager.Instance.ActivateRandomSceneItem(evolution.Value);
            }
            else if (!realizedEvolutionTermDatabase.ContainsKey(evolution.Key))
            {
                realizedEvolutionTermDatabase.Add(evolution.Key, evolution.Value);
                SceneItemManager.Instance.ActivateRandomSceneItem(evolution.Value);

                foreach (var t in evolution.Value.targetAttributeDic)
                {
                    if (t.Key == TargetObjectEnum.PLAYER)
                    {
                        CharacterManager.Instance.characterData.EvolutionAddition(evolution.Value);
                    }
                    else
                    {
                        Debug.Log("敌人属性暂不处理");
                    }
                }
            }
        }

        if (evolutionTermDatabase_Learend.database[currentClickEvolutionTerm.ID].evolutionType == EvolutionTypeEnum.MALIGNANCY)
        {
            survivalDifficulty += evolutionTermDatabase_Learend.database[currentClickEvolutionTerm.ID].evolutionQuality.GetHashCode() + 1;
        }
    }

    /// <summary>
    /// 生存难度效果实装
    /// </summary>
    public void SurvivalDifficultyAddition()
    {
        if (survivalDifficulty < 1)
        {
            return;
        }
        else if (survivalDifficulty > 0 && survivalDifficulty < 6)
        {
            survivalDifficultyUnlockDic[2] = true;
        }
        else if (survivalDifficulty >= 6 && survivalDifficulty < 11)
        {
            survivalDifficultyUnlockDic[1] = true;
        } 
        else if (survivalDifficulty >= 11 && survivalDifficulty < 16)
        {
            survivalDifficultyUnlockDic[3] = true;
        }
        else if (survivalDifficulty >= 16 && survivalDifficulty < 21)
        {
            survivalDifficultyUnlockDic[4] = true;
        }
    }

    List<EquipQuality> qualitiesList = new List<EquipQuality>();
    List<float> randomWeightList=new List<float>();

    public EquipQuality GetEvolutionQuality(int level,Dictionary<EquipQuality, List<EvolutionTermSO>> dic)
    {
        var productDic = productQualityDistribution[level - 1];
        qualitiesList.Clear();
        randomWeightList.Clear();
        float lastValue = 0;
        foreach (var kv in productDic)
        {
            if (dic.ContainsKey(kv.Key) && dic[kv.Key].Count > 0) 
            {
                qualitiesList.Add(kv.Key);
                float t = kv.Value + lastValue;
                randomWeightList.Add(t);
                lastValue = t;
            }
        }
        float v = Random.Range(0f, lastValue);
        EquipQuality result = qualitiesList[randomWeightList.Count - 1];
        for (int i = randomWeightList.Count - 1; i >= 0; i--)
        {
            if (v < randomWeightList[i])
            {
                result = qualitiesList[i];
            }
        }
        return result;
    }
}
