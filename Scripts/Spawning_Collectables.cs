using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawning_Collectables : MonoBehaviour
{

    public int interval = 7;
    public int maxSpeed = 2;

    public Vector3 forceApplied = new Vector3();

    public GameObject secondAvoidance;

    private Object collectable;
    private bool canSpawn = true;

    // Start is called before the first frame update
    void Start()
    {
        collectable = Resources.Load("Collectable");

        if(gameObject.name == "Collectable_Spawner")
        {
            secondAvoidance = GameObject.Find("Collectable_Spawner_1");
            secondAvoidance.SetActive(false);
        }
        else
        {
            secondAvoidance = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float t = Time.time;
        //Debug.Log(t);
        if(t > 1)
        {
            if (Mathf.Round(t) % interval == 0 && interval != maxSpeed && canSpawn)
            {
                //Debug.Log("Spawned collectable");

                interval--;

                GameObject collect = Instantiate(collectable, transform) as GameObject;

                Rigidbody rig = collect.GetComponent<Rigidbody>();

                rig.AddForce(forceApplied);

                canSpawn = false;

                Debug.Log("Interval: " + interval);
            }
            else if (Mathf.Round(t) % interval == 0 && interval == maxSpeed && canSpawn)
            {
                //Debug.Log("Spawned collectable");

                GameObject collect = Instantiate(collectable, transform) as GameObject;

                Rigidbody rig = collect.GetComponent<Rigidbody>();

                rig.AddForce(forceApplied);

                canSpawn = false;
            }
            else if(Mathf.Round(t) % interval != 0)
            {
                canSpawn = true;
            }
        }

        if(interval == maxSpeed && gameObject.name == "Collectable_Spawner")
        {
            secondAvoidance.SetActive(true);
        }

    }
}
