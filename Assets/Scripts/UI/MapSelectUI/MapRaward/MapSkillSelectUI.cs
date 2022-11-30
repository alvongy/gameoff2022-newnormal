using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static YKQuestWatcher;

public class MapSkillSelectUI : SerializedMonoBehaviour
{
    [SerializeField] VoidEventChannelSO _selectSkillChoiceUIEvent;
    [SerializeField] private IntEventChannelSO _addSecletcSkillEvent;

    [SerializeField] [InfoBox("胜利条件_击杀Boss的任务")]public TerrainSO _bossTerrainSO;

    [FoldoutGroup("Object")] public GameObject _skillSelectPanel;
    [FoldoutGroup("Object")] public GameObject _skillCardPrefab;
    [FoldoutGroup("Object")] public GameObject _terrainUIPrefab;
    //[FoldoutGroup("Object")] public GameObject _gameOverPanel;
    [FoldoutGroup("Object")] public Button _selectSkillBtn;
    [FoldoutGroup("Object")] public MapBuffControl _mapBuffControl;
    public Dictionary<int, MFAbility> RuntimeSkills = new Dictionary<int, MFAbility>();

    private TerrainSO selectTerrain;
    private Transform skillTransform;

    public int selectSkillID;
    private Dictionary<int, TerrainSO> _terrainRewardDic = new Dictionary<int, TerrainSO>();

    private void Awake()
    {
        _selectSkillBtn.onClick.AddListener(SelectSkill);
        RuntimeSkills = YKDataInfoManager.Instance.globalRuntimeSkills;
    }
    void OnEnable()
    {
        if (YKRewardWatcher.Instance != null)
        {
            _terrainRewardDic = YKRewardWatcher.Instance.TerrainStorer;
        }
        _selectSkillChoiceUIEvent.OnEventRaised += OpenSkillChoiceUI;
        _addSecletcSkillEvent.OnEventRaised += StartSelect;
    }
    private void OnDisable()
    {
        _selectSkillChoiceUIEvent.OnEventRaised -= OpenSkillChoiceUI;
        _addSecletcSkillEvent.OnEventRaised -= StartSelect;
    }

    private void StartSelect(int skillID)
    {
        selectSkillID = skillID;
        RefreshSelectSkill();
    }

    private void OpenSkillChoiceUI()
    {
        foreach (var q in _bossTerrainSO.GetQuests())
        {
            if (YKQuestWatcher.Instance.questData.ContainsKey(q.QID) && YKQuestWatcher.Instance.questData[q.QID].state == QuestStatus.success)
            {
                YKDataInfoManager.Instance.isChapterOver = true;
            }
        }
        if (YKDataInfoManager.Instance.isChapterOver)
        {
            // Debug.Log("第一章结束，待执行操作！");
            MapSelectUIManager.Instance.PopUpPanel.InitializeGameOver();
            return;
        }

        foreach (var a in YKDataInfoManager.Instance.LearnedAbilityDataBase.database)//删掉已经5级的技能，不需要再学习
        {
            if (a.Value.level == 4 && RuntimeSkills.ContainsKey(a.Key))
            {
                RuntimeSkills.Remove(a.Key);
            }
        }

        transform.GetChild(0).gameObject.SetActive(true);

        if (RuntimeSkills.Count == 0)
        {
            return;
        }
        else if (RuntimeSkills.Count <= 3)
        {
            foreach (var r in RuntimeSkills)
            {
                GameObject skill = Instantiate(_skillCardPrefab, _skillSelectPanel.transform);
                skill.GetComponent<SkillCardEneity>().SkillCardInit(r.Value);
            }
        }
        else
        {
            MFAbility[] result = new MFAbility[3];
            List<int> list = new List<int>();
            list.AddRange(RuntimeSkills.Keys);
            for (int i = 0; i < 3; i++)
            {
                int index = Random.Range(0, list.Count);
                result[i] = RuntimeSkills[list[index]];
                list.RemoveAt(index);
            }

            for (int i = 0; i < result.Length; i++)
            {
                GameObject skill = Instantiate(_skillCardPrefab, _skillSelectPanel.transform);
                skill.GetComponent<SkillCardEneity>().SkillCardInit(result[i]);
            }
        }
        _addSecletcSkillEvent.RaiseEvent(_skillSelectPanel.transform.GetChild(0).GetComponent<SkillCardEneity>().SkillID);
    }
    private void SelectSkill()
    {
        //将选择的技能储存起来，在进入战斗场景时学习主动技能
        YKDataInfoManager.Instance.LearnedAbilityRecordList.Add(selectSkillID);
        if (YKDataInfoManager.Instance.AbilityDataBase.database.ContainsKey(selectSkillID))
        {
            YKDataInfoManager.Instance.AbilityDataBase.database[selectSkillID].level++;

            if (!YKDataInfoManager.Instance.LearnedAbilityDataBase.database.ContainsKey(selectSkillID))//学习过的技能记录下来
            {
                YKDataInfoManager.Instance.LearnedAbilityDataBase.database.Add(selectSkillID, YKDataInfoManager.Instance.AbilityDataBase.database[selectSkillID]);
            }
        }
    
        transform.GetChild(0).gameObject.SetActive(false);
        for (int i = 0; i < _skillSelectPanel.transform.childCount; i++)
        {
            Destroy(_skillSelectPanel.transform.GetChild(i).gameObject);
        }
    }

