using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class UIButtonInInteractionListener : MonoBehaviour
{

   public InteractionUIEventChannelSO _ChannelSO;
    [SerializeField] private VoidEventChannelSO _onInteractionEnded = default;
    public InteractionType type;
    public UnityEvent OnCalledOpen;
    public UnityEvent OnCalledClose;
    private void Start()
    {
        _ChannelSO.OnEventRaised += CAll;
        _onInteractionEnded.OnEventRaised += InteractEnd;


    }
    void InteractEnd() 
    {
        CAll(false, InteractionType.NONE);
    }
    void CAll(bool onoff, InteractionType interactionType) 
    {
        //Debug.Log(onoff);
        //Debug.Log(interactionType);
        if (interactionType == InteractionType.NONE) { OnCalledClose?.Invoke();return; }

        if (type==interactionType) 
        {
            if (onoff) { OnCalledOpen?.Invoke(); }
            else { OnCalledClose?.Invoke(); }
        }
        if (!onoff) { OnCalledClose?.Invoke(); }
    }
}
