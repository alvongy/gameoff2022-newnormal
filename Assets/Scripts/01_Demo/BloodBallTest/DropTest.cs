using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DropTest : MonoBehaviour
{
    [SerializeField] GameObject bloodProjectObj;
    [SerializeField] float Speed=5;
    [Range(0,1)]
    [SerializeField] float DropProbability=0.3f;
    private Transform _tsf;
  
    private void Awake()
    {
        _tsf = transform;
    }
    public void Drop()
    {
        if (Random.Range(0f, 1f) < DropProbability)
        {
            GameObject pObj = GameObject.Instantiate(bloodProjectObj, _tsf.position + Vector3.up, _tsf.rotation);
            BloodProjectTest bloodProject = pObj.GetComponent<BloodProjectTest>();
            bloodProject.dropOrPick = BloodProjectTest.DropOrPick.Drop;
            Vector3 dir = Vector3.up;

            dir = Quaternion.AngleAxis(Random.Range(0, 60), Vector3.forward) * dir;
            dir = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up) * dir;

            //pObj.transform.Rotate(Vector3.up, Random.Range(0, 360));
            bloodProject.rigidbody.velocity = dir * Speed;
        }

        


    }
}
