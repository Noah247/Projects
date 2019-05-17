using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class WaitingForPlayers : MonoBehaviour {

    public Text playerText;

    public Text playerListText;

    public string playerList;

    public int oldPlayers = 0;

    public GameManager gameManager;

    public Image fadeImage;

	// Use this for initialization
	void Start () {
        gameManager = GetComponent<GameManager>();
        fadeImage.CrossFadeAlpha(0, 2.0f, false);
	}
	
	// Update is called once per frame
	void Update () {
        if (!gameManager.leftGame && PhotonNetwork.CurrentRoom.PlayerCount == 4)
        {
            playerText.text = "Loading Level...";
        }
        else
        {
            playerText.text = "Waiting for Players...";
        }        
        playerText.color = new Color(playerText.color.r, playerText.color.g, playerText.color.b, Mathf.PingPong(Time.time, 2));
        if (!gameManager.leftGame && oldPlayers != PhotonNetwork.CurrentRoom.PlayerCount)
        {
            UpdateList();
            oldPlayers = PhotonNetwork.CurrentRoom.PlayerCount;
        }
    }

    public void UpdateList()
    {
        playerList = "Players:\n";
        ChangeDuplicates();
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            playerList = playerList + p.NickName + "\n";
        }
        playerListText.text = playerList;
        Debug.Log("List Updated");
    }

    public void ChangeDuplicates()
    {
        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
        {
            for (int j = 0; j < PhotonNetwork.CurrentRoom.PlayerCount; j++)
            {
                if (i != j && PhotonNetwork.PlayerList[i].NickName == PhotonNetwork.PlayerList[j].NickName)
                {
                    PhotonNetwork.PlayerList[i].NickName = PhotonNetwork.PlayerList[i].NickName + Random.Range(0, 100);
                    PhotonNetwork.PlayerList[j].NickName = PhotonNetwork.PlayerList[j].NickName + Random.Range(0, 100);
                }
            }
        }
    }
}
