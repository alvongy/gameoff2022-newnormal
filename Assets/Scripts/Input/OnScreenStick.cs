
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.Serialization;

public class OnScreenStick : OnScreenControl, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
	[SerializeField] private RectTransform _stickBgRect;
	[SerializeField] private RectTransform _stickRect;

	[FormerlySerializedAs("movementRange")]
	[SerializeField]
	private float m_MovementRange = 50;

	[InputControl(layout = "Vector2")]
	[SerializeField]
	private string m_ControlPath;

	[SerializeField] private IntArrayEventChannelSO _startAbilitySel;
	[SerializeField] private IntEventChannelSO _endAbilitySel;

	private RectTransform _rect;

	private Vector2 _startPos;
	private Vector2 _offset;

	public float movementRange
	{
		get => m_MovementRange;
		set => m_MovementRange = value;
	}
	protected override string controlPathInternal
	{
		get => m_ControlPath;
		set => m_ControlPath = value;
	}
	private void Awake()
	{
		_rect = (RectTransform)transform;
		_startAbilitySel.OnEventRaised += SetDeactive;
		_endAbilitySel.OnEventRaised += SetActive;
	}
	private void OnDestroy()
	{
		_startAbilitySel.OnEventRaised -= SetDeactive;
		_endAbilitySel.OnEventRaised -= SetActive;
	}
	private void SetActive(int arg0)
	{
		gameObject.SetActive(true);
	}

	private void SetDeactive(int[] arg0)
	{
		gameObject.SetActive(false);
	}


	private void Start()
	{
		//_startPos = _stickRect.anchoredPosition;
		//_offset = _rect.sizeDelta / 2f;
		_offset = _rect.sizeDelta;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (eventData == null)
			return;
        //throw new System.ArgumentNullException(nameof(eventData));

        RectTransformUtility.ScreenPointToLocalPointInRectangle(_rect, eventData.position, eventData.pressEventCamera, out var pos);
        //pos -= _offset;
        _stickBgRect.anchoredPosition = pos;
        _stickRect.anchoredPosition = Vector2.zero;

        SendValueToControl(Vector2.zero);
        _stickBgRect.gameObject.SetActive(true);
        _stickRect.gameObject.SetActive(true);
    }

	public void OnDrag(PointerEventData eventData)
	{
		if (eventData == null)
			return;
			//throw new System.ArgumentNullException(nameof(eventData));

		RectTransformUtility.ScreenPointToLocalPointInRectangle(_stickBgRect, eventData.position, eventData.pressEventCamera, out var delta);
		delta -= _offset;
		delta = Vector2.ClampMagnitude(delta, movementRange);
		_stickRect.anchoredPosition = delta;

		var newPos = new Vector2(delta.x / movementRange, delta.y / movementRange);
		SendValueToControl(newPos);
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		//_stickRect.anchoredPosition = _startPos;
		SendValueToControl(Vector2.zero);

        _stickBgRect.gameObject.SetActive(false);
        _stickRect.gameObject.SetActive(false);
    }

}
