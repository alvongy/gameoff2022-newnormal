using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractionType
{
	NONE = 0,
	USE,
	OPERATE,
	TALK
}
public class Interaction 
{
	public InteractionType type;
	public GameObject interactableObject;

	public Interaction(InteractionType t, GameObject obj)
	{
		type = t;
		interactableObject = obj;
	}
}
