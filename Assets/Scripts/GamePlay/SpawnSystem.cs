using UnityEngine;

public class SpawnSystem : MonoBehaviour
{
	[Header("Asset References")]
	[SerializeField] private InputReader _inputReader = default;
	[SerializeField] private MainCharacter _playerPrefab = default;
	[SerializeField] private TransformAnchor _playerTransformAnchor = default;
	[SerializeField] private TransformEventChannelSO _playerInstantiatedChannel = default;

	[Header("Scene Ready Event")]
	[SerializeField] private VoidEventChannelSO _onSceneReady = default; //Raised by SceneLoader when the scene is set to active

	private SpawnPoints _spawnPoints;
	private Transform _defaultSpawnPoint;

	private void Awake()
	{
		_spawnPoints = GameObject.FindObjectOfType<SpawnPoints>();
		_defaultSpawnPoint = transform;
	}
	private void OnEnable()
	{
		_onSceneReady.OnEventRaised += SpawnPlayer;
		_onSceneReady.OnEventRaised += YKDCPoint;
	}

	private void YKDCPoint()
	{
		YKDC.YKDC.PostStartRound(null, SaveSystem.GetPlayCount().ToString());
		//try
		//{
		//	DobestAnalysisSDK.Instance.SetCustomEvent(
		//		"StartRound",
		//		"StartRound",
		//		new DobestAnalysisEvent.CustomInfo
		//		{
		//			custom = new System.Collections.Generic.Dictionary<string, object>
		//			{
		//					{"PlayCount",SaveSystem.GetPlayCount().ToString() },
		//			}
		//		},
		//		"StartRound");
		//}
		//catch (System.Exception)
		//{

		//}
	}

	private void OnDisable()
	{
		_onSceneReady.OnEventRaised -= SpawnPlayer;
		_onSceneReady.OnEventRaised -= YKDCPoint;

		_playerTransformAnchor.Unset();
	}

	private Transform GetSpawnLocation()
	{
		if (_spawnPoints == null)
		{
			return _defaultSpawnPoint;
		}

		return _spawnPoints.transform;
	}
	private void SpawnPlayer()
	{
		Transform spawnLocation = GetSpawnLocation();
		MainCharacter playerInstance = Instantiate(_playerPrefab, spawnLocation.position, spawnLocation.rotation);

		_playerInstantiatedChannel.RaiseEvent(playerInstance.transform);
		_playerTransformAnchor.Provide(playerInstance.transform); //the CameraSystem will pick this up to frame the player

		//TODO: Probably move this to the GameManager once it's up and running
		_inputReader.EnableGameplayInput();
	}
}
