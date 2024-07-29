using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;


public class SwapManager : MonoBehaviour
{
    public List<GameObject> _Obj;
    public Transform _vitriketthuc;
    public float duration = 3f;
    public int _minSL = 5;
    public int _maxSL = 15;
    public int _score = 0;
    public Manager Mangager;
    public AddSrollView SrollView;
    private List<GameObject> _DsObject;
    int count = 0;
    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    public void UPStart()
    {
        _DsObject = new List<GameObject>();
        count = 0;
        int _Count = Random.Range(_minSL, _maxSL);
        _score = _Count;
        for (int i = 0; i < _Count; i++)
        {
            int random = Random.Range(0, _Obj.Count);
            GameObject obj = Instantiate(_Obj[random]);
            obj.transform.position = transform.position;
            obj.transform.parent = transform;
            obj.SetActive(false);
            _DsObject.Add(obj);
            SrollView.AddItem(obj);
        }


        StartCoroutine(MoveObject(_DsObject[count]));
        SrollView.FindChildren();
        count++;
    }

    // Update is called once per frame
  void SwapGameObject()
    {
        if(SrollView.gameObject.transform.childCount > 0)
        {
            SrollView.RemoveItem(count);
        }
      
        //int random = Random.Range(0, _Obj.Count);

        //GameObject obj = Instantiate(_Obj[random]);

        //obj.transform.position = transform.position;
        //obj.transform.parent = transform;

        //StartCoroutine(MoveObject(obj));

        // kiem tra xem da leveUp chua 
        if (Mangager.checkLevelUp)
        {
            
            UPStart();
            Mangager.checkLevelUp = false;
        }
        else 
        {
            StartCoroutine(MoveObject(_DsObject[count]));
          
            count++;
        }
       


       
    }
    IEnumerator MoveObject(GameObject obj)
    {
        obj.SetActive(true);
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            obj.transform.position = Vector3.Lerp(this.transform.position, _vitriketthuc.position, elapsedTime / duration);
            elapsedTime += Time.deltaTime;

            // sang danh theo thoi gian 
 
            yield return null; //choi mot khung hinh moi
        }

        //dam bao toi dich
        obj.transform.position = _vitriketthuc.position;
       // Manager.Instance.checkLevelUp = false;

      //  SrollView.RemoveItem(count);
    }
}
