using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEquipItem : MonoBehaviour
{
    public bool Alive => alive;
    private bool alive = false;
    [SerializeField] CanvasGroup group;

    [SerializeField] Image Border_Image;
    [SerializeField] Image Icon_Image;
    [SerializeField] Text Name_Text;
    [SerializeField] Text Part_Text;
    [SerializeField] Text Level_Text;
    [SerializeField] Text Score_Text;

    public Equip equip;

    public void Init(Equip _equip)
    {
        equip = _equip;
        Border_Image.color = CaptureManager.Instance.QualityColorDic[equip.equipQuality];
        Icon_Image.sprite = equip.data.Icon;
        Name_Text.text = equip.data.Name.GetLocalizedString();
        Name_Text.color = CaptureManager.Instance.QualityColorDic[equip.equipQuality];
        Part_Text.text = equip.data.Part.GetLocalizedString();
        Level_Text.text = string.Format("Lv. {0}", equip.level);
        Level_Text.color = CaptureManager.Instance.QualityColorDic[equip.equipQuality];
        Score_Text.text = string.Format("{0}: {1}", CaptureManager.Instance.LocalizedMetaName_Score.GetLocalizedString(), equip.score);
        Show();
    }

    public void Hide()
    {
        group.alpha = 0;
        alive = false;
    }

    void Show()
    {
        alive = true;
        group.alpha = 1;
    }
}
