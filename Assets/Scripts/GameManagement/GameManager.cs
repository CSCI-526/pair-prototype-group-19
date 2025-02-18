using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }


    [SerializeField]
    [Range(0.0f, 1.0f)]
    [Tooltip("The game speed of the whole application (affects anything using Time)")]
    private float gameSpeed = 1.0f;
    [SerializeField]
    private GameObject inputPanel;

    [SerializeField]
    private List<Tile> availableTiles;

    // a list of ints that gets translated to input for the player
    public List<int> tileInputs = new List<int>();

    private Tile.Direction directions;

    //public UnityEvent updateInputs;
    [SerializeField] private Player player;

    [SerializeField]
    private bool debugMode = false;

    public bool isPlaying = false;
    public int sceneIndex = 0;

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

    private void Start()
    {
        Debug.Log("GameManager Initialized");
    }

    void OnPlaySceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            isPlaying = true;
            if (inputPanel == null) inputPanel = GameObject.Find("InputPanel");
            if (player == null) player = GameObject.Find("Player").GetComponent<Player>();
            tileInputs = new List<int>();
            sceneIndex = 1;
        }
        else
        {
            isPlaying = false;
            sceneIndex = 0;
        }
    }

    void OnEnable()
    {
        Debug.Log("OnEnable called");
        SceneManager.sceneLoaded += OnPlaySceneLoaded;
    }

    // called when the game is terminated
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnPlaySceneLoaded;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            //if (inputPanel == null) inputPanel = GameObject.Find("InputPanel");
            //if (player == null) player = GameObject.Find("Player").GetComponent<Player>();
        }
        if (debugMode)
        {
            DebugAddInputs();
        }
        if (player != null && player.getHealth() <= 0)
        {
            if (isPlaying)
            {
                GameOverSequence();
            }
        }
    }

    public void AddInputs(int dir)
    {
        if (Enum.IsDefined(typeof(Tile.Direction), dir))
        {
            tileInputs.Add(dir);
            //Debug.Log("Added Input: " + dir);
            //updateInputs.Invoke();
            inputPanel.GetComponent<InputPanel>().UpdateInputs();
        }
    }

    private void DebugAddInputs()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            AddInputs(0);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            AddInputs(1);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            AddInputs(2);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            AddInputs(3);
        }
    }

    public bool CheckInputs(Tile.Direction dir)
    {
        if (tileInputs.Remove((int)dir))
        {
            // evoke event
            //updateInputs.Invoke();
            inputPanel.GetComponent<InputPanel>().UpdateInputs();
            return true;
        }
        return false;
    }

    #region Get Functions
    //Get the current timeScale as stored/exposed by the GameManager
    public float getGameSpeed() { return gameSpeed; }

    #endregion

    #region Set Functions
    //Set the current timeScale stored by the GameManager
    public void setGameSpeed(float speed) { gameSpeed = speed; Time.timeScale = speed; }

    #endregion

    //Typically you'd want to make a separate Scene Manager, but for a game as small as ours
    //it's pretty common to have the Game Manager also handle Scene Management
    #region Scene Management
    public void loadSceneByName(string name)
    {
        SceneManager.LoadScene(name);
    }
    public void loadSceneByIndex(int index)
    {
        SceneManager.LoadScene(index);
    }

    private void GameOverSequence()
    {
        setGameSpeed(0.0f);
        isPlaying = false;
        ScoreManager.Instance.openLeaderboard();
        Debug.Log("Debug: GameOver");
    }

    #endregion
}
