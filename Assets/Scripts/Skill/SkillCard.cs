using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Components;

public class SkillCard : MonoBehaviour, IPointerDownHandler//, IPointerClickHandler
{
	[SerializeField] private TextMeshPro _textName;
	[SerializeField] private TextMeshPro _textDescription;
	[SerializeField] private LocalizeStringEvent _textNameEvent;
	[SerializeField] private LocalizeStringEvent _textDesEvent;
	[SerializeField] private SpriteRenderer _spriteRenderer;

	[HideInInspector] public bool EnableClick;
	public UnityAction OnPonitClickAction = delegate { };

	//public string CardName { set => _textName.SetText(value); }
	//public string CardDescription { set => _textDescription.SetText(value); }

	public Sprite CardSprite { set => _spriteRenderer.sprite = value; }
	public LocalizeStringEvent TextNameEvent { get => _textNameEvent; set => _textNameEvent = value; }
	public LocalizeStringEvent TextDesEvent { get => _textDesEvent; set => _textDesEvent = value; }

	//public void OnPointerClick(PointerEventData eventData)
	//{
	//	if (EnableClick)
	//	{
	//		OnPonitClickAction.Invoke();
	//	}
	//}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (EnableClick)
		{
			OnPonitClickAction.Invoke();
		}
	}
}
