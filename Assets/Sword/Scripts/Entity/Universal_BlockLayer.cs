using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Universal_BlockLayer : BlockLayer
{
    [SerializeField] SpriteRenderer renderer;

    private void Start()
    {
        ResetOrderInLayer();
    }

    public override void ResetOrderInLayer()
    {
        renderer.sortingOrder = -(int)(renderer.transform.position.z * 10);
    }
}
