using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISurvivalDifficulty : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Text _difficulty_Text;
    [SerializeField] private Transform _difficultyDescription;

    public void Init()
    {
        EvolutionManager.Instance.OnLearendEvolutionChange += RefreshEvolutionTerm;
        _difficulty_Text.text = EvolutionManager.Instance.survivalDifficulty.ToString();
    }
    private void RefreshEvolutionTerm()
    {
        _difficulty_Text.text = EvolutionManager.Instance.survivalDifficulty.ToString();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _difficultyDescription.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _difficultyDescription.gameObject.SetActive(false);
    }


    private void ShowDifficultyDescription()
    {

        if (EvolutionManager.Instance.survivalDifficulty >= 16)
        {
            _difficultyDescription.GetChild(0).GetComponent<Image>().color = Color.gray;
            _difficultyDescription.GetChild(1).GetComponent<Image>().color = Color.gray;
            _difficultyDescription.GetChild(2).GetComponent<Image>().color = Color.gray;
            _difficultyDescription.GetChild(3).GetComponent<Image>().color = Color.gray;
            _difficultyDescription.GetChild(4).GetComponent<Image>().color = Color.gray;
        }
        else if (EvolutionManager.Instance.survivalDifficulty >= 11)
        {
            _difficultyDescription.GetChild(0).GetComponent<Image>().color = Color.gray;
            _difficultyDescription.GetChild(1).GetComponent<Image>().color = Color.gray;
            _difficultyDescription.GetChild(2).GetComponent<Image>().color = Color.gray;
            _difficultyDescription.GetChild(3).GetComponent<Image>().color = Color.gray;
        }
    }
}
