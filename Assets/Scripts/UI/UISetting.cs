using UnityEngine;

public class UISetting : MonoBehaviour
{
	public void SetQuality(int n)
	{
		QualitySettings.SetQualityLevel(n);
	}
}
