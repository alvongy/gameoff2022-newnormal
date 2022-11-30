using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraAnimation : MonoBehaviour
{

    [SerializeField] Transform focusTsf;//�������
    [SerializeField] Transform cameraTsf;
    [SerializeField] float duration = 1f;//����ʱ��
    [SerializeField] bool startFocus=true;//���ٽ���
 
    private Transform _tsf;
    private Sequence _sequence;
    private Vector3 _offsetVector;//��ʼƫ��λ��

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
    //����ͷ
    public void PullCamera(float pullDistance,float duration=1)
    {
        _sequence.Kill();
        _sequence = DOTween.Sequence();
        _sequence.Append( cameraTsf.DOLocalMove(-pullDistance*cameraTsf.forward, duration));
    }
    //���þ�ͷ
    public void ResetCamera(float duration=1)
    {
        _sequence.Kill();
        _sequence = DOTween.Sequence();
        cameraTsf.DOLocalMove(Vector3.zero, duration);
    }
    //�۽�����
    public void Focus(Transform transform)
    {
        focusTsf = transform;
    }
    //��ͷ��
    public void ShakeCamera( float duration=0.5f,float strength=0.2f)
    {
        cameraTsf.DOShakePosition(duration, strength,15);       
    }

}
