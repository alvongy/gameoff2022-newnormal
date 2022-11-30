using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerAim : MonoBehaviour
{
    [SerializeField] public InputReader _inputReader = default;

    MFAbilityHandler handler;
    Vector2 AimingDirection;
    private void Awake()
    {
        _inputReader.LookEvent += OnLook;
        handler = GetComponent<MFAbilityHandler>();
    }

    private void Update()
    {
        WinOnAim();
    }

    public void OnLook(Vector2 vector)
    {
        if (vector != Vector2.zero)
        {
            MobileOnAim(vector);
        }
    }

    void MobileOnAim(Vector2 vector)
    {
#if UNITY_ANDROID || UNITY_IOS
        if (vector != Vector2.zero)
        {
            handler.SetHolderAimingDirection(vector);
        }
        else
        {
            handler.SetHolderAimingDirection(handler.transform.up);
        }
#endif
    }
    void WinOnAim()
    {
#if UNITY_STANDALONE_WIN || UNITY_STANDALONE || UNITY_WEBGL || UNITY_EDITOR_WIN
        AimingDirection = new Vector2(Mouse.current.position.ReadValue().x - Screen.width / 2, Mouse.current.position.ReadValue().y - Screen.height / 2);
        handler.SetHolderAimingDirection(AimingDirection);
#endif
    }
}
