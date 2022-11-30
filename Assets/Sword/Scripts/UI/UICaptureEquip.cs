using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICaptureEquip : MonoBehaviour
{
    [SerializeField] GameObject EquipItem_Prefab;
    [SerializeField] Transform UIEquipItemPool;

    [SerializeField] Transform UIRightSideEquipCapture;

    [SerializeField] UIEquip_DragTarget[] dragTargetsArray;
    Dictionary<EquipPart, List<UIEquip_DragTarget>> dragTargetDic;

    [SerializeField] Image background_Image;
    [SerializeField] Image arrow_Image;
    [SerializeField] GameObject signNew;
    public UIEquipInfoCard equippedInfoCard;
    public UIEquipInfoCard notEquippedInfoCard;

    [SerializeField] GameObject UIEquipMessagePrefab;
    [SerializeField] Transform UIEquipMessagePool;
    [SerializeField] Transform UIEquipMessageGroup;
    [SerializeField] Button UIEquipMessageGroup_Button;
    [SerializeField] float DisplayDuration;
    private bool isDisplayActive;
    private Dictionary<EquipPart, List<UIMessageText>> possessEquipDic;

    private UIDragController dragController;

    private bool visible;
    private bool tabFlag;

    private void Awake()
    {
        dragController = GetComponent<UIDragController>();
        dragTargetDic = new Dictionary<EquipPart, List<UIEquip_DragTarget>>();
        possessEquipDic = new Dictionary<EquipPart, List<UIMessageText>>();
        UIEquipMessageGroup_Button.onClick.AddListener(ShowCurrentEquip);
    }

    public void Init(CharacterSO so)
    {
        visible = false;
        tabFlag = false;
        Clear();
        InitPossessEquipMessage(so);
        InputManager.Instance.TabDownEvent += OnTabDown;
        InputManager.Instance.TabUpEvent += OnTabUp;
        int i = 0;
        foreach(var kv in so.EquipPartDic)
        {
            for (int j = 0; j < kv.Value; j++)
            {
                dragTargetsArray[i].Init(kv.Key);
                if (!dragTargetDic.ContainsKey(kv.Key))
                {
                    dragTargetDic.Add(kv.Key, new List<UIEquip_DragTarget>());
                }
                dragTargetDic[kv.Key].Add(dragTargetsArray[i]);
                i++;
            }
        }
        dragController.OnDragEnd.AddListener(Neaten);
        equipBuffer = new List<Equip>(8);
    }

    public void OnGameOver()
    {
        Hide();
        InputManager.Instance.TabDownEvent -= OnTabDown;
        InputManager.Instance.TabUpEvent -= OnTabUp;
    }

    public void Clear()
    {
        while (UIRightSideEquipCapture.childCount > 0)
        {
            PutBack(UIRightSideEquipCapture.GetChild(UIRightSideEquipCapture.childCount - 1).gameObject);
        }
        while (UIEquipMessageGroup.childCount > 0)
        {
            PutBackMessage(UIEquipMessageGroup.GetChild(UIEquipMessageGroup.childCount - 1).gameObject);
        }
        possessEquipDic.Clear();
        dragTargetDic.Clear();
        for (int i = 0; i < dragTargetsArray.Length; i++)
        {
            dragTargetsArray[i].Clear();
        }
    } 

    List<Equip> equipBuffer;

    public void Neaten()
    {
        if (equipBuffer.Count > 5)
        {
            equipBuffer.RemoveRange(0, equipBuffer.Count - 5);
        }
        for (int i = 0; i < equipBuffer.Count; i++)
        {
            AddEquip(equipBuffer[i]);
        }
        equipBuffer.Clear();
    }

    public void AddEquip(Equip equip)
    {
        UIEquipItem item;
        if (dragController.IsDragging)
        {
            equipBuffer.Add(equip);
            return;
        }
        if (UIRightSideEquipCapture.childCount >= 10)
        {
            PutBack(UIRightSideEquipCapture.GetChild(0).gameObject);
        }
        if (UIEquipItemPool.childCount > 0)
        {
            Transform t = UIEquipItemPool.GetChild(UIEquipItemPool.childCount - 1);
            item = t.GetComponent<UIEquipItem>();
            if (item)
            {
                item.Init(equip);
                t.parent = UIRightSideEquipCapture;
                item.gameObject.SetActive(true);
            }
        }
        else
        {
            GameObject go = Instantiate(EquipItem_Prefab, UIRightSideEquipCapture);
            item = go.GetComponent<UIEquipItem>();
            item.Init(equip);
        }
    }

    public void PutBack(GameObject obj)
    {
        UIEquipItem item = obj.GetComponent<UIEquipItem>();
        if (item)
        {
            obj.SetActive(false);
            obj.transform.parent = UIEquipItemPool;
        }
        else
        {
            Destroy(obj);
        }
    }

    public void OnDragStartEquipPart(Equip equip)
    {
        if (dragTargetDic.ContainsKey(equip.data.PartCode)) 
        {
            foreach(var item in dragTargetDic[equip.data.PartCode]) 
            {
                item.gameObject.SetActive(true);
            }
            if (dragTargetDic[equip.data.PartCode].Count == 1 && dragTargetDic[equip.data.PartCode][0].Equipped)
            {
                dragTargetDic[equip.data.PartCode][0].CompareEquip();
            }
        }
        notEquippedInfoCard.Show(equip);
        if (!tabFlag)
        {
            Show();
        }
    }

    public void OnDragEndEquipPart(EquipPart part)
    {
        if (dragTargetDic.ContainsKey(part))
        {
            foreach (var item in dragTargetDic[part])
            {
                item.gameObject.SetActive(false);
            }
        }
        equippedInfoCard.Hide();
        notEquippedInfoCard.Hide();
        ShowArrow(false);
        if (!tabFlag)
        {
            Hide();
        }
    }

    public void OnTabDown()
    {
        if (!visible)
        {
            Show();
            tabFlag = true;
        }
    }

    public void OnTabUp()
    {
        if (visible&&tabFlag)
        {
            Hide();
            dragController.Interrupt();
            tabFlag = false;
        }
    }

    public void Show()
    {
        UIEquipMessageGroup.gameObject.SetActive(false);
        background_Image.enabled = true;
        UI_Manager.Instance.characterInfoCard.Show();
        signNew.SetActive(true);
        Time.timeScale = 0.01f;
        visible = true;
    }

    public void Hide()
    {
        equippedInfoCard.Hide();
        notEquippedInfoCard.Hide();
        UIEquipMessageGroup.gameObject.SetActive(true);
        background_Image.enabled = false;
        UI_Manager.Instance.characterInfoCard.Hide();
        signNew.SetActive(false);
        Time.timeScale = 1f;
        foreach (var item in dragTargetsArray)
        {
            if (item.gameObject.activeSelf)
            {
                item.gameObject.SetActive(false);
            }
        }
        visible = false;
    }

    public void ShowArrow(bool show)
    {
        arrow_Image.enabled = show;
    }

    public void PutBackMessage(GameObject obj)
    {
        UIMessageText item = obj.GetComponent<UIMessageText>();
        if (item)
        {
            obj.SetActive(false);
            obj.transform.parent = UIEquipMessagePool;
        }
        else
        {
            Destroy(obj);
        }
    }

    public void ShowCaptureMessage(EquipPart part)
    {
        for (int i = 0; i < possessEquipDic[part].Count; i++) 
        {
            Equip equip = CharacterManager.Instance.characterData.equipDictionary[part][i];
            possessEquipDic[part][i].Show(string.Format("{0}:<color=#{1}>Lv.{2}-{3}-{4}</color>"
                , CaptureManager.Instance.LocalizedMetaName_EquipPartDic[part].GetLocalizedString()
                , ColorUtility.ToHtmlStringRGB(CaptureManager.Instance.QualityColorDic[equip.equipQuality])
                , equip.level
                , equip.data.Name.GetLocalizedString()
                , equip.score));
        }
    }

    public void InitPossessEquipMessage(CharacterSO characterSO)
    {
        GameObject obj;
        foreach (var kv in characterSO.EquipPartDic)
        {
            if (UIEquipMessagePool.childCount > 0)
            {
                obj = UIEquipMessagePool.GetChild(UIEquipMessagePool.childCount - 1).gameObject;
            }
            else
            {
                obj = Instantiate(UIEquipMessagePrefab, UIEquipMessagePool);
            }
            UIMessageText msg = obj.GetComponent<UIMessageText>();
            msg.Show(CaptureManager.Instance.LocalizedMetaName_EquipPartDic[kv.Key].GetLocalizedString()+":");
            obj.transform.parent = UIEquipMessageGroup;
            msg.gameObject.SetActive(true);
            if (possessEquipDic.ContainsKey(kv.Key))
            {
                if (possessEquipDic[kv.Key] == null)
                {
                    possessEquipDic[kv.Key] = new List<UIMessageText>();
                }
            }
            else
            {
                possessEquipDic.Add(kv.Key, new List<UIMessageText>());
            }
            possessEquipDic[kv.Key].Add(msg);
        }
    }

    private void ShowCurrentEquip()
    {
        if (isDisplayActive)
        {
            StopCoroutine(OnDisplay());
            UIEquipMessageGroup.gameObject.SetActive(false);
        }
        else
        {
            UIEquipMessageGroup.gameObject.SetActive(true);
            StartCoroutine(OnDisplay());
        }
    }
    IEnumerator OnDisplay()
    {
        isDisplayActive = true;
        yield return new WaitForSeconds(DisplayDuration);
        UIEquipMessageGroup.gameObject.SetActive(false);
        isDisplayActive = false;
    }
}
