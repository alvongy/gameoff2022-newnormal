using UnityEngine;
using UnityEngine.UI;
using YK.Game.Events;

public class HPBar : MonoBehaviour
{
	[SerializeField] private Image _image;
	[SerializeField] private PlayerAttributesSO _healthSO;
	[SerializeField] private Text _text;
	[SerializeField] private GameObject _hpGridObj;

	private void Awake()
	{
		_healthSO.OnValueChanged += ResetHPBar;
	}
	private void Start()
	{
		ResetHPBar();
	}
	private void OnDestroy()
	{
		_healthSO.OnValueChanged -= ResetHPBar;
	}
	private void ResetHPBar()
	{
		_image.fillAmount = Mathf.Clamp01(1f * _healthSO.CurrentValue / _healthSO.MaxValue);

        if (_text != null) { _text.text = _healthSO.CurrentValue.ToString() + "/" + _healthSO.MaxValue.ToString(); }
		if (_hpGridObj != null) { HPGrid(); }
	}
	private void HPGrid()
    {
        for (int i= transform.childCount-1; i>=0;i--)
        {
			ObjectPool.Destroy(transform.GetChild(i).gameObject);
        }
		for (int i = 0; i < _healthSO.CurrentValue; i++)
		{
			ObjectPool.Instantiate(_hpGridObj,transform.position,Quaternion.identity,transform);
		}
	}

}
