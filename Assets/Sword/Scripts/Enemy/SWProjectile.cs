using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using YK.Game.Events;

public class SWProjectile : MonoBehaviour
{
    private EntityData data;
    private float speed;
    private Vector3 direction;
    [SerializeField] float radiu;
    [SerializeField] LayerMask layerMask;

    private bool alive;
    private bool over;
    private float maxVoyage;

    public UnityAction onBrforeDestroy = delegate { };

    public void Init(Entity ent, float spe, Vector3 dir, LayerMask layers, float voyage = 1024f)
    {
        data = ent.Data.Copy();
        speed = spe;
        radiu = 1f;
        direction = dir;
        layerMask = layers;
        colliders = new Collider[20];
        alive = true;
        over = false;
        maxVoyage = voyage;
    }

    Collider[] colliders;

    private void Update()
    {
        if (!over)
        {
            if (alive)
            {
                Vector3 temp = direction * (speed * Time.deltaTime);
                transform.Translate(temp);
                if (Time.frameCount % 5 == 0)
                {
                    bool hitFlag = false;
                    bool borderFlag = false;
                    int count = Physics.OverlapSphereNonAlloc(transform.position, radiu, colliders, layerMask);
                    for (int i = 0; i < count; i++)
                    {
                        Entity hitEntity = colliders[i].GetComponent<Entity>();
                        if (hitEntity)
                        {
                            hitFlag = true;
                            //if (BattleManager.Instance.CheckAttake(hitEntity))
                            //{
                            BattleManager.Instance.Hit(data, hitEntity);
                            //}
                        }
                        else if (colliders[i].tag.Equals("Border"))
                        {
                            borderFlag = true;
                        }
                    }
                    if (hitFlag || borderFlag)
                    {
                        alive = false;
                    }
                }
                if (alive)
                {
                    maxVoyage -= temp.magnitude;
                    if (maxVoyage < 0)
                    {
                        alive = false;
                    }
                }
            }
            else
            {
                ObjectPool.Destroy(gameObject);
                over = true;
                onBrforeDestroy?.Invoke();
                onBrforeDestroy = delegate { };
            }
        }
    }

    public void Over()
    {
        alive = false;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, radiu);
    }
#endif
}
