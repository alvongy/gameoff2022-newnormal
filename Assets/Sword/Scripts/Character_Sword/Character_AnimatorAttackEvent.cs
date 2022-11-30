using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
public class Character_AnimatorAttackEvent : MonoBehaviour
{
    [SerializeField] public CharacterController_Sword characterController_Sword;

    public void Attack()
    {
        characterController_Sword.Attack();
    }

    public void CanMoveFalse()
    {

        characterController_Sword. movementInput = Vector2.zero;
       // characterController_Sword.CanMove = false;
    }
    public void CanMoveTrue() { characterController_Sword.CanMove = true; }
    public void FootStep() 
    {
        GetComponent<MMFeedbacks>().PlayFeedbacks();
    }
}
