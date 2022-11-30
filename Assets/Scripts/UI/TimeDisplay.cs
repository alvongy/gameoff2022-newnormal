using System.Collections;
using TMPro;
using UnityEngine;

public class TimeDisplay : MonoBehaviour
{
	[SerializeField] private TransformEventChannelSO _playerReady;
	[SerializeField] private TextMeshProUGUI _textTime;
	private void OnEnable()
	{
		_playerReady.OnEventRaised += StartTimeDisplay;
		StartTimeDisplay(null);//TODO
	}
	private void OnDisable()
	{
		_playerReady.OnEventRaised -= StartTimeDisplay;
		StopAllCoroutines();
	}
	private void StartTimeDisplay(Transform arg0)
	{
		StartCoroutine(StartTimeDisplay());
	}
	IEnumerator StartTimeDisplay()
	{
		WaitForFixedUpdate wait = new WaitForFixedUpdate();
		float startTime = Time.time;
		//float endTime = startTime + 60f * 15f;

		while (true)
		{
			yield return wait;
			int time = (int)(Time.time - startTime);
			_textTime.SetText($"{time / 60:00}:{time % 60:00}");
		}
	}
}
