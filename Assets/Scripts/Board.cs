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

    //Calculate distributions of tiles to ensure a mostly uniform distribution
    private float[] distributions = new float[5];

    void Awake()
    {
        rt = this.GetComponent<RectTransform>();
        tileHorizontalPadding = rt.rect.width / numCol;
        tileVerticalPadding = rt.rect.height / numRow;
    }

    void Start()
    {
        //Tile lastTile = null;
        for (int i = 0; i < numCol; i++)
        {
            for (int j = 0; j < numRow; j++)
            {
                Tile tile = Instantiate(possibleTiles[Random.Range(0, possibleTiles.Count)], this.gameObject.transform, false);
                //if (lastTile != null && tile.getDirection() == lastTile.getDirection())
                //{
                //    Destroy(tile.gameObject);
                //    tile = Instantiate(possibleTiles[Random.Range(0, possibleTiles.Count)], this.gameObject.transform, false);
                //}
                tile.gameObject.name = "("+ i + ", " + j + ")";
                tile.GetComponent<RectTransform>().anchoredPosition = new Vector2(100.0f + i * tileHorizontalPadding, 100.0f + j * tileVerticalPadding);
                tile.setPosition(i, j);
                //lastTile = tile;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
