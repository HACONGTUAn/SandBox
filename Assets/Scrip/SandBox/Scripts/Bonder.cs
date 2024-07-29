using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonder : MonoBehaviour
{
    public Vector2 boundsSize;
    public Vector2 obstacleSize;
    public Vector2 obstacleCentre;
    public List<paticel> A;
 
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var item in A)
        {
            item.FixUpdate();
            item.FixUpdate();
            foreach(var B in A)
            {
                if(B != item)
                {
                    item.Collection(B);
                }
            }
          
            checkBouder(item);
        }
        
     
   
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0, 0.4f);
        Gizmos.DrawWireCube(Vector2.zero, boundsSize);
        Gizmos.DrawWireCube(obstacleCentre, obstacleSize);
    }
    void checkBouder(paticel A)
    {
        if(A.transform.position.x + A._banKinh > (obstacleSize.x / 2) || A.transform.position.x + -A._banKinh < -(obstacleSize.x / 2) )
        {
            A._giatoc.x *= -1;

        }
        if (A.transform.position.y+ A._banKinh > (obstacleSize.y / 2) || A.transform.position.y + -A._banKinh < -(obstacleSize.y / 2))
        {
            A._giatoc.y *= -1;

        }
    }
    
}
