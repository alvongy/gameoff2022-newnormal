using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine;
using Sirenix.OdinInspector;

public class EvolutionTermCardEntity : MonoBehaviour, IPointerDownHandler,IPointerEnterHandler,IPointerExitHandler
{
	[SerializeField] private EvolutionTermSO evolutionTerm;
	[SerializeField] private Text evolutionName;
	[SerializeField] private Text evolutionDescription;
	[SerializeField] private Text evolutionDifficulty;
	[SerializeField] private Image evolutionImage;
	[SerializeField] private Image evolutionFrame;
	[SerializeField] private Image backGroundColor;

	public bool IsLearend => isLearend;
	[SerializeField] private bool isLearend;
	[ShowIf("@(this.isLearend)==true")]
	[SerializeField] private Transform evolutionTermDescription;
	[ShowIf("@(this.isLearend)==true")]
	[SerializeField] private Image evolutionImageColor;

	public void EvolutionTermCardInit(EvolutionTermSO evolution)
	{
		evolutionTerm = evolution;
		//evolutionName.text = evolution.Name.GetLocalizedString();
		evolutionDescription.text = evolution.Description.GetLocalizedString();
		evolutionImage.sprite = evolution.Icon;
		evolutionFrame.color = CaptureManager.Instance.QualityColorDic[evolution.evolutionQuality];
		backGroundColor.color = CaptureManager.Instance.EvolutionTypeColorDic[evolution.evolutionType];
		transform.GetComponent<Image>().color = CaptureManager.Instance.EvolutionTypeColorDic[evolution.evolutionType];

        if (evolution.evolutionType== EvolutionTypeEnum.MALIGNANCY) 
		{
			evolutionDifficulty.text = (1 + evolution.evolutionQuality.GetHashCode()).ToString();
			evolutionDifficulty.transform.parent.gameObject.SetActive(true);
			evolutionDifficulty.gameObject.SetActive(true);
        }
        else
        {
			evolutionDifficulty.gameObject.SetActive(false);
			evolutionDifficulty.transform.parent.gameObject.SetActive(false);
		}
		
		if (isLearend)
        {
			evolutionImageColor.color = CaptureManager.Instance.EvolutionTypeColorDic[evolution.evolutionType];
			transform.parent.GetComponent<Image>().color = CaptureManager.Instance.QualityColorDic[evolution.evolutionQuality];

		}
	}
	public void EvolutionTermCardConceal()
    {
		transform.GetChild(0).gameObject.SetActive(false);
		if (evolutionTermDescription != null)
        {
			evolutionTermDescription.gameObject.SetActive(false);
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
        if (isLearend) { return; }

		EvolutionManager.Instance.currentClickEvolutionTerm = evolutionTerm;
		for (int i = 0; i < transform.parent.childCount; i++)
		{
			transform.parent.GetChild(i).GetComponent<EvolutionTermCardEntity>().EvolutionTermCardConceal();
			transform.GetChild(0).gameObject.SetActive(true);
		}
	}
	public void OnPointerDown()
    {
		if (isLearend) { return; }

		EvolutionManager.Instance.currentClickEvolutionTerm = evolutionTerm;
		for (int i = 0; i < transform.parent.childCount; i++)
		{
			transform.parent.GetChild(i).GetComponent<EvolutionTermCardEntity>().EvolutionTermCardConceal();
			transform.GetChild(0).gameObject.SetActive(true);
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
    {
		if (!isLearend) { return; }
		transform.GetChild(0).gameObject.SetActive(true);
		evolutionTermDescription.gameObject.SetActive(true);
		//for (int i = 0; i < transform.parent.parent.childCount; i++)
		//{
		//	if (transform.parent.parent.GetChild(i).childCount > 0)
		//	{
		//		transform.parent.parent.GetChild(i).GetChild(0).GetComponent<EvolutionTermCardEntity>().EvolutionTermCardConceal();
		//	}
		//	transform.GetChild(0).gameObject.SetActive(true);
		//}
		//if (transform.GetChild(0).gameObject.activeInHierarchy)
		//{
		//	evolutionTermDescription.gameObject.SetActive(true);
		//}
	}

    public void OnPointerExit(PointerEventData eventData)
    {
		if (!isLearend) { return; }
		transform.GetChild(0).gameObject.SetActive(false);
		evolutionTermDescription.gameObject.SetActive(false);
		//for (int i = 0; i < transform.parent.parent.childCount; i++)
		//{
		//	if (transform.parent.parent.GetChild(i).childCount > 0)
		//	{
		//		transform.parent.parent.GetChild(i).GetChild(0).GetComponent<EvolutionTermCardEntity>().EvolutionTermCardConceal();
		//	}
		//	transform.GetChild(0).gameObject.SetActive(true);
		//}
		//if (transform.GetChild(0).gameObject.activeInHierarchy)
		//{
		//	evolutionTermDescription.gameObject.SetActive(true);
		//}
	}
}
