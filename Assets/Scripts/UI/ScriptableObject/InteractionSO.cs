using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "InteractionSO", menuName = "UI/Interaction")]
public class InteractionSO : ScriptableObject
{
    [SerializeField] LocalizedString _interactionName = default;
    [SerializeField] Sprite _interactionIcon = default;
    [SerializeField] InteractionType _interactionType = default;

    public Sprite InteractionIcon => _interactionIcon;
    public LocalizedString InteractionName => _interactionName;
    public InteractionType InteractionType => _interactionType;
}
