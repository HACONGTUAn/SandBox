using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderKnife : MonoBehaviour
{
    public bool _check = true;
    public float _speedDown = 10f;
    public Vector3 _forceAtPosition = Vector3.zero;
    public Vector3 _vitritacdung = Vector3.zero;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;
        if(other.tag == "knife" && _check)
        {
            AudioManager.instance.PlayAudio(global::audio.vacham);

            Vector2 otherPosition = other.transform.position;
            Vector2 thisPosition = transform.position;

            // Xác định vị trí va chạm
            if (otherPosition.x < thisPosition.x)
            {
                _forceAtPosition = _forceAtPosition;
            }
            else
            {
                _forceAtPosition = -_forceAtPosition;
            }

           
                Debug.Log("Game OVer");
            DownObject();

           Manager manager = GameObject.FindAnyObjectByType<Manager>();
            GameObject _nager = manager.gameObject;
            _nager.SendMessage("RestartGame");
        }
    }

    public void DownObject()
    {
        
        Rigidbody2D rb = transform.parent.GetComponent<Rigidbody2D>();
        if(rb.bodyType == RigidbodyType2D.Kinematic)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
        KnifeMove knifeMove = rb.GetComponent<KnifeMove>();

        rb.velocity = Vector3.down * _speedDown;
        rb.AddForceAtPosition(_forceAtPosition, _vitritacdung);

        knifeMove.check = false;
    }
}
