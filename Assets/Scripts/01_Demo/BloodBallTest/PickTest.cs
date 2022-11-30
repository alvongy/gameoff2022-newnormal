using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickTest : MonoBehaviour
{
    public GameObject BloodProject;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PickItem"))
        {
            //Debug.Log("sdadsad");
            GameObject pobj=  GameObject.Instantiate(BloodProject,other.transform.position+Vector3.up,Quaternion.identity);
            Destroy(other.gameObject);
            BloodProjectTest bloodProjectTest= pobj.GetComponent<BloodProjectTest>();
            bloodProjectTest.dropOrPick = BloodProjectTest.DropOrPick.Pick;
            bloodProjectTest.targetTsf = transform;

        }
    }
}
