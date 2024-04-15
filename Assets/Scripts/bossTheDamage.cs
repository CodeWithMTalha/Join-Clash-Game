using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossTheDamage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.AddComponent<Rigidbody>().isKinematic = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("boss") && Random.Range(0,2)==1)
        {
            bossManager.BossManagerCls.Health--;
            bossManager.BossManagerCls.HealthBarAmount.text = bossManager.BossManagerCls.Health.ToString();
            bossManager.BossManagerCls.HealthBar.value = bossManager.BossManagerCls.Health;
        }
    }
}
