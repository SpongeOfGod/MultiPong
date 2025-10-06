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

        float r = Random.Range(0f, 1f);
        float g = Random.Range(0f, 1f);
        float b = Random.Range(0f, 1f);

        PV.RPC("RPC_ChangePlayerColor", RpcTarget.All, r, g, b);
    }

    [PunRPC]
    private void RPC_ChangePlayerColor(float r, float g, float b)
    {
        Color newColor = new Color(r, g, b);

        Debug.Log(MeshRenderer);
        MaterialPropertyBlock block = new();
        MeshRenderer.GetPropertyBlock(block);
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
