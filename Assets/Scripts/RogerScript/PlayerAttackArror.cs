using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackArror : MonoBehaviour
{
    [SerializeField] public InputReader _inputReader = default;
    public Transform _arrow;
    float offsetY;
    float offsetX;

    private void OnEnable()
    {
        _inputReader.LookEvent += OnLook;
    }
    private void OnDisable()
    {
        _inputReader.LookEvent -= OnLook;
    }
    void Update()
    {
#if UNITY_EDITOR_WIN || UNITY_WEBGL
        WorldToScreenPos(Mouse.current.position.ReadValue());
#endif
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
            WorldToScreenPos(vector);
            //WorldToScreenPos(vector * Touchscreen.current.position.ReadValue());//尝试用触摸标记方向
        }
        else
        {
            WorldToScreenPos(transform.up);
        }
#endif
    } 

    void WorldToScreenPos(Vector3 dir)
    {
#if UNITY_EDITOR_WIN  || UNITY_WEBGL
        Vector2 direction = new Vector2(dir.x - Screen.width / 2, dir.y - Screen.height / 2);
        _arrow.transform.LookAt(direction);
        Vector2 arrowPos = Camera.main.ViewportToScreenPoint(_arrow.transform.position);
        Vector2 mousePos = Camera.main.ViewportToScreenPoint(dir);
        offsetY = mousePos.y - arrowPos.y;
        offsetX = mousePos.x - arrowPos.x;
#endif
#if UNITY_ANDROID || UNITY_IOS
        offsetY = dir.y;
        offsetX = dir.x;
#endif
        float jianTouRotationZ = 0f;
        if (offsetX >= 0)
        {
            if (offsetX == 0)
            {
                if (offsetY > 0)
                {
                    _arrow.transform.rotation = Quaternion.Euler(0f, 0f, 360f + 90f);
                }
                else if (offsetY < 0)
                {
                    _arrow.transform.rotation = Quaternion.Euler(0f, 0f, 360f - 90f);
                }
                else
                {
                    _arrow.transform.rotation = Quaternion.Euler(0f, 0f, 360f + 90f);
                }
            }
            else
            {
                if (offsetY > 0)
                {
                    jianTouRotationZ = Mathf.Atan2(Mathf.Abs(offsetY), Mathf.Abs(offsetX)) * 180f / Mathf.PI;
                    _arrow.transform.rotation = Quaternion.Euler(0f, 0f, 360f + jianTouRotationZ);
                }
                else if (offsetY < 0)
                {
                    jianTouRotationZ = Mathf.Atan2(Mathf.Abs(offsetY), Mathf.Abs(offsetX)) * 180f / Mathf.PI;
                    _arrow.transform.rotation = Quaternion.Euler(0f, 0f, 360f - jianTouRotationZ);
                }
                else
                {
                    _arrow.transform.rotation = Quaternion.Euler(0f, 0f, 360f);
                }
            }
        }
        else
        {
            if (offsetY > 0)
            {
                jianTouRotationZ = Mathf.Atan2(Mathf.Abs(offsetY), Mathf.Abs(offsetX)) * 180f / Mathf.PI;
                _arrow.transform.rotation = Quaternion.Euler(0f, 0f, 360f + 180f - jianTouRotationZ);
            }
            else if (offsetY < 0)
            {
                jianTouRotationZ = Mathf.Atan2(Mathf.Abs(offsetY), Mathf.Abs(offsetX)) * 180f / Mathf.PI;
                _arrow.transform.rotation = Quaternion.Euler(0f, 0f, 360f + 180f + jianTouRotationZ);
            }
            else
            {
                _arrow.transform.rotation = Quaternion.Euler(0f, 0f, 360f + 180f);
            }
        }
    }
}
