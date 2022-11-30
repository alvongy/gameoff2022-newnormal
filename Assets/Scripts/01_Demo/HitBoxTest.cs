using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxTest : MonoBehaviour
{
    [SerializeField] int Damage = 10;
    [SerializeField] GameObject EffectPrefab;
    [SerializeField] bool ShakeCamera = false;
    //[SerializeField] float Duration = 0.5f;
    //private float _activeTime=0;
    //private void FixedUpdate()
    //{
    //    _activeTime += Time.fixedDeltaTime;
    //    if (_activeTime > Duration)
    //    {
    //        this.gameObject.SetActive(false);
    //        _activeTime = 0;
    //    }
    //}
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyTest>().OnHit(Damage);
            if (ShakeCamera)
            {
                CameraAnimation cameraAnimation = Camera.main.transform.parent.GetComponent<CameraAnimation>();
                if (cameraAnimation != null)
                {
                    cameraAnimation.ShakeCamera();
                }
            }
            //ÊÜ»÷ÌØÐ§       
            if (EffectPrefab != null)
            {
                GameObject effectObj = GameObject.Instantiate(EffectPrefab);
                effectObj.transform.rotation = transform.rotation;
                effectObj.transform.position = other.gameObject.transform.position+new Vector3(0,1f,0);
                //effectObj.transform.parent = transform;
                effectObj.SetActive(true);
            }
           
        }
       


    }
    private void OnGUI()
    {
        
    }
}
