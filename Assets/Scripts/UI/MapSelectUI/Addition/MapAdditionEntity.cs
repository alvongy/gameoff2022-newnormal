using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static ArrangementRulesEventChannelSO;

public class MapAdditionEntity : SerializedMonoBehaviour, IPointerClickHandler
{
    [InfoBox("具体地图加成规则")]
    [SerializeField] public ArrangementRulesEventChannelSO currentOnMapAddition;
    [InfoBox("卡牌颜色父对象")]
    public Transform CardParent;
    [InfoBox("具体地图加成效果")]
    public Text _AdditionDescription;
    public Text _AdditionLevel;
    public GameObject OnMapAdditionFlag;

    public Dictionary<CharacterDataType, CharacterData> EntityDataPack = new Dictionary<CharacterDataType, CharacterData>();

    public bool additionFlag;
    //public int cardLevel;
    private int cardIndex;
    private MapBuffControl mapBuffControl;

    public void Init(ArrangementRulesEventChannelSO a, int num)
    {
        //currentOnMapAddition = a;
        //EntityDataPack = a.DataPack;
        //cardLevel = num;
        //
        //_AdditionDescription.text = currentOnMapAddition.Ability.ablilityName.GetLocalizedString();//排列加成换成技能描述
        //_AdditionLevel.text = cardLevel.ToString();
        //Debug.Log("111排列等级。cardLevel：" + cardLevel);
    }

    public void AdditionInitialize(ArrangementRulesEventChannelSO rule, MapBuffControl mapBuff)
    {
        cardIndex = 0;
        currentOnMapAddition = rule;
        mapBuffControl = mapBuff;
        EntityDataPack = rule.DataPack;
        currentOnMapAddition.OnEventRaised += OnTriggerMapBuff;

        if (rule.arrangeType == ArrangeType.Alone)
        {
            if (rule.cardColorNums[0].colorType == ColorType.Red)
            {
                CardParent.GetChild(4).GetComponent<Image>().color = new Color(120f / 255f, 37f / 255f, 61f / 255f, 1f);
            }
            else if (rule.cardColorNums[0].colorType == ColorType.Green)
            {
                CardParent.GetChild(4).GetComponent<Image>().color = new Color(45f / 255f, 70f / 255f, 59f / 255f, 1f);
            }
            else if (rule.cardColorNums[0].colorType == ColorType.Blue)
            {
                CardParent.GetChild(4).GetComponent<Image>().color = new Color(42f / 255f, 44f / 255f, 109f / 255f, 1f);
            }
        }
        else if (rule.arrangeType == ArrangeType.Line)
        {
            cardIndex = 3;
            foreach (var c in rule.cardColorNums)
            {
                for (int i = 0; i < c.nums; i++)
                {
                    if (c.colorType == ColorType.Red)
                    {
                        CardParent.GetChild(cardIndex).GetComponent<Image>().color = new Color(120f / 255f, 37f / 255f, 61f / 255f, 1f);
                        cardIndex++;
                    }
                    else if (c.colorType == ColorType.Green)
                    {
                        CardParent.GetChild(cardIndex).GetComponent<Image>().color = new Color(45f / 255f, 70f / 255f, 59f / 255f, 1f);
                        cardIndex++;
                    }
                    else if (c.colorType == ColorType.Blue)
                    {
                        CardParent.GetChild(cardIndex).GetComponent<Image>().color = new Color(42f / 255f, 44f / 255f, 109f / 255f, 1f);
                        cardIndex++;
                    }
                }
            }
        }
        else if (rule.arrangeType == ArrangeType.Squares)
        {
            foreach (var c in rule.cardColorNums)
            {
                for (int i = 0; i < c.nums; i++)
                {
                    if (c.colorType == ColorType.Red)
                    {
                        CardParent.GetChild(0).GetComponent<Image>().color = new Color(120f / 255f, 37f / 255f, 61f / 255f, 1f);
                        CardParent.GetChild(1).GetComponent<Image>().color = new Color(120f / 255f, 37f / 255f, 61f / 255f, 1f);
                        CardParent.GetChild(3).GetComponent<Image>().color = new Color(120f / 255f, 37f / 255f, 61f / 255f, 1f);
                        CardParent.GetChild(4).GetComponent<Image>().color = new Color(120f / 255f, 37f / 255f, 61f / 255f, 1f);
                    }
                    else if (c.colorType == ColorType.Green)
                    {
                        CardParent.GetChild(0).GetComponent<Image>().color = new Color(45f / 255f, 70f / 255f, 59f / 255f, 1f);
                        CardParent.GetChild(1).GetComponent<Image>().color = new Color(45f / 255f, 70f / 255f, 59f / 255f, 1f);
                        CardParent.GetChild(3).GetComponent<Image>().color = new Color(45f / 255f, 70f / 255f, 59f / 255f, 1f);
                        CardParent.GetChild(4).GetComponent<Image>().color = new Color(45f / 255f, 70f / 255f, 59f / 255f, 1f);
                    }
                    else if (c.colorType == ColorType.Blue)
                    {
                        CardParent.GetChild(0).GetComponent<Image>().color = new Color(42f / 255f, 44f / 255f, 109f / 255f, 1f);
                        CardParent.GetChild(1).GetComponent<Image>().color = new Color(42f / 255f, 44f / 255f, 109f / 255f, 1f);
                        CardParent.GetChild(3).GetComponent<Image>().color = new Color(42f / 255f, 44f / 255f, 109f / 255f, 1f);
                        CardParent.GetChild(4).GetComponent<Image>().color = new Color(42f / 255f, 44f / 255f, 109f / 255f, 1f);
                    }
                }
            }
        }
        else if (rule.arrangeType == ArrangeType.Oblique)
        {
            cardIndex = 0;
            foreach (var c in rule.cardColorNums)
            {
                for (int i = 0; i < c.nums; i++)
                {
                    if (c.colorType == ColorType.Red)
                    {
                        CardParent.GetChild(cardIndex).GetComponent<Image>().color = new Color(120f / 255f, 37f / 255f, 61f / 255f, 1f);
                        cardIndex += 4;
                    }
                    else if (c.colorType == ColorType.Green)
                    {
                        CardParent.GetChild(cardIndex).GetComponent<Image>().color = new Color(45f / 255f, 70f / 255f, 59f / 255f, 1f);
                        cardIndex += 4;
                    }
                    else if (c.colorType == ColorType.Blue)
                    {
                        CardParent.GetChild(cardIndex).GetComponent<Image>().color = new Color(42f / 255f, 44f / 255f, 109f / 255f, 1f);
                        cardIndex += 4;
                    }
                }
            }
        }
        else if (rule.arrangeType == ArrangeType.Homochromatic)
        {
            foreach (var c in rule.cardColorNums)
            {
                if (c.colorType == ColorType.Red)
                {
                    for (int i = 0; i < CardParent.childCount; i++)
                    {
                        CardParent.GetChild(i).GetComponent<Image>().color = new Color(120f / 255f, 37f / 255f, 61f / 255f, 1f);
                    }
                }
                else if (c.colorType == ColorType.Green)
                {
                    for (int i = 0; i < CardParent.childCount; i++)
                    {
                        CardParent.GetChild(i).GetComponent<Image>().color = new Color(45f / 255f, 70f / 255f, 59f / 255f, 1f);
                    }
                }
                else if (c.colorType == ColorType.Blue)
                {
                    for (int i = 0; i < CardParent.childCount; i++)
                    {
                        CardParent.GetChild(i).GetComponent<Image>().color = new Color(42f / 255f, 44f / 255f, 109f / 255f, 1f);
                    }
                }
            }
        }

        _AdditionLevel.text = currentOnMapAddition.RuleLevel.ToString();
        _AdditionDescription.text = currentOnMapAddition.Ability.ablilityName.GetLocalizedString();//排列加成换成技能描述
    }

