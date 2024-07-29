using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RotationObject : MonoBehaviour
{
    public GameObject _manager;
    public float smooth = 57f;
    public float tiltAngle = 360.0f;
    [Header("random toc do trong khoang")]
    public float minVeloc = 57f;
    public float maxVeloc = 100;

    [Header("cho phep doi tuong random dao vong hay khong")]
    public bool isRotation = true;
    public bool isMoving = true;
     Animator animator;
    int swapRotation = 1;
    public float timeRandom = 2f;
    public int MaxTimeRandom = 10;
    float currntTime;
    [Header("doi tuong tang giam doi tuong random")]
    public float timeRandomSpeed = 2f;
    public int MaxTimeRandomSpedd = 10;
    float currntTimeSpeed;
    [Header("khi bia bi danh (hit)")]
    [SerializeField] private Color _flashColor = Color.white;
    [SerializeField] private float _flashTime = 0.25f;
    private Material _flash;
    int _currentLevel;
    private void Start()
    {
        currntTime = timeRandom;
        currntTimeSpeed = timeRandomSpeed;
        animator = GetComponent<Animator>();
        Manager manager = GameObject.FindAnyObjectByType<Manager>();
        _manager = manager.gameObject;

        // kiem tra xem level hien tai
        _currentLevel = manager._currenLevel;
        // thiet lap chi so thong qua level
        if (_currentLevel == 0)
        {
            isRotation = false;
            isMoving = false;
        }
        else if(_currentLevel >= 1 && _currentLevel <= 5)
        {
            isRotation = false;
            isMoving = true;
            minVeloc = smooth;
           maxVeloc = smooth + _currentLevel;
        }
        else if(_currentLevel > 5)
        {
            isRotation = true;
            isMoving = true;
            minVeloc = smooth - _currentLevel;
            maxVeloc = smooth + _currentLevel;
        }
}
    
    void Update()
    {
       tiltAngle += Time.deltaTime * smooth * swapRotation;
        Quaternion target = Quaternion.Euler(0,0, tiltAngle);

        // Dampen towards the target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);

        // thay doi huong quay 
        currntTime = currntTime - Time.deltaTime;
        if (currntTime < 0 && isRotation)
        {
            swapRotation *= -1;
            timeRandom = Random.Range(0, MaxTimeRandom);
            currntTime = timeRandom;
            // thay doi toc do cua bia
        }
        currntTimeSpeed -= Time.deltaTime;
        if(currntTimeSpeed < 0  && isMoving)
        {
            smooth = Random.Range(minVeloc, maxVeloc);
            timeRandomSpeed = Random.Range(0, MaxTimeRandomSpedd);
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;
        if(other.tag == "knife")
        {
            AudioManager.instance.PlayAudio(global::audio.trungdich);
            animator.SetTrigger("GetPingPong");
           // Debug.Log("trung dan");
            // cap nhat diem so 
           
            _manager.SendMessage("Scores");
            _manager.SendMessage("CheckCreatinObject");
            StartCoroutine(DamgerFlasher());


        }
    }
    void FlashColerGameObject()
    {
        _flash =  GetComponent<SpriteRenderer>().material;
        _flash.SetColor("_FlashColor", _flashColor);

   
    }
   IEnumerator DamgerFlasher()
    {
        FlashColerGameObject();
        float currentFlashAmount = 0f;
        float elaspedTime = 0f;
        while (elaspedTime < _flashTime)
        {
            elaspedTime += Time.deltaTime;

            currentFlashAmount = Mathf.Lerp(3f, 0f, (elaspedTime / _flashTime));
            SetFlashAmount(currentFlashAmount);
            yield return null;
        }
    }

    void SetFlashAmount(float currentFlashAmount)
    {
        _flash.SetFloat("_FloashAmount", currentFlashAmount);
    }
}
