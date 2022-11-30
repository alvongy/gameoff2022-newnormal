using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.Profiling;
public class DamageDisplayPanel : MonoBehaviour
{
	class DamageDisplay
	{
		public bool InUsing;
		public bool InShowing;
		public TextMeshProUGUI Text;
		public Vector3 Pos;
		public Vector3 Offset;
		public float DisappearTime;
		public TransformAccess TransformAccess;
	}
	struct NativeDamageDisplay
	{
		public bool InUsing;
		public float3 Pos;
		public float3 Offset;
		public float DisappearTime;
		public TransformAccess Trans;
	}

	[SerializeField] private DamageDisplayChannelSO _channelSO;
	private List<DamageDisplay> _damageDisplays = new List<DamageDisplay>();
	private List<DamageDisplay> _damageDisplaysTemp = new List<DamageDisplay>();
	[SerializeField] private TextMeshProUGUI _textPrefab;
	[SerializeField] private Camera _targetcamera;
	[SerializeField] private Canvas _canvas;
	[SerializeField] private TextMeshProUGUI _textCount;
	[SerializeField] private TextMeshProUGUI _textDPS;
	[SerializeField] private bool _showDamageInfo = false;

	private Stack<DamageDisplay> _damageDisplayPool = new Stack<DamageDisplay>();
	private float2 _screenSize;
	private float _panelScale;
	private float3 _panelOffset;
	private int _count;
	private int _countLastSecond;
	private float _nextClearTime;
	private void Start()
	{
		_screenSize = new float2(Screen.width, Screen.height);
		_panelScale = _canvas.transform.localScale.x;
		_panelOffset = _canvas.transform.position;
	}

	private void Update()
	{
		if (_damageDisplays.Count > 0)
		{
			Profiler.BeginSample("DamageDisplay");
			NativeArray<NativeDamageDisplay> na = new NativeArray<NativeDamageDisplay>(_damageDisplays.Count, Allocator.TempJob);
			for (int i = 0; i < na.Length; i++)
			{
				na[i] = new NativeDamageDisplay()
				{
					InUsing = _damageDisplays[i].InUsing,
					Pos = _damageDisplays[i].Pos,
					Offset = _damageDisplays[i].Offset,
					DisappearTime = _damageDisplays[i].DisappearTime,
				};
			}
			new ShowDisplay()
			{
				DamageDisplays = na,
				MVP = math.mul(_targetcamera.projectionMatrix, _targetcamera.worldToCameraMatrix),
				ScreenSize = _screenSize,
				PanelScale = _panelScale,
				PanelOffset = _panelOffset,
				timeNow = Time.time,

			}.Schedule<ShowDisplay>(na.Length, 4).Complete();
			_damageDisplaysTemp.Clear();

			for (int i = 0; i < na.Length; i++)
			{
				if (na[i].InUsing)
				{
					if (!_damageDisplays[i].InShowing)
					{
						_damageDisplays[i].Text.gameObject.SetActive(true);
						_damageDisplays[i].InShowing = true;
					}
					_damageDisplays[i].Text.transform.position = na[i].Pos;
					_damageDisplaysTemp.Add(_damageDisplays[i]);
				}
				else
				{
					_damageDisplays[i].Text.gameObject.SetActive(false);
					_damageDisplays[i].InShowing = false;
					_damageDisplayPool.Push(_damageDisplays[i]);
				}
			}
			(_damageDisplays, _damageDisplaysTemp) = (_damageDisplaysTemp, _damageDisplays);
			na.Dispose();
			Profiler.EndSample();
			if (_showDamageInfo)
			{
				_textCount.SetText(_count.ToString());
				if (Time.time > _nextClearTime)
				{
					_countLastSecond = 0;
					_nextClearTime = Time.time + 1f;
				}
				_textDPS.SetText(_countLastSecond.ToString());//???
			}
		}
	}
	private void OnEnable()
	{
		_channelSO.OnEventRaised += ShowDamage;
	}
	private void OnDisable()
	{
		_channelSO.OnEventRaised -= ShowDamage;
	}
	private void ShowDamage(Vector3 arg0, int arg1)
	{
		DamageDisplay dd = GetDisplay();
		dd.InUsing = true;
		dd.Text.SetText(arg1.ToString());
		dd.Pos = arg0;
		dd.Offset = new Vector3(UnityEngine.Random.Range(0f, 64f), UnityEngine.Random.Range(0f, 64f), 0f);
		dd.DisappearTime = Time.time + 1f;
		_damageDisplays.Add(dd);
		_count += arg1;
		_countLastSecond += arg1;
	}
	private DamageDisplay GetDisplay()
	{
		if (_damageDisplayPool.Count > 0)
		{
			return _damageDisplayPool.Pop();
		}
		var dd = new DamageDisplay();
		dd.Text = Instantiate(_textPrefab.gameObject, transform).GetComponent<TextMeshProUGUI>();
		return dd;
	}
	struct ShowDisplay : IJobParallelFor
	{
		public NativeArray<NativeDamageDisplay> DamageDisplays;
		[ReadOnly] public float4x4 MVP;
		[ReadOnly] public float2 ScreenSize;
		[ReadOnly] public float PanelScale;
		[ReadOnly] public float3 PanelOffset;
		[ReadOnly] public float timeNow;
		public void Execute(int index)
		{
			var dd = DamageDisplays[index];
			if (timeNow > dd.DisappearTime)
			{
				dd.InUsing = false;
			}
			if (dd.InUsing)
			{
				var screenClip = math.mul(MVP, new float4(dd.Pos, 1f));
				var pos = screenClip.xy / screenClip.w / 2f;
				pos *= ScreenSize;
				pos += dd.Offset.xy;
				pos *= PanelScale;
				dd.Pos.xy = PanelOffset.xy + pos;
				dd.Pos.z = PanelOffset.z;
			}
			DamageDisplays[index] = dd;
		}
	}
}
