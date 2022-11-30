using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MoveShow : VersionedMonoBehaviour
{
	public UnityAction<float> onSpeedChange = delegate { };

	//public GameObject endOfPathEffect;

	bool isAtDestination;

	IAstarAI ai;
	Transform tr;

	protected override void Awake()
	{
		base.Awake();
		ai = GetComponent<IAstarAI>();
		tr = GetComponent<Transform>();
	}

	protected Vector3 lastTarget;

	void OnTargetReached()
	{
		//if (endOfPathEffect != null && Vector3.Distance(tr.position, lastTarget) > 1)
		//{
		//	GameObject.Instantiate(endOfPathEffect, tr.position, tr.rotation);寻路目标点标记
		//	lastTarget = tr.position;
		//}
		if (Vector3.Distance(tr.position, lastTarget) > 1)
		{
			lastTarget = tr.position;
		}
	}

	protected void Update()
	{
		if (ai.reachedEndOfPath)
		{
			if (!isAtDestination) OnTargetReached();
			isAtDestination = true;
		}
		else isAtDestination = false;

		Vector3 relVelocity = tr.InverseTransformDirection(ai.velocity);
		relVelocity.y = 0;
		onSpeedChange.Invoke(relVelocity.magnitude);
	}
}
