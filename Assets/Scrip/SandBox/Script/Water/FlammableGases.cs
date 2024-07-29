using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlammableGases : MonoBehaviour
{

    public float speed = 1f;
    public float time = 2f;
    public int BanKinh = 90;
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
        int randomValue = Random.Range(0, 2) == 0 ? -BanKinh : BanKinh;
       
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
        int randomIndex = Random.Range(0, 2);

        Vector3 selectedVector = (randomIndex == 0) ? Vector3.left : Vector3.right;
        while (currentTime < time)
        {
            this.transform.position = transform.position + (selectedVector + vectorGoc + Vector3.up) * Time.deltaTime * speed;

            currentTime += Time.deltaTime;
            yield return null;
        }

        ObjPooling.Instance.ReturnToPool(this.gameObject);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;
        SetObject component = other.GetComponent<SetObject>();
        if (component == null) { Debug.Log("khong co"); }

        else if (other.GetComponent<SetObject>().cell.cellName == "Fire" && component != null)
        {
            Debug.Log("game va cham");
            for(int i = 0; i < 4; i++)
            {
                GameObject gameObject = ObjPooling.Instance.SpawnFromPool("Fire");
                gameObject.transform.position = this.transform.position + Vector3.up * i * 0.1f;
               
            }
            ObjPooling.Instance.ReturnToPool(this.gameObject);
        }
    }
}
