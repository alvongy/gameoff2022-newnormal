using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugSystem : MonoBehaviour
{
	[SerializeField] private GameObject panel;
	[SerializeField] private TransformEventChannelSO _playerReadyEvent;
	[SerializeField] private InputReader _inputReader;
	[SerializeField] private MFAbilityDatabase _database;
	[SerializeField] private TMP_Dropdown _dropdown;
	bool _inShowing = false;

	MainCharacter _character;
	Damageable _damageable;
	MFAbilityHandler _abilityHandler;
	Dictionary<string, int> _abilities = new Dictionary<string, int>();
	string _dropDwonSkill;
	private void Awake()
	{
		_playerReadyEvent.OnEventRaised += ResetPlayer;
	}
	private void OnDestroy()
	{
		_playerReadyEvent.OnEventRaised -= ResetPlayer;
	}
	private void OnEnable()
	{
		ResetDropDown();
		_dropdown.onValueChanged.AddListener(SetDropDownItem);
		_inputReader.DebugEventZ += SwitchPanel;
	}
	private void OnDisable()
	{
		_dropdown.onValueChanged.RemoveAllListeners();
		_inputReader.DebugEventZ -= SwitchPanel;
	}
	private void SetDropDownItem(int arg0)
	{
		_dropDwonSkill = _dropdown.options[arg0].text;
	}
	private void SwitchPanel(UnityEngine.InputSystem.InputAction.CallbackContext arg0)
	{
		if (arg0.phase == UnityEngine.InputSystem.InputActionPhase.Performed)
		{
			panel.SetActive(_inShowing);
			_inShowing = !_inShowing;
		}
	}

	private void ResetPlayer(Transform arg0)
	{
		_character = arg0.GetComponent<MainCharacter>();
		_damageable = arg0.GetComponent<Damageable>();
		_abilityHandler = arg0.GetComponent<MFAbilityHandler>();
	}
	private void ResetDropDown()
	{
		_dropdown.ClearOptions();
		//_dropdown.AddOptions()
		List<TMP_Dropdown.OptionData> opts = new List<TMP_Dropdown.OptionData>();
		_abilities.Clear();
		foreach (var item in _database.database)
		{
			_abilities.Add(item.Value.AID.ToString(), item.Key);
			//opts.Add(new TMP_Dropdown.OptionData(item.Value.ablilityName.GetLocalizedString()));
			opts.Add(new TMP_Dropdown.OptionData(item.Value.AID.ToString()));

		}
		_dropdown.AddOptions(opts);
		_dropDwonSkill = _dropdown.options[0].text;
	}

	public void DisableSelectCard(bool b)
	{
		_character.DisableSelectCord = b;
	}

	public void HyperMuteki(bool b)
	{
		_damageable.HyperMuteki = b;
	}
	public void ClearSkill()
	{
		_abilityHandler.MFClearAbility();
	}
	public void AddDropDownSkill()
	{
		if (_abilities.ContainsKey(_dropDwonSkill))
		{
			_abilityHandler.AddAbility(_abilities[_dropDwonSkill]);
		}
	}
}
