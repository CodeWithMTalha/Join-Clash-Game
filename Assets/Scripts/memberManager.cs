using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class memberManager : MonoBehaviour
{
    public Animator chacator_animator;
    public GameObject Particle_Death;
   [SerializeField] private Transform Boss;
    public int Health;
    public float MinDistanceOfEnemy, MaxDistanceOfEnemy, MoveSpeed;
    public bool Fight, member;
    private Rigidbody Rb;
    private CapsuleCollider _capsuleCollider;

    // Start is called before the first frame update
    void Start()
    {
        chacator_animator = GetComponent<Animator>();

        Rb = GetComponent<Rigidbody>();
        _capsuleCollider = GetComponent<CapsuleCollider>();

        Boss = GameObject.FindWithTag("boss").transform;
        Health = 5;
    }

    // Update is called once per frame
    void Update()
    {
        var bossDistance = Boss.position - transform.position;

        if (!Fight)
        {
            if (bossDistance.sqrMagnitude <= MaxDistanceOfEnemy * MaxDistanceOfEnemy)
            {
                Player_Manager.playerManagerCls.attackToBoss = true;
                Player_Manager.playerManagerCls.gameState = false;
            }
           // Debug.LogError(Player_Manager.playerManagerCls.attackToBoss+"   "+member);

            if (Player_Manager.playerManagerCls.attackToBoss && member)//stickMan will run and find the boss
            {
                //Debug.LogError("move toward boos");
                transform.position = Vector3.MoveTowards(transform.position, Boss.position, MoveSpeed * Time.deltaTime);
                var stickManRotation = new Vector3(Boss.position.x, transform.position.y, Boss.position.z) - transform.position;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(stickManRotation, Vector3.up), 10f * Time.deltaTime);

                chacator_animator.SetFloat("run", 1f);

                Rb.velocity = Vector3.zero;
            }

        }

        if (bossDistance.sqrMagnitude <= MinDistanceOfEnemy * MinDistanceOfEnemy)
        {
            Fight = true;

            var stickManRotation = new Vector3(Boss.position.x, transform.position.y, Boss.position.z) - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(stickManRotation, Vector3.up), 10f * Time.deltaTime);
            chacator_animator.SetBool("fight", true);

            MinDistanceOfEnemy = MaxDistanceOfEnemy;

            Rb.velocity = Vector3.zero;

        }
        else
        {
            Fight = false;
        }
    }

    public void changeAttackMode()
    {
        chacator_animator.SetFloat("attackMode", Random.Range(0, 2));
       // print("Attack Change");
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("damage"))
        {
            Health--;
            if (Health <= 0)
            {
                Instantiate(Particle_Death, transform.position, Quaternion.identity);

                if(gameObject.name != Player_Manager.playerManagerCls.rblist[0].name)
                {
                    gameObject.SetActive(false);
                    transform.parent = null;
                }
                else
                {
                    _capsuleCollider.enabled = false;
                    transform.GetChild(0).gameObject.SetActive(false);
                    transform.GetChild(1).gameObject.SetActive(false);
                }

                for(int i=0; i< bossManager.BossManagerCls.Enemies.Count; i++)
                {
                    if(bossManager.BossManagerCls.Enemies[i].name == gameObject.name)
                    {
                        bossManager.BossManagerCls.Enemies.RemoveAt(i);
                        break;
                    }
                }
                bossManager.BossManagerCls.LockOnTarget = false;
            }
        }
    }
}
