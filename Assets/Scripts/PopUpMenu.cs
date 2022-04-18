using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpMenu : MonoBehaviour
{
    public GameObject popUpMenu;

    public void Open(){
        popUpMenu.SetActive(true);
    }
    public void Close(){
        popUpMenu.SetActive(false);
    }
}
