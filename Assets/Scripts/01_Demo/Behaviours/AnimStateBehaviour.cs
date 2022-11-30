using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimStateBehaviour : StateMachineBehaviour
{
    private BehaviourContorller _behaviourContorller;
    [TableList]
    public List<EffectBehaviourDateBind> effectStateBehaviourList = new List<EffectBehaviourDateBind>();
    [TableList]
    public List<HitBoxBehaviourDateBind> hitBoxStateBehaviourList = new List<HitBoxBehaviourDateBind>();

    public BehaviourContorller BehaviourContorller { set => _behaviourContorller = value; }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float animDuration = stateInfo.length;
        for (int i = 0; i < effectStateBehaviourList.Count; i++)
        {
            EffectBehaviourDateBind ebdb = effectStateBehaviourList[i];
            _behaviourContorller.PlayEffect(ebdb.behaviourIndex, ebdb.DynamicRange * animDuration, ebdb.EndLoop, ebdb.StopBehavior);
        }
        for (int i = 0; i < hitBoxStateBehaviourList.Count; i++)
        {
            _behaviourContorller.PlayHitBox(hitBoxStateBehaviourList[i].behaviourIndex, hitBoxStateBehaviourList[i].DynamicRange * animDuration);
        }
        //effectPlay.Play(startTime * animDuration);
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        for (int i = 0; i < effectStateBehaviourList.Count; i++)
        {
            if (effectStateBehaviourList[i].EndLoop)
            {
                EffectBehaviourDateBind ebdb = effectStateBehaviourList[i];
                _behaviourContorller.StopEffect(ebdb.behaviourIndex, ebdb.StopBehavior);
            }

        }
    }
    [System.Serializable]
    public class EffectBehaviourDateBind : BehaviourDateBind
    {
        public bool EndLoop;
        public ParticleSystemStopBehavior StopBehavior;
    }
    [System.Serializable]
    public class HitBoxBehaviourDateBind : BehaviourDateBind
    {
    }
    [System.Serializable]
    public class BehaviourDateBind
    {

        [TableColumnWidth(40)]
        [MinMaxSlider(0, 1, true)]
        public Vector2 DynamicRange = new Vector2(0, 1);
        public int behaviourIndex;
    }
}
