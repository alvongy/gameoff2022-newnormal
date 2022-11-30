using System;
using System.Text;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
	const string PLAYER_ID = "PlAYER_ID";
	const string PLAY_COUNT = "PLAY_COUNT";

	const string CHAR_LIST = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

	private static string _playerID;


	private void Awake()
	{
#if UNITY_EDITOR || UNITY_WEBGL
		return;
#endif
		YKDC.YKDC.Init();
	}
	private void Start()
	{
#if UNITY_EDITOR || UNITY_WEBGL
		return;
#endif
		byte[] b = new byte[4];
		new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(b);
		System.Random r = new System.Random(BitConverter.ToInt32(b, 0));

		if (PlayerPrefs.HasKey(PLAYER_ID))
		{
			_playerID = PlayerPrefs.GetString(PLAYER_ID);
#if UNITY_EDITOR
			Debug.Log($"Get PlayerID {_playerID}");
#endif
			YKDC.YKDC.SetUserID(_playerID);
			YKDC.YKDC.PostLogin(false);
			DobestAnalysisSDK.Instance.OnLogin(new DobestAnalysisEvent.LoginInfo
			{
				type = DobestAnalysisEvent.LoginInfo.LOGIN_TYPE_SUCCESS,
				isUserNew = false,
			});
		}
		else
		{
			StringBuilder sb = new StringBuilder();
			sb.Clear();
			for (int i = 0; i < 20; i++)
			{
				sb.Append(CHAR_LIST.Substring(r.Next(0, CHAR_LIST.Length), 1));
			}
			_playerID = sb.ToString();
#if UNITY_EDITOR
			Debug.Log($"Create PlayerID {_playerID}");
#endif
			PlayerPrefs.SetString(PLAYER_ID, _playerID);
			YKDC.YKDC.SetUserID(_playerID);
			YKDC.YKDC.PostLogin(true);
			DobestAnalysisSDK.Instance.OnLogin(new DobestAnalysisEvent.LoginInfo
			{
				type = DobestAnalysisEvent.LoginInfo.LOGIN_TYPE_SUCCESS,
				isUserNew = true,
			});
		}
		YKDC.YKDC.PostGameLoading((int)Time.realtimeSinceStartup);
		//try
		//{
		//	DobestAnalysisSDK.Instance.SetCustomEvent(
		//		"GameLoading",
		//		"GameLoading",
		//		new DobestAnalysisEvent.CustomInfo
		//		{
		//			custom = new System.Collections.Generic.Dictionary<string, object>
		//			{
		//					{"realtimeSinceStartup",(int)Time.realtimeSinceStartup },
		//			}
		//		},
		//		"GameLoading");
		//}
		//catch (Exception)
		//{
		//}
		//#if UNITY_EDITOR
		//		Debug.Log(Time.realtimeSinceStartup);
		//#endif
	}

	public static void AddPlayCount()
	{
		if (!PlayerPrefs.HasKey(PLAY_COUNT))
		{
			PlayerPrefs.SetInt(PLAY_COUNT, 1);
		}
		else
		{
			PlayerPrefs.SetInt(PLAY_COUNT, PlayerPrefs.GetInt(PLAY_COUNT));
		}
	}
	public static int GetPlayCount()
	{
#if UNITY_EDITOR || UNITY_WEBGL
		return 0;
#endif
		if (PlayerPrefs.HasKey(PLAY_COUNT))
		{
			return PlayerPrefs.GetInt(PLAY_COUNT);
		}
		else
		{
			PlayerPrefs.SetInt(PLAY_COUNT, 0);
			return 0;
		}
	}
}