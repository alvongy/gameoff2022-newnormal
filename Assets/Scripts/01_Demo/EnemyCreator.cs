using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyCreator : MonoBehaviour
{
    [SerializeField] float range=3;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] float spacTime=1;
     private Transform _tsf;
    int priority = 99;
    private void Awake()
    {
        _tsf = transform;
    }

    float tick = 0;
    void Update()
    {
        tick += Time.deltaTime;
        if(tick> spacTime)
        {
           tick = 0;
           //Transform gTsf=  GameObject.Instantiate(enemyPrefab).transform;
           //gTsf.position = _tsf.position + new Vector3(Random.Range(-range, range), 0, Random.Range(-range, range));
           // NavMeshAgent  nag=gTsf.GetComponent<NavMeshAgent>();
            //nag.avoidancePriority = priority;
            //if (priority > 0)
            //{
            //    priority--;
            //}
            //else
            //{
            //    priority = 99;
            //}
           

            
        }
    }
}
