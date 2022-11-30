using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MapBuffControl : SerializedMonoBehaviour
{
    [SerializeField] public Dictionary<Vector2, MapTerrainUISlot> _mapTerrainUISlotDic;

    [SerializeField] private TerrainEventChannelSO OnPickTerrainChannelSO;//���ǽ�������ο��͸�ֵ�����ε��߼�������
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
        MapSelectUIManager.Instance.EnterGame_Btn.onClick.AddListener(SetAddition);//Buttonδ�����������ܰ󶨼����¼���
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
    /// ��ʾ���еĵ�ͼ���мӳɹ���
    /// </summary>
    void ShowMapAddition()
    {
        AllMapAdditionContent.gameObject.SetActive(IsOnAllMapAdd);
        IsOnAllMapAdd = !IsOnAllMapAdd;

        //�����������й������Ҫ���еĲ���
        for (int i = 0; i < AllMapAdditionContent.childCount; i++)
        {
            if (AllMapAdditionContent.GetChild(i).GetComponent<MapAdditionEntity>().currentOnMapAddition.IsActive)
            {
                //AllMapAdditionContent.GetChild(i).GetComponent<MapAdditionEntity>().OnMapAdditionFlag.SetActive(false);
            }
        }
    }

    /// <summary>
    /// ���տ������мӳ�Ч�����
    /// </summary>
    public void SetAddition()
    {
        //�˴��������е�ͼ����ѭ�����
        for (int i = 0; i < AllMapAdditionContent.childCount; i++)
        {
            //��ӵ�ǰ�������й���ļ��ܼ��ȼ�
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
    /// ��ӵ�ͼ�ӳɵ�����
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

        //�������������ļӳ�
        for (int i = 0; i < OnArrangementRulesTemp.Count; i++)
        {
            MapAdditionEntity mapAddition = Instantiate(MapAdditionPrefab, AllMapAdditionContent).GetComponent<MapAdditionEntity>();
            mapAddition.AdditionInitialize(OnArrangementRulesTemp[i], this);
            mapAddition.GetComponent<MapAdditionEntity>().OnMapAdditionFlag.SetActive(true);
        }

        //�������еļӳɹ���
        for (int i = 0; i < OnArrangementRule.Count; i++)
        {
            MapAdditionEntity mapAddition = Instantiate(MapAdditionPrefab, AllMapAdditionContent).GetComponent<MapAdditionEntity>();
            mapAddition.AdditionInitialize(OnArrangementRule[i], this);
        }
    }

    /// <summary>
    /// ��ѯ��ͼ�ӳɹ���ķ���
    /// </summary>
    private void GetMapAddition()
    {
        // for (int i = MapAdditionContent.childCount-1; i>=0; i--)
        // {
        //     ObjectPool.Destroy(MapAdditionContent.GetChild(i).gameObject);
        // }
        //���ò�ѯ��ͼ�ӳɹ���ķ���
        MapAdditionDeckUtility.CheckCondition(_mapTerrainUISlotDic, this);

        SetMapAddition();
    }


    private void SetLastPickTerrain(TerrainSO terrain)
    {
        OnTerrainAttibuteSO.value = terrain;
    }

    /// <summary>
    /// ������������һ�ſ��Ƴͷ�
    /// </summary>
    public void PlayerDiedPunishment()
    {

    }

    #region �ɼӳɹ���
    /*
    /// <summary>
    /// ��ɫ���������
    /// </summary>
    public void GetMapBuffRedCardFarLeft(bool flag)
    {
        OnMapBuffRedCardFarLeft.RaiseEvent(flag);
    }

    /// <summary>
    /// ��ɫ�������ұ�
    /// </summary>
    public void GetMapBuffGreenCardFarRight(bool flag)
    {
        OnMapBuffGreenCardFarRight.RaiseEvent(flag);
    }

    /// <summary>
    /// ��ɫ����Boss�ұ�
    /// </summary>
    public void GetMapBuffRedCardOnBossRight(bool flag)
    {
        OnMapBuffRedCardOnBossRight.RaiseEvent(flag);
    }
    /// <summary>
    /// ���꣩3.1 ����ɫ���ƴ�������λ��ʱ
    /// </summary>
    public void GetMapBuff1(bool flag)
    {
        OnMapBuff1.RaiseEvent(flag);
    }
    /// <summary>
    ///    ���꣩3.2 ����������ɫ�����ٽ����������ң��ĺ�ɫ��������<4
    /// </summary>
    public void GetMapBuff2(bool flag)
    {
        OnMapBuff2.RaiseEvent(flag);
    }
    /// <summary>
    ///    ���꣩3.3 û����ɫ��������ɫ��������
    /// </summary>
    public void GetMapBuff3(bool flag)
    {
        OnMapBuff3.RaiseEvent(flag);
    }
    /// <summary>
    ///   ���꣩ 3.4 ����ɫ���ƴ�������λ��ʱ
    /// </summary>
    public void GetMapBuff4(bool flag)
    {
        OnMapBuff4.RaiseEvent(flag);
    }
    /// <summary>
    ///  ���꣩  3.5 ���������ɫ�����ٽ����������ң�����ɫ��������<4
    /// </summary>
    public void GetMapBuff5(bool flag)
    {
        OnMapBuff5.RaiseEvent(flag);
    }
    /// <summary>
    ///     ���꣩3.6 û�к�ɫ��������ɫ��������
    /// </summary>
    public void GetMapBuff6(bool flag)
    {
        OnMapBuff6.RaiseEvent(flag);
    }
    /// <summary>
    ///   ���꣩ 3.7 ����ɫ���ƴ�������λ��ʱ
    /// </summary>
    public void GetMapBuff7(bool flag)
    {
        OnMapBuff7.RaiseEvent(flag);
    }
    /// <summary>
    ///    ���꣩3.8 ����ɫ���ƴ��ڷ�����λ��ʱ
    /// </summary>
    public void GetMapBuff8(bool flag)
    {
        OnMapBuff8.RaiseEvent(flag);
    }
    /// <summary>
    ///    3.9 ���п������ڵĿ��ƾ�������ɫ��ͬ
    /// </summary>
    public void GetMapBuff9(bool flag)
    {
        OnMapBuff9.RaiseEvent(flag);
    }
    /// <summary>
    ///    ���꣩3.10 ������һ��ɫ�������ڵĺ�ɫ��������>2
    /// </summary>
    public void GetMapBuff10(bool flag)
    {
        OnMapBuff10.RaiseEvent(flag);
    }
    /// <summary>
    ///    ���꣩3.11 ������һ��ɫ�������ڵ���ɫ��������>2
    /// </summary>
    public void GetMapBuff11(bool flag)
    {
        OnMapBuff11.RaiseEvent(flag);
    }

    /// <summary>
    ///   ����Boss���ڵ����⿨������>0
    /// </summary>
    public void GetMapNearBoss(int num)
    {
        if (num <= 0) { return; }
        OnMapBuffNearBossChannelSO.RaiseEvent();
        //Debug.Log("�ж���������Boss���ڣ�"+ num);
    }

    */
    #endregion

    /// <summary>
    /// �����еĹ�����¼��ܵȼ�
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
    ///    1�ź�ɫ���Ƶ���
    /// </summary>
    public void GetMapBuffOnAloneRed1(bool flag, int num = 0)
    {
        OnAloneRuleRed1.RaiseEvent(flag);
        OnAloneRuleRed1.RuleLevel = num;
    }
    /// <summary>
    ///    1����ɫ���Ƶ���
    /// </summary>
    public void GetMapBuffOnAloneGreen1(bool flag, int num = 0)
    {
        OnAloneRuleGreen1.RaiseEvent(flag);
        OnAloneRuleGreen1.RuleLevel = num;
    }
    /// <summary>
    ///    1����ɫ���Ƶ���
    /// </summary>
    public void GetMapBuffOnAloneBlue1(bool flag, int num = 0)
    {
        OnAloneRuleBlue1.RaiseEvent(flag);
        OnAloneRuleBlue1.RuleLevel = num;
    }
    /// <summary>
    ///    3�ź�ɫ�����ų� һ ��/��
    /// </summary>
    public void GetMapBuffOnLineRuleRed3(bool flag, int num = 0)
    {
        OnLineRuleRed3.RaiseEvent(flag);
        OnLineRuleRed3.RuleLevel = num;
        //CreateMapAdditionEntity(OnLineRuleRed3,flag, num);
    }

    /// <summary>
    ///    3����ɫ�����ų� һ ��/��
    /// </summary>
    public void GetMapBuffOnLineRuleGreen3(bool flag, int num = 0)
    {
        OnLineRuleGreen3.RaiseEvent(flag);
        OnLineRuleGreen3.RuleLevel = num;
        //CreateMapAdditionEntity(OnLineRuleGreen3,flag,num);
    }
    /// <summary>
    ///    3����ɫ�����ų� һ ��/��
    /// </summary>
    public void GetMapBuffOnLineRuleBlue3(bool flag, int num = 0)
    {
        OnLineRuleBlue3.RaiseEvent(flag);
        OnLineRuleBlue3.RuleLevel = num;
        //CreateMapAdditionEntity(OnLineRuleBlue3,flag,num);
    }
    /// <summary>
    ///    ��2��1 һ ��/��
    /// </summary>
    public void GetMapBuffOnLineRuleRed2Green1(bool flag, int num = 0)
    {
        OnLineRuleRed2Green1.RaiseEvent(flag);
        OnLineRuleRed2Green1.RuleLevel = num;
        //CreateMapAdditionEntityOld(OnLineRuleRed2Green1,flag, num);
    }
    /// <summary>
    ///     ��2��1 һ ��/��
    /// </summary>
    public void GetMapBuffOnLineRuleRed2Blue1(bool flag, int num = 0)
    {
        OnLineRuleRed2Blue1.RaiseEvent(flag);
        OnLineRuleRed2Blue1.RuleLevel = num;
        //CreateMapAdditionEntity(OnLineRuleRed2Blue1,flag, num);
    }
    /// <summary>
    ///    ��2��1 һ ��/��
    /// </summary>
    public void GetMapBuffOnLineRuleGreen2Red1(bool flag, int num = 0)
    {
        OnLineRuleGreen2Red1.RaiseEvent(flag);
        OnLineRuleGreen2Red1.RuleLevel = num;
        //CreateMapAdditionEntity(OnLineRuleGreen2Red1,flag, num);
    }
    /// <summary>
    ///    ��2��1 һ ��/��
    /// </summary>
    public void GetMapBuffOnLineRuleGreen2Blue1(bool flag, int num = 0)
    {
        OnLineRuleGreen2Blue1.RaiseEvent(flag);
        OnLineRuleGreen2Blue1.RuleLevel = num;
        //CreateMapAdditionEntity(OnLineRuleGreen2Blue1, flag, num);
    }
    /// <summary>
    ///    ��2��1 һ ��/��
    /// </summary>
    public void GetMapBuffOnLineRuleBlue2Red1(bool flag, int num = 0)
    {
        OnLineRuleBlue2Red1.RaiseEvent(flag);
        OnLineRuleBlue2Red1.RuleLevel = num;
        //CreateMapAdditionEntity(OnLineRuleBlue2Red1, flag, num);
    }
    /// <summary>
    ///   ��2��1 һ ��/��
    /// </summary>
    public void GetMapBuffOnLineRuleBlue2Green1(bool flag, int num = 0)
    {
        OnLineRuleBlue2Green1.RaiseEvent(flag);
        OnLineRuleBlue2Green1.RuleLevel = num;
        //CreateMapAdditionEntity(OnLineRuleBlue2Green1, flag, num);
    }
    /// <summary>
    ///    ��1��1��1 һ ��/��
    /// </summary>
    public void GetMapBuffOnLineRuleRed1Green1Blue1(bool flag, int num = 0)
    {
        OnLineRuleRed1Green1Blue1.RaiseEvent(flag);
        OnLineRuleRed1Green1Blue1.RuleLevel = num;
        //CreateMapAdditionEntity(OnLineRuleRed1Green1Blue1, flag, num);
    }

    /// <summary>
    ///    3�ź�ɫ�����ų� һ б��
    /// </summary>
    public void GetMapBuffOnObliqueRuleRed3(bool flag, int num = 0)
    {
        //OnObliqueRuleRed3.RaiseEvent(flag);
        //CreateMapAdditionEntity(OnObliqueRuleRed3, flag,num);
    }
    /// <summary>
    ///     3����ɫ�����ų� һ б��
    /// </summary>
    public void GetMapBuffOnObliqueRuleGreen3(bool flag, int num = 0)
    {
        //OnObliqueRuleGreen3.RaiseEvent(flag);
        //CreateMapAdditionEntity(OnObliqueRuleGreen3, flag,num);
    }
    /// <summary>
    ///   3����ɫ�����ų� һ б��
    /// </summary>
    public void GetMapBuffOnObliqueRuleBlue3(bool flag, int num = 0)
    {
        //OnObliqueRuleBlue3.RaiseEvent(flag);
        //CreateMapAdditionEntity(OnObliqueRuleBlue3, flag,num);
    }
    /// <summary>
    ///    1��1��1�� �ų� һ б��
    /// </summary>
    public void GetMapBuffOnObliqueRuleRed1Green1Blue1(bool flag, int num = 0)
    {
        //OnObliqueRuleRed1Green1Blue1.RaiseEvent(flag);
        //CreateMapAdditionEntity(OnObliqueRuleRed1Green1Blue1, flag,num);
    }

    /// <summary>
    ///  �췽����
    /// </summary>
    public void GetMapBuffOnSquaresRed(bool flag, int num = 0)
    {
        OnSquaresRuleRed.RaiseEvent(flag);
        OnSquaresRuleRed.RuleLevel = num;
    }
    /// <summary>
    ///  �̷�����
    /// </summary>
    public void GetMapBuffOnSquaresGreen(bool flag, int num = 0)
    {
        OnSquaresRuleGreen.RaiseEvent(flag);
        OnSquaresRuleGreen.RuleLevel = num;
    }
    /// <summary>
    ///  ��������
    /// </summary>
    public void GetMapBuffOnSquaresBlue(bool flag, int num = 0)
    {
        OnSquaresRuleBlue.RaiseEvent(flag);
        OnSquaresRuleBlue.RuleLevel = num;
    }

    /// <summary>
    ///  ��б���
    /// </summary>
    public void GetMapBuffOnObliqueRed(bool flag, int num = 0)
    {
        OnObliqueRuleRed3.RaiseEvent(flag);
        OnObliqueRuleRed3.RuleLevel = num;
    }
    /// <summary>
    ///  ��б���
    /// </summary>
    public void GetMapBuffOnObliqueGreen(bool flag, int num = 0)
    {
        OnObliqueRuleGreen3.RaiseEvent(flag);
        OnObliqueRuleGreen3.RuleLevel = num;
    }
    /// <summary>
    ///  ��б���
    /// </summary>
    public void GetMapBuffOnObliqueBlue(bool flag, int num = 0)
    {
        OnObliqueRuleBlue3.RaiseEvent(flag);
        OnObliqueRuleBlue3.RuleLevel = num;
    }
    /// <summary>
    ///  ���б���
    /// </summary>
    public void GetMapBuffOnObliqueMix(bool flag, int num = 0)
    {
        OnObliqueRuleRed1Green1Blue1.RaiseEvent(flag);
        OnObliqueRuleRed1Green1Blue1.RuleLevel = num;
    }

    /// <summary>
    ///  ȫΪ��ɫ���
    /// </summary>
    public void GetMapBuffOnHomochromaticRed(bool flag, int num = 0)
    {
        OnHomochromaticRuleRed.RaiseEvent(flag);
        OnHomochromaticRuleRed.RuleLevel = num;
    }
    /// <summary>
    ///  ȫΪ��ɫ���
    /// </summary>
    public void GetMapBuffOnHomochromaticGreen(bool flag, int num = 0)
    {
        OnHomochromaticRuleGreen.RaiseEvent(flag);
        OnHomochromaticRuleGreen.RuleLevel = num;
    }
    /// <summary>
    ///  ȫΪ��ɫ���
    /// </summary>
    public void GetMapBuffOnHomochromaticBlue(bool flag, int num = 0)
    {
        OnHomochromaticRuleBlue.RaiseEvent(flag);
        OnHomochromaticRuleBlue.RuleLevel = num;
    }
}
