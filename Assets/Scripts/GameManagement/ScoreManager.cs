using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    private static ScoreManager _instance;
    public static ScoreManager Instance { get { return _instance; } }

    [Header("Panels")]
    //[SerializeField] private GameObject scorePanel;
    [SerializeField] private GameObject leaderboardPanel;
    [Header("TMPs")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI[] leaderboardTexts;

    [SerializeField]
    private float scoreMultiplier = 1.0f;

    // Current game score
    public float score;

    private string scoreString;
    private float[] highScores = new float[5];

    // Start is called before the first frame update
    void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
            // Create PlayerPrefs on game load if doesn't exist, else set highScores array
            if (!PlayerPrefs.HasKey("Leaderboard1"))
            {
                PlayerPrefs.SetFloat("Leaderboard1", 0.0f);
                PlayerPrefs.SetFloat("Leaderboard2", 0.0f);
                PlayerPrefs.SetFloat("Leaderboard3", 0.0f);
                PlayerPrefs.SetFloat("Leaderboard4", 0.0f);
                PlayerPrefs.SetFloat("Leaderboard5", 0.0f);
            }
            else
            {
                highScores[0] = PlayerPrefs.GetFloat("Leaderboard1");
                highScores[1] = PlayerPrefs.GetFloat("Leaderboard2");
                highScores[2] = PlayerPrefs.GetFloat("Leaderboard3");
                highScores[3] = PlayerPrefs.GetFloat("Leaderboard4");
                highScores[4] = PlayerPrefs.GetFloat("Leaderboard5");
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            wipeScores();
        }
        if (GameManager.Instance.isPlaying)
        {
            score += Time.fixedDeltaTime * scoreMultiplier;
            scoreText.text = "Score: " + score.ToString("000000000");
        }
        else
        {
            scoreText.text = "";
        }
    }

    void updateHighScores()
    {
        bool added = false;
        for (int i = 0; i < highScores.Length; i++)
        {
            if (!added && score > highScores[i])
            {
                for (int j = 4; j > i; --j)
                {
                    highScores[j] = highScores[j - 1];
                }
                highScores[i] = score;
                added = true;
            }
        }
    }

    void updatePlayerPrefs()
    {
        PlayerPrefs.SetFloat("Leaderboard1", highScores[0]);
        PlayerPrefs.SetFloat("Leaderboard2", highScores[1]);
        PlayerPrefs.SetFloat("Leaderboard3", highScores[2]);
        PlayerPrefs.SetFloat("Leaderboard4", highScores[3]);
        PlayerPrefs.SetFloat("Leaderboard5", highScores[4]);
    }

    void updateLeaderboard()
    {
        updateHighScores();
        updatePlayerPrefs();
        for (int i = 0; i < leaderboardTexts.Length; i++)
        {
            leaderboardTexts[i].text = (i+1) + ". " + highScores[i].ToString("000000000");
        }
    }

    //public void openScorePanel()
    //{
    //    scorePanel.SetActive(true);
    //}

    //public void closeScorePanel()
    //{
    //    scorePanel.SetActive(false);
    //}

    public void openLeaderboard()
    {
        updateLeaderboard();
        leaderboardPanel.SetActive(true);
    }
    public void closeLeaderboard()
    {
        leaderboardPanel.SetActive(true);
        scoreText.gameObject.SetActive(false);
    }

    public void wipeScores()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetFloat("Leaderboard1", 0.0f);
        PlayerPrefs.SetFloat("Leaderboard2", 0.0f);
        PlayerPrefs.SetFloat("Leaderboard3", 0.0f);
        PlayerPrefs.SetFloat("Leaderboard4", 0.0f);
        PlayerPrefs.SetFloat("Leaderboard5", 0.0f);
        highScores[0] = PlayerPrefs.GetFloat("Leaderboard1");
        highScores[1] = PlayerPrefs.GetFloat("Leaderboard2");
        highScores[2] = PlayerPrefs.GetFloat("Leaderboard3");
        highScores[3] = PlayerPrefs.GetFloat("Leaderboard4");
        highScores[4] = PlayerPrefs.GetFloat("Leaderboard5");
    }

    // Contextual Close button
    public void onClose()
    {
        leaderboardPanel.SetActive(false);
        if (GameManager.Instance.sceneIndex == 1)
        {
            score = 0.0f;
            GameManager.Instance.loadSceneByIndex(0);
        }
    }

    // Save PlayerPrefs on game end
    private void OnDisable()
    {
        PlayerPrefs.Save();
    }
}
