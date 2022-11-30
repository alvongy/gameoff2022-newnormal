using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using YK.Game.Events;
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class PickItem : MonoBehaviour
{
	[SerializeField] protected TransformAnchor _playerAnchor;
	[SerializeField] protected float _speed = 1f;
    [SerializeField] float YOffset = 0.5f;
    protected Transform _tsf;
    protected bool _isPicking = false;
    protected bool IsDead = false;
    //private void Awake()
    //{
    //    _tsf = transform;
    //    Vector3 pos = _tsf.position;
    //    pos.y = pos.y + YOffset;
    //    _tsf.position = pos;
    //    _isPicking = false;
    //}
    private void OnEnable()
    {

        Init();
    }

    public void Init()
    {
        _tsf = transform;
        _isPicking = false;
        IsDead = false;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isPicking)
        {
            return;
        }else
        if (other.CompareTag("Player"))
        {
            OnPick();
        }
    }
    void OnPick()
    {
        _isPicking = true;
        Picking();



    }

    protected void DestroySelf()
    {
        IsDead = true;
        //Debug.Log("ddddddd");
        //Destroy(this.gameObject);//TODO use pool
        ObjectPool.Destroy(this.gameObject);

        

    }


    protected virtual void Picked()
    {

    }
    protected virtual void Picking()
    {

    }

   

}
