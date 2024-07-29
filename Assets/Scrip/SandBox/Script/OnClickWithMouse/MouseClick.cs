using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseClick : MonoBehaviour
{
    public float TimeSwapn = 0.5f;
    float currentTime = 0f;
   // public Transform _simulation;
   // public Vector2 init_speed = new Vector2(1.0f, 0.0f);
   // public Sprite mySprite;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if(Input.GetMouseButton(0)  && !EventSystem.current.IsPointerOverGameObject() ) {
            Vector3 pos = Input.mousePosition;

            Vector3 mouseInWorld = Camera.main.ScreenToWorldPoint(pos);
            mouseInWorld.z = 0;

            if (currentTime > TimeSwapn)
            {

                GameObject vuong = CreatObject.instance.CreatCellObject();
                vuong.layer = 4;
                vuong.transform.position = mouseInWorld;
                // update the particle's position
                
                //vuong.GetComponent<Particle>().pos = mouseInWorld;
                //vuong.GetComponent<Particle>().previous_pos = mouseInWorld;
                //vuong.GetComponent<Particle>().visual_pos = mouseInWorld;
                //vuong.GetComponent<Particle>().vel = init_speed;


                // vuong.transform.parent = _simulation;


                currentTime = 0f;

            }
        }
    }

   
    /*
    GameObject CreatePartialWater()
    {
        GameObject vuong = new GameObject();
        SpriteRenderer spriteRenderer = vuong.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = CreateSprite();
       // spriteRenderer.sprite = mySprite;
        vuong.AddComponent<BoxCollider2D>();
        BoxCollider2D boxCollider = vuong.AddComponent<BoxCollider2D>();
        boxCollider.size = new Vector2(0.1f, 0.1f);
        Rigidbody2D rg2 = vuong.AddComponent<Rigidbody2D>();
        rg2.gravityScale = 1;
        rg2.constraints = RigidbodyConstraints2D.FreezeRotation;
       vuong.AddComponent<Particle>();
        return vuong;
    }
    */
}
