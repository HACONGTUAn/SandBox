using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddSrollView : MonoBehaviour
{
    public GameObject content;

    private int stringName = 0;
    Transform[] children;
    private void Start()
    {
       
    }

    public void AddItem(GameObject obj)
    {
        string name = stringName.ToString();
        GameObject image = new GameObject(name);
        image.transform.localScale = new Vector3(-0.3f, 0.5f, 0f);
        image.transform.rotation = Quaternion.Euler(0, 180, 90);
       Image _Ig = image.AddComponent<Image>();
        _Ig.sprite = obj.GetComponent<SpriteRenderer>().sprite;
        image.transform.SetParent(content.transform);
        stringName++;
    }
    public void FindChildren()
    {
        children = content.GetComponentsInChildren<Transform>();
    }
    public void RemoveItem(int index)
    {
       
     //   Debug.Log(children.Length + "" +index);
        children[index].gameObject.SetActive(false);
    }
    public void DeleteItemAll()
    {
        for (int i = 1; i < children.Length; i++)
        {
            Destroy(children[i].gameObject); // Hủy đối tượng con
        }
       // itemList.Clear(); // Xóa toàn bộ danh sách
    }
}
