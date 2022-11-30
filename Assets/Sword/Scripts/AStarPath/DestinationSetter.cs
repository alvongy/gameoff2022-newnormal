using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestinationSetter : VersionedMonoBehaviour
{
	[HideInInspector] public Vector3? Target => target;
	[HideInInspector] public float speed;
	Vector3? target;
	IAstarAI ai;

	[SerializeField] Transform debugTarget;

    void OnEnable()
	{
		target = null;
		ai = GetComponent<IAstarAI>();
		if (ai != null)
		{
			ai.onSearchPath += OnSearchPath;
		}
		speed = 0f;
	}

	void OnDisable()
	{
		if (ai != null)
		{
			ai.onSearchPath -= OnSearchPath;
		}
	}

    private void Update()
    {
        if (debugTarget)
        {
			SetDestination(debugTarget.position);
		}
	}

    public void SetDestination(Vector3 destination)
    {
		target = destination;
		ai.destination = destination;
		ai.maxSpeed = speed;
	}

	public void CanMove(bool flag)
    {
		ai.canMove = flag;
	}

	void OnSearchPath()
    {
        if (target != null)
        {
			ai.destination = (Vector3)target;
		}
	}
}
