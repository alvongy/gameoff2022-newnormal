using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Bat : EnemyCtrl
{
    public override int HP { get => data.hp; set => data.hp = value; }

    [SerializeField] BatController controller;

    public override void Init()
    {
        base.Init();
        controller.Init();
    }

    private void Update()
    {
        Data.OnUpdate();
    }

    public override void Death()
    {
        if (alive)
        {
            base.Death();
            MonsterManager.Instance.EnemyDestroy(this);
        }
    }

    public void BatAttack()
    {
        transform.DOShakePosition(0.5f, new Vector3(3, 0, 0), 30);
    }
}
