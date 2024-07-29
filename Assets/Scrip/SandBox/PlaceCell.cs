using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class PlaceCell : MonoBehaviour
{
    public Texture2D texture;
    public GameObject test;
    public bool check = false;
    // Start is called before the first frame update
    void Start()
    {
        for(int x = 0; x < texture.width; x++)
        {
            for(int y = 0; y < texture.height; y++)
            {
                if (x == texture.width - 1 || x == 0 || y == 0 || y == texture.height - 1)
                {
                    texture.SetPixel(x, y, Color.black);
                }
                else if (texture.GetPixel(x, y) != Color.white)
                {
                    texture.SetPixel(x, y, Color.white);
                }
            }
        }
       
       // texture.Apply();
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Input.GetMouseButton(0))
        {
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector2 pixelUV = hit.textureCoord;
                pixelUV.x *= texture.width;
                pixelUV.y *= texture.height;

                if (hit.transform.gameObject.layer != LayerMask.NameToLayer("UI"))
                {
                    texture.SetPixel((int)pixelUV.x, (int)pixelUV.y, Color.red);  
                }
            }
        }
     
        texture.Apply();
    }
}
