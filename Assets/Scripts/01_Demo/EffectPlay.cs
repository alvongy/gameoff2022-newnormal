using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPlay : MonoBehaviour
{
    [SerializeField] float shutDownTime;//��Ч�Զ��ر�ʱ�䣬Ϊ0���ر�
    private List<GameObject> _effects;
    bool _isPlay=false;
    IEnumerator PlayEffect(float startTime)
    {
        _isPlay = true;
        yield return new WaitForSeconds(startTime);
        foreach (GameObject effect in _effects)
        {
            effect.SetActive(true);
        }
        if (shutDownTime != 0)
        {
            yield return new WaitForSeconds(shutDownTime);
            foreach (GameObject effect in _effects)
            {
                effect.SetActive(false);
            }
            _isPlay = false;
        }
       
       
    }
    /// <summary>
    /// ������Ч
    /// </summary>
    /// <param name="startTime">�ӳٿ�ʼʱ��</param>
    public void Play(float startTime)
    {
        if (!_isPlay)
        {
             StartCoroutine("PlayEffect", startTime);  
        }
        
    }
    //public void PlayLoop(float startTime)
    //{
    //    if (!_isPlay)
    //    {
    //        _isPlay = true;
    //        foreach (GameObject effect in _effects)
    //        {
    //            effect.SetActive(true);
    //        }

    //    }

    //}
    public void Stop()
    {
        foreach (GameObject effect in _effects)
        {
            effect.SetActive(false);
        }
        _isPlay = false;
    }

    private void Awake()
    {
        //������Ч
        _effects = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++)
        {
            _effects.Add(transform.GetChild(i).gameObject);
        }
        
        foreach (GameObject effect in _effects)
        {
            effect.SetActive(false);
        }
    }
 
}
