using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtonScripts : MonoBehaviour
{
    public void Play()
    {
        GameManager.Instance.loadSceneByIndex(1);
    }

    public void Pause()
    {
        GameManager.Instance.setGameSpeed(0.0f);
    }

    public void Unpause()
    {
        GameManager.Instance.setGameSpeed(1.0f);
    }

    public void OpenLeaderboard()
    {
        ScoreManager.Instance.openLeaderboard();
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void DeleteScores()
    {
        PlayerPrefs.DeleteAll();
    }
}