    /*
    public void AdditionInit(ArrangementRulesEventChannelSO rule,int n)
    {
        if (n % 2 == 0)
        {
            transform.GetChild(0).GetComponent<Image>().color = new Color(192f/255f, 192f / 255f, 192f / 255f, 255f / 255f);
        }
        else
        {
            transform.GetChild(0).GetComponent<Image>().color = new Color(152f / 255f, 152f / 255f, 152f / 255f, 255f / 255f);
        }

        currentOnMapAddition = rule;
        EntityDataPack = rule.DataPack;
        currentOnMapAddition.OnEventRaised += OnTriggerMapBuff;

        //只显示具体的加成效果时使用

        _RedCardNum.text = (rule.CardColorNum.ContainsKey(ColorType.Red)? rule.CardColorNum[ColorType.Red].ToString():0.ToString());
        _GreenCardNum.text = (rule.CardColorNum.ContainsKey(ColorType.Green)? rule.CardColorNum[ColorType.Green].ToString():0.ToString());
        _BlueCardNum.text = (rule.CardColorNum.ContainsKey(ColorType.Blue)? rule.CardColorNum[ColorType.Blue].ToString():0.ToString());

       _AdditionDescription.text = null;
       foreach (var d in rule.DataPack)
       {
           //_AdditionName.text = rule.ArrangementDes.GetLocalizedString();
           _AdditionDescription.text +=  d.Key+" + "+ d.Value.GetData();
       }
    }
    */

    private void OnDestroy()
    {
        currentOnMapAddition.OnEventRaised -= OnTriggerMapBuff;
    }

    public void OnTriggerMapBuff(bool flag)
    {
        if (OnMapAdditionFlag == null) { return; }
        //OnMapAdditionFlag.SetActive(flag);
        //additionFlag = flag;
        if (flag)
        {
            mapBuffControl.OnArrangementRulesTemp.Add(currentOnMapAddition);
        }
        else
        {
            if (mapBuffControl.OnArrangementRulesTemp.Exists(t => t == currentOnMapAddition))
            {
                mapBuffControl.OnArrangementRulesTemp.Remove(currentOnMapAddition);
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        MapSelectUIManager.Instance.PopUpPanel.InitializeCurrentSkill(currentOnMapAddition.Ability);
    }
}
