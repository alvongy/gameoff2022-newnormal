using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMessageText : MonoBehaviour
{
    [SerializeField] Text message_Text;

    public void Show(string msg)
    {
        message_Text.text = msg;
    }
}
