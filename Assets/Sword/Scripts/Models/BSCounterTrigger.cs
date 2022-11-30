using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BSCounterTrigger
{
	[SerializeField] float counter;
	public float Counter
	{
		get => counter;
	}

	UnityAction<BSAbilityHolder, float> OnChange = delegate { };
	UnityAction<BSAbilityHolder> OnEnd = delegate { };


	public BSCounterTrigger(float _counter, UnityAction<BSAbilityHolder, float> onChange, UnityAction<BSAbilityHolder> onEnd)
	{
		counter = _counter;
		if (onChange != null)
		{
			OnChange += onChange;
		}
		if (onEnd != null)
		{
			OnEnd += onEnd;
		}
	}

	public void CounterChange(BSAbilityHolder holder, float value)
	{
		if (counter >= 0)
		{
			counter += value;
			if (counter < 0)
			{
				OnChange.Invoke(holder, 0);
				OnEnd.Invoke(holder);
			}
			else
			{
				OnChange.Invoke(holder, counter);
			}
		}
	}
}
