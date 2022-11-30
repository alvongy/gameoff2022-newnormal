using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/DamageDisplayChannelSO")]
public class DamageDisplayChannelSO : DescriptionBaseSO
{
	public event UnityAction<Vector3, int> OnEventRaised;

	public void RaiseEvent(Vector3 position, int damage)
	{
		if (OnEventRaised != null)
			OnEventRaised.Invoke(position, damage);
	}
}
