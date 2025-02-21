using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField]
    private List<Tile> possibleTiles;

    //[SerializeField][Range(100.0f, 200f)] private float tileHorizontalPadding;
    //[SerializeField][Range(100.0f, 200f)] private float tileVerticalPadding;
    private float tileHorizontalPadding;
    private float tileVerticalPadding;
    // Start is called before the first frame update
    private RectTransform rt;
    public int numCol = 5;
    public int numRow = 7;

    public const int MAX_ADDED_INPUTS = 2;

    //[SerializeField]
    public Tile[][] boardTiles;

    private int asyncCount = 0;

    private bool asyncInProgress = false;

    private readonly List<Tile> _selection = new List<Tile>();

    public static Board Instance { get; private set; }

    void Awake()
    {
        if (Instance != null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Board.Instance);
            Instance = this;
        }
        rt = this.GetComponent<RectTransform>();
        tileHorizontalPadding = rt.rect.width / numCol;
        tileVerticalPadding = rt.rect.height / numRow;
        boardTiles = new Tile[numCol][];
        for (int i = 0; i < numCol; i++)
        {
            boardTiles[i] = new Tile[numRow];
        }
    }

    void Start()
    {
        for (int i = 0; i < numCol; i++)
        {
            for (int j = 0; j < numRow; j++)
            {
                Tile tile = Instantiate(possibleTiles[Random.Range(0, possibleTiles.Count)], this.gameObject.transform, false);
                tile.gameObject.name = "("+ i + ", " + j + ")";
                tile.GetComponent<RectTransform>().anchoredPosition = new Vector2(75.0f + i * tileHorizontalPadding, 62.5f + j * tileVerticalPadding);
                tile.setPosition(i, j);
                boardTiles[i][j] = tile;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if no possible moves, wipe the board
        //if (!asyncInProgress && CanPop())
        //{
        //    StartCoroutine(BoardWipe());
        //}
    }

    public void SelectWrapper(Tile tile)
    {
        StartCoroutine(Select(tile));
    }

    public IEnumerator Select(Tile tile)
    {
        if (!_selection.Contains(tile)) _selection.Add(tile);

        if (_selection.Count < 2 || asyncInProgress) yield break;

        Debug.Log($"Selected tiles at ({_selection[0].x}, {_selection[0].y}) and ({_selection[1].x},{_selection[1].y})");

        asyncInProgress = true;

        yield return StartCoroutine(Swap(_selection[0], _selection[1]));

        if (CanPop())
        {
            yield return StartCoroutine(Pop());
        }
        else
        {
            yield return StartCoroutine(Swap(_selection[0], _selection[1]));
        }


        asyncInProgress = false;

        _selection.Clear();
    }

    public IEnumerator Swap(Tile tile1, Tile tile2)
    {
        var tile1Transform = tile1.gameObject.transform.position;
        var tile2Transform = tile2.gameObject.transform.position;

        //var tile1Parent = tile1.gameObject.transform.parent;
        //var tile2Parent = tile2.gameObject.transform.parent;

        var tile1Destination = new Vector3(tile2.gameObject.transform.position.x, tile2.gameObject.transform.position.y, tile2.gameObject.transform.position.z);
        var tile2Destination = new Vector3(tile1.gameObject.transform.position.x, tile1.gameObject.transform.position.y, tile1.gameObject.transform.position.z);

        float elapsedTime = 0f;
        float waitTime = 0.15f;

        while (elapsedTime < waitTime)
        {
            tile1.gameObject.transform.position = Vector3.Lerp(tile1Transform, tile1Destination, (elapsedTime / waitTime));
            tile2.gameObject.transform.position = Vector3.Lerp(tile2Transform, tile2Destination, (elapsedTime / waitTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        tile1.gameObject.transform.position = tile1Destination;
        tile2.gameObject.transform.position = tile2Destination;

        //TODO, swap rows in boardTiles & names

        //Deprecated, all tiles under same parent
        //tile1.gameObject.transform.SetParent(tile2Parent);
        //tile2.gameObject.transform.SetParent(tile1Parent);

        //Swap position in boardTiles
        boardTiles[tile2.x][tile2.y] = tile1;
        boardTiles[tile1.x][tile1.y] = tile2;

        //Swap x,y position
        int tempx = tile1.x, tempy = tile1.y;
        tile1.x = tile2.x;
        tile1.y = tile2.y;
        tile2.x = tempx;
        tile2.y = tempy;

        //Swap direction; edit, unnecessary as Tiles themselves move
        //var tempDir = tile1.direction;
        //tile1.direction = tile2.direction;
        //tile2.direction = tempDir;

        //Reset name
        tile1.gameObject.name = "(" + tile1.x + ", " + tile1.y + ")";
        tile2.gameObject.name = "(" + tile2.x + ", " + tile2.y + ")";


        yield return null;
    }

    private bool CanPop()
    {
        for (int i = 0; i < numCol; i++)
        {
            for (int j = 0; j < numRow; j++)
            {
                var connected = boardTiles[i][j].getConnectedTiles();
                foreach (var neighbor in connected)
                    //Debug.Log($"Connection from ({i}, {j}) to ({neighbor.x}, {neighbor.y})");
                if (boardTiles[i][j].getRowTiles(connected) != null || boardTiles[i][j].getColTiles(connected) != null)
                {
                    //Debug.Log($"Can Pop Detected at: ({i}, {j})");
                    //if (boardTiles[i][j].getRowTiles(connected) != null)
                    //    Debug.Log("Called CanPop() via getRowTiles");
                    //if (boardTiles[i][j].getColTiles(connected) != null)
                    //    Debug.Log("Called CanPop() via getColTiles");
                    return true;
                }
                //var connected = boardTiles[i][j].getConnectedTiles();
                //foreach (var neighbor in connected)
                //    Debug.Log($"Connection from ({i}, {j}) to ({neighbor.x}, {neighbor.y})");
                //if (connected.Count > 2) return true;
            }
        }
        //Debug.Log("NO MATCH THIS ITERATION");
        return false;
    }

    private IEnumerator Pop()
    {
        //Debug.Log("Can Pop!");
        while(CanPop())
        {
            for (int i = 0; i < numCol; i++)
            {
                for (int j = 0; j < numRow; j++)
                {
                    asyncCount = 0;
                    var tile = boardTiles[i][j];

                    var connected = tile.getConnectedTiles();

                    var rowConnected = tile.getRowTiles(connected);

                    var colConnected = tile.getColTiles(connected);

                    int count = (rowConnected != null ? rowConnected.Count : 0) + (colConnected != null ? colConnected.Count : 0);
                    //Delete Sequence
                    //Rows

                    if (rowConnected != null)
                    {
                        int inputs = 0;
                        foreach (var neighbor in rowConnected)
                        {
                            if (inputs++ <= MAX_ADDED_INPUTS && neighbor.direction != Tile.Direction.NULL)
                                GameManager.Instance.AddInputs((int)neighbor.direction);
                            StartCoroutine(scaleTile(0.0f, neighbor));
                        }
                    }

                    //Cols
                    if (colConnected != null)
                    {
                        int inputs = 0;
                        foreach (var neighbor in colConnected)
                        {
                            if (inputs++ <= MAX_ADDED_INPUTS && neighbor.direction != Tile.Direction.NULL)
                                GameManager.Instance.AddInputs((int)neighbor.direction);
                            StartCoroutine(scaleTile(0.0f, neighbor));
                        }
                    }

                    while (asyncCount < count)
                    {
                        yield return null;
                    }

                    //yield return new WaitForSeconds(.25f);

                    //Reset Sequence
                    if (rowConnected != null)
                    {
                        foreach (var neighbor in rowConnected)
                        {
                            //x = neighbor.x;
                            //y = neighbor.y;
                            //Tile newTile = Instantiate(possibleTiles[Random.Range(0, possibleTiles.Count)], this.gameObject.transform, false);
                            //newTile.gameObject.name = "(" + x + ", " + y + ")";
                            //newTile.GetComponent<RectTransform>().anchoredPosition = new Vector2(75.0f + x * tileHorizontalPadding, 62.5f + y * tileVerticalPadding);
                            //newTile.setPosition(x, y);
                            //boardTiles[x][y] = newTile;
                            StartCoroutine(addTile(neighbor));
                            
                        }
                    }

                    //Cols
                    if (colConnected != null)
                    {
                        foreach (var neighbor in colConnected)
                        {
                            //x = neighbor.x;
                            //y = neighbor.y;
                            //Tile newTile = Instantiate(possibleTiles[Random.Range(0, possibleTiles.Count)], this.gameObject.transform, false);
                            //newTile.gameObject.name = "(" + x + ", " + y + ")";
                            //newTile.GetComponent<RectTransform>().anchoredPosition = new Vector2(75.0f + x * tileHorizontalPadding, 62.5f + y * tileVerticalPadding);
                            //newTile.setPosition(x, y);
                            //boardTiles[x][y] = newTile;
                            StartCoroutine(addTile(neighbor));
                        }
                    }

                }
            }
            yield return null;
        }
        yield return null;
    }

    private IEnumerator scaleTile(float scale, Tile tile)
    {

        var tileTransform = tile.gameObject.transform.localScale;

        var tileTarget = new Vector3(tileTransform.x * scale, tileTransform.x * scale, tileTransform.x * scale);
      
        float elapsedTime = 0f;
        float waitTime = 0.10f;

        while (elapsedTime < waitTime)
        {
            tile.gameObject.transform.localScale = Vector3.Lerp(tileTransform, tileTarget, (elapsedTime / waitTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        asyncCount++;
        yield return null;
    }

    private IEnumerator addTile(Tile tile)
    {
        int x = tile.x;
        int y = tile.y;
        Tile newTile = Instantiate(possibleTiles[Random.Range(0, possibleTiles.Count)], this.gameObject.transform, false);
        var tileTransform = newTile.gameObject.transform.localScale;
        newTile.gameObject.transform.localScale = new Vector3(0f, 0f, 0f);
        newTile.gameObject.name = "(" + x + ", " + y + ")";
        newTile.GetComponent<RectTransform>().anchoredPosition = new Vector2(75.0f + x * tileHorizontalPadding, 62.5f + y * tileVerticalPadding);
        newTile.setPosition(x, y);
        boardTiles[x][y] = newTile;
        

        var tileTarget = new Vector3(1.0f, 1.0f, 1.0f);

        float elapsedTime = 0f;
        float waitTime = 0.25f;

        while (elapsedTime < waitTime)
        {
            newTile.gameObject.transform.localScale = Vector3.Lerp(tileTransform, tileTarget, (elapsedTime / waitTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(tile.gameObject);

        yield return null;
    }

    private IEnumerator BoardWipe()
    {
        asyncInProgress = true;
        asyncCount = 0;
        int count = (numRow - 1) * (numCol - 1);
        for (int i = 0; i < numCol; i++)
        {
            for (int j = 0; j < numRow; j++)
            {
                var tile = boardTiles[i][j];
                StartCoroutine(scaleTile(0.0f, tile));
            }
        }
        while (asyncCount < count)
        {
            yield return null;
        }
        for (int i = 0; i < numCol; i++)
        {
            for (int j = 0; j < numRow; j++)
            {
                var tile = boardTiles[i][j];
                StartCoroutine(addTile(tile));
            }
        }
        asyncInProgress = false;
        yield return null;
    }

    //private IEnumorator scaleTiles()
}
