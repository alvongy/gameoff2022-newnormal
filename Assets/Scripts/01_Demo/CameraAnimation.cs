using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraAnimation : MonoBehaviour
{

    [SerializeField] Transform focusTsf;//焦点对象
    [SerializeField] Transform cameraTsf;
    [SerializeField] float duration = 1f;//缓动时长
    [SerializeField] bool startFocus=true;//跟踪焦点
 
    private Transform _tsf;
    private Sequence _sequence;
    private Vector3 _offsetVector;//初始偏移位置

    public bool StartFocus { get => startFocus; set => startFocus = value; }

    private void Awake()
    {
        _tsf = transform;
        _sequence = DOTween.Sequence();
    }
    private void Start()
    {
        _offsetVector = _tsf.position-focusTsf.position;

    }
    private void LateUpdate()
    {
        if (startFocus)
        {
            FocusPos(focusTsf.position, duration);
        }    
    }
    private void FocusPos(Vector3 focusPos,float duration)
    {
        _sequence.Append(_tsf.DOMove(focusPos + _offsetVector, duration));

    }
    //拉镜头
    public void PullCamera(float pullDistance,float duration=1)
    {
        _sequence.Kill();
        _sequence = DOTween.Sequence();
        _sequence.Append( cameraTsf.DOLocalMove(-pullDistance*cameraTsf.forward, duration));
    }
    //重置镜头
    public void ResetCamera(float duration=1)
    {
        _sequence.Kill();
        _sequence = DOTween.Sequence();
        cameraTsf.DOLocalMove(Vector3.zero, duration);
    }
    //聚焦物体
    public void Focus(Transform transform)
    {
        focusTsf = transform;
    }
    //镜头震动
    public void ShakeCamera( float duration=0.5f,float strength=0.2f)
    {
        cameraTsf.DOShakePosition(duration, strength,15);       
    }

}
