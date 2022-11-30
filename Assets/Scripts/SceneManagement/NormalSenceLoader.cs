using UnityEngine;

public class NormalSenceLoader : MonoBehaviour
{
	[SerializeField] private VoidEventChannelSO _deathEvent;
	[SerializeField] private LoadEventChannelSO _loadEventChannel;
	[SerializeField] private GameSceneSO _sceneSO;
	[SerializeField] private bool _showLoading;
	[SerializeField] private BoolEventChannelSO _toggleLoadingScreen = default;
	private float _startTime;
	private void OnEnable()
	{
		_deathEvent.OnEventRaised += LoadScene;
		_startTime = Time.time;
	}
	private void OnDisable()
	{
		_deathEvent.OnEventRaised -= LoadScene;
	}
	public void LoadScene()
	{
		YKDC.YKDC.PostRoundResult((int)(Time.time - _startTime));
		//try
		//{
		//	DobestAnalysisSDK.Instance.SetCustomEvent(
		//		"RoundResult",
		//		"RoundResult",
		//		new DobestAnalysisEvent.CustomInfo
		//		{
		//			custom = new System.Collections.Generic.Dictionary<string, object>
		//			{
		//							{"Duration",((int)(Time.time - _startTime)).ToString() },
		//			}
		//		},
		//		"RoundResult");
		//}
		//catch (System.Exception)
		//{
		//}
			_toggleLoadingScreen.RaiseEvent(true);
		_loadEventChannel.RaiseEvent(_sceneSO, _showLoading);
	}
}
