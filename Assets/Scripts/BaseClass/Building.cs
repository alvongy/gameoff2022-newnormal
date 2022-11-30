using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public int Duration = 0;
    private void Start()
    {
        if (Duration > 0)
        {
            Invoke("DestroyParent", Duration);
        }

    }
    protected void DestroyParent()
    {
        Destroy(transform.parent.gameObject);
    }
    protected void DestroySelf()
    {
        Destroy(this.gameObject);
    }
}
