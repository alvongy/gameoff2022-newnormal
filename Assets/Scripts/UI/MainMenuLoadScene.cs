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
	/// ˢ��ȫ�����ݣ�Ӧ����Ϸ���¿�ʼʱͳһ��ʼ������
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
