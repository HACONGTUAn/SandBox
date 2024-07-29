using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.UI;

enum Huong
{
    up,
    down,
    left,
    right,
    upleft,
    upright,
    downleft,
    downright,
}
public class SimulateCells : MonoBehaviour
{
    public Texture2D texture;
    public float updateSpeed;
    public bool doTheThing;

    Color pixel;

    Color up;
    Color down;
    Color left;
    Color right;
    Color upLeft;
    Color upRight;
    Color downLeft;
    Color downRight;

    int texCoordX;
    int texCoordY;
    
    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Debug.Log("bam nut");

            for(int y = 0; y < texture.height; y++)
            {
                for(int x = 0; x < texture.width; x++)
                {
                    texCoordX = x;
                    texCoordY = y;

                    pixel = texture.GetPixel(texCoordX, texCoordY);

                    down = texture.GetPixel(texCoordX, texCoordY - 1);
                    downLeft = texture.GetPixel(texCoordX - 1, texCoordY - 1);
                    downRight = texture.GetPixel(texCoordX + 1, texCoordY - 1);
                    up = texture.GetPixel(texCoordX, texCoordY + 1);
                    upLeft = texture.GetPixel(texCoordX - 1, texCoordY + 1);
                    upRight = texture.GetPixel(texCoordX + 1, texCoordY + 1);
                    left = texture.GetPixel(texCoordX - 1, texCoordY);
                    right = texture.GetPixel(texCoordX + 1, texCoordY);

                    if (down == Color.white)
                    {
                        texture.SetPixel(texCoordX, texCoordY, down);
                        texture.SetPixel(texCoordX, texCoordY - 1, pixel);
                       
                    }
                }
            }
            texture.Apply();
        }
       

    }

    //private IEnumerator Start()
    //{
        
    //}


}
