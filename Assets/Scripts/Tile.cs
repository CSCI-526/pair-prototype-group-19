using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public enum Direction { UP, DOWN, LEFT, RIGHT, NULL };
    //[SerializeField] private Sprite icon;
    [SerializeField] private Direction direction;
    [SerializeField] private Vector2 position;

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
    
    public Direction getDirection() {  return direction; }

    public void setPosition(int x, int y)
    {
        position = new Vector2(x, y);
    }
}
