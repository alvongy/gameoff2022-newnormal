using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_BossA : EnemyCtrl
{
    public override int HP { get => data.hp; set => data.hp = value; }

    [SerializeField] BossAController controller;

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
            //MonsterManager.Instance.SetLevelComplete(); È¡Ê¤¹ý¹Ø
            EntranceManager.Instance.Victory();
        }
    }
}
