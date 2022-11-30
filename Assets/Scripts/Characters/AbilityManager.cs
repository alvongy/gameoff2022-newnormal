using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AbilityManager : MonoBehaviour
{
	//[SerializeField] private InputReader _inputReader;
	//[SerializeField] private MFAbilityDatabase _dataBase;
	//[SerializeField] private UnityEvent _unitAbilities;
	//[SerializeField] private UnityEvent _initAbilities;
	//[SerializeField] private UnityEvent _afterAbilitySelect;

	//[SerializeField] private IntArrayEventChannelSO _selectSkillChannelSO;
	//[SerializeField] private VoidEventChannelSO _levelUpChannelSO;
	//[SerializeField] private IntEventChannelSO _addSkillChannelSO;
	//public Dictionary<int, MFAbilityHolder> RuntimeAbilities = new Dictionary<int, MFAbilityHolder>();

	//void Start()
	//{
	//	_dataBase.Refresh();
	//	AddAbility(13);
	//}
	//private void OnEnable()
	//{
	//	_levelUpChannelSO.OnEventRaised += StartAbilitySelect;
	//	_addSkillChannelSO.OnEventRaised += AddAbility;
	//}
	//private void OnDisable()
	//{
	//	_levelUpChannelSO.OnEventRaised -= StartAbilitySelect;
	//	_addSkillChannelSO.OnEventRaised -= AddAbility;
	//}
	//public void StartAbilitySelect()
	//{
	//	List<int> rewardList = new List<int>();
	//	List<int> l = new List<int>();
	//	for (int i = 0, count = _dataBase.database.Count; i < 3; i++)
	//	{
	//		l.Add(Random.Range(0, count));
	//	}
	//	l.Sort();
	//	int k = 0;
	//	var enu = _dataBase.database.GetEnumerator();
	//	for (int i = 0; i < 3; i++)
	//	{
	//		for (; k < l[i]; k++)
	//		{
	//			enu.MoveNext();
	//		}
	//		rewardList.Add(enu.Current.Key);
	//	}

	//	_initAbilities.Invoke();

	//	_selectSkillChannelSO.RaiseEvent(rewardList.ToArray());
	//}
	//public void AddAbility(int id)
	//{

	//	if (_dataBase.database.ContainsKey(id))
	//	{
	//		Debug.Log("TAG：MFAbilityHandler——进入AddAbility方法，id：" + id);
	//		MFAbility ab = _dataBase.database[id];
	//		//叠加
	//		if (RuntimeAbilities.ContainsKey(id)) { RuntimeAbilities[id].AddSameAbilities(); }
	//		else
	//		{
	//			if (ab.AbType == MFAbility.AbilityType.TimeControl)
	//			{
	//				Debug.Log("TAG：MFAbilityHandler——进入添加时间控制的技能的方法AddAbility，ab：" + ab);
	//				var holder = AddAbilityHolder(ab);
	//				holder.Activate();

	//			}
	//			else if (ab.AbType == MFAbility.AbilityType.InputControl)
	//			{
	//				AddAbilityHolder(ab);
	//			}
	//			//一次性就不加入
	//			else if (ab.AbType == MFAbility.AbilityType.OneTimeActive)
	//			{
	//				ab.Activate(this.gameObject);
	//			}
	//			else if (ab.AbType == MFAbility.AbilityType.AnimationControl)
	//			{
	//				AddAbilityHolder(ab);
	//			}
	//			else if (ab.AbType == MFAbility.AbilityType.ActiveUpdate)
	//			{
	//				AddAbilityHolder(ab);
	//			}
	//		}
	//	}
	//	StopAbilitySelect();
	//}
	//MFAbilityHolder AddAbilityHolder(MFAbility ab)
	//{
	//	var go = new GameObject(ab.AID + "_" + ab.ablilityName, typeof(MFAbilityHolder));
	//	go.transform.parent = this.transform;
	//	go.transform.localPosition = Vector3.zero;
	//	go.transform.localRotation = Quaternion.identity;
	//	MFAbilityHolder holder = go.GetComponent<MFAbilityHolder>();
	//	holder.Init(ab, );
	//	RuntimeAbilities.Add(ab.AID, holder);
	//	Debug.Log("TAG：MFAbilityHandler——进入AddAbilityHolder方法，holder：" + holder);
	//	return holder.GetComponent<MFAbilityHolder>();
	//}
	//public void StopAbilitySelect()
	//{
	//	_unitAbilities.Invoke();
	//	_afterAbilitySelect.Invoke();
	//}
}
