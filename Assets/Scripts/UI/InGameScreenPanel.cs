using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameScreenPanel : BasePanel
{

	//[Header("Gameplay")]
	//[SerializeField] private GameStateSO _gameStateManager = default;

	//[SerializeField] private InteractionUIEventChannelSO _setInteractionEvent = default;
	//[SerializeField] private VoidEventChannelSO _UIUpdateNeeded = default; //The player's Damageable issues this

	//[Header("UIHealthBar")]
	//[SerializeField] private HealthSO _mainCharacterHealth = default;
	//[SerializeField] private Image _healthbarFill = default;

	//[Header("UIInteraction")]
	//[SerializeField] List<InteractionSO> _listInteractions = default;
	//[SerializeField] Dictionary<InteractionType, Sprite> _dictInteractionSprite = new Dictionary<InteractionType, Sprite>(new EnumInteractionComparer());
	//[SerializeField] GameObject interactionPanelObj;
	//[SerializeField] Image _interactionIcon = default;

	//protected override void OnInit()
	//{
	//	_setInteractionEvent.OnEventRaised += SetInteractionPanel;

	//	InitInteraction();
	//}

	//protected override void OnShow()
	//{
	//	_UIUpdateNeeded.OnEventRaised += UpdateHealthbar;
	//	UpdateHealthbar();
	//}

	//protected override void OnClose()
	//{
	//	_UIUpdateNeeded.OnEventRaised -= UpdateHealthbar;
	//}

	//protected override void OnPanelDestroy()
	//{
	//	_setInteractionEvent.OnEventRaised -= SetInteractionPanel;
	//}

	//#region HealthBar
	//private void UpdateHealthbar()
	//{
	//	_healthbarFill.fillAmount = 1f * _mainCharacterHealth.CurrentHealth / _mainCharacterHealth.MaxHealth;
	//}
	//#endregion

	//#region Interaction
	//private void InitInteraction()
	//{
	//	foreach (InteractionSO interactionSO in _listInteractions)
	//	{
	//		_dictInteractionSprite.Add(interactionSO.InteractionType, interactionSO.InteractionIcon);
	//	}
	//}

	//void SetInteractionPanel(bool isOpen, InteractionType interactionType)
	//{
	//	if (isOpen)
	//	{
	//		FillInteractionPanel(interactionType);
	//	}
	//	interactionPanelObj.gameObject.SetActive(isOpen);
	//}
	//public void FillInteractionPanel(InteractionType interactionType)
	//{
	//	if (_dictInteractionSprite.ContainsKey(interactionType))
	//	{
	//		_interactionIcon.sprite = _dictInteractionSprite[interactionType];
	//	}
	//}
	//#endregion

	//public class EnumInteractionComparer : IEqualityComparer<InteractionType>
	//{
	//	public bool Equals(InteractionType x, InteractionType y)
	//	{
	//		return x == y;
	//	}

	//	public int GetHashCode(InteractionType obj)
	//	{
	//		return (int)obj;
	//	}
	//}
	protected override void OnClose()
	{
	}

	protected override void OnInit()
	{
	}

	protected override void OnPanelDestroy()
	{
	}

	protected override void OnShow()
	{
	}
}
