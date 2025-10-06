using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    [field: SerializeField] public Transform[] Spawnpoints {  get; private set; }

    [field: Header("Spawnpoints")]
    [field: SerializeField] public Transform Player1Spawn {  get; private set; }
    [field: SerializeField] public Transform Player2Spawn { get; private set; }
    [field: SerializeField] public Transform Player3Spawn { get; private set; }
    [field: SerializeField] public Transform Player4Spawn { get; private set; }

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
}
