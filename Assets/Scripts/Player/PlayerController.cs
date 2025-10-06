using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviourPunCallbacks
{
    [SerializeField] private float velocity;

    private PhotonView PV;
    private MeshRenderer MeshRenderer;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        MeshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        if (!PV.IsMine) return;

        PV.RPC("RPC_ChangePlayerColor", RpcTarget.All);
    }

    [PunRPC]
    private void RPC_ChangePlayerColor()
    {
        Debug.Log(MeshRenderer);
        MaterialPropertyBlock block = new();
        MeshRenderer.GetPropertyBlock(block);
        Color newColor = new Color(Random.Range(0f, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
        Debug.Log(newColor);
        block.SetColor("_Color", newColor);
        MeshRenderer.SetPropertyBlock(block);
    }

    void Update()
    {
        if (!PV.IsMine) return;

        var inputY = Input.GetAxisRaw("Vertical");
        var actualPosition = transform.position;

        var amountToMove = inputY * velocity * Time.deltaTime;


        if (inputY > 0 && actualPosition.y + (transform.lossyScale.y / 2) + amountToMove >= GameManager.Instance.BorderUpY - 0.05f ||
            inputY < 0 && actualPosition.y - (transform.lossyScale.y / 2) + amountToMove <= GameManager.Instance.BorderDownY + 0.05f)
            amountToMove = 0;


        actualPosition.y += amountToMove;
        transform.position = actualPosition;
    }
}
