using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Vine : MonoBehaviour
{
    public float tangchuongX = 1;
    public float tangchuongy = 1;
    public int soluongMin = 3;
    public int soluongMax = 3;
    public float tgc = 1f;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;
        if (other.GetComponent<SetObject>().cell.cellName == "Water")
        {
            ContactPoint2D contact = collision.contacts[0];

            Vector2 pointOnCurrentObject = contact.point - (Vector2)transform.position;

            if (pointOnCurrentObject.x < 0)
            {
                tangchuongX *= -1;
            }
            int soluong = Random.Range(soluongMin, soluongMax);
            for (int i = 0; i < soluong; i++)
            {
                StartCoroutine(CreatVine(i));


            }

            ObjPooling.Instance.ReturnToPool(other);
        }
        if (other.GetComponent<SetObject>().cell.cellName == "Fire")
        {
            Debug.Log("game va cham");
            GameObject gameObject = ObjPooling.Instance.SpawnFromPool("Fire");
            gameObject.transform.position = this.transform.position;
            ObjPooling.Instance.ReturnToPool(this.gameObject);
        }
    }
    IEnumerator CreatVine(int i)
    {
        yield return new WaitForSeconds(tgc);
        GameObject gameObject = ObjPooling.Instance.SpawnFromPool("Vine");
        gameObject.transform.position = transform.position + new Vector3(tangchuongX, tangchuongy, 0) * i;
    }
}
