using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public enum Direction { UP, DOWN, LEFT, RIGHT, HEALTH, NULL };
    //[SerializeField] private Sprite icon;
    [SerializeField] private Direction direction;
    [SerializeField] private Button button;
    public Vector2 position;
    

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
        button.onClick.AddListener(() => Board.Instance.Select(this));
    }
    
    public Direction getDirection() {  return direction; }

    public void setPosition(int x, int y)
    {
        position = new Vector2(x, y);
    }
}
