using System;
using UnityEngine;

public abstract class BasePanel : MonoBehaviour
{
	[NonSerialized] public string panelName;
	[NonSerialized] public bool inShowing;

	public virtual void Init()
	{
		panelName = gameObject.name;
		OnInit();
	}
	public virtual void Show()
	{
		if (inShowing)
		{
			return;
		}
		UIManager.inOpeningPanel.Push(this);
		gameObject.SetActive(true);
		OnShow();
		inShowing = true;
	}
	public virtual void Close()
	{
		if (!inShowing)
		{
			return;
		}
		gameObject.SetActive(false);
		OnClose();
		inShowing = false;
	}

	public virtual void PanelDestroy()
	{
		OnPanelDestroy();
	}
	protected abstract void OnInit();
	protected abstract void OnShow();
	protected abstract void OnClose();
	protected abstract void OnPanelDestroy();
}
