using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YK.Game.Events;

public class CannonTower : Building
{
    [SerializeField] GameObject projectile;
    [SerializeField] private int demage = 1;
    [SerializeField] private float speed = 6f;
    [SerializeField] private float spac = 0.5f;
    [SerializeField] private float range=5f;
    Transform _tsf;
    Transform lookedTsf;
    //Queue<Transform> enemyList;
    private float _duration;
    private void Awake()
    {
        _tsf = transform;
        _duration = spac;
        //enemyList = new Queue<Transform>();
    }
    //private void OnTriggerEnter(Collider other)
    //{


    //        if (other.CompareTag("Enemy"))
    //        {
    //            //lookedTsf = other.transform;
    //            enemyList.Enqueue(other.transform);
    //        }
        
    //}
    bool AcquireTarget()
    {
        Collider[] targets = Physics.OverlapSphere(_tsf.localPosition, range,LayerMask.GetMask("Enemy"));
        if (targets.Length > 0)
        {
            lookedTsf = targets[0].transform;
            return true;
        }
        lookedTsf = null;
        return false;
    }
    private void FixedUpdate()
    {

        if (AcquireTarget())
        {
            _tsf.LookAt(lookedTsf.position+Vector3.up*5);
            _duration += Time.fixedDeltaTime;
            if (_duration > spac)
            {
                GameObject go = Instantiate<GameObject>(projectile, _tsf.position, Quaternion.identity);
                SubProjectile pj = go.GetComponent<SubProjectile>();
                pj.Init(_tsf.position, _tsf.rotation, speed, demage);
                _duration = 0;
            }
        }

        //if (lookedTsf == null)
        //{

        //    if (enemyList.Count > 0)
        //    {
        //        lookedTsf = enemyList.Dequeue();
        //    }
        //}
        //else
        //{

        //    _tsf.LookAt(lookedTsf,lookedTsf.forward);
        //    _duration += Time.fixedDeltaTime;
        //    if (_duration > spac)
        //    {
        //        GameObject go = ObjectPool.Instantiate<GameObject>(projectile, _tsf.position, Quaternion.identity);
        //        Projectile pj = go.GetComponent<Projectile>();
        //        pj.Init(_tsf.position, _tsf.rotation, speed, demage);
        //        _duration = 0;
        //    }
          

        //}

    }
}
