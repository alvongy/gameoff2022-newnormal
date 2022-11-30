using System.Collections;
using UnityEngine;
using ArtSetting;
public class BloodBall : PickItem
{

	[SerializeField] private IntEventChannelSO _playerGetHPChannel;
	[SerializeField] private EffectManage _objInStatic;
	[SerializeField] private EffectManage _objInMoving;
    [SerializeField] private int _count = 5;
    private bool _isPicked = false;
    

    private void OnEnable()
    {
        Init();
        _objInMoving.Stop();
        _objInStatic.Play();
        _isPicked = false;
    }
    protected override void Picking()
    {
        _objInStatic.Stop();
        _objInMoving.Play();
        StartCoroutine(DirectFollow());
    }
    IEnumerator DirectFollow()
    {

        WaitForFixedUpdate wait = new WaitForFixedUpdate();
        Vector3 targetDir;
        while (true)
        {
            targetDir = _playerAnchor.Value.position + Vector3.up - _tsf.position;
            if (!_isPicked && targetDir.sqrMagnitude < 0.05f)
            {
                Picked();
                Invoke("DestroySelf", 0.5f);
                _isPicked = true;
            }
            _tsf.Translate(_speed * Time.fixedDeltaTime * (targetDir).normalized);
            yield return wait;
        }

    }
    protected override void Picked()
    {
        _objInMoving.Stop();
        _playerGetHPChannel.RaiseEvent(_count);
    }
}
