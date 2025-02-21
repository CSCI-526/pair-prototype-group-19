using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public enum Direction { UP, DOWN, LEFT, RIGHT, HEALTH, NULL };
    //[SerializeField] private Sprite icon;
    [SerializeField] private Button button;
    public Direction direction;
    public int x;
    public int y;

    public Tile Left => x > 0 ? Board.Instance.boardTiles[x - 1][y] : null;
    public Tile Top => y < Board.Instance.numRow - 1 ? Board.Instance.boardTiles[x][y+1] : null;
    public Tile Right => x < Board.Instance.numCol - 1 ? Board.Instance.boardTiles[x + 1][y] : null;
    public Tile Bottom => y > 0 ? Board.Instance.boardTiles[x][y - 1] : null;

    public Tile[] Neighbors => new[]
    {
        Left,
        Top,
        Right,
        Bottom
    };


    public Tile(Direction direction)
    {
        this.direction = direction;
    }

    void Awake()
    {
        //if (direction == null) { direction = Direction.NULL; }
        //if (position == null)
        //{
        //    position = new Vector2(0, 0);
        //}
    }

    private void Start()
    {
        button.onClick.AddListener(() => Board.Instance.SelectWrapper(this));
    }
    
    public Direction getDirection() {  return direction; }

    public void setPosition(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    //Gets all connected tiles
    public List<Tile> getConnectedTiles(List<Tile> exclude = null)
    {
        var result = new List<Tile> { this, };

        if (exclude == null)
        {
            exclude = new List<Tile> { this, };
        }
        else
        {
            exclude.Add(this);
        }

        foreach (var neighbor in Neighbors)
        {
            //conditions to add to range
            if (neighbor == null || exclude.Contains(neighbor) || neighbor.direction != this.direction) continue;

            //Set conditions for adding neighbors here?
            result.AddRange(neighbor.getConnectedTiles(exclude));
        }

        return result;
    }

    public List<Tile> getRowTiles(List<Tile> connected)
    {
        var result = new List<Tile>();

        int leftCount = x;
        //Case 1: Left
        foreach(var neighbor in connected)
        {
            if (neighbor.y == this.y && neighbor.x == (leftCount+1))
            {
                result.Add(neighbor);
                ++leftCount;
            }
        }

        leftCount = x;
        //Case2: Right
        foreach (var neighbor in connected)
        {
            if (neighbor.y == this.y && neighbor.x == (leftCount - 1))
            {
                result.Add(neighbor);
                --leftCount;
            }
        }
        //Case3: Middle, up to 2, might get covered by above?


        return result.Count >= 2 ? result : null;
    }

    public List<Tile> getColTiles(List<Tile> connected)
    {
        var result = new List<Tile>();

        int topCount = this.y;
        //Case 1: Above
        foreach (var neighbor in connected)
        {
            if (neighbor.x == this.x && neighbor.y == (topCount + 1))
            {
                result.Add(neighbor);
                ++topCount;
            }
        }

        topCount = this.y;
        //Case2: Below
        foreach (var neighbor in connected)
        {
            if (neighbor.x == this.x && neighbor.y == (topCount - 1))
            {
                result.Add(neighbor);
                --topCount;
            }
        }
        //Case3: Middle, up to 2, might get covered by above?


        return result.Count >= 2 ? result : null;
    }
}
