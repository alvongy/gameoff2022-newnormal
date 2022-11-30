using TMPro;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Jobs;

public class DamageDisplayAlterPanel : MonoBehaviour
{
	struct DisplayUnit
	{
		public float3 WorldPos;
		public float3 ScreenOffset;
		public float DisappearTime;
	}
	[SerializeField] private DamageDisplayChannelSO _channelSO;


	private Transform[] _transs;
	private TextMeshProUGUI[] _texts;
	private NativeArray<bool> _usingState;

	private int _displayCount = 0;

	struct ShowDisplay : IJobParallelForTransform
	{
		[ReadOnly] public NativeArray<DisplayUnit> _displayList;
		public NativeArray<bool> UsingStates;
		public int count;
		[ReadOnly] public float4x4 MVP;
		[ReadOnly] public float2 ScreenSize;
		[ReadOnly] public float PanelScale;
		[ReadOnly] public float3 PanelOffset;
		[ReadOnly] public float timeNow;
		public void Execute(int index, TransformAccess transform)
		{
			if (!UsingStates[index])
			{
				return;
			}
			float3 screenOffset;
			var du = _displayList[index];
			if (timeNow > Time.time)
			{
				UsingStates[index] = false;
				screenOffset = new float3(-1f, -1f, 0f);
				count--;
			}
			else
			{
				screenOffset = du.ScreenOffset;
			}
			var screenClip = math.mul(MVP, new float4(du.WorldPos, 1f));
			var pos = screenClip.xyz / screenClip.w / 2f;
			pos.xy *= ScreenSize;
			pos += screenOffset;
			pos *= PanelScale;
			pos += PanelOffset;
			pos.z = PanelOffset.z;
			transform.position = pos;
		}
	}

	private void OnEnable()
	{
		_channelSO.OnEventRaised += AddDisplay;
	}

	private void AddDisplay(Vector3 arg0, int arg1)
	{
		for (int i = 0; i < _usingState.Length; i++)
		{
			if (!_usingState[i])
			{
				
			}
		}
	}

	private void LateUpdate()
	{
		if (_displayCount > 0)
		{
			new ShowDisplay()
			{

			}.Schedule(new TransformAccessArray(_transs));
			var a = new TransformAccessArray();
		}
	}
}
