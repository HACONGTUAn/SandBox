using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class paticel : MonoBehaviour
{
    public Vector3 _position;
    public Vector2 _velocity;
    public Vector2 _giatoc;
    public float _banKinh;
    public float _khoiluong;
    public float _t = 0.01f;
    // Start is called before the first frame update
    void Start()
    {
        _position = transform.position;
       // _velocity = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
       // _giatoc = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        _khoiluong = Random.Range(1f, 5f);
        
    }

    // Update is called once per frame
   public void FixUpdate()
    {
       
            ThayDoiVanToc(this._giatoc);
            ThayDoiViTri(this._velocity);
           
    }

    private void ThayDoiViTri(Vector2 velocity)
    {
        transform.position = this._position + (Vector3)velocity;
    }

    private void ThayDoiVanToc(Vector2 giatoc)
    {
        this._velocity = _velocity + giatoc;
    }
    public void Collection(paticel other)
    {
        float distance = Vector3.Distance(this.transform.position,other.gameObject.transform.position);
        if (distance < this._banKinh + other._banKinh)
        {
            Debug.Log("va cham");
           var mSub = this._khoiluong + other._khoiluong;
            var impact = other.gameObject.transform.position - this.transform.position;
            var vDiff = other._velocity - this._velocity;

            var numA = 2 * other._khoiluong * vDiff * (Vector2)impact * (Vector2)impact;
            var denA = mSub * distance * distance ;

          //  Debug.Log(numA +" "+ denA);
            Vector2 deltav = new Vector2(numA.x/denA ,numA.y /denA ) * 0.1f;
            Debug.Log(deltav);
            this._giatoc = deltav;
            this.ThayDoiVanToc(deltav);

            var numB = 2 * this._khoiluong * vDiff * (Vector2)impact * (Vector2)impact;
            var denB = mSub * distance * distance;
             deltav = new Vector2(numB.x / denB, numB.y / denB) * -1 * 0.1f;
            other._giatoc += deltav;
            other.ThayDoiVanToc(deltav);


        }
        else
        {
            return;
        }
    }
}
