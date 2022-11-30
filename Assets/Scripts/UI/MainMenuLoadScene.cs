using UnityEngine;
using static YKQuestWatcher;

public class MainMenuLoadScene : MonoBehaviour
{
	[SerializeField] private GameSceneSO _gameScene;
	[SerializeField] private LoadEventChannelSO _loadEventChannel;
	[SerializeField] private BoolEventChannelSO _toggleLoadingScreen = default;

	public void Load()
	{
		_loadEventChannel.RaiseEvent(_gameScene, true, false);
		//_loadEventChannel.RaiseEvent(_gameScene, false, false);
		//	_toggleLoadingScreen.RaiseEvent(true);
		Time.timeScale = 1;
	}
	/// <summary>
	/// 刷新全局数据，应在游戏重新开始时统一初始化数据
	/// </summary>
	public void RefreshIsFirstTerrain()
    {
		YKDataInfoManager.Instance.OnPlayerDie();
	}
	public void QuitGame()
    {
#if UNITY_EDITOR 
		UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif

	}
}
