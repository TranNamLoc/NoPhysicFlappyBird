using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapController: MonoBehaviour
{
    [SerializeField]
    private float gravity = -0.3f;
    [SerializeField]
    private float flapping = 1.5f;
    bool isFlapping = false;
    private float dY = 0f;
    private bool dead = false;
    public Vector3 startPos = new Vector3(-6, 0, 0);
    GameManager game;

    public delegate void PlayerDelegate();
    public static event PlayerDelegate OnPlayerDied;
    public static event PlayerDelegate OnPlayerScored;

    Rigidbody2D rigidbody;
    void Start()
    {
        game = GameManager.Instance;
        rigidbody = GetComponent<Rigidbody2D>();
        transform.position = new Vector3(-6, 0, 0);
        dead = false;
    }

    private void OnEnable()
    {
        GameManager.OnGameStarted += OnGameStarted;
        GameManager.OnGameOverConfirmed += OnGameOverConfirmed;
    }

    private void OnDisable()
    {
        GameManager.OnGameStarted -= OnGameStarted;
        GameManager.OnGameOverConfirmed -= OnGameOverConfirmed;
    }

    void OnGameStarted()
    {
        dY = 0;
        dead = false;
    }

    void OnGameOverConfirmed()
    {
        transform.localPosition = new Vector3(-6,0,0);
    }
    // Update is called once per frame
    void Update()
    {
        if (game.GameOver) return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            
            isFlapping = true;
        }
        else
        {
            isFlapping = false;
        }

        flaps(gravity, flapping);
    }
    private void flaps(float gravity, float flapping)
    {
        if (!dead)
        {
            if (isFlapping)
            {
                dY += gravity + flapping;
            }
            else
            {
                dY += gravity;
            }
            transform.position = new Vector3(-6, transform.position.y + dY, 0);
        }
        else
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "scoreZone")
        {
            OnPlayerScored();
        }
        if (collision.gameObject.tag == "deadZone")
        {
            
            dead = true;
            OnPlayerDied();
        }
    }
}
