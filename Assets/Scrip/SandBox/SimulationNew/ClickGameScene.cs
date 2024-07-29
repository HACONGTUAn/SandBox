using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum Status
{
    water,
    fire,
    vine,
    virut,
    alcohol,
    lightning,
    flammable,
    eraser,
    sunshine,
    seed,
    blackhole,
    Nuclearbomb
}
public class ClickGameScene : MonoBehaviour
{
    [Header("tao khu vuc mo phong")]
    [Tooltip("dat vi tri cho khu vuc mo phong")]
    public Vector2 obstacleCentre;
    [Tooltip("dat kich thuoc ")]
    public Vector2 boundsSize;
    [Tooltip("ban Kinh vung cham tay")]
    public float interactionRadius;

    Status status = Status.water;
    public DisplayGame simulationWater;
    public DisplayGame1 simulationFire;
    Dictionary<int,Status> keyValuePairs = new Dictionary<int,Status>();

   // public bool water = true;
  //  public bool fire = false;
  //  public bool isclickone = false;
    public float top;
    public float bottom;
    public float left;
    public float right;

    public GameObject pont1;
    public GameObject pont2;

    public Button[] ListBtn = new Button[12];
    bool check = false;
 
    private void Awake()
    {
        right = pont1.transform.position.x;
        left = pont2.transform.position.x;
        bottom = pont2.transform.position.y;
        top = pont1.transform.position.y;

        int screenWidth = Screen.width;
        int screenHeight = Screen.height;
        boundsSize.x = Mathf.Abs(pont2.transform.position.x) + pont1.transform.position.x;
        boundsSize.y = pont1.transform.position.y + Mathf.Abs(pont2.transform.position.y);
       // obstacleCentre.y = pont2.transform.position.y;
        Debug.Log(pont2.transform.position.x + " " + pont1.transform.position.x + " " + pont1.transform.position.y + " " + pont2.transform.position.y);
        Debug.Log($"Kích thước màn hình: {screenWidth}x{screenHeight}");


        // Thêm các đối tượng vào keyValuePairs
        keyValuePairs.Add(0, Status.water);
        keyValuePairs.Add(1, Status.fire);
        keyValuePairs.Add(2, Status.vine);
        keyValuePairs.Add(3, Status.virut);
        keyValuePairs.Add(4, Status.alcohol);
        keyValuePairs.Add(5, Status.lightning);
        keyValuePairs.Add(6, Status.flammable);
        keyValuePairs.Add(7, Status.eraser);
        keyValuePairs.Add(8, Status.sunshine);
        keyValuePairs.Add(9, Status.seed);
        keyValuePairs.Add(10, Status.blackhole);
        keyValuePairs.Add(11, Status.Nuclearbomb);

        // Đảm bảo bạn chỉ sử dụng các khóa hợp lệ
        for (int i = 0; i < ListBtn.Length; i++)
        {
            int index = i; // Capture the value of i
            ListBtn[i].onClick.AddListener(() => ClickOnButton(index));
        }
    }

    private void LateUpdate()
    {
#if !UNITY_EDITOR
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
            bool isPullInteraction = touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved;
            bool isInteracting = isPullInteraction;

            if (isInteracting && CheckAreaBound(touchPos) && water)
            {
                simulationWater.AddInstances(1, touchPos);
            }
            else if (isInteracting && CheckAreaBound(touchPos) && fire)
            {
               
                simulationFire.AddInstances(1, touchPos);
            }
        }
#else
        if (Input.GetMouseButton(0))
        {
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (CheckAreaBound(touchPos))
            {
                switch (status)
                {
                    case Status.water:
                        simulationWater.AddInstances(1, touchPos);
                        if (!check)
                        {
                            ClickOnButton(0);
                            check = true;
                        }
                        break;
                    case Status.fire:
                        simulationFire.AddInstances(1, touchPos);
                        check = true;
                        break;
                    case Status.vine:
                        break;
                    case Status.virut:
                        break;
                    case Status.alcohol:
                        break;
                    case Status.lightning:
                        break;
                    case Status.flammable:
                        break;
                    case Status.eraser:
                        break;
                    case Status.sunshine:
                        break;
                    case Status.seed:
                        break;
                    case Status.blackhole:
                        break;
                    case Status.Nuclearbomb:
                        break;

                }
            }

        }
#endif
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0, 0.4f);
        Gizmos.DrawWireCube(obstacleCentre, boundsSize);

        if (Application.isPlaying && Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
            bool isPullInteraction = touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved;
            bool isInteracting = isPullInteraction;

            if (isInteracting && CheckAreaBound(touchPos))
            {
                Gizmos.color = isPullInteraction ? Color.green : Color.red;
                Gizmos.DrawWireSphere(touchPos, interactionRadius);
            }
        }
    }

    bool CheckAreaBound(Vector2 pos)
    {
        return left <= pos.x && pos.x <= right && bottom <= pos.y && pos.y <= top;
    }

    void ClickOnButton(int i)
    {
        ControllerSImulation.Instance.normalparticles[i].statusOn = true;
         status = keyValuePairs[i];
    }
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("_Scene_Home");
    }
}
