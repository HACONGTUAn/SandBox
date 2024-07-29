using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public float speed = 5f;
    public float time = 2f;
    public float BanKinh = 15f;
    public float timeTD = 0.5f;
    private float currentTime = 0f;
    private float goc;
    private Vector3 vectorGoc = Vector3.zero;
    // Start is called before the first frame update
    //void Start()
    //{
    //     goc = Random.Range(-BanKinh, BanKinh);
    //    Debug.Log(Mathf.Sin(goc * Mathf.Deg2Rad) );
    //    vectorGoc = new Vector3(Mathf.Sin(goc * Mathf.Deg2Rad), 0, 0);
    //    StartCoroutine(CheckTime());
      
    //}
    void OnEnable()
    {
        goc = Random.Range(-BanKinh, BanKinh);

        vectorGoc = new Vector3(Mathf.Sin(goc * Mathf.Deg2Rad), 0, 0);
        Invoke("Started", timeTD);
       
    }
    private void OnDisable()
    {
        currentTime = 0f;
    }
     void Started()
    {
        if (!gameObject.activeInHierarchy)
        {
            return;
        }
        else
        {
            StartCoroutine(CheckTime());
        }
       
    }
    IEnumerator CheckTime()
    {
        while ( currentTime < time)
        {
            this.transform.position = transform.position +( Vector3.up + vectorGoc )* Time.deltaTime * speed;
           
            currentTime += Time.deltaTime;
            yield return null;
        }

        ObjPooling.Instance.ReturnToPool(this.gameObject);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
}
