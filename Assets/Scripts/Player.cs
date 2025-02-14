using UnityEngine;

using System.Collections;

public class SimplePlayer : MonoBehaviour
{
    public float jumpPower = 10f;
    public float laneWidth = 3f;
    public float gravityMultiplier = 2.5f;
    [SerializeField][Range(10.0f,100.0f)] private float moveTime = 1.0f;

    private Rigidbody rb;
    [SerializeField] private bool canJump = true;
    [SerializeField] private bool canDuck = true;
    [SerializeField][Range(0.0f, 5.0f)] private float duckTimer = 1.0f;

    [SerializeField] private int currentLane = 0;
    private Vector3 targetPosition;

    private Vector3 leftTP;
    private Vector3 rightTP;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        targetPosition = transform.position;
        rb.useGravity = false;
        leftTP = new Vector3(-2 * laneWidth, transform.position.y, transform.position.z);
        rightTP = new Vector3(2 * laneWidth, transform.position.y, transform.position.z);
    }

    void Update()
    {
        Move();
    }

    void FixedUpdate()
    {
        rb.AddForce(Vector3.down * gravityMultiplier * 9.81f, ForceMode.Acceleration);
        leftTP.y = transform.position.y;
        leftTP.z = transform.position.z;
        rightTP.y = transform.position.y;
        rightTP.z = transform.position.z;
        transform.position = new Vector3(Vector3.Lerp(transform.position, targetPosition, Time.fixedDeltaTime * moveTime).x, transform.position.y, transform.position.z);
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
            canJump = true;
    }

    void Move()
    {
        // Move Left
        if (Input.GetKeyDown(KeyCode.A) /*&& currentLane > -1*/)
        {
            if (GameManager.Instance.CheckInputs(Tile.Direction.LEFT))
            {
                //// Uncomment for no TP
                //currentLane--;
                //targetPosition = new Vector3(currentLane * laneWidth, transform.position.y, transform.position.z);
                if (currentLane > -1)
                {
                    --currentLane;
                    targetPosition = new Vector3(currentLane * laneWidth, transform.position.y, transform.position.z);
                }
                else
                {
                    currentLane = 1;
                    transform.position = rightTP;
                    targetPosition = new Vector3(currentLane * laneWidth, transform.position.y, transform.position.z);
                }
            }
        }

        // Move Right
        if (Input.GetKeyDown(KeyCode.D) /*&& currentLane < 1*/)
        {
            if (GameManager.Instance.CheckInputs(Tile.Direction.RIGHT))
            {
                //currentLane++;
                //targetPosition = new Vector3(currentLane * laneWidth, transform.position.y, transform.position.z);
                if (currentLane < 1)
                {
                    ++currentLane;
                    targetPosition = new Vector3(currentLane * laneWidth, transform.position.y, transform.position.z);
                }
                else
                {
                    currentLane = -1;
                    transform.position = leftTP;
                    targetPosition = new Vector3(currentLane * laneWidth, transform.position.y, transform.position.z);
                }
            }
        }

        // Move Up
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
        {
            if (GameManager.Instance.CheckInputs(Tile.Direction.UP) && canJump)
            {                    
                rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
                canJump = false;    
            }
        }

        // Move Down
        if (canJump && Input.GetKeyDown(KeyCode.S))
        {
            if (GameManager.Instance.CheckInputs(Tile.Direction.DOWN))
            {
                StopCoroutine("Duck");
                rb.AddForce(Vector3.down * 100.0f, ForceMode.Impulse);
                //canDuck = false;
                StartCoroutine("Duck");
            }
        }
    }

    // Coroutine
    private IEnumerator Duck()
    {
        transform.localScale = new Vector3(1.0f, 0.5f, 1.0f);
        yield return new WaitForSeconds(duckTimer);
        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        canDuck = true;
    }
}
