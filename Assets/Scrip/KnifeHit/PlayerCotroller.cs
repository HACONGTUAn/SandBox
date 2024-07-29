using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerCotroller : MonoBehaviour 
{

    public GameObject swapManager;
    Transform transfrom_dao;
    GameObject dao;
    public void Start()
    {
        swapManager = GameObject.Find("SwapManager");
       // transfrom_dao = swapManager.transform.GetChild(0);
       // dao = transfrom_dao.gameObject;
    }
    private void Update()
    {
        
        
        if (Input.GetMouseButtonDown(0) && !Manager.Instance.checkLevelUp)
        {
            transfrom_dao = swapManager.transform.GetChild(0);
            dao = transfrom_dao.gameObject;
        }
        // Kiểm tra nếu nút chuột trái được thả ra
        if (Input.GetMouseButtonUp(0) && !Manager.Instance.checkLevelUp)
        {
           // Debug.Log("Left mouse button released");
            dao.SendMessage("PhiDao");


        }
    }
}
