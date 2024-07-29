using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CreatParicel : MonoBehaviour
{
    static public CreatParicel instance;
   // public List<CellObject> _listCellObject;
    public List<Button> _onClickButton;
    public int _index = 0;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        // Thêm EventListener cho từng Button trong List
        foreach (Button button in _onClickButton)
        {
            button.onClick.AddListener(() => OnButtonClick(button));
        }
    }

    void OnButtonClick(Button clickedButton)
    {
      
        // Kiểm tra xem clickedButton là Button nào trong List
        int index = _onClickButton.IndexOf(clickedButton);
        if (index != -1)
        {
            _index = index;
           // Debug.Log("Button " + clickedButton.gameObject.name + " at index " + index + " is clicked." + _listCellObject[index].cellName);
           
        }
    }
}
