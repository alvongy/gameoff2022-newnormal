using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIButtonListenerCD : MonoBehaviour
{

    public VoidEventChannelSO _ChannelSO;
    public PlayerAttributesFloatSO TimerSO;
    public Image Fill;
    float counter;
    float total;
    private void Start()
    {
        total=  TimerSO.MaxValue;
        _ChannelSO.OnEventRaised += CAll;
        counter = total;
 
    }
    
    private void Update()
    {
        if (counter< total) 
        {
            counter += Time.deltaTime;
            Fill.fillAmount = counter /(total);
            if (counter>=total) { GetComponent<Button>().enabled = true; Fill.fillAmount = 1; }
        }
    }
    private void OnDestroy()
    {
        _ChannelSO.OnEventRaised -= CAll;
    }
    void CAll() 
    {
        counter = 0;
        GetComponent<Button>().enabled=false;
        Fill.fillAmount = 0;
    }
}
