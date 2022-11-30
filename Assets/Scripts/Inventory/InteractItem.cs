using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
public class InteractItem : MonoBehaviour
{
	[SerializeField] protected ItemSO _currentItem = default;
	[SerializeField] private GameObject _itemGO = default;
	public UnityEvent OnInteract;
	public UnityEvent OnExecute;
	private void Start()
	{
		AnimateItem();
	}

	public ItemSO GetItem()
	{
		return _currentItem;
	}
	public virtual void Interaction() 
	{
		OnInteract.Invoke();
	}
	public virtual void Execute()
	{
		OnExecute.Invoke();
	}

	public void SetItem(ItemSO item)
	{
		_currentItem = item;
	}

	public virtual void AnimateItem()
	{
		if (_itemGO != null)
		{
			//_itemGO.transform.DORotate(Vector3.one * 180, 5, RotateMode.Fast).SetLoops(-1, LoopType.Incremental);
		}
	}
}
