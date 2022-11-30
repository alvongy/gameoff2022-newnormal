using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
	[SerializeField] InputReader _inputReader;
	[SerializeField] BasePanel[] panelList;
	[SerializeField] private UIInventory _inventoryPanel = default;

	[SerializeField] private VoidEventChannelSO _onSceneReady = default;
	[SerializeField] private TransformEventChannelSO _onPlayerReady = default;

	public static Stack<BasePanel> inOpeningPanel = new Stack<BasePanel>();
	static Dictionary<string, BasePanel> panelDict = new Dictionary<string, BasePanel>();
	private void Awake()
	{
		panelDict.Clear();
		foreach (BasePanel panel in panelList)
		{
			panel.Init();
			if (!panelDict.ContainsKey(panel.panelName))
			{
				//#if UNITY_EDITOR
				//			Debug.Log(panel.name);
				//#endif
				panelDict.Add(panel.panelName, panel);
			}
		}
		_inputReader.CloseMenuEvent += ClosePanelTop;
	}
	private void Start()
	{
#if UNITY_EDITOR
		if (EditorColdStartup.isColdStart)
		{
			ResetUI();
		}
#endif
	}
	private void OnEnable()
	{
		//_onSceneReady.OnEventRaised += ResetUI;
		_onPlayerReady.OnEventRaised += ResetUI;
		_inventoryPanel.FillInventory();
		_inventoryPanel.gameObject.SetActive(true);
	}
	private void OnDisable()
	{
		//_onSceneReady.OnEventRaised -= ResetUI;
		_onPlayerReady.OnEventRaised -= ResetUI;
	}
	private void ResetUI(Transform tsf = null)
	{
		if (panelDict.ContainsKey("InGamePanel"))
		{
			panelDict["InGamePanel"].Show();
		}
	}

	static void ClosePanelTop()
	{
		if (inOpeningPanel.Count > 1)
		{
			inOpeningPanel.Pop().Close();
		}
	}

}
