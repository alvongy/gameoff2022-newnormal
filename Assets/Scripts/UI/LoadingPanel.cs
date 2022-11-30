using UnityEngine;
using UnityEngine.Playables;

public class LoadingPanel : MonoBehaviour
{
	[SerializeField] private GameObject _fadeInPanelObj;
	[SerializeField] private PlayableDirector _fadeInPlayableDirector;
	[SerializeField] private GameObject _fadeOutPanelObj;
	[SerializeField] private PlayableDirector _fadeOutPlayableDirector;

	[SerializeField] private BoolEventChannelSO _toggleLoadingScreen = default;

	private bool _inShowing = false;
	private void OnEnable()
	{
		//_fadeInPlayableDirector.stopped += FadeInPlayableDirector_stopped;
		_fadeOutPlayableDirector.stopped += FadeOutPlayableDirector_stopped;
		_toggleLoadingScreen.OnEventRaised += ToggleLoadingScreen_OnEventRaised;
	}


	private void OnDisable()
	{
		//_fadeInPlayableDirector.stopped -= FadeInPlayableDirector_stopped;
		_fadeOutPlayableDirector.stopped -= FadeOutPlayableDirector_stopped;
		_toggleLoadingScreen.OnEventRaised -= ToggleLoadingScreen_OnEventRaised;
	}

	private void ToggleLoadingScreen_OnEventRaised(bool arg0)
	{
		if (arg0)
		{
			if (!_inShowing)
			{
				_inShowing = true;
				_fadeInPanelObj.SetActive(true);
				_fadeOutPanelObj.SetActive(false);
				_fadeInPlayableDirector.Play();
			}
		}
		else
		{
			if (_inShowing)
			{
				_inShowing = false;
				_fadeInPanelObj.SetActive(false);
				_fadeOutPanelObj.SetActive(true);
				_fadeOutPlayableDirector.Play();
			}
		}
	}
	//private void FadeInPlayableDirector_stopped(PlayableDirector obj)
	//{
	//	_fadeInPanelObj.SetActive(false);
	//	_fadeOutPanelObj.SetActive(true);
	//}

	private void FadeOutPlayableDirector_stopped(PlayableDirector obj)
	{
		_inShowing = false;
		_fadeOutPanelObj.SetActive(false);
	}
}
