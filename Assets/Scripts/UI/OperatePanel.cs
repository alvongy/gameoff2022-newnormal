using UnityEngine;

public class OperatePanel : BasePanel
{
	[SerializeField] InputReader _inputReader;
	[SerializeField] VoidEventChannelSO _toggleOperateUI;
	protected override void OnInit()
	{
		_toggleOperateUI.OnEventRaised += Show;
	}

	protected override void OnShow()
	{
		_inputReader.EnableMenuInput();
	}
	protected override void OnClose()
	{
		_inputReader.EnableGameplayInput();
	}

	protected override void OnPanelDestroy()
	{
		_toggleOperateUI.OnEventRaised -= Show;
	}

}
