using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourContorller : MonoBehaviour
{
    [SerializeField] Animator Animator;
    [SerializeField] EffectContorller Effect;
    [SerializeField] HitBoxContorller HitBox;
    public void PlayEffect(int index,Vector2 range, bool endloop ,ParticleSystemStopBehavior pssb)
    {

        Effect.Play(index, range,endloop,pssb);

    }
    public void StopEffect(int index, ParticleSystemStopBehavior pssb)
    {
        Effect.Stop(index, pssb);
    }
    public void PlayHitBox(int index, Vector2 range)
    {
        HitBox.Play(index, range);
    }
    void Awake()
    {
        //°ó¶¨¿ØÖÆÆ÷
        AnimStateBehaviour[] animStateBehaviours = Animator.GetBehaviours<AnimStateBehaviour>();
        for (int i = 0; i < animStateBehaviours.Length; i++)
        {
            animStateBehaviours[i].BehaviourContorller = this;
        }

    }
}
