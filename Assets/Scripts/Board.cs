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
    private int numCol = 5;
    private int numRow = 7;

    [SerializeField]
    private Tile[][] boardTiles;

    //Calculate distributions of tiles to ensure a mostly uniform distribution
    private float[] distributions = new float[5];

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
        
    }

    public void Select(Tile tile)
    {
        if (!_selection.Contains(tile)) _selection.Add(tile);

        if (_selection.Count < 2) return;

        Debug.Log($"Selected tiles at ({_selection[0].position[0]}, {_selection[0].position[1]}) and ({_selection[1].position[0]},{_selection[1].position[1]})");

        _selection.Clear();
    }
}