    void RefreshSelectSkill()
    {
        for (int i = 0; i < _skillSelectPanel.transform.childCount; i++)
        {
            skillTransform = _skillSelectPanel.transform.GetChild(i);
            if (skillTransform.GetComponent<SkillCardEneity>().SkillID == selectSkillID)
            {
                skillTransform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                skillTransform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    public void OpenTerrainChoiceUI()
    {
        foreach (var q in _bossTerrainSO.GetQuests())
        {
            if (YKQuestWatcher.Instance.questData.ContainsKey(q.QID) && YKQuestWatcher.Instance.questData[q.QID].state == QuestStatus.success)
            {
                YKDataInfoManager.Instance.isChapterOver = true;
            }
        }
        if (YKDataInfoManager.Instance.isChapterOver)
        {
           // Debug.Log("第一章结束，待执行操作！");
            MapSelectUIManager.Instance.PopUpPanel.InitializeGameOver();
            return;
        }
        if (YKDataInfoManager.Instance.desktopCardNum >= 9)
        {
           // Debug.Log("桌面卡牌已经有九张了，不再选牌！");
            MapSelectUIManager.Instance.EnterGame_Btn.gameObject.SetActive(true);
            MapSelectUIManager.Instance.IsActiveEnterGame_Btn = true; ;
            return;
        }

        MapSelectUIManager.Instance.IsActiveEnterGame_Btn = false;
        MapSelectUIManager.Instance.EnterGame_Btn.gameObject.SetActive(false);
        _skillSelectPanel.SetActive(true);

        if (_terrainRewardDic.Count == 0)
        {
            YKDataInfoManager.Instance.isChapterOver = true;
            //Debug.Log("第一章所有卡牌都已经刷完了");
            return;
        }
        else if (_terrainRewardDic.Count <= 3)
        {
            foreach (var r in _terrainRewardDic)
            {
                GameObject terrain = Instantiate(_skillCardPrefab, _skillSelectPanel.transform.GetChild(0));
                terrain.GetComponent<MapSkillCardEneity>().TerrainUIInit(r.Value);
            }
        }
        else
        {
            TerrainSO[] result = new TerrainSO[3];
            List<int> list = new List<int>();
            list.AddRange(_terrainRewardDic.Keys);
            for (int i = 0; i < 3; i++)
            {
                int index = Random.Range(0, list.Count);
                result[i] = _terrainRewardDic[list[index]];
                list.RemoveAt(index);
            }

            for (int i = 0; i < result.Length; i++)
            {
                GameObject terrain = Instantiate(_skillCardPrefab, _skillSelectPanel.transform.GetChild(0));
                terrain.GetComponent<MapSkillCardEneity>().TerrainUIInit(result[i]);
            }
        } 
    }

    private void SelectTerrainUI(TerrainSO terrain)
    {
        selectTerrain = terrain;
    }
    private void SelectTerrainUI()
    {
        if (selectTerrain == null) { return; }
        if (!YKRewardWatcher.Instance._terrainDataBaseSelected.database.ContainsKey(selectTerrain.TID))
        {
            YKRewardWatcher.Instance._terrainDataBaseSelected.database.Add(selectTerrain.TID, selectTerrain);
        }

        _skillSelectPanel.SetActive(false);
        for (int i = 0; i < _skillSelectPanel.transform.childCount; i++)
        {
            Destroy(_skillSelectPanel.transform.GetChild(i).gameObject);
        }
        AddTerrainInDesktop(selectTerrain);

        //待优化
        MapSelectUIManager.Instance.EnterGame_Btn.gameObject.SetActive(true);
        MapSelectUIManager.Instance.IsActiveEnterGame_Btn = true; ;
    }
    void AddTerrainInDesktop(TerrainSO terrain)
    {
        YKRewardWatcher.Instance.TerrainStorer.Remove(terrain.TID);
        for (int i=0;i < _mapBuffControl.TerrainUIParent.childCount;i++)
        {
            if (_mapBuffControl.TerrainUIParent.GetChild(i).GetComponent<MapTerrainUISlot>()._currentTerrainUI==null)
            {
                GameObject terrainEntity = Instantiate(_terrainUIPrefab, _mapBuffControl.TerrainUIParent.GetChild(i));
                terrainEntity.GetComponent<TerrainUI>().TerrainUIInit(terrain);
                break;
            }
        }
    }
    private void GameOver()
    {
        //此处应该是进入第二章的内容，需将一些数据进行初始化等操作
        UnityEngine.SceneManagement.SceneManager.LoadScene("Initialization");
    }
}