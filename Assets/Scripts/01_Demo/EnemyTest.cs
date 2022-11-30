using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyTest : MonoBehaviour
{
    [SerializeField] int life = 100;
    public float centerOffset = 1;//ÖÊÐÄÆ«ÒÆ
    [SerializeField] GameObject deathEffectPrefab;
    List<Material> _materials;
    private NavMeshAgent _navMeshAgent;
    private NavMeshObstacle _navMeshObstacle;
    [SerializeField] Transform playerTsf;
    [SerializeField] float speed = 2f;
    float ONHITFORCE = 8f;
    Rigidbody _rigidbody;
    Animator animator;
    Transform tsf;
    bool isDead = false;
    [SerializeField] SphereCollider sphereCollider;
    private void Awake()
    {
        _materials = new List<Material>();
        tsf = transform;
        _navMeshAgent = tsf.GetComponent<NavMeshAgent>();
        _navMeshObstacle = tsf.GetComponent<NavMeshObstacle>();
        animator = tsf.GetComponent<Animator>();
        _rigidbody = tsf.GetComponent<Rigidbody>();

    }
    void Start()
    {
        playerTsf = GameStateCyz.GetPlayerTsf();
        SkinnedMeshRenderer[] skinnedMeshRenderers = this.transform.GetComponentsInChildren<SkinnedMeshRenderer>(false);
        foreach (SkinnedMeshRenderer render in skinnedMeshRenderers)
        {
            _materials.AddRange(render.materials);
        }
        SetDestination(playerTsf.position);
    }

    private void ChageColor(Material material)
    {

        Sequence sequence = DOTween.Sequence();
        sequence.Append(material.DOFloat(1f, "_Hit1", 0.1f));
        sequence.Append(material.DOFloat(0f, "_Hit1", 0.1f));
    }
    public void OnHit(int damage = 5)
    {
        life -= damage;
        foreach (Material mat in _materials)
        {
            ChageColor(mat);
        }
        if (life <= 0 && !isDead)
        {
            isDead = true;
            GameObject.Instantiate(deathEffectPrefab).transform.SetPositionAndRotation(transform.position + new Vector3(0, centerOffset, 0), transform.rotation);
            SendMessage("Drop");
            Destroy(this.gameObject);
        }
        //Vector3 force = (tsf.position - playerTsf.position).normalized * HITFORCE;
        //Debug.Log("Force:"+ force.magnitude);
        _rigidbody.velocity = (tsf.position - playerTsf.position).normalized * ONHITFORCE;
    }
    float tick = 0;
    bool isStop = false;

    public void SetDestination(Vector3 postion)
    {
        _navMeshAgent.destination = postion;
        _navMeshAgent.speed = speed;
    }
    bool stop=false;
    Vector3 tempPosition;
    private void FixedUpdate()
    {


        Ray ray = new Ray(tsf.position + Vector3.up, tsf.forward);

        RaycastHit hit;
        bool isCollider = Physics.Raycast(ray, out hit, 2f);
        Debug.DrawLine(ray.origin, hit.point);
        //Debug.Log(isCollider);
        if (isCollider && hit.collider.CompareTag("Player"))
        {
            //Debug.Log("xxxxx");
            animator.SetBool("Attack", true);
            _navMeshAgent.speed = 0;
        }
        else
        {
            animator.SetBool("Attack", false);
            if (Physics.OverlapSphere(tsf.position, 1).Length < 5)
            {
                SetDestination(playerTsf.position);
                
                if (stop)
                {
                    foreach (Material mat in _materials)
                    {
                        mat.DOFloat(0f, "_Hit1", 0.1f);
                    }
                    stop = false;
                }
            }
            else {
                if (!stop)
                {
                    foreach (Material mat in _materials)
                    {
                        mat.DOFloat(1f, "_Hit1", 0.1f);
                    }
                    stop = true;
                    tempPosition = playerTsf.position;
                }
               
            };

         

        }
  


        //if ((playerTsf.position - tsf.position).sqrMagnitude < Mathf.Pow(_navMeshAgent.stoppingDistance * 1, 2))
        //{
        //    //if (!isStop)
        //    //{
        //    //    _navMeshAgent.enabled = false;
        //    //    _navMeshObstacle.enabled = true;
        //    //    GameStateCyz.ObsCount++;
        //    //    Debug.Log("xx_" + GameStateCyz.ObsCount);
        //    //    isStop = true;
        //    //    //foreach (Material mat in _materials)
        //    //    //{
        //    //    //    mat.DOFloat(1f, "_Hit1", 0.1f);
        //    //    //}
        //    //}



        //}
        //else
        //{


        //    if (isStop)
        //    {
        //        _navMeshAgent.enabled = true;
        //        _navMeshObstacle.enabled = false;

        //        isStop = false;
        //        GameStateCyz.ObsCount--;
        //        Debug.Log("xx_" + GameStateCyz.ObsCount);
        //        //foreach (Material mat in _materials)
        //        //{
        //        //    mat.DOFloat(0f, "_Hit1", 0.1f);
        //        //}
        //    }


        //    _navMeshAgent.destination = playerTsf.position;
        //    _navMeshAgent.speed = speed;


        //}

    }



}




//using UnityEngine; 
//using System.Collections;
// public class enemyMovement : MonoBehaviour
//{
//    public Transform player;
//    public Transform model;
//    public Transform proxy;
//    NavMeshAgent agent;
//    NavMeshObstacle obstacle;
//    void Start()
//    {
//        agent = proxy.GetComponent<NavMeshAgent>();
//        obstacle = proxy.GetComponent<NavMeshObstacle>();
//    }
//    void Update()
//    {
//        // Test if the distance between the agent (which is now the proxy) and the player
//        // is less than the attack range (or the stoppingDistance parameter) if ((player.position - proxy.position).sqrMagnitude < Mathf.Pow(agent.stoppingDistance, 2)) {
//        // If the agent is in attack range, become an obstacle and 
//        // disable the NavMeshAgent component obstacle.enabled = true; agent.enabled = false; } else { 
//        // If we are not in range, become an agent again obstacle.enabled = false; agent.enabled = true; 
//        // And move to the player's position agent.destination = player.position; }
//        // model.position = Vector3.Lerp(model.position, proxy.position, Time.deltaTime * 2); model.rotation = proxy.rotation; } }