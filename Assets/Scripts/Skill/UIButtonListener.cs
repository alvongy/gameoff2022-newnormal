using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class UIButtonListener : MonoBehaviour
{

   public VoidEventChannelSO _ChannelSO;

    public UnityEvent OnCalled;
    private void Start()
    {
        _ChannelSO.OnEventRaised += CAll;

 
    }
    
    void CAll() 
    {
        OnCalled?.Invoke();
    }
}
