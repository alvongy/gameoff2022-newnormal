using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerArea : MonoBehaviour
{
    [SerializeField] int enemyID;
    [SerializeField] float duration;
    [SerializeField] float spawnRadiusMin;
    [SerializeField] float spawnRadiusMax;

    private bool alive = false;

    Vector3 GetRandomPosition()
    {
        Vector3 p = Vector3.right * Random.Range(spawnRadiusMin, spawnRadiusMax);
        int angel = Random.Range(0, 360);
        Quaternion q;
        q = Quaternion.Euler(Vector3.up * angel);
        p = q * p;
        return (p + transform.position);
    }

    public void Begin()
    {
        alive = true;
    }

    public void Stop()
    {
        alive = false;
        timer = 0;
    }

    float timer = 0;

    private void Update()
    {
        if (!alive)
        {
            return;
        }
        if (timer < 0)
        {
            MonsterManager.Instance.SpawnEnemy(enemyID, GetRandomPosition());
            timer = duration;

            //alive = false;
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, spawnRadiusMin);
        Gizmos.DrawWireSphere(transform.position, spawnRadiusMax);
    }
#endif
}
