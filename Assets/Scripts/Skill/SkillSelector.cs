

using System.Collections;
using UnityEngine;

public class SkillSelector : MonoBehaviour
{
	[SerializeField] private MFAbilityDatabase _skillDataBase;
	[SerializeField] private GameObject _selectPanel;
	[SerializeField] private SkillCard[] _skillCards;

	[SerializeField] private IntArrayEventChannelSO _startSelectSkillChannel;
	[SerializeField] private IntEventChannelSO _addSkillChannel;
	[SerializeField, Range(0.1f, 10f)] private float _rotationSpeed = 1f;

	private int[] _skillIDs;
	private bool _selected;
	private void Awake()
	{
		_skillCards[0].OnPonitClickAction += () => EndSelect(0);
		_skillCards[1].OnPonitClickAction += () => EndSelect(1);
		_skillCards[2].OnPonitClickAction += () => EndSelect(2);
	}
	private void OnEnable()
	{
		_startSelectSkillChannel.OnEventRaised += StartSelect;
	}
	private void OnDisable()
	{
		_startSelectSkillChannel.OnEventRaised -= StartSelect;
	}
	private void OnDestroy()
	{
		StopAllCoroutines();
	}
	private void StartSelect(int[] arg0)
	{
		_selected = false;
		_selectPanel.SetActive(true);
		_skillIDs = arg0;
		for (int i = 0; i < 3; i++)
		{
			_skillCards[i].EnableClick = false;
			_skillCards[i].transform.rotation = Quaternion.Euler(0f, 180f, 0f);
			var skill = _skillDataBase.database[arg0[i]];
			//_skillCards[i].CardName = skill.ablilityName.GetLocalizedString();
			//_skillCards[i].CardDescription = skill.Description.GetLocalizedString();
			_skillCards[i].TextNameEvent.StringReference = skill.ablilityName;
			_skillCards[i].TextDesEvent.StringReference = skill.Description;
			//skill.ablilityName.GetLocalizedStringAsync().Completed += v =>
			//{
			//	_skillCards[i].CardName = v.Result;
			//};
			//skill.Description.GetLocalizedStringAsync().Completed += v =>
			//{
			//	_skillCards[i].CardDescription = v.Result;
			//};
			_skillCards[i].CardSprite = skill.Front;
		}
		StartCoroutine(CardRotation());
	}
	IEnumerator CardRotation()
	{
		float progress = 3f;

		while (progress > 0f)
		{
			_skillCards[0].transform.rotation = Quaternion.Euler(0f, Mathf.Clamp01((progress - 2f)) * 180f, 0f);
			_skillCards[1].transform.rotation = Quaternion.Euler(0f, Mathf.Clamp01((progress - 1f) / 2f) * 180f, 0f);
			_skillCards[2].transform.rotation = Quaternion.Euler(0f, Mathf.Clamp01(progress / 3f) * 180f, 0f);
			progress -= Time.unscaledDeltaTime * _rotationSpeed;
			yield return null;
		}
		for (int i = 0; i < 3; i++)
		{
			_skillCards[i].transform.rotation = Quaternion.Euler(0f, 0f, 0f);
			_skillCards[i].EnableClick = true;
		}
	}
	private void EndSelect(int n)
	{
		if (_selected)
		{
			return;
		}
		_selected = true;
		_selectPanel.SetActive(false);
		_addSkillChannel.RaiseEvent(_skillIDs[n]);
	}
}
