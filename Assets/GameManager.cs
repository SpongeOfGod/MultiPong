using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public float BorderUpY, BorderDownY = 0;
    private void Awake()
    {
        Instance = this;
    }
}
