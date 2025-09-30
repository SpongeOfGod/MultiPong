using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

public class mainmenu : MonoBehaviour
{
    public GameObject roomui;




    public void Play()
    {
        roomui.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void exit()
    {
        Application.Quit();
        Debug.Log("homero");
    }
}


