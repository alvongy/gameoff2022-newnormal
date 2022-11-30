using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AchievementEntity : MonoBehaviour,IPointerDownHandler,IPointerUpHandler,IPointerClickHandler
{
    public enum AchievementType
    {
        Pray,
        Param
    }
    [SerializeField]
    private PlayerAttributesSO _achievementSO;
    public ItemSO _currentItem;
    public Text _energyAmount;
    public MapPlayerBasicData _mapPlayerBasic;
    public AchievementType achievementType;

    private float _expendEnergy;
    private float _expendSpeed=10f;
    private Slider _energySlider;
    private bool isDown;
    private bool isCharged;


    private void Start()
    {
        _energySlider = transform.GetChild(0).GetComponent<Slider>();
        RefreshData();
    }
    private void Update()
    {
        if (isDown && YKDataInfoManager.Instance._energySO.CurrentValue > 0 && _achievementSO.CurrentValue < _achievementSO.MaxValue)
        {
            _expendEnergy += _expendSpeed * Time.deltaTime;
            if (_expendEnergy >= 1)
            {
                YKDataInfoManager.Instance._energySO.Decrease(1);
                _achievementSO.Increace(1);
                RefreshData();
                _expendEnergy = 0;
                _expendSpeed += 1;
            }
        }
    }
    void RefreshData()
    {
        _energySlider.value = _achievementSO.CurrentValue;
        if (_achievementSO.CurrentValue <= _achievementSO.MaxValue) 
        {
            _energyAmount.text = _achievementSO.CurrentValue + "/" + _achievementSO.MaxValue;    
        }
        else
        {
            _energyAmount.text = _achievementSO.MaxValue + "/" + _achievementSO.MaxValue;
            _energySlider.fillRect.GetComponent<Image>().color = Color.green;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDown = true;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        isDown = false;
        _expendSpeed = 10f;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //@@@ ��ʾ�óɾ͵ľ�����Ϣ

        if (!isCharged && _achievementSO.CurrentValue == _achievementSO.MaxValue)
        {
            //��ȡ����
            isCharged = true;
            _achievementSO.Increace(1, false);

            switch (achievementType)
            {
                case AchievementType.Pray:
                    CreatePray();
                    break;
                case AchievementType.Param:
                    ParamAddition();
                    break;
                default:

                    break;
            }
        }
    }

    public void CreatePray()
    {
        GameObject prayGo = Instantiate(_mapPlayerBasic.Pray, _mapPlayerBasic.PrayControl);
        prayGo.GetComponent<PrayStoneEneity>().SetItem(_currentItem);
        _mapPlayerBasic._currentInventory.Add(_currentItem);
    }

    public void ParamAddition()
    {
        if (!YKDataInfoManager.Instance.inBulkAbilityList.Exists(t => t == _currentItem.DataPack["MFAbility"]))
        {
            YKDataInfoManager.Instance.inBulkAbilityList.Add((int)_currentItem.DataPack["MFAbility"]);
        };
        _energySlider.fillRect.GetComponent<Image>().color = Color.green;

        //���Է���һ��button�����������ʯ,���Կ��Ǽ�һ���������ȷ��
        OpenAchievementDes();
        Debug.Log("���ɢװ���ܣ�"+ YKDataInfoManager.Instance.AbilityDataBase.database[(int)_currentItem.DataPack["MFAbility"]].ablilityName.GetLocalizedString());
    }

    private void OpenAchievementDes()
    {
        MapSelectUIManager.Instance.PopUpPanel.InitializeAchievementData(_currentItem);
    }
}
