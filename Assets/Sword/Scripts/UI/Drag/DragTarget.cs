using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DragTarget : MonoBehaviour
{ 
    public abstract void OnArrival(DragableItem dragableItem);

    public abstract void OnDragEnter();

    public abstract void OnDragExit();

    public abstract void OnReady();

    public abstract void OnMiss();
}