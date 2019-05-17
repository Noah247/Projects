using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AutoMove : MonoBehaviour {

    public Vector2 jumpHeight;

    public bool isGrounded;

    public static int moveSpeed, bestScore;

    public int coinsCollected = 0;

    public Text coinText;

    public SpriteRenderer background, sprite;

    public AudioSource audioSource, coinSource, loseSource;

    public bool isPaused = false;

    public Button playButton, restartButton, menuButton, endRestartButton, endMenuButton;

    GameObject[] pauseObjects, finishObjects;

	void Start () 
    {
        moveSpeed = 10;
        Time.timeScale = 1;
        playButton.onClick.AddListener(PauseGame);
        restartButton.onClick.AddListener(RestartGame);
        menuButton.onClick.AddListener(ReturnToMenu);
        endRestartButton.onClick.AddListener(RestartGame);
        endMenuButton.onClick.AddListener(ReturnToMenu);
        pauseObjects = GameObject.FindGameObjectsWithTag("ShowOnPause");
        finishObjects = GameObject.FindGameObjectsWithTag("ShowOnFinish");
        hidePaused();
        hideFinished();
        bestScore = PlayerPrefs.GetInt("ScoreHolder");
    }

    void Update()
    {
        transform.Translate(Time.deltaTime * moveSpeed, 0, 0);
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !SpawnLevel.gameEnded)
        {
            isGrounded = false;
            GetComponent<Rigidbody2D>().AddForce(jumpHeight, ForceMode2D.Impulse);
        }
        SetCountText();
        if (Input.GetKeyDown(KeyCode.P))
        {
            PauseGame();
        }
    }

    void OnCollisionStay2D(Collision2D other)
    {
        isGrounded = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Coin")
        {
            coinsCollected++;
            coinSource.Play();
            other.gameObject.SetActive(false);
        }
        if (other.gameObject.tag == "Obstacle")
        {
            endGame();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Obstacle")
        {
            endGame();
        }
        if (other.gameObject.tag == "Trampoline")
        {
            GetComponent<Rigidbody2D>().AddForce(jumpHeight * 1.5f, ForceMode2D.Impulse);
        }
    }

    void endGame()
    {
        moveSpeed = 0;
        GetComponent<Rigidbody2D>().gravityScale = 0f;
        SpawnLevel.gameEnded = true;
        background.color = Color.red;
        audioSource.mute = !audioSource.mute;
        coinText.color = new Color(1, 1, 1, 1);
        sprite.sortingOrder = -3;
        PlayerPrefs.SetInt("ScoreHolder", Mathf.Max(bestScore, coinsCollected));
        PlayerPrefs.Save();
        showFinished();
        loseSource.Play();
    }

    void SetCountText()
    {
        if (!SpawnLevel.gameEnded)
        {
            if (isPaused)
            {
                coinText.text = "Game Paused";
            }
            else
            {
                coinText.text = "Coins collected: " + coinsCollected.ToString();
            }
        }
        else
        {
            coinText.text = "Best Score: " + PlayerPrefs.GetInt("ScoreHolder").ToString() + " Coins";
        }
    }

    void PauseGame()
    {
        if (!SpawnLevel.gameEnded)
        {
            isPaused = !isPaused;
            if (isPaused)
            {
                Time.timeScale = 0;
                showPaused();
            }
            else
            {
                Time.timeScale = 1;
                hidePaused();
            }
        }
    }

    void RestartGame()
    {
        PlayerPrefs.SetInt("ScoreHolder", Mathf.Max(bestScore, coinsCollected));
        PlayerPrefs.Save();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void showPaused()
    {
        foreach (GameObject g in pauseObjects)
        {
            g.SetActive(true);
        }
    }

    public void hidePaused()
    {
        foreach (GameObject g in pauseObjects)
        {
            g.SetActive(false);
        }
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void showFinished()
    {
        foreach (GameObject g in finishObjects)
        {
            g.SetActive(true);
        }
    }

    public void hideFinished()
    {
        foreach (GameObject g in finishObjects)
        {
            g.SetActive(false);
        }
    }

    void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("ScoreHolder", Mathf.Max(bestScore, coinsCollected));
        PlayerPrefs.Save();
    }
}
