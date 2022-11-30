using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MapBuffControl : SerializedMonoBehaviour
{
    [SerializeField] public Dictionary<Vector2, MapTerrainUISlot> _mapTerrainUISlotDic;

    [SerializeField] private TerrainEventChannelSO OnPickTerrainChannelSO;//考虑将点击地形卡就赋值最后地形的逻辑放在这
    [SerializeField] private TerrainAttibuteSO OnTerrainAttibuteSO;

    [FoldoutGroup("MapBuff SO")] [SerializeField] private VoidEventChannelSO OnMapBuffDetectionChannelSO;
    [FoldoutGroup("MapBuff SO")] [SerializeField] private VoidEventChannelSO OnMapBuffNearBossChannelSO;

    [FoldoutGroup("Rule SO")] [SerializeField] private ArrangementRulesEventChannelSO OnAloneRuleRed1;
    [FoldoutGroup("Rule SO")] [SerializeField] private ArrangementRulesEventChannelSO OnAloneRuleGreen1;
    [FoldoutGroup("Rule SO")] [SerializeField] private ArrangementRulesEventChannelSO OnAloneRuleBlue1;
    [FoldoutGroup("Rule SO")] [SerializeField] private ArrangementRulesEventChannelSO OnLineRuleBlue2Green1;
    [FoldoutGroup("Rule SO")] [SerializeField] private ArrangementRulesEventChannelSO OnLineRuleBlue3;
    [FoldoutGroup("Rule SO")] [SerializeField] private ArrangementRulesEventChannelSO OnLineRuleBlue2Red1;
    [FoldoutGroup("Rule SO")] [SerializeField] private ArrangementRulesEventChannelSO OnLineRuleGreen2Blue1;
    [FoldoutGroup("Rule SO")] [SerializeField] private ArrangementRulesEventChannelSO OnLineRuleGreen2Red1;
    [FoldoutGroup("Rule SO")] [SerializeField] private ArrangementRulesEventChannelSO OnLineRuleGreen3;
    [FoldoutGroup("Rule SO")] [SerializeField] private ArrangementRulesEventChannelSO OnLineRuleRed1Green1Blue1;
    [FoldoutGroup("Rule SO")] [SerializeField] private ArrangementRulesEventChannelSO OnLineRuleRed2Blue1;
    [FoldoutGroup("Rule SO")] [SerializeField] private ArrangementRulesEventChannelSO OnLineRuleRed2Green1;
    [FoldoutGroup("Rule SO")] [SerializeField] private ArrangementRulesEventChannelSO OnLineRuleRed3;

    [FoldoutGroup("Rule SO")] [SerializeField] private ArrangementRulesEventChannelSO OnObliqueRuleBlue3;
    [FoldoutGroup("Rule SO")] [SerializeField] private ArrangementRulesEventChannelSO OnObliqueRuleGreen3;
    [FoldoutGroup("Rule SO")] [SerializeField] private ArrangementRulesEventChannelSO OnObliqueRuleRed1Green1Blue1;
    [FoldoutGroup("Rule SO")] [SerializeField] private ArrangementRulesEventChannelSO OnObliqueRuleRed3;

    [FoldoutGroup("Rule SO")] [SerializeField] private ArrangementRulesEventChannelSO OnSquaresRuleRed;
    [FoldoutGroup("Rule SO")] [SerializeField] private ArrangementRulesEventChannelSO OnSquaresRuleGreen;
    [FoldoutGroup("Rule SO")] [SerializeField] private ArrangementRulesEventChannelSO OnSquaresRuleBlue;

    [FoldoutGroup("Rule SO")] [SerializeField] private ArrangementRulesEventChannelSO OnHomochromaticRuleRed;
    [FoldoutGroup("Rule SO")] [SerializeField] private ArrangementRulesEventChannelSO OnHomochromaticRuleGreen;
    [FoldoutGroup("Rule SO")] [SerializeField] private ArrangementRulesEventChannelSO OnHomochromaticRuleBlue;


    //[FoldoutGroup("Rule SO")] public List<ArrangementRulesEventChannelSO> OnArrangementRules = new List<ArrangementRulesEventChannelSO>();
    [FoldoutGroup("Rule SO")] public List<ArrangementRulesEventChannelSO> OnArrangementRuleList = new List<ArrangementRulesEventChannelSO>();

    [FoldoutGroup("Object")] public Transform AllMapAdditionContent;
    [FoldoutGroup("Object")] public Transform TerrainUIParent;
    [FoldoutGroup("Object")] public GameObject MapAdditionPrefab;

    [HideInInspector] public List<ArrangementRulesEventChannelSO> OnArrangementRule = new List<ArrangementRulesEventChannelSO>();
    [HideInInspector] public List<ArrangementRulesEventChannelSO> OnArrangementRulesTemp = new List<ArrangementRulesEventChannelSO>();

    // public List<MapAdditionEntity> _mapBuffList = new List<MapAdditionEntity>();

    private PrayStoneType PrayStonen_Type;
    private bool IsOnAllMapAdd = true;

    private void Awake()
    {
        MapSelectUIManager.Instance.EnterGame_Btn.onClick.AddListener(SetAddition);//Button未激活的情况下能绑定监听事件否
        //AllMapAdditionBtn.onClick.AddListener(ShowMapAddition);

        YKDataInfoManager.Instance.desktopCardNum = 0;
        for (int i = 0; i < TerrainUIParent.childCount; i++)
        {
            if (TerrainUIParent.GetChild(i).childCount > 0 && TerrainUIParent.GetChild(i).GetChild(0).GetComponent<DesktopGirdLock>() != null)
            {
                YKDataInfoManager.Instance.desktopCardNum++;
            }
        }
    }
    private void Start()
    {
        YKDataInfoManager.Instance.CurrentSkillRecord.Clear();
        SetMapAddition();
        GetMapAddition();
    }
    private void OnEnable()
    {
        OnMapBuffDetectionChannelSO.OnEventRaised += GetMapAddition;
        OnPickTerrainChannelSO.OnEventRaised += SetLastPickTerrain;
    }
    private void OnDisable()
    {
        OnMapBuffDetectionChannelSO.OnEventRaised -= GetMapAddition;
        OnPickTerrainChannelSO.OnEventRaised -= SetLastPickTerrain;
    }

    /// <summary>
    /// 显示所有的地图排列加成规则
    /// </summary>
    void ShowMapAddition()
    {
        AllMapAdditionContent.gameObject.SetActive(IsOnAllMapAdd);
        IsOnAllMapAdd = !IsOnAllMapAdd;

        //激活所有排列规则后需要进行的操作
        for (int i = 0; i < AllMapAdditionContent.childCount; i++)
        {
            if (AllMapAdditionContent.GetChild(i).GetComponent<MapAdditionEntity>().currentOnMapAddition.IsActive)
            {
                //AllMapAdditionContent.GetChild(i).GetComponent<MapAdditionEntity>().OnMapAdditionFlag.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 最终卡牌排列加成效果添加
    /// </summary>
    public void SetAddition()
    {
        //此处换成所有地图排列循环检测
        for (int i = 0; i < AllMapAdditionContent.childCount; i++)
        {
            //添加当前满足排列规则的技能及等级
            //if (AllMapAdditionContent.GetChild(i).GetComponent<MapAdditionEntity>().additionFlag)
            if (AllMapAdditionContent.GetChild(i).GetComponent<MapAdditionEntity>().OnMapAdditionFlag.activeInHierarchy)
            {
                if (!YKDataInfoManager.Instance.CurrentSkillRecord.ContainsKey(AllMapAdditionContent.GetChild(i).GetComponent<MapAdditionEntity>().currentOnMapAddition.Ability))
                {
                    YKDataInfoManager.Instance.CurrentSkillRecord.Add(AllMapAdditionContent.GetChild(i).GetComponent<MapAdditionEntity>().currentOnMapAddition.Ability, AllMapAdditionContent.GetChild(i).GetComponent<MapAdditionEntity>().currentOnMapAddition.RuleLevel);
                }
            }
        }
    }

    /// <summary>
    /// 添加地图加成到场景
    /// </summary>
    public void SetMapAddition()
    {
        for (int i = 0; i < AllMapAdditionContent.childCount; i++)
        {
            Destroy(AllMapAdditionContent.GetChild(i).gameObject);
        }
        OnArrangementRule.Clear();
        foreach (var r in OnArrangementRuleList)
        {
            OnArrangementRule.Add(r);
        }

        for (int i = 0; i < OnArrangementRulesTemp.Count; i++)
        {
            if (OnArrangementRule.Exists(t => t == OnArrangementRulesTemp[i]))
            {
                OnArrangementRule.Remove(OnArrangementRulesTemp[i]);
            }
        }

        OnArrangementRulesTemp.Sort((item1, item2) => { return item1.RuleLevel < item2.RuleLevel ? 1 : -1; });

        //生成满足条件的加成
        for (int i = 0; i < OnArrangementRulesTemp.Count; i++)
        {
            MapAdditionEntity mapAddition = Instantiate(MapAdditionPrefab, AllMapAdditionContent).GetComponent<MapAdditionEntity>();
            mapAddition.AdditionInitialize(OnArrangementRulesTemp[i], this);
            mapAddition.GetComponent<MapAdditionEntity>().OnMapAdditionFlag.SetActive(true);
        }

        //生成所有的加成规则
        for (int i = 0; i < OnArrangementRule.Count; i++)
        {
            MapAdditionEntity mapAddition = Instantiate(MapAdditionPrefab, AllMapAdditionContent).GetComponent<MapAdditionEntity>();
            mapAddition.AdditionInitialize(OnArrangementRule[i], this);
        }
    }

    /// <summary>
    /// 查询地图加成规则的方法
    /// </summary>
    private void GetMapAddition()
    {
        // for (int i = MapAdditionContent.childCount-1; i>=0; i--)
        // {
        //     ObjectPool.Destroy(MapAdditionContent.GetChild(i).gameObject);
        // }
        //调用查询地图加成规则的方法
        MapAdditionDeckUtility.CheckCondition(_mapTerrainUISlotDic, this);

        SetMapAddition();
    }


    private void SetLastPickTerrain(TerrainSO terrain)
    {
        OnTerrainAttibuteSO.value = terrain;
    }

    /// <summary>
    /// 玩家死亡后减掉一张卡牌惩罚
    /// </summary>
    public void PlayerDiedPunishment()
    {

    }

    #region 旧加成规则
    /*
    /// <summary>
    /// 红色牌在最左边
    /// </summary>
    public void GetMapBuffRedCardFarLeft(bool flag)
    {
        OnMapBuffRedCardFarLeft.RaiseEvent(flag);
    }

    /// <summary>
    /// 绿色牌在最右边
    /// </summary>
    public void GetMapBuffGreenCardFarRight(bool flag)
    {
        OnMapBuffGreenCardFarRight.RaiseEvent(flag);
    }

    /// <summary>
    /// 红色牌在Boss右边
    /// </summary>
    public void GetMapBuffRedCardOnBossRight(bool flag)
    {
        OnMapBuffRedCardOnBossRight.RaiseEvent(flag);
    }
    /// <summary>
    /// （完）3.1 当绿色卡牌处于中心位置时
    /// </summary>
    public void GetMapBuff1(bool flag)
    {
        OnMapBuff1.RaiseEvent(flag);
    }
    /// <summary>
    ///    （完）3.2 当所有与绿色卡牌临近（上下左右）的红色卡牌数量<4
    /// </summary>
    public void GetMapBuff2(bool flag)
    {
        OnMapBuff2.RaiseEvent(flag);
    }
    /// <summary>
    ///    （完）3.3 没有绿色卡牌与蓝色卡牌相邻
    /// </summary>
    public void GetMapBuff3(bool flag)
    {
        OnMapBuff3.RaiseEvent(flag);
    }
    /// <summary>
    ///   （完） 3.4 当红色卡牌处于中心位置时
    /// </summary>
    public void GetMapBuff4(bool flag)
    {
        OnMapBuff4.RaiseEvent(flag);
    }
    /// <summary>
    ///  （完）  3.5 当所有与红色卡牌临近（上下左右）的绿色卡牌数量<4
    /// </summary>
    public void GetMapBuff5(bool flag)
    {
        OnMapBuff5.RaiseEvent(flag);
    }
    /// <summary>
    ///     （完）3.6 没有红色卡牌与蓝色卡牌相邻
    /// </summary>
    public void GetMapBuff6(bool flag)
    {
        OnMapBuff6.RaiseEvent(flag);
    }
    /// <summary>
    ///   （完） 3.7 当蓝色卡牌处于中心位置时
    /// </summary>
    public void GetMapBuff7(bool flag)
    {
        OnMapBuff7.RaiseEvent(flag);
    }
    /// <summary>
    ///    （完）3.8 当蓝色卡牌处于非中心位置时
    /// </summary>
    public void GetMapBuff8(bool flag)
    {
        OnMapBuff8.RaiseEvent(flag);
    }
    /// <summary>
    ///    3.9 所有卡牌相邻的卡牌均与其颜色不同
    /// </summary>
    public void GetMapBuff9(bool flag)
    {
        OnMapBuff9.RaiseEvent(flag);
    }
    /// <summary>
    ///    （完）3.10 当与任一蓝色卡牌相邻的红色卡牌数量>2
    /// </summary>
    public void GetMapBuff10(bool flag)
    {
        OnMapBuff10.RaiseEvent(flag);
    }
    /// <summary>
    ///    （完）3.11 当与任一蓝色卡牌相邻的绿色卡牌数量>2
    /// </summary>
    public void GetMapBuff11(bool flag)
    {
        OnMapBuff11.RaiseEvent(flag);
    }

    /// <summary>
    ///   当与Boss相邻的任意卡牌数量>0
    /// </summary>
    public void GetMapNearBoss(int num)
    {
        if (num <= 0) { return; }
        OnMapBuffNearBossChannelSO.RaiseEvent();
        //Debug.Log("有多少张牌与Boss相邻："+ num);
    }

    */
    #endregion

    /// <summary>
    /// 给排列的规则更新技能等级
    /// </summary>
    /// <param name="rule"></param>
    /// <param name="flag"></param>
    void CreateMapAdditionEntityOld(ArrangementRulesEventChannelSO rule, bool flag, int num)
    {
        //GameObject mapAddition = ObjectPool.Instantiate(MapAdditionPrefab, transform.position, Quaternion.identity, MapAdditionContent);
        //mapAddition.GetComponent<MapAdditionEntity>().Init(rule,num);
        for (int i = 0; i < AllMapAdditionContent.childCount; i++)
        {
            if (flag)
            {
                //AllMapAdditionContent.GetChild(i).GetComponent<MapAdditionEntity>().Init(rule, num);
            }
        }
    }

    /// <summary>
    ///    1张红色卡牌单独
    /// </summary>
    public void GetMapBuffOnAloneRed1(bool flag, int num = 0)
    {
        OnAloneRuleRed1.RaiseEvent(flag);
        OnAloneRuleRed1.RuleLevel = num;
    }
    /// <summary>
    ///    1张绿色卡牌单独
    /// </summary>
    public void GetMapBuffOnAloneGreen1(bool flag, int num = 0)
    {
        OnAloneRuleGreen1.RaiseEvent(flag);
        OnAloneRuleGreen1.RuleLevel = num;
    }
    /// <summary>
    ///    1张蓝色卡牌单独
    /// </summary>
    public void GetMapBuffOnAloneBlue1(bool flag, int num = 0)
    {
        OnAloneRuleBlue1.RaiseEvent(flag);
        OnAloneRuleBlue1.RuleLevel = num;
    }
    /// <summary>
    ///    3张红色卡牌排成 一 行/列
    /// </summary>
    public void GetMapBuffOnLineRuleRed3(bool flag, int num = 0)
    {
        OnLineRuleRed3.RaiseEvent(flag);
        OnLineRuleRed3.RuleLevel = num;
        //CreateMapAdditionEntity(OnLineRuleRed3,flag, num);
    }

    /// <summary>
    ///    3张绿色卡牌排成 一 行/列
    /// </summary>
    public void GetMapBuffOnLineRuleGreen3(bool flag, int num = 0)
    {
        OnLineRuleGreen3.RaiseEvent(flag);
        OnLineRuleGreen3.RuleLevel = num;
        //CreateMapAdditionEntity(OnLineRuleGreen3,flag,num);
    }
    /// <summary>
    ///    3张蓝色卡牌排成 一 行/列
    /// </summary>
    public void GetMapBuffOnLineRuleBlue3(bool flag, int num = 0)
    {
        OnLineRuleBlue3.RaiseEvent(flag);
        OnLineRuleBlue3.RuleLevel = num;
        //CreateMapAdditionEntity(OnLineRuleBlue3,flag,num);
    }
    /// <summary>
    ///    红2绿1 一 行/列
    /// </summary>
    public void GetMapBuffOnLineRuleRed2Green1(bool flag, int num = 0)
    {
        OnLineRuleRed2Green1.RaiseEvent(flag);
        OnLineRuleRed2Green1.RuleLevel = num;
        //CreateMapAdditionEntityOld(OnLineRuleRed2Green1,flag, num);
    }
    /// <summary>
    ///     红2蓝1 一 行/列
    /// </summary>
    public void GetMapBuffOnLineRuleRed2Blue1(bool flag, int num = 0)
    {
        OnLineRuleRed2Blue1.RaiseEvent(flag);
        OnLineRuleRed2Blue1.RuleLevel = num;
        //CreateMapAdditionEntity(OnLineRuleRed2Blue1,flag, num);
    }
    /// <summary>
    ///    绿2红1 一 行/列
    /// </summary>
    public void GetMapBuffOnLineRuleGreen2Red1(bool flag, int num = 0)
    {
        OnLineRuleGreen2Red1.RaiseEvent(flag);
        OnLineRuleGreen2Red1.RuleLevel = num;
        //CreateMapAdditionEntity(OnLineRuleGreen2Red1,flag, num);
    }
    /// <summary>
    ///    绿2蓝1 一 行/列
    /// </summary>
    public void GetMapBuffOnLineRuleGreen2Blue1(bool flag, int num = 0)
    {
        OnLineRuleGreen2Blue1.RaiseEvent(flag);
        OnLineRuleGreen2Blue1.RuleLevel = num;
        //CreateMapAdditionEntity(OnLineRuleGreen2Blue1, flag, num);
    }
    /// <summary>
    ///    蓝2红1 一 行/列
    /// </summary>
    public void GetMapBuffOnLineRuleBlue2Red1(bool flag, int num = 0)
    {
        OnLineRuleBlue2Red1.RaiseEvent(flag);
        OnLineRuleBlue2Red1.RuleLevel = num;
        //CreateMapAdditionEntity(OnLineRuleBlue2Red1, flag, num);
    }
    /// <summary>
    ///   蓝2绿1 一 行/列
    /// </summary>
    public void GetMapBuffOnLineRuleBlue2Green1(bool flag, int num = 0)
    {
        OnLineRuleBlue2Green1.RaiseEvent(flag);
        OnLineRuleBlue2Green1.RuleLevel = num;
        //CreateMapAdditionEntity(OnLineRuleBlue2Green1, flag, num);
    }
    /// <summary>
    ///    红1绿1蓝1 一 行/列
    /// </summary>
    public void GetMapBuffOnLineRuleRed1Green1Blue1(bool flag, int num = 0)
    {
        OnLineRuleRed1Green1Blue1.RaiseEvent(flag);
        OnLineRuleRed1Green1Blue1.RuleLevel = num;
        //CreateMapAdditionEntity(OnLineRuleRed1Green1Blue1, flag, num);
    }

    /// <summary>
    ///    3张红色卡牌排成 一 斜对
    /// </summary>
    public void GetMapBuffOnObliqueRuleRed3(bool flag, int num = 0)
    {
        //OnObliqueRuleRed3.RaiseEvent(flag);
        //CreateMapAdditionEntity(OnObliqueRuleRed3, flag,num);
    }
    /// <summary>
    ///     3张绿色卡牌排成 一 斜对
    /// </summary>
    public void GetMapBuffOnObliqueRuleGreen3(bool flag, int num = 0)
    {
        //OnObliqueRuleGreen3.RaiseEvent(flag);
        //CreateMapAdditionEntity(OnObliqueRuleGreen3, flag,num);
    }
    /// <summary>
    ///   3张蓝色卡牌排成 一 斜对
    /// </summary>
    public void GetMapBuffOnObliqueRuleBlue3(bool flag, int num = 0)
    {
        //OnObliqueRuleBlue3.RaiseEvent(flag);
        //CreateMapAdditionEntity(OnObliqueRuleBlue3, flag,num);
    }
    /// <summary>
    ///    1红1绿1蓝 排成 一 斜对
    /// </summary>
    public void GetMapBuffOnObliqueRuleRed1Green1Blue1(bool flag, int num = 0)
    {
        //OnObliqueRuleRed1Green1Blue1.RaiseEvent(flag);
        //CreateMapAdditionEntity(OnObliqueRuleRed1Green1Blue1, flag,num);
    }

    /// <summary>
    ///  红方阵达成
    /// </summary>
    public void GetMapBuffOnSquaresRed(bool flag, int num = 0)
    {
        OnSquaresRuleRed.RaiseEvent(flag);
        OnSquaresRuleRed.RuleLevel = num;
    }
    /// <summary>
    ///  绿方阵达成
    /// </summary>
    public void GetMapBuffOnSquaresGreen(bool flag, int num = 0)
    {
        OnSquaresRuleGreen.RaiseEvent(flag);
        OnSquaresRuleGreen.RuleLevel = num;
    }
    /// <summary>
    ///  蓝方阵达成
    /// </summary>
    public void GetMapBuffOnSquaresBlue(bool flag, int num = 0)
    {
        OnSquaresRuleBlue.RaiseEvent(flag);
        OnSquaresRuleBlue.RuleLevel = num;
    }

    /// <summary>
    ///  红斜达成
    /// </summary>
    public void GetMapBuffOnObliqueRed(bool flag, int num = 0)
    {
        OnObliqueRuleRed3.RaiseEvent(flag);
        OnObliqueRuleRed3.RuleLevel = num;
    }
    /// <summary>
    ///  绿斜达成
    /// </summary>
    public void GetMapBuffOnObliqueGreen(bool flag, int num = 0)
    {
        OnObliqueRuleGreen3.RaiseEvent(flag);
        OnObliqueRuleGreen3.RuleLevel = num;
    }
    /// <summary>
    ///  蓝斜达成
    /// </summary>
    public void GetMapBuffOnObliqueBlue(bool flag, int num = 0)
    {
        OnObliqueRuleBlue3.RaiseEvent(flag);
        OnObliqueRuleBlue3.RuleLevel = num;
    }
    /// <summary>
    ///  混合斜达成
    /// </summary>
    public void GetMapBuffOnObliqueMix(bool flag, int num = 0)
    {
        OnObliqueRuleRed1Green1Blue1.RaiseEvent(flag);
        OnObliqueRuleRed1Green1Blue1.RuleLevel = num;
    }

    /// <summary>
    ///  全为红色达成
    /// </summary>
    public void GetMapBuffOnHomochromaticRed(bool flag, int num = 0)
    {
        OnHomochromaticRuleRed.RaiseEvent(flag);
        OnHomochromaticRuleRed.RuleLevel = num;
    }
    /// <summary>
    ///  全为绿色达成
    /// </summary>
    public void GetMapBuffOnHomochromaticGreen(bool flag, int num = 0)
    {
        OnHomochromaticRuleGreen.RaiseEvent(flag);
        OnHomochromaticRuleGreen.RuleLevel = num;
    }
    /// <summary>
    ///  全为蓝色达成
    /// </summary>
    public void GetMapBuffOnHomochromaticBlue(bool flag, int num = 0)
    {
        OnHomochromaticRuleBlue.RaiseEvent(flag);
        OnHomochromaticRuleBlue.RuleLevel = num;
    }
}
