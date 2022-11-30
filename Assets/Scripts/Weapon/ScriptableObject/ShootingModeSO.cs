using UnityEngine;

[CreateAssetMenu(fileName = "ShootingModeSO", menuName = "Weapon/ShootingMode")]
public class ShootingModeSO : DescriptionBaseSO
{
	public enum SelectiveFire
	{
		FULLY_AUTO,
		SEMI_AUTO,
		BURST,
	}
	public BulletSO bullet = default;
	public SelectiveFire selectiveFire = SelectiveFire.FULLY_AUTO;
	
	public int bulletsPerShoot = 1;
	public float shootingIdle = 0.3f;
	
	[Header("When using BURST mode")]
	public int shootsPerBurst = 1;
	public float burstIdle = 0.3f;

	[Space]
	public bool useAngelOffset = false;
	public float angelOffset = 5f;
}
