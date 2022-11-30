using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AbilityPool : SerializedMonoBehaviour
{
    public Dictionary<int, MFAbility> RuntimeAbilities = new Dictionary<int, MFAbility>();

    [SerializeField] private IntEventChannelSO ObtainAbilityIDEvent;
    [SerializeField] private VoidEventChannelSO OpenAbilityChoiceUIEvent;
    [SerializeField] private IntEventChannelSO _addSkillEvent;
    [SerializeField] private IntEventChannelSO _addSecletcSkillEvent;

    public Transform SelectPanel;
    public GameObject SkillCardObj;
    public Button SelectSkillBtn;

    private MFAbilityDatabase abilityDataBase;
    private Transform skillTransform;
    public int currentSkillID;

    private void Awake()
    {
        abilityDataBase = YKDataInfoManager.Instance.AbilityDataBase;
        RuntimeAbilities = YKDataInfoManager.Instance.globalRuntimeAbilities;

        //获取祈祷石上面的基础技能
        //int basicAID = (int)YKDataInfoManager.Instance.globalPrayStone.DataPack["MFAbility"];
        //MFAbility basicAbility = DataBase.database[basicAID];
        //if (!RuntimeAbilities.ContainsKey(basicAID)) { RuntimeAbilities.Add(basicAID, basicAbility); }

        SelectSkillBtn.onClick.AddListener(EndSelect);
    }

    void OnEnable()
    {
        ObtainAbilityIDEvent.OnEventRaised += OnObtainAbility;
        OpenAbilityChoiceUIEvent.OnEventRaised += OpenAbilityChoiceUI;
        _addSecletcSkillEvent.OnEventRaised += StartSelect;
    }
    private void OnDisable()
    {
        ObtainAbilityIDEvent.OnEventRaised -= OnObtainAbility;
        OpenAbilityChoiceUIEvent.OnEventRaised -= OpenAbilityChoiceUI;
        _addSecletcSkillEvent.OnEventRaised -= StartSelect;
    }

    public void OnObtainAbility(int id)
    {
        if (RuntimeAbilities.ContainsKey(id))
        {
            for (int i = 0; i < abilityDataBase.database[id].AdvancedAbiltyIDArray.Length; i++)
            {
                if (!RuntimeAbilities.ContainsKey(abilityDataBase.database[id].AdvancedAbiltyIDArray[i])){
                    RuntimeAbilities.Add(abilityDataBase.database[id].AdvancedAbiltyIDArray[i], abilityDataBase.database[abilityDataBase.database[id].AdvancedAbiltyIDArray[i]]);
                }
            }
            RuntimeAbilities.Remove(id);
        }
    }

    public void OpenAbilityChoiceUI()
    {
        foreach (var a in YKDataInfoManager.Instance.LearnedAbilityDataBase.database)
        {
            if (RuntimeAbilities.ContainsKey(a.Key))
            {
                RuntimeAbilities.Remove(a.Key);
            }
        }
        SelectPanel.gameObject.SetActive(true);

        if (RuntimeAbilities.Count == 0)
        {
            return;
        }
        else if (RuntimeAbilities.Count <= 3)
        {
            foreach (var r in RuntimeAbilities)
            {
                GameObject skill = Instantiate(SkillCardObj, SelectPanel);
                skill.GetComponent<SkillCardEneity>().SkillCardInit(r.Value);
            }
        }
        else
        {
            MFAbility[] result = new MFAbility[3];
            List<int> list = new List<int>();
            list.AddRange(RuntimeAbilities.Keys);
            for (int i = 0; i < 3; i++)
            {
                int index = Random.Range(0, list.Count);
                result[i] = RuntimeAbilities[list[index]];
                list.RemoveAt(index);
            }

            for (int i = 0; i < result.Length; i++)
            {
                GameObject skill = Instantiate(SkillCardObj, SelectPanel);
                skill.GetComponent<SkillCardEneity>().SkillCardInit(result[i]); 
            }
            _addSecletcSkillEvent.RaiseEvent(SelectPanel.GetChild(1).GetComponent<SkillCardEneity>().SkillID) ;
        }
    }

    private void StartSelect(int a)
    {
        currentSkillID = a;
        //OnClickSkillCardEvent?.Invoke();
        RefreshSelectSkill();
    }
    private void EndSelect()
    {
        _addSkillEvent.RaiseEvent(currentSkillID);

        SelectPanel.gameObject.SetActive(false);
        for (int i = 1; i < SelectPanel.childCount; i++)
        {
            Destroy(SelectPanel.GetChild(i).gameObject);
        }
    }

    void RefreshSelectSkill()
    {
        for (int i = 1; i < SelectPanel.childCount; i++)
        {
            skillTransform = SelectPanel.GetChild(i);
            if (skillTransform.GetComponent<SkillCardEneity>().SkillID == currentSkillID)
            {
                skillTransform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                skillTransform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

}
