using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapQuestEneity : SerializedMonoBehaviour, IPointerClickHandler
{
	public int TerrainID;
	public Text MapQuestDescription;
	public Text MapRewardDescription;
	public GameObject _selectFlag;

	[SerializeField] public TerrainSO _currentTerrainSO;
	[SerializeField] public TerrainEventChannelSO OnPickTerrain;

	public void OnPointerClick(PointerEventData eventData)
	{
		OnPickTerrain.RaiseEvent(_currentTerrainSO);
	}
	public void MapQuestInit(TerrainSO terrain)
	{
		TerrainID = terrain.TID;
		_currentTerrainSO = terrain;
		MapQuestDescription.text = terrain.GetQuests()[0].QuestDesp.GetLocalizedString();
		MapRewardDescription.text = terrain.GetQuests()[0].Rewards[0].RewardName.GetLocalizedString();
		_currentTerrainSO = terrain;
	}
}
