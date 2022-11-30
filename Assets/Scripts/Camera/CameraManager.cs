using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

	public Camera mainCamera;
	public CinemachineVirtualCamera freeLookVCam;
	public CinemachineImpulseSource impulseSource;

	[SerializeField] private TransformAnchor _cameraTransformAnchor = default;
	[SerializeField] private TransformAnchor _mainCharacterTransformAnchor = default;
	[SerializeField] private VoidEventChannelSO _camShakeEvent = default;

	private void Start()
	{
		if (_mainCharacterTransformAnchor.isSet)
		{
			SetCameraTarget();
		}
	}
	private void OnEnable()
	{
		_cameraTransformAnchor.Provide(mainCamera.transform);
		_mainCharacterTransformAnchor.OnAnchorProvided += SetCameraTarget;
		_camShakeEvent.OnEventRaised += impulseSource.GenerateImpulse;
	}
	private void OnDisable()
	{
		_cameraTransformAnchor.Unset();
		_mainCharacterTransformAnchor.OnAnchorProvided -= SetCameraTarget;
		_camShakeEvent.OnEventRaised -= impulseSource.GenerateImpulse;
	}
	[ContextMenu("A")]
	void text()
	{
		impulseSource.GenerateImpulse();
	}
	void SetCameraTarget()
	{
		Transform target = _mainCharacterTransformAnchor.Value;

		freeLookVCam.Follow = target;
		freeLookVCam.LookAt = target;
		freeLookVCam.OnTargetObjectWarped(target, target.position - freeLookVCam.transform.position - Vector3.forward);
	}
}
