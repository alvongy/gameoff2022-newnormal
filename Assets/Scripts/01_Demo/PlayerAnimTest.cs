using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

enum WeaponType
{
    Sword,
    Bow,
}
public class PlayerAnimTest : MonoBehaviour
{
    //[SerializeField] GameObject _swordObj;
    [SerializeField] Animator _animation;
    [SerializeField] InputReader inputReader;
    [SerializeField] GameObject Sword;
    [SerializeField] GameObject Bow;
    [SerializeField] List<GameObject> Arows;
    [SerializeField] float speed=10;
    [SerializeField] GameObject fireBallTest;
    private void Awake()
    {
        //_swordObj.SetActive(false);

        inputReader.DebugEventLeft+= OnL;
        inputReader.DebugEventRight += OnR;
        inputReader.DebugEventV += OnV;
        //inputReader.DebugEventX += OnX;
        inputReader.DebugEventZ += OnZ;
        //GetComponent<CharacterController>().enableOverlapRecovery = false;
    }



    private void Start()
    {
        AnimEffectBehaviour[] animEffectBehaviours = _animation.GetBehaviours<AnimEffectBehaviour>();
        foreach (AnimEffectBehaviour aeb in animEffectBehaviours)
        {
            aeb.effectPlay = transform.Find(aeb.effectName).GetComponent<EffectPlay>();
        }
        

    }
    float tick = 0;
    private void Update()
    {
        
        if (tick >= 1)
        {
            tick = 0;
            float angle= Random.Range(0, 360);
            for (int i = 0; i < 20; i++)
            {
                GameObject go = GameObject.Instantiate<GameObject>(fireBallTest);
                go.GetComponent<Projectile>().Init(transform.position + new Vector3(0, 1, 0), transform.rotation, 10f,1);

            }
        }
        tick += Time.deltaTime;
    }
    #region °´¼üÊÂ¼þ
    private void OnZ(InputAction.CallbackContext arg0)
    {
        if (Sword.activeSelf)
        {
            Sword.SetActive(false);
            Bow.SetActive(true);
        }
        else
        {
            Sword.SetActive(true);
            Bow.SetActive(false);
        }
    }

    private void OnV(InputAction.CallbackContext arg0)
    {
        SwitchWeapon(WeaponType.Bow);
        if (arg0.performed)
        {
            HoldBow(true);
        }
        else if(arg0.canceled)
        {
            HoldBow(false);
            Shoot();
        }
    }

    private void OnR(InputAction.CallbackContext arg0)
    {
        SwitchWeapon(WeaponType.Sword);
        if (arg0.performed)
        {
            Attack2();
        }
    }

    private void OnL(InputAction.CallbackContext arg0)
    {
        SwitchWeapon(WeaponType.Sword);
        if (arg0.performed)
        {
            Attack1();
          
        }
    }
    #endregion 
    void HoldBow(bool flag)
    {
        _animation.SetBool("HoldBow", flag);

    }
    void  Attack1()
    {
        _animation.SetTrigger("A1");

    }
    void  Attack2()
    {
        _animation.SetTrigger("A2");

    }
    void Shoot()
    {
        for (int i = 0; i < Arows.Count; i++)
        {
            GameObject go = GameObject.Instantiate<GameObject>(Arows[i]);
            go.SetActive(true);
            go.transform.parent = this.transform;
            go.transform.SetPositionAndRotation(Arows[i].transform.position, Arows[i].transform.rotation);
            go.transform.parent = this.transform.parent;
            go.GetComponent<Rigidbody>().velocity = go.transform.forward * speed;
            go.GetComponent<EffectPlay>().Play(0);
        }
        
        
    }
    void SwitchWeapon(WeaponType weaponType)
    {
        switch (weaponType)
        {
            case WeaponType.Sword:
                Sword.SetActive(true);
                Bow.SetActive(false);
                break;
            case WeaponType.Bow:
                Sword.SetActive(false);
                Bow.SetActive(true);
                break;
            default:
                break;
        }
    }
}
