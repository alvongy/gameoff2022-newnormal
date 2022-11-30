using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tarb : Building
{
    public int Demage = 1;
    public int Count = 10;
    //public GameObject pro
    private void OnTriggerEnter(Collider other)
    {

        if (Count > 0)
        {
            if (other.CompareTag("Enemy"))
            {
                
                other.GetComponent<Damageable>().ReceiveAnAttack(Demage);
                Count--;
            }
        }
        else
        {
            DestroySelf();
        }
       
    }
}
