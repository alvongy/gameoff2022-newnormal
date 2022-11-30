using UnityEngine;

public class CameraController : MonoBehaviour
{
	[SerializeField] Camera _camera;
	[SerializeField] CameraConfigSO _cameraConfig;

	bool _targetExist;
	Transform _targetTransform;

	public Transform TargetTransform
	{
		get => _targetTransform;
		set
		{
			_targetTransform = value;
			_targetExist = value != null;
		}
	}

	private void LateUpdate()
	{
		if (_targetExist)
		{
			_camera.transform.position = Vector3.Lerp(_camera.transform.position, TargetTransform.position + _cameraConfig.CamOffset, 0.1f);
			//_camera.transform.position = TargetTransform.position + _cameraConfig.CamOffset;
		}
	}

}
