using ArtSetting;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using YK.Game.Events;
using static EnemyWaveSO;
using Random = UnityEngine.Random;

/*
public class WaveSpawner_SwordOld : MonoBehaviour
{
	
	public UnityAction<float> OnTimerChange;
	public UnityAction<int> OnWaveBegin;
	public UnityEvent OnWaveEnd;

	[Title("���ﲨ��SO")]
	public EnemyWaveSO _enemyWaveSO;
	[FoldoutGroup("Bool"), InfoBox("�Ƿ�ʹ��ȫ�����ݼ�¼ˢ��")][HideInInspector]
	public bool isGlobaleSpawner = false;
	[FoldoutGroup("Bool"), InfoBox("�Ƿ�ʼ����ˢ��")][HideInInspector]
	public bool isStartRefreshEnemy = false;
	[FoldoutGroup("Bool"), InfoBox("�Ƿ񽫴˴�ˢ�ּ�¼��ȫ��")][HideInInspector]
	public bool isRefreshEnemyRecord = false;

	[InfoBox("�ؿ�����ˢ����Ϲ㲥��ʶ")]
	public bool isLevelIdentification;
	[InfoBox("�Ƿ�ѭ��ˢ�֣�ѭ�����ʱ�䣩")]
	public bool loop;
	[ShowIf("@(this.loop)==true")]
	public float durationBetweenLoop = 10f;

	[InfoBox("����ˢ�µ�ÿһ�����")]
	public float durationBetweenWaves = 5f;

	int LoopIndex = 0;
	bool free;//ˢ��ȫ������֮������Ϊtrue
	int waveIndex = 0;
	float timer;//����ˢ��֮����һ���ļ�ʱ��
    public void Begin()
	{
		free = true;
	}
	public void Stop()
	{
		StopAllCoroutines();
		free = false;
		timer = -1f;
		MonsterManager.Instance.globalWaveIndex = 0;
	}

	void Update()
	{
		ContinueRefreshEnemy();
	}

	private void ContinueRefreshEnemy()
	{
		if (free && isStartRefreshEnemy && timer <= 0)
		{
			free = false;
			if (isGlobaleSpawner)
			{
				SpawnWaveIndex(MonsterManager.Instance.globalWaveIndex);
				OnWaveBegin?.Invoke(MonsterManager.Instance.globalWaveIndex + 1);
			}
			else
			{
				SpawnWaveIndex(waveIndex);
				OnWaveBegin?.Invoke(waveIndex + 1);
			}

			OnTimerChange?.Invoke(0);
		}
		else
		{
			timer -= Time.deltaTime;
		}
	}

	private void SpawnWaveIndex(int index)
	{
		if (index < _enemyWaveSO.waves.Length)
		{
			StartCoroutine(SpawnWave(_enemyWaveSO.waves[index], WaveEnd));
		}
	}

	IEnumerator SpawnWave(WaveS wave, UnityAction onComplete)
	{
		//foreach (var e in wave.enemyList)
		//{
		//	float timer = e.spawnduration;
		//	int enemyAmount = e.value != 0 ? e.value : 1;
		//	float spawnVelocity = timer / enemyAmount;
		//	float eachWaveVelocity = spawnVelocity * e.spawnRate;
		//
		//	while (enemyAmount > 0)
		//	{
		//		if (eachWaveVelocity < 0)
		//		{
		//			for (int i = 0; i < e.spawnRate; i++)
		//			{
		//				//����Ҫˢ�ֿյ��ڿ����ڴ˼��ؿն�����idΪ0ʱ�����ɹ֣�������ʱ��
		//				MonsterManager.Instance.SpawnEnemy(e.enemyEntity.ID, GetRandomPosition(wave));
		//				enemyAmount--;
		//			}
		//			eachWaveVelocity = spawnVelocity * e.spawnRate;
		//		}
		//		else
		//		{
		//			eachWaveVelocity -= Time.deltaTime;
		//			yield return null;
		//		}
		//	}
		//}

		yield return null;
		onComplete?.Invoke();
	}

	private void WaveEnd()
	{
		if (isLevelIdentification)
		{
			OnWaveEnd.Invoke();
		}

		free = true;
		if (MonsterManager.Instance.globalWaveIndex + 1 < _enemyWaveSO.waves.Length)
		{
			timer = durationBetweenWaves;
			if (isRefreshEnemyRecord) { MonsterManager.Instance.globalWaveIndex++; }
		}
		else if (loop)
		{
			timer = durationBetweenLoop;
			waveIndex = 0;
			MonsterManager.Instance.globalWaveIndex = 0;
		}
		else
		{
			free = false;
			OnWaveEnd.Invoke();
		}
	}

	Vector3 GetRandomPosition(WaveS wave)
	{
		Vector3 p = Vector3.zero;
		switch (wave.spawnArea)
		{
			case SpawnAreaType.Point:
				return wave.zero;
			case SpawnAreaType.Rect:

				float rx = Random.Range(wave.spawnAreaSizeMin.x, wave.spawnAreaSizeMax.x);
				float ry = Random.Range(wave.spawnAreaSizeMin.y, wave.spawnAreaSizeMax.y);
				int i = Random.Range(0, 4);

				switch (i)
				{
					case 0:
						p = new Vector3(rx, 0, Random.Range(-wave.spawnAreaSizeMax.y, wave.spawnAreaSizeMax.y));
						break;
					case 1:
						p = new Vector3(-rx, 0, Random.Range(-wave.spawnAreaSizeMax.y, wave.spawnAreaSizeMax.y));
						break;
					case 2:
						p = new Vector3(Random.Range(-wave.spawnAreaSizeMax.x, wave.spawnAreaSizeMax.x), 0, ry);
						break;
					case 3:
						p = new Vector3(Random.Range(-wave.spawnAreaSizeMax.x, wave.spawnAreaSizeMax.x), 0, -ry);
						break;
					default:
						break;
				}
				//�ٶȷ����޳�
				//Vector3 dir = Vector3.zero;
				//dir = mainCharacter.velocity;
				//if (dir.x > 0)
				//{
				//	p.x = Mathf.Abs(p.x);
				//}
				//else if (dir.x < 0)
				//{
				//	p.x = -Mathf.Abs(p.x);
				//}
				//if (dir.y > 0)
				//{
				//	p.y = Mathf.Abs(p.y);
				//}
				//else if (dir.y < 0)
				//{
				//	p.y = -Mathf.Abs(p.y);
				//}

				p = p + wave.zero;
				return (p);
			case SpawnAreaType.Ring:
				p = Vector3.right * Random.Range(wave.spawnRadiusMin, wave.spawnRadiusMax);
				int angel = Random.Range(0, 360);
				Quaternion q;
				q = Quaternion.Euler(Vector3.up * angel);
				p = q * p;
				return (p + wave.zero);
			default:
				return wave.zero;
		}
	}


	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		for (int i = 0; i < _enemyWaveSO.waves.Length; i++)
		{
			WaveS wave = _enemyWaveSO.waves[i];
			Vector3 spawnCenter = wave.zero;
			Gizmos.DrawIcon(spawnCenter, "Wave " + i + " SpawnCenter");
			switch (wave.spawnArea)
			{
				case SpawnAreaType.Point:
					break;
				case SpawnAreaType.Ring:
					Gizmos.DrawWireSphere(spawnCenter, wave.spawnRadiusMin);
					Gizmos.DrawWireSphere(spawnCenter, wave.spawnRadiusMax);
					break;
				case SpawnAreaType.Rect:
					Gizmos.DrawWireCube(wave.zero, new Vector3(wave.spawnAreaSizeMin.x * 2, 1, wave.spawnAreaSizeMin.y * 2));
					Gizmos.DrawWireCube(wave.zero, new Vector3(wave.spawnAreaSizeMax.x * 2, 1, wave.spawnAreaSizeMax.y * 2));
					break;
			}
		}
	}


    #region Old
    private void StartWaveSpawn(Transform arg0)
	{
		ResetSpawner();
	}

	/// <summary>
	/// ����ˢ����
	/// </summary>
	public void ResetSpawner()
	{
		StopAllCoroutines();
		free = true;
		timer = -1f;
		MonsterManager.Instance.globalWaveIndex = 0;
	}

	int tempHp = 100;
	void SpawnBoss(GameObject boss, Vector3 position, Quaternion orientation)
	{
		GameObject bo = ObjectPool.Instantiate(boss, position, orientation);
		Damageable Da = bo.GetComponent<Damageable>();
		Da.Revive();
		Da.AddTempHp(tempHp);
		tempHp += (LoopIndex * LoopIndex);
	}

	/*
	/// <summary>
	/// ͬʱˢ���ֹ�(δ��)
	/// </summary>
	/// <param name="wave"></param>
	/// <param name="onComplete"></param>
	private void SpawnWaveMultiple(WaveS wave, UnityAction onComplete)
	{
		foreach (var e in wave.enemyList)
		{
			StartCoroutine(SpawnWave(wave, e, onComplete));
		}
	}
	IEnumerator SpawnWave(WaveS wave, EnemyStructData e, UnityAction onComplete)
	{
		float timer = e.spawnduration;
		int enemyAmount = e.value != 0 ? e.value : 1;
		float spawnVelocity = timer / enemyAmount;
		float eachWaveVelocity = spawnVelocity * e.spawnRate;

		while (enemyAmount > 0)
		{
			if (eachWaveVelocity < 0)
			{
				for (int i = 0; i < e.spawnRate; i++)
				{
					MonsterManager.Instance.SpawnEnemy(e.enemyEntity.ID, GetRandomPosition(wave));
					enemyAmount--;
				}
				eachWaveVelocity = spawnVelocity * e.spawnRate;
			}
			else
			{
				eachWaveVelocity -= Time.deltaTime;
				yield return null;
			}
		}
		yield return null;
		onComplete?.Invoke();
	}
  //   /
    #endregion
}
*/
