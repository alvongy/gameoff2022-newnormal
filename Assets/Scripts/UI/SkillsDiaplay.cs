using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillsDiaplay : MonoBehaviour
{
	[SerializeField] private MFAbilityDatabase _database;
	[SerializeField] private Image[] _skillImages;
	[SerializeField] private TextMeshProUGUI[] _skillTexts;
	[SerializeField] private IntArrayEventChannelSO _refreshSkillEventChannelSO;
	[SerializeField] private Sprite _defaultSprite;
	class Slot
	{
		public bool InUsing;
		public int SkillID;
		public int SkillLevel;
		public Image Image;
		public TextMeshProUGUI Text;
	}
	Stack<Slot> _slotsPool = new Stack<Slot>();
	Dictionary<int, Slot> _slotsInUsing = new Dictionary<int, Slot>();
	private void OnEnable()
	{
		_refreshSkillEventChannelSO.OnEventRaised += RefreshSkills;
		_slotsPool.Clear();
		for (int i = _skillImages.Length - 1; i >= 0; i--)
		{
			_skillImages[i].sprite = _defaultSprite;
			_skillTexts[i].gameObject.SetActive(false);
			_slotsPool.Push(new Slot { Image = _skillImages[i], Text = _skillTexts[i] });
		}
	}
	private void OnDisable()
	{
		_refreshSkillEventChannelSO.OnEventRaised -= RefreshSkills;
	}

	private void RefreshSkills(int[] arg0)
	{
		int length = Mathf.Min(arg0.Length / 2, 6);
		IList<int> skillIds = new int[length];
		IList<int> skillLevels = new int[length];

		for (int i = 0; i < length; i++)
		{
			skillIds[i] = arg0[2 * i];
			skillLevels[i] = arg0[2 * i + 1];
		}

		List<int> temp = new List<int>();
		foreach (var slot in _slotsInUsing.Keys)
		{
			if (!skillIds.Contains(slot))
			{
				_slotsInUsing[slot].InUsing = false;
				_slotsInUsing[slot].Image.sprite = _defaultSprite;
				_slotsInUsing[slot].Text.gameObject.SetActive(false);
				temp.Add(slot);
			}
		}
		foreach (var ids in temp)
		{
			_slotsPool.Push(_slotsInUsing[ids]);
			_slotsInUsing.Remove(ids);
		}
		for (int i = 0; i < skillIds.Count; i++)
		{
			var id = skillIds[i];
			var lv = skillLevels[i];
			if (_slotsInUsing.TryGetValue(id, out Slot slot))
			{
				if (slot.SkillLevel != lv)
				{
					slot.SkillLevel = lv;
					slot.Text.SetText(lv.ToString());
					if (lv > 0)
					{
						slot.Text.gameObject.SetActive(true);
					}
				}
			}
			else
			{
				Slot newSlot = _slotsPool.Pop();
				_slotsInUsing.Add(id, newSlot);
				newSlot.InUsing = true;
				newSlot.SkillID = id;
				newSlot.SkillLevel = lv;
				newSlot.Image.sprite = _database.database[id].Front;
				newSlot.Text.SetText(lv.ToString());
				if (lv > 0)
				{
					newSlot.Text.gameObject.SetActive(true);
				}
			}
		}
	}
}
