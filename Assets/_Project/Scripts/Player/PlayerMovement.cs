using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [field: SerializeField] public float MoveForce { get; private set; } = 10f; 
    [field: SerializeField] public float MaxSpeed { get; private set; } = 5f;   
    [field: SerializeField] public float Drag { get; private set; } = 2f;       
    [field: SerializeField] public float RotationSpeed { get; private set; } = 120f; 

    [field: SerializeField] public TMP_Text SpeedText { get; private set; }      
    [field: SerializeField] public TMP_Text PositionText { get; private set; }    
    [field: SerializeField] public TMP_Text RotationText { get; private set; }    

    [field: SerializeField] public GameObject CanvasGo { get; private set; }      
    [field: SerializeField] public GameObject CanvasGame { get; private set; }    
    [field: SerializeField] public GameObject Player { get; private set; }  
    
    [field: SerializeField] public Button PlayAgainButton { get; private set; }  
    [field: SerializeField] public ScoreManager ScoreManager { get; private set; }


    private Rigidbody2D rb;
    private bool shouldMoveForward; 
    private float rotatonDirection; 

    public bool live = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); 
        rb.drag = Drag; 

        if (PlayAgainButton != null)
        {
            PlayAgainButton.onClick.AddListener(ButtonPlayAgain);
        }
    }

    public void Move() 
    {
        shouldMoveForward = true;
    }

    public void Rotate(int direction) 
    {
        rotatonDirection = direction;
    }

    private void FixedUpdate()
    {
        if (shouldMoveForward) 
        {
            Vector2 forwardDirection = transform.up; 

            if (Vector2.Dot(rb.velocity, forwardDirection) < MaxSpeed)
            {
                rb.AddForce(forwardDirection *  MoveForce, ForceMode2D.Force);
            }
        }

        if (rotatonDirection != 0) 
        {
            rb.MoveRotation(rb.rotation + rotatonDirection * RotationSpeed * Time.fixedDeltaTime);
        }

        shouldMoveForward = false;

        UpdateSpeed();
        UpdatePosition();
        UpdateRotation();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var asteroid = collision.GetComponent<AsteroidsEnemy>();
        var smallAsteroid = collision.GetComponent<SmallAsteroidEnemy>();
        var ufo = collision.GetComponent<UFOEnemy>();

        if (asteroid != null || smallAsteroid != null || ufo != null)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;    
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;

            Time.timeScale = 0f;
            live = false;
            UpdateSpeed();
            UpdatePosition();
            UpdateRotation();
            CanvasGo.SetActive(true);
            CanvasGame.SetActive(false);
        }
    }

    public void ButtonPlayAgain()
    {
        Time.timeScale = 1f;
        live = true;
        CanvasGo.SetActive(false);
        CanvasGame.SetActive(true);
        ScoreManager.ResetScore();
    }

    private void UpdateSpeed() 
    {
        float currentSpeed = rb.velocity.magnitude; 
        SpeedText.text = $"{currentSpeed.ToString("F2")}"; 
    }

    private void UpdatePosition() 
    {
        Vector2 playerPos = transform.position; 
        PositionText.text = $"{playerPos.x:F1} | {playerPos.y:F1}";
    }

    private void UpdateRotation() 
    {
        float angle = transform.eulerAngles.z; 
        RotationText.text = $"{angle:F0}°"; 
    }
}
