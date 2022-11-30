using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEffectBehaviour : StateMachineBehaviour
{
    public string effectName;
    [SerializeField] bool endWithAnim;//动画结束时关闭
    public EffectPlay effectPlay;
    [Range(0, 1)]
    [SerializeField] float startTime;//特效绑定时间点

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float animDuration= stateInfo.length;
        effectPlay.Play(startTime * animDuration);
        
        
        
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (endWithAnim)
        {
            effectPlay.Stop();
        }
       
    }

}
