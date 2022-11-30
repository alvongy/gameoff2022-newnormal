using System.Collections.Generic;
using UnityEngine;

public abstract class DragableItem : MonoBehaviour
{
    protected DragTrigger dragTrigger;

    public abstract DragTarget SearchTarget(List<DragTarget> resultsList);

    public abstract bool CheckTarget(DragTarget dragTarget);

    public abstract void OnDragCompelete(Vector2 pos);

    public abstract void Dragging(bool canset, Vector2 pos);

    public abstract void DragStart(DragTrigger _dragTrigger);

    public abstract void DragEnd();
}