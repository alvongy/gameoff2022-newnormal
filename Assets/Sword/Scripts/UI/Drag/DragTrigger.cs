using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DragTrigger : MonoBehaviour
{
    public abstract DragableItem DragStart();

    public abstract void DragEnd(bool success);
}
