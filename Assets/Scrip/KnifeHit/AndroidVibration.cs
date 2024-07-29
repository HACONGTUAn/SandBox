using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidVibration : MonoBehaviour
{
    private AndroidJavaObject vibrator;

    void Start()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        // Lấy đối tượng Vibrator từ hệ thống Android
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
#endif
    }

    public void Vibrate(long milliseconds)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        // Gọi phương thức rung trên Android
        if (vibrator != null)
        {
            vibrator.Call("vibrate", milliseconds);
        }
#else
        // Sử dụng Handheld.Vibrate() trên các nền tảng khác hoặc trong Unity Editor
        Handheld.Vibrate();
#endif
    }

    public void VibratePattern(long[] pattern, int repeat)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        // Gọi phương thức rung theo mẫu trên Android
        if (vibrator != null)
        {
            vibrator.Call("vibrate", pattern, repeat);
        }
#else
        // Sử dụng Handheld.Vibrate() trên các nền tảng khác hoặc trong Unity Editor
        Handheld.Vibrate();
#endif
    }

    public void CancelVibration()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        // Hủy rung trên Android
        if (vibrator != null)
        {
            vibrator.Call("cancel");
        }
#endif
    }
}
