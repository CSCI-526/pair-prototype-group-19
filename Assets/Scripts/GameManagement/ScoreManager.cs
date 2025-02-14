using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    private static ScoreManager _instance;
    public static ScoreManager Instance { get { return _instance; } }

    [SerializeField]
    private TextMeshProUGUI scoreText;

    [SerializeField]
    private TextMeshProUGUI[] leaderboard;

    [SerializeField]
    private float scoreMultiplier = 1.0f;

    // Current game score
    public float score;

    private string scoreString;


    private float[] highScores = new float[10];

    // Start is called before the first frame update
    void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    void Update()
    {
        if (GameManager.Instance.isPlaying)
        {
            score += Time.fixedDeltaTime * scoreMultiplier;
            scoreText.text = "Score: " + score.ToString("000000000");
        }
    }

    void openLeaderboard()
    {

    }


}
