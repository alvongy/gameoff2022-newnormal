using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PrayStoneEneity : MonoBehaviour,IPointerClickHandler
{
	[SerializeField] private ItemSO _currentItem = default;
	[SerializeField] private ItemEventChannelSO OnClickItemChannelSO;

	public ItemSO GetItem()
	{
		return _currentItem;
	}
    public void SetItem(ItemSO item)
	{
		_currentItem = item;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		OnClickItemChannelSO.RaiseEvent(_currentItem);
		MapSelectUIManager.Instance.PlayerBasicData.OnClickPrayStoneEvent?.Invoke();
		MapSelectUIManager.Instance.PopUpPanel.InitializePrayData(_currentItem);

	}

}
