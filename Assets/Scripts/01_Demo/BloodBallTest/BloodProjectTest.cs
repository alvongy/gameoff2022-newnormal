using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodProjectTest : MonoBehaviour
{
    [SerializeField] GameObject HitEffectObj;
    [SerializeField] ParticleSystem ProjectEffect;
    [SerializeField] GameObject BloodObj;
    [SerializeField] float Dropfroce = 30;
    [SerializeField] float PickSpeed = 10;

    [System.NonSerialized] public Rigidbody rigidbody;

    public DropOrPick dropOrPick;
    public Transform targetTsf;
    private Transform _tsf;
    bool Dead = false;

    const float OFFSET = 0.1f;//高度偏移
    const float PLAYERBODYOFFSET =1f;//身体位置偏移
    public enum DropOrPick
    {
        Drop,
        Pick,
    }
    private void Awake()
    {
        _tsf = transform;
        ProjectEffect.Play();
        rigidbody = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        if (dropOrPick == DropOrPick.Pick)
        {
            rigidbody.AddForce((targetTsf.position + Vector3.up* PLAYERBODYOFFSET - _tsf.position).normalized * -10f+Vector3.up * PLAYERBODYOFFSET * 2f, ForceMode.Acceleration);
            //rigidbody.velocity = (targetTsf.position + Vector3.up - _tsf.position).normalized * -5f;
        }
    }
    public void DropHit()
    {
        ProjectEffect.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        rigidbody.velocity = Vector3.zero;
        GameObject.Instantiate(BloodObj, transform.position+new Vector3(0, OFFSET, 0), Quaternion.identity);
        Dead = true;
        Invoke("DestroySelf", 1f);

    }
    public void PickHit()
    {
        GameObject.Instantiate(HitEffectObj, targetTsf.position + Vector3.up * PLAYERBODYOFFSET, Quaternion.identity);
        ProjectEffect.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        rigidbody.velocity = Vector3.zero;
        Dead = true;
        Invoke("DestroySelf", 1f);
    }
    void DestroySelf()
    {
        Destroy(this.gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!Dead)
        {
            switch (dropOrPick)
            {
                case DropOrPick.Drop:
                    if (other.CompareTag("Ground") && rigidbody.velocity.y < 0)
                    {
                        DropHit();

                    }
                    break;
                case DropOrPick.Pick:
                    if (other.CompareTag("Player"))
                    {
                        PickHit();
                    }
                    break;
                default:
                    break;
            }
        }

    }
    private void FixedUpdate()
    {
        if (!Dead)
        {
            if (dropOrPick == DropOrPick.Drop)
            {
                rigidbody.AddForce(Vector3.down * Dropfroce, ForceMode.Force);
            }
            else
            if (dropOrPick == DropOrPick.Pick)
            {
                //rigidbody.AddForce((targetTsf.position + Vector3.up * PLAYERBODYOFFSET- _tsf.position).normalized * PickSpeed, ForceMode.VelocityChange);//BUG技能
                rigidbody.velocity = (targetTsf.position + Vector3.up * PLAYERBODYOFFSET - _tsf.position).normalized * PickSpeed;

            }
        }

    }
}
