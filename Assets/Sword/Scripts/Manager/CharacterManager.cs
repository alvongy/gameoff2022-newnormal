using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : SerializedMonoSingleton<CharacterManager>
{
    [Title("翻滚冷却")]
    [SerializeField] float rollCoolDownDuration;
    private float rollCoolDownTimer;
    [Title("翻滚累积次数")]
    [SerializeField] int rollTotalAmount;
    public int RollAmount => rollAmount;
    private int rollAmount;

    [Title("角色开局出现地点")]
    [SerializeField] Transform spawnPoint;
    
    [HideInInspector]
    public EntityData_Character characterData;
    [HideInInspector]
    public Character_Sword character;

    public void Init(CharacterSO characterSO)
    {
        if (characterData == null)
        {
            characterData = EntityData_Character.GetCharacterData(characterSO);
        }
        else
        {
            characterData.Reset(characterSO);
        }
        GameObject go = Instantiate(characterSO.Prefab, spawnPoint.position, Quaternion.identity);
        go.transform.parent = transform;
        character = go.GetComponent<Character_Sword>();
        character.Init();
        rollAmount = rollTotalAmount;
        rollCoolDownTimer = rollCoolDownDuration;
    }

    private void Update()
    {
        if (EntranceManager.Instance.InGame)
        {
            UI_Manager.Instance.roolTimer_Text.text = string.Format("{0:0}", rollAmount);
            if (rollAmount < rollTotalAmount)
            {
                UI_Manager.Instance.rollMask_Image.enabled = true;
                UI_Manager.Instance.rollMask_Image.fillAmount = rollCoolDownTimer / rollCoolDownDuration;
                //UI_Manager.Instance.roolTimer_Text.enabled = true;
                if (rollCoolDownTimer >= 0)
                {
                    rollCoolDownTimer -= Time.deltaTime;
                    if (rollCoolDownTimer < 0)
                    {
                        rollAmount++;
                        rollCoolDownTimer = rollCoolDownDuration;
                    }
                }
            }
            else
            {
                UI_Manager.Instance.rollMask_Image.enabled = false;
            }
        }
    }

    public void OnRoll()
    {
        rollAmount--;
    }

    public void GameOver()
    {
        Destroy(character.gameObject);
        character = null;
        EntranceManager.Instance.GameOver();
    }

    public void StopMove()
    {
        character.GetComponent<CharacterController_Sword>().CanMove = false;
    }
    public void ResumeMove()
    {
        character.GetComponent<CharacterController_Sword>().CanMove = true;
    }
}
