using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance;

    private PhotonView PV;

    [Header("Screen Borders")]
    [SerializeField] private Transform TopBorder;
    [SerializeField] private Transform BottomBorder;

    public float BorderUpY
    {
        get
        {
            return TopBorder.position.y;
        }
    }

    public float BorderDownY
    {
        get
        {
            return BottomBorder.position.y;
        }
    }

    [Header("Score Text")]
    [SerializeField] private TMP_Text team1PointsText;
    [SerializeField] private TMP_Text team2PointsText;

    private int team1Score;
    private int team2Score;

    [Header("Endgame Screen")]
    [SerializeField] private GameObject victoryScreen;
    [SerializeField] private TMP_Text victoryText;

    public bool IsGameOver;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        PV = GetComponent<PhotonView>();
    }

    private void Start()
    {
        IsGameOver = false;
        victoryScreen.SetActive(false);

        team1Score = 0;
        team2Score = 0;

        team1PointsText.text = team1Score.ToString();
        team2PointsText.text = team2Score.ToString();
    }

    public void Team1ScoreUp()
    {
        PV.RPC("RPC_Team1ScoreUp", RpcTarget.All);
    }

    public void Team2ScoreUp()
    {
        PV.RPC("RPC_Team2ScoreUp", RpcTarget.All);
    }

    [PunRPC]
    private void RPC_Team1ScoreUp()
    {
        team1Score++;
        team1PointsText.text = team1Score.ToString();

        if (team1Score >= 5)
        {
            IsGameOver = true;
            victoryText.text = "LEFT TEAM WINS";
            victoryScreen.SetActive(true);
        }
    }

    [PunRPC]
    private void RPC_Team2ScoreUp()
    {
        team2Score++;
        team2PointsText.text = team2Score.ToString();

        if (team2Score >= 5)
        {
            IsGameOver = true;
            victoryText.text = "RIGHT TEAM WINS";
            victoryScreen.SetActive(true);
        }
    }

    public void LeaveRoom()
    {
        Destroy(RoomManager.Instance.gameObject);
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel(0);
    }
}
