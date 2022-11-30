using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectContorller : MonoBehaviour
{
    [SerializeField] List<ParticleSystem> Effects;
    [SerializeField] bool PlayOnLoad = false;
    void Awake()
    {
        if (!PlayOnLoad)
        {
            foreach (ParticleSystem particle in Effects)
            {
                particle.Stop();
            }
        }
    }
    IEnumerator PlayEffect(int index, Vector2 range, bool endloop, ParticleSystemStopBehavior pssb)
    {
        yield return new WaitForSeconds(range.x);
        Effects[index].Play();
        if (!endloop)
        {
            yield return new WaitForSeconds(range.y);
            Stop(index, pssb);
            //Effects[index].Stop();
        }

    }


    public void Play(int index,Vector2 range,bool endloop, ParticleSystemStopBehavior pssb)
    {
        if (index>=0&&index<Effects.Count)
        {
            StartCoroutine(PlayEffect(index, range, endloop,pssb));
        }
        else
        {
            Debug.LogError("Out of range");
        }

    }
    public void Stop(int index, ParticleSystemStopBehavior pssb)
    {
        Effects[index].Stop(true, pssb);
    }
}
