using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class ItemPickUpDisplay : MonoBehaviour
{

	[SerializeField] private ItemEventChannelSO _OnPickUp;
	[SerializeField] private Text _text;
	public float DisplayDuration;
	private void Awake()
	{
		_OnPickUp.OnEventRaised += ItemPicked;
	}
	private void Start()
	{
		_text.text = "";
	}
	private void OnDestroy()
	{
		_OnPickUp.OnEventRaised -= ItemPicked;
	}
	private void ItemPicked(ItemSO Item)
	{
		_text.text = Item.Name.GetLocalizedString() + " + 1";
		StartCoroutine(OnDisplay());
	}
	IEnumerator OnDisplay() 
	
	{
		yield return new WaitForSeconds(DisplayDuration);
		_text.text = "";
	}
}
