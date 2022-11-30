using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif
// Created with collaboration from:
// https://forum.unity.com/threads/inventory-system.980646/

[CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item")]
public class ItemSO : SerializedScriptableObject 
{

	[DrawWithUnity]
	[Tooltip("The name of the item")]
	[SerializeField] private LocalizedString _name = default;

	[Tooltip("A preview image for the item")]
	[SerializeField]
	private Sprite _previewImage = default;

	[Tooltip("A description of the item")]
	[SerializeField]
	[DrawWithUnity]
	private LocalizedString _description = default;

	[Tooltip("A description of the item")]
	[SerializeField]
	public Dictionary<string, float> DataPack = new Dictionary<string, float>()
	{
		
	};
	[Tooltip("The id of item")]
	[SerializeField]
	private int _itemID = default;

	[Tooltip("The type of item")]
	[SerializeField]
	private ItemTypeSO _itemType = default;

	[Tooltip("A prefab reference for the model of the item")]
	[SerializeField]
	private GameObject _prefab = default;


	public LocalizedString Name => _name;
	public Sprite PreviewImage => _previewImage;
	public LocalizedString Description => _description;
	//public int HealthResorationValue => _healthResorationValue;
	public int ItemID => _itemID;
	public ItemTypeSO ItemType => _itemType;
	public GameObject Prefab => _prefab;
	public virtual List<ItemStack> IngredientsList { get; }
	public virtual ItemSO ResultingDish { get; }

	public virtual bool IsLocalized { get; }
	public virtual LocalizedSprite LocalizePreviewImage { get; }
	[SerializeField, HideInInspector] private string _guid;
	public string Guid => _guid;

#if UNITY_EDITOR
	void OnValidate()
	{
		var path = AssetDatabase.GetAssetPath(this);
		_guid = AssetDatabase.AssetPathToGUID(path);
	}
#endif

}
