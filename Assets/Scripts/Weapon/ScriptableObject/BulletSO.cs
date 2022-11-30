using UnityEngine;


[CreateAssetMenu(fileName = "BulletSO", menuName = "Weapon/Ammo")]
public class BulletSO : ItemSO
{
	[SerializeField] float bulletSpeed=5f;

	public float BulletSpeed { get => bulletSpeed; set => bulletSpeed = value; }
}
