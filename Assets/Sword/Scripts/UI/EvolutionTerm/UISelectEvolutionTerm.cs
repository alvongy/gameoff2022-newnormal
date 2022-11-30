using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISelectEvolutionTerm : MonoBehaviour
{
    [SerializeField] private Transform _selectEvolutionPanel;
    [SerializeField] private Transform _selectEvolutionParent;
    [SerializeField] private Transform _learendEvolutionTermParent;
    [SerializeField] private Transform _UIEvolutionItemPool;
    [SerializeField] private Transform _UILearendEvolutionItemPool;
    [SerializeField] private Button _selectEvolutionBtn;
    [SerializeField] private Button RefreshEvolutionBtn;

    [SerializeField] private GameObject _evolutionTermCardObj;
    [SerializeField] private GameObject _learebdEvolutionTermCardObj;

    //public List<EvolutionTermSO> _begignTermList = new List<EvolutionTermSO>();
    //public List<EvolutionTermSO> _neutralTermList = new List<EvolutionTermSO>();
    //public List<EvolutionTermSO> _malignancyTermList = new List<EvolutionTermSO>();
    //private List<EvolutionTermSO> _tempTermList = new List<EvolutionTermSO>();

    public void Init()
    {
        //foreach (var a in EvolutionManager.Instance.evolutionTermDatabase.partDic)
        //{
        //    foreach (var e in a.Value)
        //    {
        //        if (a.Key == EvolutionTypeEnum.BENIGN)
        //        {
        //            _begignTermList.Add(e);
        //        }
        //        else if (a.Key == EvolutionTypeEnum.NEUTRAL)
        //        {
        //            _neutralTermList.Add(e);
        //        }
        //        else if (a.Key == EvolutionTypeEnum.MALIGNANCY)
        //        {
        //            _malignancyTermList.Add(e);
        //        }
        //    }
        //}
    }

    public void OnGameOver()
    {
        RefreshLearendEvolutionTerm();
    }

    private void Awake()
    {
        RegisteredEvents();
    }

    private void RegisteredEvents()
    {
        EvolutionManager.Instance.OnLearendEvolutionChange += RefreshLearendEvolutionTerm;
        _selectEvolutionBtn.onClick.AddListener(SelectedEvolutionTerm);
        RefreshEvolutionBtn.onClick.AddListener(RefreshEvolutionTerm);
    }

    private void SelectedEvolutionTerm()
    {
        while (_selectEvolutionParent.childCount > 0)
        {
            PutBack(_selectEvolutionParent.GetChild(_selectEvolutionParent.childCount - 1).gameObject);
        }

        EvolutionManager.Instance.EndSelectcEvolutionTerm();
        _selectEvolutionPanel.gameObject.SetActive(false);
        BackTheListPool();
    }
    private void RefreshEvolutionTerm()
    {
        while (_selectEvolutionParent.childCount > 0)
        {
            PutBack(_selectEvolutionParent.GetChild(_selectEvolutionParent.childCount - 1).gameObject);
        }
        CaptureEvolutionTerm();
        //RefreshEvolutionBtn.enabled = false;
        RefreshEvolutionBtn.gameObject.SetActive(false);
    }

    /// <summary>
    /// 打开进化的界面。按照品性获得概率 -> 按照品质获得概率 -> 某个品性的品质里随机一张
    /// </summary>
    public void CaptureEvolutionTerm()
    {
        CaptureEvolutionCard();

        _selectEvolutionPanel.gameObject.SetActive(true);
        _selectEvolutionParent.GetChild(0).GetComponent<EvolutionTermCardEntity>().OnPointerDown();
        if (EvolutionManager.Instance.survivalDifficultyUnlockDic[1])
        {
            RefreshEvolutionBtn.enabled = true;
            RefreshEvolutionBtn.gameObject.SetActive(true);
        }
        else
        {
            RefreshEvolutionBtn.gameObject.SetActive(false);
        }
    }
    public void ShowEvolutionTermChoiceUI()
    {
        /*
        foreach (var a in EvolutionManager.Instance.evolutionTermDatabase_Learend.database)
        {
            if (_begignTermList.Exists(t => t == a.Value))
            {
                _begignTermList.Remove(a.Value);
            }
            else if (_neutralTermList.Exists(t => t == a.Value))
            {
                _neutralTermList.Remove(a.Value);
            }
            else if (_malignancyTermList.Exists(t => t == a.Value))
            {
                _malignancyTermList.Remove(a.Value);
            }
        }
        _selectEvolutionPanel.gameObject.SetActive(true);

        int evolutionRandom = Random.Range(0, 3);
        //int evolutionRandom = 1;
        switch (evolutionRandom)
        {
            case 0:
                ExtractionEvolutionTerm(_begignTermList);
                break;
            case 1:
                ExtractionEvolutionTerm(_neutralTermList);
                break;
            case 2:
                ExtractionEvolutionTerm(_malignancyTermList);
                break;
            default:
                Debug.Log("未找到进化类型词条。");
                break;
        }
        */
    }

    private void CaptureEvolutionCard()
    {
        Dictionary<EvolutionTypeEnum, Dictionary<EquipQuality, List<EvolutionTermSO>>> t = new Dictionary<EvolutionTypeEnum, Dictionary<EquipQuality, List<EvolutionTermSO>>>()
        {
            { EvolutionTypeEnum.BENIGN, EvolutionManager.Instance.benignPoolDic} ,
            { EvolutionTypeEnum.NEUTRAL, EvolutionManager.Instance.neutralPoolDic} ,
            { EvolutionTypeEnum.MALIGNANCY, EvolutionManager.Instance.malignancyPoolDic} ,

        };
        var sss = EvolutionManager.Instance.GetRandomEvolutionType();
        List<EvolutionTermSO> evolutionsTemp11111 = new List<EvolutionTermSO>();
        List<EvolutionTermSO> evolutionsTemp = new List<EvolutionTermSO>();
        for (int i = 0; i < 3; i++)
        {
            evolutionsTemp.Clear();
            foreach (var l in t[sss]
                [EvolutionManager.Instance.GetEvolutionQuality(GameLevelManager.Instance.GetCurrentLevel, t[sss])])
            {
                if (!evolutionsTemp.Contains(l)&& !evolutionsTemp11111.Contains(l))
                {
                    evolutionsTemp.Add(l);
                    Debug.Log("测试：" + l);
                }
            }
            evolutionsTemp11111.Add(ExtractionEvolutionTerm(evolutionsTemp));
        }


        //switch ()
        //{
        //    case EvolutionTypeEnum.BENIGN:
        //        List<EvolutionTermSO> evolutionsTemp = new List<EvolutionTermSO>();
        //        foreach (var l in EvolutionManager.Instance.benignPoolDic[
        //            EvolutionManager.Instance.GetEvolutionQuality(GameLevelManager.Instance.GetCurrentLevel, EvolutionManager.Instance.benignPoolDic)
        //            ])
        //        {
        //            if (!evolutionsTemp.Contains(l))
        //            {
        //                evolutionsTemp.Add(l);
        //            }
        //        }

        //        for (int i = 0; i < 3; i++)
        //        {
        //            ExtractionEvolutionTerm(evolutionsTemp);
        //        }
        //        break;
        //    case EvolutionTypeEnum.NEUTRAL:
        //        for (int i = 0; i < 3; i++)
        //        {
        //            ExtractionEvolutionTerm(evolutionsTemp}
        //        break;
        //    case EvolutionTypeEnum.MALIGNANCY:
        //        for (int i = 0; i < 3; i++)
        //        {
        //            ExtractionEvolutionTerm(EvolutionManager.Instance.malignancyPoolDic
        //   [EvolutionManager.Instance.GetEvolutionQuality(GameLevelManager.Instance.GetCurrentLevel, EvolutionManager.Instance.malignancyPoolDic)]);
        //        }
        //        break;
        //    default:
        //        Debug.Log("未找到进化类型词条。");
        //        break;
        //}
    }

    private EvolutionTermSO ExtractionEvolutionTerm(List<EvolutionTermSO> tempList)
    {
        int index = Random.Range(0, tempList.Count);
        EvolutionTermSO result = tempList[index];
        tempList.RemoveAt(index);
        EvolutionTermCardEntity item;
        if (_UIEvolutionItemPool.childCount > 0)
        {
            Transform t = _UIEvolutionItemPool.GetChild(_UIEvolutionItemPool.childCount - 1);
            item = t.GetComponent<EvolutionTermCardEntity>();
            if (item)
            {
                item.EvolutionTermCardInit(result);
                t.parent = _selectEvolutionParent;
                item.gameObject.SetActive(true);
            }
            else
            {
                Destroy(t.gameObject);
            }
        }
        else
        {
            GameObject go = Instantiate(_evolutionTermCardObj, _selectEvolutionParent);
            item = go.GetComponent<EvolutionTermCardEntity>();
            item.EvolutionTermCardInit(result);
        }
        return result;
    }

    /// <summary>
    /// 将抽进化牌时，未选中的两张牌按照品性和品质放回对应的池子
    /// </summary>
    private void BackTheListPool()
    {
        EvolutionTermSO tempEvolutionTerm = EvolutionManager.Instance.currentClickEvolutionTerm;

        if (tempEvolutionTerm.evolutionType == EvolutionTypeEnum.BENIGN)
        {
            EvolutionManager.Instance.benignPoolDic[tempEvolutionTerm.evolutionQuality].Remove(tempEvolutionTerm);
        }
        else if (tempEvolutionTerm.evolutionType == EvolutionTypeEnum.NEUTRAL)
        {
            EvolutionManager.Instance.neutralPoolDic[tempEvolutionTerm.evolutionQuality].Remove(tempEvolutionTerm);
        }
        else
        {
            EvolutionManager.Instance.malignancyPoolDic[tempEvolutionTerm.evolutionQuality].Remove(tempEvolutionTerm);
        }
    }

    private void RefreshLearendEvolutionTerm()
    {
        for (int i = _learendEvolutionTermParent.childCount - 1; i >= 0; i--)
        {
            if (_learendEvolutionTermParent.GetChild(i).childCount > 0)
            {
                _learendEvolutionTermParent.GetChild(i).GetComponent<Image>().color = new Color(0 / 255f, 0 / 255f, 0 / 255f, 100 / 255f);
                PutBack(_learendEvolutionTermParent.GetChild(i).GetChild(0).gameObject);
            }
        }

        foreach (var e in EvolutionManager.Instance.evolutionTermDatabase_Learend.database)
        {
            for (int i = 0; i < _learendEvolutionTermParent.childCount; i++)
            {
                if (_learendEvolutionTermParent.GetChild(i).childCount <= 0)
                {
                    EvolutionTermCardEntity item;
                    if (_UILearendEvolutionItemPool.childCount > 0)
                    {
                        Transform t = _UILearendEvolutionItemPool.GetChild(_UILearendEvolutionItemPool.childCount - 1);
                        item = t.GetComponent<EvolutionTermCardEntity>();
                        if (item)
                        {
                            t.parent = _learendEvolutionTermParent.GetChild(i);
                            item.EvolutionTermCardInit(e.Value);
                            item.gameObject.SetActive(true);
                        }
                        else
                        {
                            Destroy(t.gameObject);
                        }
                    }
                    else
                    {
                        GameObject go = Instantiate(_learebdEvolutionTermCardObj, _learendEvolutionTermParent.GetChild(i));
                        go.GetComponent<EvolutionTermCardEntity>().EvolutionTermCardInit(e.Value);
                    }
                    //GameObject go = Instantiate(_learebdEvolutionTermCardObj, _learendEvolutionTermParent.GetChild(i));
                    //go.GetComponent<EvolutionTermCardEntity>().EvolutionTermCardInit(e.Value);
                    break;
                }
            }
        }
    }

    public void PutBack(GameObject obj)
    {
        EvolutionTermCardEntity item = obj.GetComponent<EvolutionTermCardEntity>();
        if (item)
        {
            obj.SetActive(false);
            if (item.IsLearend)
            {
                obj.transform.parent = _UILearendEvolutionItemPool;
            }
            else
            {
                obj.transform.parent = _UIEvolutionItemPool;
            }
        }
        else
        {
            Destroy(obj);
        }
    }
}
