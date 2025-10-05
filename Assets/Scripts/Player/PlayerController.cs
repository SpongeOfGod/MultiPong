using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    PhotonView PhotonView;
    MeshRenderer MeshRenderer;
    private void Awake()
    {
        PhotonView = GetComponent<PhotonView>();
        MeshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        Debug.Log(MeshRenderer);
        MaterialPropertyBlock block = new();
        MeshRenderer.GetPropertyBlock(block);
        Color newColor = new Color(Random.Range(0f, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
        Debug.Log(newColor);
        block.SetColor("_Color", newColor);
        MeshRenderer.SetPropertyBlock(block);
    }
    public float velocity = 0;

    void Update()
    {
        if (!PhotonView.IsMine) return;

        var inputY = Input.GetAxisRaw("Vertical");
        var actualPosition = transform.position;

        var amountToMove = inputY * velocity * Time.deltaTime;


        if (inputY > 0 && actualPosition.y + amountToMove >= GameManager.Instance.BorderUpY ||
            inputY < 0 && actualPosition.y + amountToMove <= GameManager.Instance.BorderDownY)
            amountToMove = 0;


        actualPosition.y += amountToMove;
        transform.position = actualPosition;
    }
}
