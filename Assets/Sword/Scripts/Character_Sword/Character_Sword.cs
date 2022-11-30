using MoreMountains.Feedbacks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character_Sword : Entity
{
    [SerializeField] MMFeedbacks feedbacks;
    private bool reviveActive;

    public override EntityData Data => CharacterManager.Instance.characterData;

    public override int HP
    {
        get => CharacterManager.Instance.characterData.hp;
        set
        {
            CharacterManager.Instance.characterData.hp = value;
        }
    }

    public void Init()
    {

    }

    private void Update()
    {
        Data.OnUpdate();
    }

    public override void Damaged(int damage)
    {
        if (GetComponent<CharacterController_Sword>().rollTrigger) { return; }
        GetComponent<CharacterController_Sword>().movementInput = Vector2.zero;
        int hp = Data.hp;
        Data.hp = Mathf.Max(0, Data.hp - damage);
        int dmgV = Mathf.Max(hp - Data.hp, 0);
        OnDamaged.Invoke(dmgV);
        feedbacks.PlayFeedbacks(transform.position, dmgV);
        Debug.Log("Character_Damaged");
    }

    public override void Death()
    {
        if (EvolutionManager.Instance.survivalDifficultyUnlockDic[4] && !reviveActive)
        {
            Debug.Log("∏¥ªÓ“ª¥Œ");
            reviveActive = true;
            HP = 100;
            PlayerRollTrigger();
        }
        else
        {
            OnDeath.Invoke();
            CharacterManager.Instance.GameOver();
        }
    }

    private void PlayerRollTrigger()
    {
        MMTimeScaleEvent.Trigger(MMTimeScaleMethods.For, 0.2f, 5f, true, 1f, false);
        //   StartCoroutine(OnDisplay());
    }
    IEnumerator OnDisplay()
    {
        yield return new WaitForSeconds(2f);
        MMTimeScaleEvent.Trigger(MMTimeScaleMethods.Reset, 1f, 0.5f, true, 0.5f, true);
    }
}
