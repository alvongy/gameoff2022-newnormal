using UnityEngine;

[CreateAssetMenu(fileName = "CameraConfig", menuName = "Config/Camera Config")]
public class CameraConfigSO : ScriptableObject
{
	[SerializeField] private float _camOffsetY;
	[SerializeField] private float _camOffsetZ;
	[SerializeField] [Range(0, 90)] private float _camRotation;

	Vector3 _camOffset = Vector3.zero;
	public float CamOffsetY { get => _camOffsetY; set => _camOffsetY = value; }
	public float CamOffsetZ { get => _camOffsetZ; set => _camOffsetZ = value; }
	public float CamRotation { get => _camRotation; set => _camRotation = value; }
	public Vector3 CamOffset { get => _camOffset; set => _camOffset = value; }

	private void Awake()
	{
		ResetCameraOffSet();
	}

	[ContextMenu("ResetCameraOffSet")]
	private void ResetCameraOffSet()
	{
		_camOffset = new Vector3(0f, _camOffsetY, _camOffsetZ);
	}
}
