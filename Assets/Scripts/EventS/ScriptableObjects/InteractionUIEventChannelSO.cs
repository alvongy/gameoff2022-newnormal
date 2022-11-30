using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Toogle Interaction UI Event Channel")]
public class InteractionUIEventChannelSO : DescriptionBaseSO
{
	public UnityAction<bool, InteractionType> OnEventRaised;

	public void RaiseEvent(bool state, InteractionType interactionType)
	{
		OnEventRaised?.Invoke(state, interactionType);
	}
}
