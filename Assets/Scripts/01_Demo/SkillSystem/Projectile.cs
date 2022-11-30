using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArtSetting;
using YK.Game.Events;


public class Projectile : MonoBehaviour
{
    [SerializeField] GameObject hitEffect;
    [SerializeField] float duration = 2f;
    [SerializeField] float speed = 1f;
    [SerializeField] int demage = 1;
    [SerializeField] int passCount = 1;
    [SerializeField] EffectManage effectManage;

    bool isDead = false;
    private Transform _tsf;
    private GameObject hitEffectObj;
    void Awake()
    {
        _tsf = transform;
    }
    void Start()
    {
        this.GetComponent<Rigidbody>().velocity = _tsf.forward * speed;
        Invoke("DestorySelf", duration);
    }
    public void Init(Vector3 pos, Quaternion rot, float speed, int demage)
    {
        _tsf.SetPositionAndRotation(pos, rot);
        this.speed = speed;
        this.demage = demage;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (passCount > 0)
        {
            if (other.CompareTag("Enemy"))
            {
                hitEffectObj = Instantiate(hitEffect, _tsf.position, _tsf.rotation);
                other.GetComponent<Damageable>().ReceiveAnAttack(demage);
                StartCoroutine(DestorySelf());
                passCount--;
            }
        }


    }

    IEnumerator DestorySelf()
    {
        if (!isDead)
        {
            isDead = true;
            effectManage.Stop();
            yield return new WaitForSeconds((float)effectManage.remainTime);
            Destroy(this.gameObject);
            Destroy(hitEffectObj);
        }

    }



}
