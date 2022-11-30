using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class EditorColdStartup : MonoBehaviour
{
#if UNITY_EDITOR
	[SerializeField] private GameSceneSO _thisSceneSO = default;
	[SerializeField] private GameSceneSO _persistentManagersSO = default;

	[SerializeField] private AssetReference _notifyColdStartupChannel = default;
	[SerializeField] private VoidEventChannelSO _onSceneReadyChannel = default;

	public static bool isColdStart = false;

	private void Awake()
	{
		if (isColdStart)
		{
			isColdStart = false;
		}
		if (!SceneManager.GetSceneByName(_persistentManagersSO.sceneReference.editorAsset.name).isLoaded)
		{
			isColdStart = true;
		}
	}
	private void Start()
	{
		if (isColdStart)
		{
			_persistentManagersSO.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true).Completed += LoadEventChannel;
			//_persistentManagersSO.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true).Completed += TriggerNotifyColdStartup;
		}
	}

	private void LoadEventChannel(AsyncOperationHandle<SceneInstance> obj)
	{
		_notifyColdStartupChannel.LoadAssetAsync<LoadEventChannelSO>().Completed += OnNotifyChannelLoaded;
	}

	private void OnNotifyChannelLoaded(AsyncOperationHandle<LoadEventChannelSO> obj)
	{
		if (_thisSceneSO != null)
		{
			obj.Result.RaiseEvent(_thisSceneSO);
		}
		else
		{
			//Raise a fake scene ready event, so the player is spawned
			_onSceneReadyChannel.RaiseEvent();
			//When this happens, the player won't be able to move between scenes because the SceneLoader has no conception of which scene we are in
		}
	}

	//private void TriggerNotifyColdStartup(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<UnityEngine.ResourceManagement.ResourceProviders.SceneInstance> obj)
	//{
	//	if (_thisSceneSO != null)
	//	{
	//		EvtNotifyColdStartup evt = EvtNotifyColdStartup.GetEvents();
	//		evt.locationToLoad = _thisSceneSO;
	//		EvtNotifyColdStartup.Trigger();
	//	}
	//	else
	//	{
	//		EvtOnSceneReady.Trigger();
	//	}
	//}
#endif
}
