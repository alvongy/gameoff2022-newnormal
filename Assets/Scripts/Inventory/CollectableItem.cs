using UnityEngine;
using DG.Tweening;
using YK.Game.Events;
using System;

public class CollectableItem : MonoBehaviour
{
	[SerializeField] private ItemSO _currentItem = default;
	[SerializeField] private GameObject _itemGO = default;

	private void Start()
	{
		AnimateItem();
	}

	public ItemSO GetItem()
	{
		InitMethod();
		return _currentItem;
	}

	public void SetItem(ItemSO item)
	{
		_currentItem = item;
	}

	public void AnimateItem()
	{
		if (_itemGO != null)
		{
			//_itemGO.transform.DORotate(Vector3.one * 180, 5, RotateMode.Fast).SetLoops(-1, LoopType.Incremental);
		}
	}

	public virtual void InitMethod()
	{

	}

	public virtual void UseItemTrigger()
    {
		DestroyOneself();
	}

	public void DestroyOneself()
	{
		//ObjectPool.Destroy(gameObject);
		Destroy(gameObject);
	}
}
