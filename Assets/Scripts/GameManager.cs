using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }


    [SerializeField]
    [Range(0.0f, 1.0f)]
    [Tooltip("The game speed of the whole application (affects anything using Time)")]
    private float gameSpeed = 1.0f;

    void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
            
    }

    private void Start()
    {
        Debug.Log("GameManager Initialized");
    }

    // Update is called once per frame
    void Update()
    {
        
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

    #endregion
}
