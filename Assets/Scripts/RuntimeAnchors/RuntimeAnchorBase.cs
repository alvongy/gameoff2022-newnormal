using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

public class RuntimeAnchorBase<T> : DescriptionBaseSO where T : UnityEngine.Object
{
	public UnityAction OnAnchorProvided;

	[Header("Debug")]
	[ReadOnlyInspector] public bool isSet = false; // Any script can check if the transform is null before using it, by just checking this bool

	[ReadOnlyInspector] [SerializeField] private T _value;

	public T Value
	{
		get => _value;
	}

	public void Provide(T value)
	{
		if (value == null)
		{
			Debug.LogError("A null value was provided to the " + this.name + " runtime anchor.");
			return;
		}

		_value = value;
		isSet = true;

		OnAnchorProvided?.Invoke();
	}
	public void Unset()
	{
		_value = null;
		isSet = false;
	}
	private void OnDisable()
	{
		Unset();
	}
}
