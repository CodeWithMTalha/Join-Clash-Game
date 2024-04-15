using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class bossManager : MonoBehaviour
{
    public List<GameObject> Enemies = new List<GameObject>();
    public Animator BossAnimator;
    public static bossManager BossManagerCls;
    private float attackMode;
    public bool LockOnTarget, BossIsAlive;
    private Transform target;
    public Slider HealthBar;
    public TextMeshProUGUI HealthBarAmount;
    public int Health;
    public GameObject Particle_Death;
    public float Min_Ditance, Max_Distance;

    void Start()
    {
        BossManagerCls = this;

        var enemy = GameObject.FindGameObjectsWithTag("Add");
        foreach(var i in enemy)
            Enemies.Add(i);
        
        BossAnimator = GetComponent<Animator>();
        BossIsAlive = true;
       
        HealthBar.value = HealthBar.maxValue = Health = 150;

        HealthBarAmount.text = Health.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        HealthBar.transform.rotation = Quaternion.Euler(HealthBar.transform.rotation.x, 0f, HealthBar.transform.rotation.y);

        if(Enemies.Count > 0)
            foreach (var stickMan in Enemies)
            {
            var stickManDistance = stickMan.transform.position - transform.position;

            if(stickManDistance.sqrMagnitude < Max_Distance * Max_Distance && !LockOnTarget)
            {
                //Debug.LogError("lockONTarget" + LockOnTarget);

                target = stickMan.transform;
                BossAnimator.SetBool("fightboss", true);
                transform.position = Vector3.MoveTowards(transform.position, target.position, 1f * Time.deltaTime);
            }
            if (stickManDistance.sqrMagnitude < Min_Ditance * Min_Ditance)
                LockOnTarget = true;
            
           }

        if (LockOnTarget)
        {
            var bossRotation = new Vector3(target.position.x,transform.position.y,target.position.z) - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(bossRotation, Vector3.up),10f * Time.deltaTime);
            for(int i =0; i< Enemies.Count; i++)
            {
                if (!Enemies[i].GetComponent<memberManager>().member)
                {
                    Enemies.RemoveAt(i);
                }
            }
        }
        if(Enemies.Count == 0)
        {
            BossAnimator.SetBool("fightboss", false);
            BossAnimator.SetFloat("attackmode", 4f);

            Invoke("CompletePanel", 5);
        }

        if(Health <= 0 && BossIsAlive)
        {
            gameObject.SetActive(false);
            BossIsAlive = false;
            Instantiate(Particle_Death, transform.position, Quaternion.identity);

            Invoke("GameplaySc.InstanceGamePlay.CompletePanel", 5);
            
        }
    }

    public void ChangeBossAttackMode()
    {
        BossAnimator.SetFloat("attackmode", Random.Range(2,4));
    }


}
