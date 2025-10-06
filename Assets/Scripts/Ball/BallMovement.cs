using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    [SerializeField] private float speed;

    [SerializeField] private float timeToLaunch;

    private Vector3 moveVector;

    private float speedMultiplier = 1;

    private PhotonView PV;

    private bool ballLaunched = false;

    private float timeStart;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (!PV.IsMine) return;
        timeStart = Time.time;
    }

    private void Update()
    {
        if (!PV.IsMine || GameManager.Instance.IsGameOver) return;

        if (!ballLaunched)
        {
            if (Time.time > timeStart + timeToLaunch)
            {
                LaunchBall();
            }
        }
        else
        {
            transform.position += moveVector * speedMultiplier * Time.deltaTime;
        }
    }

    private void LaunchBall()
    {
        ballLaunched = true;
        speedMultiplier = 1;
        moveVector = GetLaunchDirection() * speed;
    }

    private Vector2 GetLaunchDirection()
    {
        int xDir = 0;

        if (Random.value > 0.5f) xDir = 1;
        else xDir = -1;

        float yDir = 0;
        float yAngle = Random.Range(0.3f, 0.7f);

        if (Random.value > 0.5f) yAngle *= -1;

        yDir = yAngle;

        return new Vector2(xDir, yDir);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!PV.IsMine) return;

        switch (other.gameObject.tag)
        {
            case "Paddles":
                moveVector.x *= -1;
                speedMultiplier += 0.05f;
                break;
            case "Walls":
                moveVector.y *= -1;
                break;
            case "Goals":
                ScorePoint();

                ResetBall();
                break;
            default:
                break;
        }
    }

    private void ScorePoint()
    {
        if (transform.position.x > 0)
        {
            GameManager.Instance.Team1ScoreUp();
        }
        else if(transform.position.x < 0)
        {
            GameManager.Instance.Team2ScoreUp();
        }
    }

    public void ResetBall()
    {
        ballLaunched = false;
        transform.position = Vector3.zero;
        timeStart = Time.time;
    }
}
