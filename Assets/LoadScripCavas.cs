using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LoadScripCavas : MonoBehaviour
{
    public GameObject loaderUI;
    public Slider progressSlider;

    public Button button1;
    public Button button2;

    [Header("tu dong")]
    public Image imgaeItem;
    public Text nameGame;
    void Start()
    {
        if (button1 != null)
        {
            button1.onClick.AddListener(() => OnButtonClick("Throwing Knife"));
        }

        if (button2 != null)
        {
            button2.onClick.AddListener(() => OnButtonClick("SandBox"));
        }
    }

    void OnButtonClick(string buttonName)
    {
        Debug.Log(buttonName + " was clicked!");
        GameObject other = GameObject.Find(buttonName);
        imgaeItem.sprite = other.transform.Find("ImageITem").GetComponent<Image>().sprite;
        nameGame.text = other.transform.Find("NameGame").GetComponent<Text>().text;
    }

    public void LoadScene(int indexer)
    {
        StartCoroutine(LoadScene_Coroutine(indexer));
    }
    public IEnumerator LoadScene_Coroutine(int index)
    {
        progressSlider.value = 0;
        loaderUI.SetActive(true);

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(index);
        asyncOperation.allowSceneActivation = false;

        float progress = 0;
        while (!asyncOperation.isDone)
        {
            progress = Mathf.MoveTowards(progress,asyncOperation.progress,Time.deltaTime);
            progressSlider.value = progress;
            if(progress >= 0.9f)
            {
                progressSlider.value = 1;
                asyncOperation.allowSceneActivation = true;
            }
            yield return null;
        }

    }

   
}
