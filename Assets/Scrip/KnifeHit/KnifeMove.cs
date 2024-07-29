using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class KnifeMove : MonoBehaviour
{
    public float todobandau = 5f;
    public float tocdocongthem = 5f;
    public ColliderKnife checkCollider;
    Rigidbody2D rb2;
    public bool check = true;
    // Start is called before the first frame update
    void Start()
    {
        rb2 = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
  
     void PhiDao()
    {
       
            StartCoroutine(TangToc());
        
       
    }
    IEnumerator TangToc()
    {
        while (check)
        {
            rb2.velocity = Vector3.up * todobandau;
            todobandau += tocdocongthem *Time.deltaTime;
            yield return null;
        }
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject ob = collision.gameObject;
        if(ob.tag == "bang") {           
            rb2.velocity = Vector3.zero;
            RotationObject rotationObject = GameObject.FindAnyObjectByType<RotationObject>();
            GameObject parent = rotationObject.gameObject;
            this.transform.SetParent(parent.transform);
            check = false;
            rb2.bodyType = RigidbodyType2D.Kinematic;
            checkCollider._check = false;
        }
    }
}
