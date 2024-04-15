using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recruitment : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Add")){
            Player_Manager.playerManagerCls.rblist.Add(other.collider.GetComponent<Rigidbody>());

            other.transform.parent = null;
            other.transform.parent = Player_Manager.playerManagerCls.transform;
            other.gameObject.GetComponent<memberManager>().member = true;

            if (!other.collider.gameObject.GetComponent<Recruitment>())
            {
                other.collider.gameObject.AddComponent<Recruitment>();
            }

            other.collider.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material = 
                Player_Manager.playerManagerCls.rblist[0].transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material;
        }
    }
}
