using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

[CreateAssetMenu(fileName = "InputReader", menuName = "Game/Input Reader")]
public class InputReader : DescriptionBaseSO, GameInput.IGameplayActions, GameInput.IMenuActions, GameInput.IDebugActions
{
	[SerializeField] private GameStateSO _gameStateManager;

	public event UnityAction<bool> AttackMainEvent = delegate { };
	public event UnityAction<bool> AttackSubEvent = delegate { };
	public event UnityAction<bool> SidestepEvent = delegate { };

	public event UnityAction<Vector2> MoveEvent = delegate { };
	public event UnityAction<Vector2> LookEvent = delegate { };
	public event UnityAction MonseLeftDownEvent = delegate { };
	public event UnityAction MonseLeftHoldEvent = delegate { };
	public event UnityAction MonseLeftUpEvent = delegate { };
	public event UnityAction MonseRightDownEvent = delegate { };
	public event UnityAction MonseRightUpEvent = delegate { };
	public event UnityAction SpaceDownEvent = delegate { };
	
	public event UnityAction InteractEvent = delegate { };
	public event UnityAction ReloadEvent = delegate { };
	public event UnityAction WeaponFirst = delegate { };
	public event UnityAction WeaponSecond = delegate { };
	public event UnityAction<bool> LoopSwitchWeapon = delegate { };

	public event UnityAction CloseMenuEvent = delegate { };

	public event UnityAction<InputAction.CallbackContext> DebugEventZ = delegate { };
	public event UnityAction<InputAction.CallbackContext> DebugEventX = delegate { };
	public event UnityAction<InputAction.CallbackContext> DebugEventC = delegate { };
	public event UnityAction<InputAction.CallbackContext> DebugEventV = delegate { };
	public event UnityAction<InputAction.CallbackContext> DebugEventLeft = delegate { };
	public event UnityAction<InputAction.CallbackContext> DebugEventRight = delegate { };


	// Menus
	public event UnityAction menuMouseMoveEvent = delegate { };
	// Shared between menus and dialogues
	public event UnityAction moveSelectionEvent = delegate { };

	public event UnityAction<float> TabSwitched = delegate { };

	GameInput _gameInput;
	private void OnEnable()
	{
		if (_gameInput == null)
		{
			_gameInput = new GameInput();
			_gameInput.Gameplay.SetCallbacks(this);
			_gameInput.Menu.SetCallbacks(this);
			_gameInput.Gameplay.Enable();


			_gameInput.Debug.SetCallbacks(this);
			_gameInput.Debug.Enable();

        }
	}
	private void OnDisable()
	{
		DisableAllInput();
	}

	public void EnableGameplayInput()
	{
		_gameInput.Gameplay.Enable();
		_gameInput.Menu.Disable();
	}
	public void EnableMenuInput()
	{
		_gameInput.Gameplay.Disable();
		_gameInput.Menu.Enable();
	}
	public void DisableAllInput()
	{
		_gameInput.Gameplay.Disable();
		_gameInput.Menu.Disable();
	}
	public void OnMove(InputAction.CallbackContext context)
	{
		MoveEvent.Invoke(context.ReadValue<Vector2>());
	}

	public void OnLook(InputAction.CallbackContext context)
	{
		LookEvent.Invoke(context.ReadValue<Vector2>());
	}

	public void OnSidestep(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Performed)
		{
			SidestepEvent.Invoke(true);
		}
		else if (context.phase == InputActionPhase.Canceled)
		{
			SidestepEvent.Invoke(false);
		}
	}

	public void OnInteraction(InputAction.CallbackContext context)
	{
		if ((context.phase == InputActionPhase.Performed) && (_gameStateManager.CurrentGameState == GameState.Gameplay))
		{
			InteractEvent.Invoke();
		}
	}

	public void OnAttackMain(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Performed)
		{
            if (context.interaction is TapInteraction)
            {
				MonseLeftDownEvent.Invoke();
			}
			else if(context.interaction is MultiTapInteraction)
            {
				//Ë«»÷
            }
            else if(context.interaction is HoldInteraction)
            {
				//³¤°´
				MonseLeftHoldEvent.Invoke();
			}
		}
		else if (context.phase == InputActionPhase.Canceled)
		{
			MonseLeftUpEvent.Invoke();
		}
	}

	public void OnAttackSub(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Performed)
		{
			MonseRightDownEvent.Invoke();
		}
		else if (context.phase == InputActionPhase.Canceled)
		{
			MonseRightUpEvent.Invoke();
		}
	}

	public void OnReload(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Performed)
		{
			ReloadEvent.Invoke();
		}
	}

	public void OnWeaponFirst(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Performed)
		{
			WeaponFirst.Invoke();
		}
	}

	public void OnWeaponSecond(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Performed)
		{
			WeaponSecond.Invoke();
		}
	}

	public void OnLoopSwitchWeapon(InputAction.CallbackContext context)
	{
		if (context.ReadValue<Vector2>().y > 0)
		{
			LoopSwitchWeapon.Invoke(true);
		}
		else
		{
			LoopSwitchWeapon.Invoke(false);
		}
	}

	public void OnUseSkill1(InputAction.CallbackContext context)
	{

	}

	public void OnUseSkill2(InputAction.CallbackContext context)
	{

	}

	public void OnUseSkill3(InputAction.CallbackContext context)
	{

	}

	public void OnPause(InputAction.CallbackContext context)
	{

	}

	public void OnCloseMenu(InputAction.CallbackContext context)
	{
		CloseMenuEvent.Invoke();
	}

	public void OnDebug1(InputAction.CallbackContext context)
	{
		DebugEventZ.Invoke(context);
	}

	public void OnDebug2(InputAction.CallbackContext context)
	{
		DebugEventX.Invoke(context);
	}

	public void OnDebug3(InputAction.CallbackContext context)
	{
		DebugEventC.Invoke(context);
	}

	public void OnDebug4(InputAction.CallbackContext context)
	{
		DebugEventV.Invoke(context);
	}

	public void OnDebug5(InputAction.CallbackContext context)
	{
		DebugEventLeft.Invoke(context);
	}

	public void OnDebug6(InputAction.CallbackContext context)
	{
		DebugEventRight.Invoke(context);
	}
	public bool LeftMouseDown() => Mouse.current.leftButton.isPressed;

}
