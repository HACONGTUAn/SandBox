using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatObject : MonoBehaviour
{
    static public CreatObject instance;
    public List<GameObject> objects = new List<GameObject>();
    private void Start()
    {
        instance = this;


    }
    public GameObject CreatCellObject()
    {
        // kiem tra xem doi doi tuong duoc tao se la loai nào 
        switch (objects[CreatParicel.instance._index].GetComponent<SetObject>().cell.cellName)
        {
            case "Water":
                return CreaterWater();
         
            case "Fire":
                return CreateFire();
              
            case "Vine":
                return CreateVine();
               
            case "Virut":
                return CreateVirut();
       
            case "Alcohol":
                return CreateAlcohol();
            
            case "Seed":
                return CreateSeed();
            case "FlammableGases":
                return CreateFlammableGases();
        }

        return null;
    }

    private GameObject CreaterWater()
    {
        //  GameObject vuong = Instantiate(objects[CreatParicel.instance._index]);
        GameObject vuong = ObjPooling.Instance.SpawnFromPool(objects[CreatParicel.instance._index].GetComponent<SetObject>().cell.cellName);
        SpriteRenderer spriteRenderer = vuong.GetComponent<SpriteRenderer>();
        spriteRenderer.color = objects[CreatParicel.instance._index].GetComponent<SetObject>().cell.cellColor;

        Rigidbody2D rg2 = vuong.GetComponent<Rigidbody2D>();
        rg2.mass = 1000f;
        rg2.gravityScale = objects[CreatParicel.instance._index].GetComponent<SetObject>().cell.Gravity;
        rg2.constraints = RigidbodyConstraints2D.FreezeRotation;
        rg2.drag = 0;
        rg2.angularDrag = 0;
        return vuong;
    }


    private GameObject CreateFire()
    {
        GameObject vuong = ObjPooling.Instance.SpawnFromPool(objects[CreatParicel.instance._index].GetComponent<SetObject>().cell.cellName);
        SpriteRenderer spriteRenderer = vuong.GetComponent<SpriteRenderer>();
        spriteRenderer.color = objects[CreatParicel.instance._index].GetComponent<SetObject>().cell.cellColor;

      //  Rigidbody2D rg2 = vuong.GetComponent<Rigidbody2D>();
     //   rg2.bodyType = RigidbodyType2D.Kinematic;
     //   rg2.gravityScale = objects[CreatParicel.instance._index].GetComponent<SetObject>().cell.Gravity;
        return vuong;
    }

    private GameObject CreateVine() {

        GameObject vuong = ObjPooling.Instance.SpawnFromPool(objects[CreatParicel.instance._index].GetComponent<SetObject>().cell.cellName);
        SpriteRenderer spriteRenderer = vuong.GetComponent<SpriteRenderer>();
        spriteRenderer.color = objects[CreatParicel.instance._index].GetComponent<SetObject>().cell.cellColor;
        return vuong;
    }
    private GameObject CreateVirut()
    {
        GameObject vuong = ObjPooling.Instance.SpawnFromPool(objects[CreatParicel.instance._index].GetComponent<SetObject>().cell.cellName);
        SpriteRenderer spriteRenderer = vuong.GetComponent<SpriteRenderer>();
        spriteRenderer.color = objects[CreatParicel.instance._index].GetComponent<SetObject>().cell.cellColor;
        return vuong;
    }

    private GameObject CreateAlcohol()
    {
        GameObject vuong = ObjPooling.Instance.SpawnFromPool(objects[CreatParicel.instance._index].GetComponent<SetObject>().cell.cellName);
        SpriteRenderer spriteRenderer = vuong.GetComponent<SpriteRenderer>();
        spriteRenderer.color = objects[CreatParicel.instance._index].GetComponent<SetObject>().cell.cellColor;
        return vuong;
    }

    private GameObject CreateSeed() {

        GameObject vuong = ObjPooling.Instance.SpawnFromPool(objects[CreatParicel.instance._index].GetComponent<SetObject>().cell.cellName);
        SpriteRenderer spriteRenderer = vuong.GetComponent<SpriteRenderer>();
        spriteRenderer.color = objects[CreatParicel.instance._index].GetComponent<SetObject>().cell.cellColor;
        return vuong;
    }
    private GameObject CreateFlammableGases()
    {
        GameObject vuong = ObjPooling.Instance.SpawnFromPool(objects[CreatParicel.instance._index].GetComponent<SetObject>().cell.cellName);
        SpriteRenderer spriteRenderer = vuong.GetComponent<SpriteRenderer>();
        spriteRenderer.color = objects[CreatParicel.instance._index].GetComponent<SetObject>().cell.cellColor;
        Rigidbody2D rg2 = vuong.GetComponent<Rigidbody2D>();
        //   rg2.bodyType = RigidbodyType2D.Kinematic;
       // rg2.gravityScale = objects[CreatParicel.instance._index].GetComponent<SetObject>().cell.Gravity;
        return vuong;
    }
    //Sprite CreateSprite()
    //{

    //    Texture2D texture = new Texture2D(10, 10);
    //    Color[] pixels = new Color[10 * 10];
    //    for (int i = 0; i < pixels.Length; i++)
    //    {
    //        if (objects[CreatParicel.instance._index].GetComponent<SetObject>().cell.cellColor == null)
    //        {
    //            pixels[i] = Color.white;
    //        }
    //        else
    //        {
    //            pixels[i] = objects[CreatParicel.instance._index].GetComponent<SetObject>().cell.cellColor;
    //        }


    //        //   pixels[i] = Color.blue;
    //    }
    //    texture.SetPixels(pixels);
    //    texture.Apply();


    //    return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    //}

}
