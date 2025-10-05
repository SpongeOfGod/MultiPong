using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Linq;

public class NetworkLauncher : MonoBehaviourPunCallbacks
{
    public static NetworkLauncher Instance;

    [Header("Input Fields")]
    [SerializeField] private TMP_InputField roomNameInputField;

    [Header("Text Fields")]
    [SerializeField] private TMP_Text roomNameText;
    [SerializeField] private TMP_Text errorText;

    [Header("Buttons")]
    [SerializeField] private GameObject startGameButton; 

    [Header("Room List")]
    [SerializeField] private Transform roomListContent;
    [SerializeField] private GameObject roomListItemPrefab;

    private Dictionary<string, GameObject> activeLobbies = new Dictionary<string, GameObject>();

    [Header("Player List")]
    [SerializeField] private Transform playerListContent;
    [SerializeField] private GameObject playerListItemPrefab;

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
    }

    private void Start()
    {
        Debug.Log("Connecting to Master");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinedLobby()
    {
        MenuManager.Instance.OpenMenu("main");
        Debug.Log("Joined Lobby ");

        PhotonNetwork.NickName = "Player " + Random.Range(0, 1000).ToString("0000");
    }

    public void CreateRoom()
    {
        if(string.IsNullOrEmpty(roomNameInputField.text)) //Checks to make sure a room name was entered in the input field.
        {
            return;
        }

        PhotonNetwork.CreateRoom(roomNameInputField.text);
        MenuManager.Instance.OpenMenu("loading");
    }

    public override void OnJoinedRoom()
    {
        MenuManager.Instance.OpenMenu("room");
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;

        Player[] players = PhotonNetwork.PlayerList;

        foreach (Transform child in playerListContent) //Clears List of players.
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < players.Length; i++)  //.Count() before
        {
            Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
        }

        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "Room Creation Failed: " + message; 
        MenuManager.Instance.OpenMenu("error");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        errorText.text = "Failed To Join Room: " + message;
        MenuManager.Instance.OpenMenu("error");
    }

    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.Instance.OpenMenu("loading");
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel(1);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.OpenMenu("loading");
    }

    public override void OnLeftRoom()
    {
        MenuManager.Instance.OpenMenu("main");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform trans in roomListContent)
        {
            Debug.Log("Room destroyed = " + trans.name);
            Destroy(trans.gameObject);
        }

        for (int i = 0; i < roomList.Count; i++)
        {
            Debug.Log("Room List Count = " + roomList.Count);
            if (roomList[i].RemovedFromList) continue;

            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
            Debug.Log("Room Created = " + roomList[i].Name);
        }






        //foreach (var room in new List<string>(activeLobbies.Keys))
        //{
        //    if (!roomList.Exists(r => r.Name == room))
        //    {
        //        Destroy(activeLobbies[room]);
        //        activeLobbies.Remove(room);
        //    }
        //}

        //foreach (RoomInfo roomInfo in roomList)
        //{
        //    if (activeLobbies.ContainsKey(roomInfo.Name))
        //    {
        //        var text = activeLobbies[roomInfo.Name].GetComponentInChildren<TextMeshProUGUI>();
        //        text.text = $"{roomInfo.Name} ({roomInfo.PlayerCount}/{roomInfo.MaxPlayers})";
        //    }
        //    else
        //    {
        //        GameObject entry = Instantiate(LobbyEntryPrefab, LobbyListContainer);
        //        var text = entry.GetComponentInChildren<TextMeshProUGUI>();
        //        text.text = $"{roomInfo.Name} ({roomInfo.PlayerCount}/{roomInfo.MaxPlayers})";

        //        Button btn = entry.GetComponentInChildren<Button>();
        //        string lobbyName = roomInfo.Name;
        //        btn.onClick.AddListener(() => JoinLobbyFromButton(lobbyName));

        //        activeLobbies.Add(roomInfo.Name, entry);
        //    }
        //}
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
