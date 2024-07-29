using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class FlashScene : MonoBehaviour
{
    Coroutine _currenFlashRoutin = null;
    private Image _image;
    // Start is called before the first frame update
    void Start()
    {
        _image = GetComponent<Image>();
    }

   public void StartFlash(float maxTimeFlash, float maxApha, Color colorFlash) {
        _image.color = colorFlash;

        maxApha = Mathf.Clamp(maxApha, 0f, 1f);

        if(_currenFlashRoutin != null )
        {
            StopCoroutine( _currenFlashRoutin );
            
        }
        _currenFlashRoutin = StartCoroutine(Flash(maxTimeFlash, maxApha));
    }
    IEnumerator Flash(float maxTimeFlash, float Apha)
    {
        float currenFrame = maxTimeFlash / 2;
        float curreTime = 0;
        while (curreTime <= currenFrame)
        {
            Color colorThisFrame = _image.color;
            colorThisFrame.a = Mathf.Lerp(0,Apha, curreTime / currenFrame);
            _image.color = colorThisFrame;

            curreTime += Time.deltaTime;
            yield return null;
        }
        curreTime = 0;
        
        // flash out
        while (curreTime <= currenFrame)
        {
            Color colorThisFrame = _image.color;
            colorThisFrame.a = Mathf.Lerp(Apha,0, curreTime / currenFrame);
            _image.color = colorThisFrame;

            curreTime += Time.deltaTime;
           
        }
    }
}
