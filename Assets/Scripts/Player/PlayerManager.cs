using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private PhotonView PV;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (PV.IsMine)
        {
            CreateController();
        }
    }

    private void CreateController()
    {
        Debug.Log("Instantiate Player Controller");
        PhotonNetwork.Instantiate("PlayerPaddle", GetPlayerSpawnLocation(), Quaternion.identity);
    }

    private Vector3 GetPlayerSpawnLocation()
    {
        // Get the local player's index in the room
        int playerIndex = System.Array.IndexOf(PhotonNetwork.PlayerList, PhotonNetwork.LocalPlayer);

        return SpawnManager.Instance.Spawnpoints[playerIndex].position;
    }
}
