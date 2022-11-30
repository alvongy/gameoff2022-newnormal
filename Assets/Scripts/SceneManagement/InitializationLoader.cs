using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
#if PLATFORM_ANDROID
using UnityEngine.InputSystem.EnhancedTouch;
#endif
public class InitializationLoader : MonoBehaviour
{
	[SerializeField] private GameSceneSO _managersScene = default;
	[SerializeField] private GameSceneSO _menuToLoad = default;

	[Header("Broadcasting on")]
	[SerializeField] private AssetReference _menuLoadChannel = default;

	private void Start()
	{
		//Load the persistent managers scene
		Application.targetFrameRate = 60;
#if PLATFORM_ANDROID
		EnhancedTouchSupport.Enable();
#endif
		//if (Application.isMobilePlatform)
		//	QualitySettings.vSyncCount = 0;
		_managersScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true).Completed += LoadEventChannel;
	}

	private void LoadEventChannel(AsyncOperationHandle<SceneInstance> obj)
	{
		_menuLoadChannel.LoadAssetAsync<LoadEventChannelSO>().Completed += LoadMainMenu;
	}

	private void LoadMainMenu(AsyncOperationHandle<LoadEventChannelSO> obj)
	{
		obj.Result.RaiseEvent(_menuToLoad);

		SceneManager.UnloadSceneAsync(0); //Initialization is the only scene in BuildSettings, thus it has index 0
	}
}
