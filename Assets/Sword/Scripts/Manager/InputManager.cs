using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class InputManager : SerializedMonoSingleton<InputManager>, GameInput.IGameplayActions, GameInput.IMenuActions
{
    private GameInput gameInput;

    public event UnityAction<Vector2> MoveEvent = delegate { };
    public event UnityAction<Vector2> LookEvent = delegate { };
    public event UnityAction MonseLeftDownEvent = delegate { };
    public event UnityAction MonseLeftHoldEvent = delegate { };
    public event UnityAction MonseLeftUpEvent = delegate { };
    public event UnityAction MonseRightDownEvent = delegate { };
    public event UnityAction MonseRightUpEvent = delegate { };
    public event UnityAction SpaceDownEvent = delegate { };
	public event UnityAction InteractDownEvent = delegate { };
	public event UnityAction InteractUpEvent = delegate { };
	public event UnityAction TabDownEvent = delegate { };
	public event UnityAction TabUpEvent = delegate { };
	public event UnityAction ReloadEvent = delegate { };

	public event UnityAction TabEvent = delegate { };

	protected override void OnAwake()
    {
        gameInput = new GameInput();
        gameInput.Gameplay.SetCallbacks(this);
        gameInput.Menu.SetCallbacks(this);

        gameInput.Gameplay.Enable();
        gameInput.Menu.Enable();
    }

	public void GameplayDisable()
    {
		gameInput.Gameplay.Disable();
	}

	public void GameplayEnable()
    {
		gameInput.Gameplay.Enable();
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
			SpaceDownEvent.Invoke();
		}
		else if (context.phase == InputActionPhase.Canceled)
		{
			
		}
	}

	public void OnTab(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Performed)
		{
			if (context.interaction is HoldInteraction)
			{

			}
			else
			{
				TabDownEvent.Invoke();
			}
		}
		else if (context.phase == InputActionPhase.Canceled)
		{
			TabUpEvent.Invoke();
		}
	}
	
	public void OnInteraction(InputAction.CallbackContext context)//E
	{
		if (context.phase == InputActionPhase.Performed)
		{
			if (context.interaction is HoldInteraction) 
			{ 

			}
            else
            {
				InteractDownEvent.Invoke();
			}
		}
		else if (context.phase == InputActionPhase.Canceled)
		{
			InteractUpEvent.Invoke();
		}
	}

	public void OnAttackMain(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Performed)
		{
			if (context.interaction is HoldInteraction)
			{
				MonseLeftHoldEvent.Invoke();
			}
            else
            {
				MonseLeftDownEvent.Invoke();
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
		//if (context.phase == InputActionPhase.Performed)
		//{
		//	WeaponFirst.Invoke();
		//}
	}

	public void OnWeaponSecond(InputAction.CallbackContext context)
	{
		//if (context.phase == InputActionPhase.Performed)
		//{
		//	WeaponSecond.Invoke();
		//}
	}

	public void OnLoopSwitchWeapon(InputAction.CallbackContext context)
	{
		//if (context.ReadValue<Vector2>().y > 0)
		//{
		//	LoopSwitchWeapon.Invoke(true);
		//}
		//else
		//{
		//	LoopSwitchWeapon.Invoke(false);
		//}
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
		if (context.phase == InputActionPhase.Performed)
        {
			TabEvent.Invoke();
		}
	}
}
