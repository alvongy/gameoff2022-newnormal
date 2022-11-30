using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapDataControl : SerializedMonoBehaviour
{
    public Text _terrainQuestName;
    public Text _terrainQuestDescription;
    public Text _terrainAdditionDes;

    [SerializeField] TerrainEventChannelSO OnPickTerrain;
    List<YKQuestSO> quests = new List<YKQuestSO>();

    void OnEnable()
    {
        OnPickTerrain.OnEventRaised += PickTerrain;
    }
    private void OnDisable()
    {
        OnPickTerrain.OnEventRaised -= PickTerrain;

    }

    void PickTerrain(TerrainSO so)
    {
        quests.Clear();
        quests.AddRange(so.GetQuests());
        StartDetection(so);
    }

    void StartDetection(TerrainSO so)
    {
        if (quests.Count<0) { return; }
        _terrainQuestName.text = quests[0].QuestName.GetLocalizedString();
        _terrainQuestDescription.text = quests[0].QuestDesp.GetLocalizedString();
        //_terrainAdditionDes.text = so.GetMapAdditions()[0].addition.ArrangementDes.GetLocalizedString();
        if (so.GetMapAddition().Count<=0) { return; }
        _terrainAdditionDes.text = so.GetMapAddition()[0].ArrangementDes.GetLocalizedString();

        //_terrainQuestName.text = so.GetQuests()[0].QuestName.GetLocalizedString();
        //_terrainQuestDescription.text = so.GetQuests()[0].QuestDesp.GetLocalizedString();
        //_terrainAdditionDes.text = so.GetMapAdditions()[0].addition.ArrangementDes.GetLocalizedString();
    }
}
