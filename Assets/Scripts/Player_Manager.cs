using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Manager : MonoBehaviour
{

    public bool moveByTouch,gameState,attackToBoss;
    private Vector3 Direction;
    public List<Rigidbody> rblist = new List<Rigidbody>();
    //private Animator stickMan_Anim;

    [SerializeField] private float runspeed, swipSpeed, velocity, roadSpeed;
    [SerializeField] private Transform road;

    public static Player_Manager playerManagerCls;
    // Start is called before the first frame update
    void Start()
    {
        playerManagerCls = this;
        rblist.Add(transform.GetChild(0).GetComponent<Rigidbody>());

        gameState = true;
        //stickMan_Anim = transform.GetChild(0).GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState)
        {
            if (Input.touches.Length > 0)
            {
                if (Input.touches[0].phase == TouchPhase.Began)
                {
                    moveByTouch = true;
                }
                else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
                {
                    moveByTouch = false;
                }
            }

           /* if (Input.GetMouseButtonDown(0))
            {
                 moveByTouch = true;
            }

            if (Input.GetMouseButtonUp(0))
            {
                moveByTouch = false;
            }*/

            if (moveByTouch)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.position.x < Screen.width / 2)
                {
                    // Move player left
                    transform.Translate(Vector3.left * runspeed * Time.deltaTime);
                }
                // Check if touch is on the right half of the screen
                else if (touch.position.x >= Screen.width / 2)
                {
                    // Move player right
                    transform.Translate(Vector3.right * runspeed * Time.deltaTime);
                }

                //Direction = new Vector3(Mathf.Lerp(Direction.x, Input.GetAxis("Mouse X"), runspeed * Time.deltaTime), 0f);
                //Direction = Vector3.ClampMagnitude(Direction, 1f);

                road.position = new Vector3(0f, 0f, Mathf.SmoothStep(road.position.z, -100f, Time.deltaTime * roadSpeed));

                foreach (var stickMan_Anim in rblist)
                    stickMan_Anim.GetComponent<Animator>().SetFloat("run", 1f);
            }
            else
            {
                foreach (var stickMan_Anim in rblist)
                    stickMan_Anim.GetComponent<Animator>().SetFloat("run", 0f);
            }

            foreach (var stickMan_Rb in rblist)
            {
                if (stickMan_Rb.velocity.magnitude > 0.5f)
                {
                    // Debug.LogError("rotation");
                    stickMan_Rb.rotation = Quaternion.Slerp(stickMan_Rb.rotation, Quaternion.LookRotation(stickMan_Rb.velocity, Vector3.up), Time.deltaTime + velocity);
                }
                else
                {
                    // Debug.LogError("forward");

                    stickMan_Rb.rotation = Quaternion.Slerp(stickMan_Rb.rotation, Quaternion.identity, Time.deltaTime + velocity);
                }
            }
        }
        else
        {
            if (!bossManager.BossManagerCls.BossIsAlive)
            {
                foreach(var i in rblist)
                {
                    i.GetComponent<Animator>().SetFloat("attackMode", 4);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (gameState)
        {
            if (moveByTouch)
            {
                Vector3 displacement = new Vector3(Direction.x, 0f, 0f) * Time.fixedDeltaTime;

                foreach (var stickMan_Rb in rblist)
                    stickMan_Rb.velocity = new Vector3(Direction.x * Time.fixedDeltaTime * swipSpeed, 0f, 0f) + displacement;
            }
            else
            {
                foreach (var stickMan_Rb in rblist)
                    stickMan_Rb.velocity = Vector3.zero;
            }
        }
    }
}
