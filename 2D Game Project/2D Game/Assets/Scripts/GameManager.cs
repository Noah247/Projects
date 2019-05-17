using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public Button playButton;

    public Button instructionButton;

    public Text instructionText;

    public GameObject instructionPanel;

    public InputField resetField;

	// Use this for initialization
	void Start () {
        playButton.onClick.AddListener(LoadLevel);
        instructionButton.onClick.AddListener(ShowInstructions);
        instructionPanel.SetActive(false);
        resetField.text = "Dev Reset...";
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (resetField.text == "Reset Game")
            {
                PlayerPrefs.SetInt("ScoreHolder", 0);
                resetField.text = "Game Has Been Reset!";
            }
            else
            {
                resetField.text = "Try Again";
            }
        }
	}
    
    public void LoadLevel ()
    {
        SceneManager.LoadScene("Game");
    }

    public void ShowInstructions()
    {
        instructionText.text = "Press Space to Jump (You Can Double Jump!) \nPress P to Pause the Game";
        instructionPanel.SetActive(true);
    }
}
