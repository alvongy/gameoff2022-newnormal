using ArtSetting;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using YK.Game.Events;
using static EnemyWaveSO;
using Random = UnityEngine.Random;

public class WaveSpawner_Sword : MonoBehaviour
{
	public UnityAction<float> OnTimerChange;
	public UnityAction<int> OnWaveBegin;
	public UnityEvent OnWaveEnd;
	public UnityEvent OnEnemyClear;

	[Title("怪物波数SO")]
	public EnemyWaveSO _enemyWaveSO;
	//[FoldoutGroup("Bool"), InfoBox("是否使用全局数据记录刷怪")][HideInInspector]
	//public bool isGlobaleSpawner = false;
	[/*FoldoutGroup("Bool"), */InfoBox("是否开始进行刷怪")]
	public bool isStartRefreshEnemy = false;
	//[FoldoutGroup("Bool"), InfoBox("是否将此次刷怪记录到全局")][HideInInspector]
	//public bool isRefreshEnemyRecord = false;

	[InfoBox("是否循环刷怪（循环间隔时间）")]
	public bool loop;
	[ShowIf("@(this.loop)==true")]
	public float durationBetweenLoop = 10f;

    [InfoBox("怪物刷新的每一波间隔")]
    public float durationBetweenWaves = 5f;

    //int LoopIndex = 0;

    public bool Free => free;
	bool free = true;//刷怪器是否空闲（即并未执行刷怪任务）

	int waveIndex = 0;

	public void Init()
    {
        if (isStartRefreshEnemy)
        {
			Begin();
        }
    }

	public void Begin()
	{
        if (free)
        {
			free = false;
			waveIndex = 0;
			StartCoroutine(AllWave());
		}
	}

	public void Stop()
	{
		StopAllCoroutines();
		free = true;
		OnWaveEnd.RemoveAllListeners();
	}

	void Update()
	{
		
	}

	IEnumerator AllWave()
    {
		yield return EachWave();
		WaitForSeconds durationLoop = new WaitForSeconds(durationBetweenLoop);
		while (loop)
		{
			if (durationBetweenLoop > 0)
			{
				yield return durationLoop;
			}
			waveIndex = 0;
			yield return EachWave();
		}
		OnWaveEnd.Invoke();
		OnWaveEnd.RemoveAllListeners();
		Debug.Log("本关卡配置的SO刷怪完毕");
	}

	IEnumerator EachWave()
    {
		WaitForSeconds durationWaves = new WaitForSeconds(durationBetweenWaves);
		yield return SpawnWave(_enemyWaveSO.waves[waveIndex]);
		waveIndex++;
		while (waveIndex < _enemyWaveSO.waves.Length)
		{
			if (durationBetweenWaves > 0)
			{
				yield return durationWaves;
			}
			yield return SpawnWave(_enemyWaveSO.waves[waveIndex]);
			waveIndex++;
		}
	}

	WaitUntil wait = new WaitUntil(() => MonsterManager.Instance.CanSpawn);

	IEnumerator SpawnWave(WaveS wave)
	{
		float timer = wave.spawnduration;
		int enemyAmount = wave.spawnCount > 0 ? wave.spawnCount : 1;
		float spawnVelocity = timer / enemyAmount;
		float eachWaveVelocity = spawnVelocity * wave.spawnRate;

		while (enemyAmount > 0)
		{
			if (eachWaveVelocity < 0)
			{
				for (int i = 0; i < wave.spawnRate && enemyAmount > 0; i++)
				{
					wait.Reset();
					yield return wait;
					MonsterManager.Instance.SpawnEnemy(wave.enemyId, GetRandomPosition(wave));
					//MonsterManager.Instance.SpawnEnemy(wave.enemyEntity.ID, GetRandomPosition(wave));
					enemyAmount--;
				}
				eachWaveVelocity = spawnVelocity * wave.spawnRate;
			}
			else
			{
				eachWaveVelocity -= Time.deltaTime;
				yield return null;
			}
		}
		yield return null;
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
#if UNITY_EDITOR
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		if (_enemyWaveSO)
		{
			for (int i = 0; i < _enemyWaveSO.waves.Length; i++)
			{
				WaveS wave = _enemyWaveSO.waves[i];
				Gizmos.DrawIcon(wave.zero, "Wave " + i + " SpawnCenter");
				switch (wave.spawnArea)
				{
					case SpawnAreaType.Point:
						Gizmos.DrawWireCube(wave.zero, new Vector3(1, 1, 1));
						break;
					case SpawnAreaType.Ring:
						Gizmos.DrawWireSphere(wave.zero, wave.spawnRadiusMin);
						Gizmos.DrawWireSphere(wave.zero, wave.spawnRadiusMax);
						break;
					case SpawnAreaType.Rect:
						Gizmos.DrawWireCube(wave.zero, new Vector3(wave.spawnAreaSizeMin.x * 2, 1, wave.spawnAreaSizeMin.y * 2));
						Gizmos.DrawWireCube(wave.zero, new Vector3(wave.spawnAreaSizeMax.x * 2, 1, wave.spawnAreaSizeMax.y * 2));
						break;
				}
			}
		}
	}
#endif
}
