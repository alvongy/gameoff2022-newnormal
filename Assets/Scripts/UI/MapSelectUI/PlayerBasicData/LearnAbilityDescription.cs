using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YK.Game.Events;

public class LearnAbilityDescription : MonoBehaviour
{
    public GameObject LearnAbilityPrefab;
    public Transform ParentCount;

    public void ReadLearnAbility()
    {
        for (int i = ParentCount.childCount - 1; i >= 0; i--)
        {
            ObjectPool.Destroy(ParentCount.GetChild(i).gameObject);
        }
        //foreach (var aid in YKDataInfoManager.Instance.runtimeAbilitiesData.RuntimeAbilities)
        foreach (var aid in YKDataInfoManager.Instance.LearnedAbilityDataBase.database)
        {
            GameObject learnGo =ObjectPool.Instantiate(LearnAbilityPrefab, transform.position,Quaternion.identity,ParentCount);
            learnGo.GetComponent<LearnAbilityEneity>().Initialize(aid.Key);
        }
    }
   
}
