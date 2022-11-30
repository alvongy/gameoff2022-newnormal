using UnityEngine;
using UnityEngine.Events;


[CreateAssetMenu(menuName = "Events/Int Array Event Channel")]
public class IntArrayEventChannelSO : DescriptionBaseSO
{
	public UnityAction<int[]> OnEventRaised;

	public void RaiseEvent(int[] value)
	{
		if (OnEventRaised != null)
			OnEventRaised.Invoke(value);
	}
}
