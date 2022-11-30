using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapSkillCardEneity : SerializedMonoBehaviour,IPointerClickHandler
{
	public int TerrainID;
	public Text TerrainName;
	public Text TerrainDescription;
	public Image TerrainImage;

	[SerializeField] public TerrainSO _currentTerrainSO;
	[SerializeField] TerrainEventChannelSO OnSelectTerrainUI;
	[SerializeField] TerrainEventChannelSO OnPickTerrain;

    public void OnPointerClick(PointerEventData eventData)
    {
		OnSelectTerrainUI.RaiseEvent(_currentTerrainSO);
		OnPickTerrain.RaiseEvent(_currentTerrainSO);
	}
	public void TerrainUIInit(TerrainSO terrain)
	{
		TerrainID = terrain.TID;
		//TerrainName.text = terrain.TerrainName.GetLocalizedString();
		//TerrainDescription.text = terrain.TerrainDescription.GetLocalizedString();
		TerrainImage.sprite = terrain.TerrainSprite;
		_currentTerrainSO = terrain;
	}
}
