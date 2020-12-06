using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player_Controller : MonoBehaviour
{

    public float speed;

    private Rigidbody rig;

    private float startTime;
    private float timeTaken;

    private int collectablesPicked;

    private GameObject spawningObject;
    private GameObject spawningObject2;

    public int maxCollectables = 10;

    private bool isPlaying;
    private bool isJumping = false;

    public GameObject playButton;
    public TextMeshProUGUI curTimeText;

    public Vector3 jumpingForce = new Vector3();

    void Awake()
    {
        rig = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        spawningObject = GameObject.Find("Collectable_Spawner");
        spawningObject2 = GameObject.Find("Collectable_Spawner_1");
        spawningObject.SetActive(false);
        spawningObject2.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlaying)
            return;

        bool jump = Input.GetKey(KeyCode.Space);

        if (jump && !isJumping)
        {
            rig.AddForce(jumpingForce, ForceMode.Impulse);
            isJumping = true;
        }
        //float x = Input.GetAxis("Horizontal") * speed;
        //float z = Input.GetAxis("Vertical") * speed;

        //rig.velocity = new Vector3(x, rig.velocity.y, z);

        curTimeText.text = (Time.time - startTime).ToString("F2");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Collectable") && isPlaying)
        {
            collectablesPicked++;
            Destroy(other.gameObject);
            if (collectablesPicked == maxCollectables)
                End();
        }
        else
        {
            Destroy(other.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "Ground")
        {
            isJumping = false;
        }
    }

    public void Begin()
    {
        startTime = Time.time;
        isPlaying = true;
        playButton.SetActive(false);

        Leaderboard.instance.leaderboardCanvas.SetActive(false);

        spawningObject.SetActive(true);
        spawningObject2.SetActive(true);
    }

    void End()
    {

        timeTaken = Time.time - startTime;
        isPlaying = false;
        playButton.SetActive(true);

        spawningObject.SetActive(false);
        spawningObject2.SetActive(false);

        Leaderboard.instance.leaderboardCanvas.SetActive(true);

        Leaderboard.instance.SetLeaderboardEntry(-Mathf.RoundToInt(timeTaken * 1000.0f));
    }
}
