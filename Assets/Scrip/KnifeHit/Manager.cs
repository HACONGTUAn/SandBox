using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEngine.ParticleSystem;
public class Manager : MonoBehaviour
{
    public List<Sprite> sprites;
    static public Manager Instance;
    public bool checkLevelUp = false;
    public int _currenLevel = 0;
    // Start is called before the first frame update
    public GameObject bia;
    public TextMeshProUGUI _scoreText;
    public GameObject thungchuadao;
    public int score = 0;
 //   public GameObject swapManager;
    private SaveHightScore _saveHightScore;
    public FlashScene _flash;
    public Transform poindParther;
    public ParticleSystem _particle;
    public AddSrollView SrollView;
    private int checkScoreNew = 0;
    //  private AndroidVibration androidVibration;
    SwapManager spmanager;
   private GameObject _swapManager;
    private void Awake()
    {
        spmanager = GameObject.FindAnyObjectByType<SwapManager>();
        _swapManager = spmanager.gameObject;
    }
    void Start()
    {
        Instance = this;
        CreatGameObject();
        spmanager.UPStart();
        //   androidVibration = FindObjectOfType<AndroidVibration>();
        _saveHightScore = GetComponent<SaveHightScore>();   
    }

 


    void CreatGameObject()
    {
        int _random = Random.Range(0,sprites.Count);
        Sprite _sprite = sprites[_random];

        GameObject _bia = Instantiate(bia);
        _bia.GetComponent<SpriteRenderer>().sprite = _sprite;
       // _bia.transform.position = new Vector3(0f, 1.5f, 0f);
        _bia.transform.SetParent(poindParther);
        // _bia.transform.localScale = Vector3.zero;
        _bia.GetComponent<Animator>().SetTrigger("Appear");
        _scoreText.text = score.ToString();
       // checkLevelUp = false;
    }
    public void RestartGame()
    {
        _flash.StartFlash(.25f ,1, Color.white);
        checkLevelUp = true;
        _currenLevel = 0;
        // androidVibration.Vibrate(1000);
        Handheld.Vibrate();
        StartCoroutine(RestartAfterDelay());
        
    }
    private IEnumerator RestartAfterDelay()
    {

        
        yield return new WaitForSeconds(1.5f);
   
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
       
        checkLevelUp = false;
    }

    public void Scores()
    {
        score++;
        _scoreText.text = score.ToString();
        _scoreText.GetComponent<Animator>().SetTrigger("check");
        checkScoreNew ++;
        if (checkScoreNew == spmanager._score)
        {
            _currenLevel += 1;
            checkLevelUp = true;
            SrollView.DeleteItemAll();
            UpdateLeve();
            checkScoreNew = 0;
            
        }
        if(score > _saveHightScore.GetHightScore())
        {
            _saveHightScore.UpdateHightScore(score);
        }
    }
    private void UpdateLeve()
    {
        GameObject bia = GameObject.FindAnyObjectByType<RotationObject>().gameObject;
     

        foreach (Transform child in bia.transform)
        {
            Debug.Log("Child name: " + child.name);
            child.transform.SetParent(thungchuadao.transform);
            child.GetChild(0).gameObject.GetComponent<ColliderKnife>().SendMessage("DownObject");
            child.GetChild(0).gameObject.GetComponent<ColliderKnife>().enabled = false;
            child.GetComponent<KnifeMove>().enabled = false;
        }
         Destroy(bia);
        _particle.Play();
        StartCoroutine(startCreateGame());


    }
    private IEnumerator startCreateGame()
    {
        yield return new WaitForSeconds(1.5f);
     
        CreatGameObject();
    }

    public void CheckCreatinObject()
    {

        if (checkLevelUp)
        {
            AudioManager.instance.PlayAudio(global::audio.phahuy);
            //startcoroutine(waitingsecond());
            Invoke("waitingSecond", 2f);
        }
        else
        {
            spmanager.SendMessage("SwapGameObject");
        }


    }
    private void waitingSecond()
    {
        Debug.Log("goi qua som");
        spmanager.SendMessage("SwapGameObject");
       // checkLevelUp = false;
    }
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("_Scene_Home");
    }
}
