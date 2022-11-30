using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleAbilityManager : SerializedMonoSingleton<RoleAbilityManager>
{
    //public MFAbilityDatabase DataBase;
    public Dictionary<int, BSAbilityHolder> RuntimeAbilities = new Dictionary<int, BSAbilityHolder>();

	public BSAbilityHolder AddAbilityHolder(MFAbility ab)
	{
		var go = new GameObject(ab.AID + "_" + ab.ablilityName.GetLocalizedString(), typeof(BSAbilityHolder));
		go.transform.parent = this.transform;
		go.transform.localPosition = Vector3.zero;
		go.transform.localRotation = Quaternion.identity;
		BSAbilityHolder holder = go.GetComponent<BSAbilityHolder>();
		//holder.Init(ab);

		RuntimeAbilities.Add(ab.AID, holder);
		return holder;
	}
}
