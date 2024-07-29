using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public enum State
{
    moveDown,
    moveLeft,
    moveRight,
    none,
}
public class Test : MonoBehaviour
{
    public float speed = 1f;
    public float _gravity = 9.8f;
    private Rigidbody2D rb;
  //  public float moveSpeed = 5f;
    private State _currenState = State.moveDown;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;
        SetObject component  = other.GetComponent<SetObject>();
        if (component == null) { Debug.Log("khong co"); }
        else if (other.GetComponent<SetObject>().cell.cellName == "Fire")
        {
            SpriteRenderer spriteRenderer = this.GetComponent<SpriteRenderer>();
            spriteRenderer.color = Color.white;
            Rigidbody2D rg2 = this.GetComponent<Rigidbody2D>();
            rg2.gravityScale = -5f;
        }
        else if (other.GetComponent<SetObject>().cell.cellName == "Virut")
        {
            SpriteRenderer spriteRenderer = this.GetComponent<SpriteRenderer>();
            spriteRenderer.color = other.GetComponent<SetObject>().cell.cellColor;
         
        }

        if (rb.mass != 1)
        {
            if (other.layer == 3)
            {
                rb.mass = 1f;
            }
            if (other.layer == 4)
            {
                rb.mass = rb.mass / 10;
                ContactPoint2D contact = collision.contacts[0];

                Vector2 pointOnCurrentObject = contact.point - (Vector2)transform.position;

                if (pointOnCurrentObject.x < 0)
                {
                    _currenState = State.moveRight;
                    // Va chạm bên trái đối tượng hiện tại
                    Debug.Log("ben trai");
                  //  rb.AddForce(Vector2.right * rb.mass, ForceMode2D.Impulse);

                }
                else if (pointOnCurrentObject.x > 0)
                {
                    _currenState = State.moveLeft;
                    // Va chạm bên phải đối tượng hiện tại
                    Debug.Log("Ben Phai");
                   // rb.AddForce(Vector2.left * rb.mass, ForceMode2D.Impulse);

                }
                //if (pointOnCurrentObject.y > 0)
                //{
                //    Debug.Log("ben tren");
                //    rb.AddForce(Vector2.up * rb.mass, ForceMode2D.Impulse);
                //}
                //else if (pointOnCurrentObject.y < 0)
                //{
                //    Debug.Log("ben duoi");
                //    rb.AddForce(Vector2.down * rb.mass, ForceMode2D.Impulse);
                //}
            }
        }
        else
        {
            if (other.layer == 4)
            {

                ContactPoint2D contact = collision.contacts[0];
                Vector2 pointOnCurrentObject = contact.point - (Vector2)transform.position;

                if (pointOnCurrentObject.x < 0)
                {
                    // Va chạm bên trái đối tượng hiện tại
                    Debug.Log("ben trai");
                    //rb.AddForce(Vector2.right * rb.mass, ForceMode2D.Impulse);
                    _currenState = State.moveLeft;

                }
                else if (pointOnCurrentObject.x > 0)
                {
                    // Va chạm bên phải đối tượng hiện tại
                    Debug.Log("Ben Phai");
                    // rb.AddForce(Vector2.left * rb.mass, ForceMode2D.Impulse);
                    _currenState = State.moveRight;
                }
            }
            if(other.layer == 3 || other.layer == 6)
            {
                ContactPoint2D contact = collision.contacts[0];
                Vector2 pointOnCurrentObject = contact.point - (Vector2)transform.position;

                if (pointOnCurrentObject.x < 0)
                {
                    // Va chạm bên trái đối tượng hiện tại
                    Debug.Log("ben trai");
                    //rb.AddForce(Vector2.right * rb.mass, ForceMode2D.Impulse);
                    _currenState = State.moveRight;

                }
                else if (pointOnCurrentObject.x > 0)
                {
                    // Va chạm bên phải đối tượng hiện tại
                    Debug.Log("Ben Phai");
                    // rb.AddForce(Vector2.left * rb.mass, ForceMode2D.Impulse);
                    _currenState = State.moveLeft;
                }
                //if (pointOnCurrentObject.y > 0)
                //{
                //    Debug.Log("ben tren");
                //    rb.AddForce(Vector2.up * rb.mass, ForceMode2D.Impulse);
                //}
                //else if (pointOnCurrentObject.y < 0)
                //{
                //    Debug.Log("ben duoi");
                //    rb.AddForce(Vector2.down * rb.mass, ForceMode2D.Impulse);
                //}



            }
        }

        

    }
    private void Update()
    {
        switch (_currenState)
        {
          
            case State.moveLeft:
                //  rb.AddForce(Vector2.right *0.3f , ForceMode2D.Impulse);
                Vector3 move = Vector3.right * speed;
                transform.position = transform.position + move * Time.deltaTime;
                break;
            case State.moveRight:
              //  rb.AddForce(Vector2.left * rb.mass, ForceMode2D.Impulse);
                Vector3 moveL = Vector3.left * speed;
                transform.position = transform.position + moveL * Time.deltaTime;
                break;
            default: break;
        }
     //   Debug.Log(Time.deltaTime);
    }
}

