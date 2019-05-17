using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LauncherNew : MonoBehaviour {

    [Tooltip("Toggles The Title Box and the Buttons")]
    [SerializeField]
    private GameObject controlPanel;

    [Tooltip("Toggles the Instruction Text and Panel")]
    [SerializeField]
    public GameObject instructionLabel;

    public bool isSeen;

    // Use this for initialization
    void Start () {
        isSeen = false;
        instructionLabel.SetActive(isSeen);
        controlPanel.SetActive(!isSeen);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LoadGameLobby()
    {
        SceneManager.LoadScene("GameLobby", LoadSceneMode.Single);
    }

    public void ToggleInstructions()
    {
        isSeen = !isSeen;
        instructionLabel.SetActive(isSeen);
        controlPanel.SetActive(!isSeen);
    }
}
