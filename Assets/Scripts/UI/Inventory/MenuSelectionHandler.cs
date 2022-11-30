using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuSelectionHandler : MonoBehaviour
{
	[SerializeField] private InputReader _inputReader;
	//[SerializeField] private GameObject _defaultSelection;
	[SerializeField] private TransformAnchor _playerTransformAnchor;
	//public GameObject currentSelection;
	//public GameObject mouseSelection;

	[SerializeField] private ItemEventChannelSO _removeItemEvent = default;
	[SerializeField] private UIInspectorDescription _inspectorDescription = default;
	private UIInventoryItem _UIInventoryItem;
	public GameObject BagItemUse_Btn;
	public Button buttonYes;
	public Button buttonNo;


    private void Awake()
    {
		_UIInventoryItem = transform.GetComponent<UIInventoryItem>();

		buttonNo.onClick.AddListener(() =>
		{
			BagItemUse_Btn.SetActive(false);
		});
	}

    private void OnEnable()
	{
		_inputReader.menuMouseMoveEvent += HandleMoveCursor;
		_inputReader.moveSelectionEvent += HandleMoveSelection;

		StartCoroutine(SelectDefault());
	}

	private void OnDisable()
	{
		_inputReader.menuMouseMoveEvent -= HandleMoveCursor;
		_inputReader.moveSelectionEvent -= HandleMoveSelection;
	}

	void clicked()
	{
		_playerTransformAnchor.Value.GetComponent<MFAbilityHandler>().AddAbility((int)_UIInventoryItem._currentItem.Item.DataPack["MFAbility"]);
		BagItemUse_Btn.SetActive(false);
		buttonYes.onClick.RemoveListener(clicked);
		_removeItemEvent.RaiseEvent(_UIInventoryItem._currentItem.Item);
	}


	public void UpdateDefault(GameObject newDefault)
	{
		//_defaultSelection = newDefault;

	}
	/// <summary>
	/// Highlights the default element
	/// </summary>
	private IEnumerator SelectDefault()
	{
		yield return new WaitForSeconds(.03f); // Necessary wait otherwise the highlight won't show up

		//if (_defaultSelection != null)
		//	UpdateSelection(_defaultSelection);
	}

	public void Unselect()
	{
		//currentSelection = null;
		EventSystem.current.SetSelectedGameObject(null);
	}

	/// <summary>
	/// Fired by keyboard and gamepad inputs. Current selected UI element will be the ui Element that was selected
	/// when the event was fired. The _currentSelection is updated later on, after the EventSystem moves to the
	/// desired UI element, the UI element will call into UpdateSelection()
	/// </summary>
	private void HandleMoveSelection()
	{
		Cursor.visible = false;

		// Handle case where no UI element is selected because mouse left selectable bounds
	//	if (EventSystem.current.currentSelectedGameObject == null)
	//		EventSystem.current.SetSelectedGameObject(currentSelection);
	}

	private void HandleMoveCursor()
	{
		//if (mouseSelection != null)
		//{
		//	EventSystem.current.SetSelectedGameObject(mouseSelection);
		//}

		Cursor.visible = true;
	}

	public void HandleMouseEnter(GameObject UIElement)
    {
		_inspectorDescription.FillDescription(_UIInventoryItem._currentItem.Item);


        //mouseSelection = UIElement;
        EventSystem.current.SetSelectedGameObject(UIElement);
	}

	public void HandleMouseExit(GameObject UIElement)
	{
		_inspectorDescription.UnFillDescription();
		if (EventSystem.current.currentSelectedGameObject != UIElement)
		{
			return;
		}

		// keep selecting the last thing the mouse has selected 
		//mouseSelection = null;
		//EventSystem.current.SetSelectedGameObject(currentSelection);
	}

    /// <summary>
    /// Method interactable UI elements should call on Submit interaction to determine whether to continue or not.
    /// </summary>
    /// <returns></returns>
    public bool AllowsSubmit()
    {

		// if LMB is not down, there is no edge case to handle, allow the event to continue
		return !_inputReader.LeftMouseDown();
               // if we know mouse & keyboard are on different elements, do not allow interaction to continue
               //|| mouseSelection != null && mouseSelection == currentSelection;
    }

    /// <summary>
    /// Fired by gamepad or keyboard navigation inputs
    /// </summary>
    /// <param name="UIElement"></param>
    public void UpdateSelection(GameObject UIElement)
	{
		if ((UIElement.GetComponent<MultiInputSelectableElement>() != null) || (UIElement.GetComponent<MultiInputButton>() != null))
		{
			//mouseSelection = UIElement;
			//currentSelection = UIElement;
		}

	}
	public void UpdateClick(GameObject UIElement)
	{
		BagItemUse_Btn.SetActive(true);
		buttonYes.onClick.AddListener(clicked);

		if ((UIElement.GetComponent<MultiInputSelectableElement>() != null) || (UIElement.GetComponent<MultiInputButton>() != null))
		{
			//mouseSelection = UIElement;
			//currentSelection = UIElement;
		}

	}


	// Debug
	// private void OnGUI()
	// {
	//	 	GUILayout.Box($"_currentSelection: {(_currentSelection != null ? _currentSelection.name : "null")}");
	//	 	GUILayout.Box($"_mouseSelection: {(_mouseSelection != null ? _mouseSelection.name : "null")}");
	// }
	private void Update()
	{
		//if ((EventSystem.current != null) && (EventSystem.current.currentSelectedGameObject == null) && (currentSelection != null))
		//{

		//	EventSystem.current.SetSelectedGameObject(currentSelection);
		//}


	}
}
