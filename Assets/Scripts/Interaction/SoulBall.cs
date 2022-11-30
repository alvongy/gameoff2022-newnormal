using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArtSetting;
public class SoulBall : PickItem
{
    [SerializeField] EffectManage effectManage;
    //[SerializeField] private Rigidbody _rigidbody;
    //[SerializeField] float RotDelta = 8f;
    //[SerializeField] float EmitAngle = 60;
    //[SerializeField] bool EmitToward = false;
    //[SerializeField] AnimationCurve curve;
    //[SerializeField] float _velocity = 3f;
    [SerializeField] float _acceleration = 1;
    [SerializeField] private IntEventChannelSO _playerGetExpChannel;
    [SerializeField] private int _count=1;
   [SerializeField] private bool _isPicked=false;
    private void OnEnable()
    {
        Init();
        _isPicked = false;
        effectManage.Play();
    }
    protected override void  Picked()
    {
        effectManage.Stop();
        _playerGetExpChannel.RaiseEvent(_count);
    }
    protected override void Picking()
    {      
        StartCoroutine(PongFollow());
    }


    ///// <summary>
    ///// Å×ÎïÏßÒÆ¶¯
    ///// </summary>
    ///// <returns></returns>
    //IEnumerator ParabolaFollow()
    //{

    //    Vector3 targetDir;
    //    targetDir = _playerAnchor.Value.position - _tsf.position;
    //    //Quaternion qua1 = Quaternion.FromToRotation(_tsf.forward, targetDir );
    //    //_tsf.rotation = qua1;
    //    //_tsf.rotation.SetFromToRotation(_tsf.forward, targetDir* (EmitToward?1:-1));
    //    //Quaternion.FromToRotation(_tsf.forward, -targetDir) ;
    //    _tsf.LookAt(_playerAnchor.Value.position);
    //    //float angle = Random.Range(0, EmitAngle);
    //    _tsf.Rotate(_tsf.right, EmitAngle);
    //    //_tsf.Rotate(targetDir, Random.Range(-90, 90));

    //    WaitForFixedUpdate wait = new WaitForFixedUpdate();
    //    //targetDir = _playerAnchor.Value.position - _tsf.position;
    //    //_rigidbody.velocity = (targetDir).normalized * _speed;
    //    //_rigidbody.AddForce(Vector3.up * 20, ForceMode.Force);
    //    while (true)
    //    {
    //        targetDir = _playerAnchor.Value.position - _tsf.position;
    //        if (!_isPicked && targetDir.sqrMagnitude < 0.05f)
    //        {
    //            Picked();
    //            Invoke("DestroySelf", 0.5f);
    //            _isPicked = true;
    //        }
    //        _tsf.Translate(_tsf.forward * _speed * Time.fixedDeltaTime);

    //        Quaternion qua2 = Quaternion.FromToRotation(_tsf.forward, targetDir);
    //        _tsf.rotation = Quaternion.Lerp(transform.rotation, qua2, Time.fixedDeltaTime * RotDelta);

    //        //float angle = Vector3.Angle(_tsf.forward, targetDir);
    //        //float needTime = angle / (RotDelta);
    //        //if (needTime < 0.01f)
    //        //{
    //        //    _tsf.forward = targetDir;
    //        //}
    //        //else
    //        //{
    //        //    _tsf.forward = Vector3.Lerp(_tsf.forward, targetDir, Time.fixedDeltaTime * needTime).normalized;
    //        //}
    //        //Vector3 rot=Vector3.RotateTowards(_tsf.forward, targetDir, 0.1f,1);
    //        //_tsf.
    //        //_rigidbody.velocity = (targetDir).normalized * _speed;
    //        yield return wait;

    //    }

    //}

    IEnumerator PongFollow()
    {
        Vector3 targetDir;
        WaitForFixedUpdate wait = new WaitForFixedUpdate();
        float currentSpeed = _speed;
        while (!IsDead)
        {
            targetDir = _playerAnchor.Value.position+Vector3.up - _tsf.position;
            if (!_isPicked && targetDir.sqrMagnitude < 0.05f)
            {
                Picked();
                //Debug.Log("aaaaa");
                //this.gameObject.SetActive(false);
                Invoke("DestroySelf", 0.5f);
                //DestroySelf();
                _isPicked = true;
            }
            float deltaTime = Time.fixedDeltaTime;
            _tsf.Translate(targetDir.normalized * currentSpeed * deltaTime);
            currentSpeed = currentSpeed + _acceleration * deltaTime;
            yield return wait;
        }
      }
 

}


