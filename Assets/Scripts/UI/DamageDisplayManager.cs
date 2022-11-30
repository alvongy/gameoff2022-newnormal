using Unity.Collections;
using UnityEngine;

public class DamageDisplayManager : MonoBehaviour
{
	const int MAX_SINGLE_NUMBER = 64;

	[SerializeField] private DamageDisplayChannelSO _channelSO;

	[SerializeField] private Mesh _mesh;
	[SerializeField] private Material _material;

	private Quaternion _camRot;

	private MaterialPropertyBlock[] _blocks = new MaterialPropertyBlock[10];
	private NativeArray<Matrix4x4> _matrixs;
	private NativeArray<float> _inUsings;
	private NativeArray<int> _numIdx;

	private void OnEnable()
	{
		_camRot = Camera.main.transform.rotation;

		_matrixs = new NativeArray<Matrix4x4>(MAX_SINGLE_NUMBER * 10, Allocator.Persistent);
		_inUsings = new NativeArray<float>(MAX_SINGLE_NUMBER * 10, Allocator.Persistent);
		_numIdx = new NativeArray<int>(10, Allocator.Persistent);
		_channelSO.OnEventRaised += AddDisplay;
	}
	private void OnDisable()
	{
		_matrixs.Dispose();
		_inUsings.Dispose();
		_numIdx.Dispose();
		_channelSO.OnEventRaised -= AddDisplay;
	}


	private void Start()
	{
		for (int i = 0; i < 10; i++)
		{
			_blocks[i] = new MaterialPropertyBlock();
			_blocks[i].SetVector("_BaseMap_ST", new Vector4(0.0625f, 1f, 0.0625f * i, 0f));
		}
	}
	private void LateUpdate()
	{
		for (int i = 0; i < 10; i++)
		{
			if (_numIdx[i] > 0)
			{
				Graphics.DrawMeshInstanced(
					_mesh,
					0,
					_material,
					_matrixs.Slice(i * MAX_SINGLE_NUMBER, _numIdx[i]).ToArray(),
					_numIdx[i],
					_blocks[i],
					UnityEngine.Rendering.ShadowCastingMode.Off,
					false,
					0,
					null,
					UnityEngine.Rendering.LightProbeUsage.Off);
			}
		}
	}

	private void AddDisplay(Vector3 arg0, int arg1)
	{
		Vector3 offset = new Vector3(Random.Range(0f, 1f), 0f, Random.Range(0f, 1f));
		while (true)
		{
			int n = arg1 % 10;
			if (_numIdx[n] < MAX_SINGLE_NUMBER)
			{
				_matrixs[n * MAX_SINGLE_NUMBER + _numIdx[n]] = Matrix4x4.TRS(arg0 + offset, _camRot, Vector3.one);
				_numIdx[n]++;
			}
			if (arg1 < 10)
			{
				break;
			}
			else
			{
				arg1 /= 10;
				offset.x += 1f;
			}
		}
	}
}
