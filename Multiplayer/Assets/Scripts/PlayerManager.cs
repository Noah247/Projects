using UnityEngine;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
{
    #region IPunObservable implementation

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(this.IsFiring);
            stream.SendNext(this.Health);
        }
        else
        {
            // Network player, receive data
            this.IsFiring = (bool)stream.ReceiveNext();
            this.Health = (float)stream.ReceiveNext();
        }
    }

    #endregion

    #region Public Fields

    [Tooltip("The current Health of our player")]
    public float Health = 1f;

    [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
    public static GameObject LocalPlayerInstance;

    RaycastHit hit;

    private LineRenderer laserLine;

    public AudioSource laserSound;

    #endregion

    #region Private Fields

    [Tooltip("The Beams GameObject to control")]
    [SerializeField]
    private GameObject beams;

    [Tooltip("The Player's UI GameObject Prefab")]
    [SerializeField]
    private GameObject PlayerUiPrefab;

    //True, when the user is firing
    public bool IsFiring;

    PlayerAnimatorManager playerAnimatorManager;

    private int layerMask = 1 << 8;

    #endregion

    #region MonoBehaviour CallBacks

    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
    /// </summary>
    void Awake()
    {
        if (this.beams == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> Beams Reference.", this);
        }
        else
        {
            this.beams.SetActive(false);
        }
        if (photonView.IsMine)
        {
            LocalPlayerInstance = gameObject;
        }
        // #Critical
        // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
        DontDestroyOnLoad(gameObject);
        layerMask = ~layerMask;
    }

    void Start()
    {
        laserLine = beams.GetComponent<LineRenderer>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        CameraWork _cameraWork = this.gameObject.GetComponent<CameraWork>();
        if (_cameraWork != null)
        {
            if (photonView.IsMine)
            {
                _cameraWork.OnStartFollowing();
            }
        }
        else
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> CameraWork Component on playerPrefab.", this);
        }
        if (this.PlayerUiPrefab != null)
        {
            GameObject _uiGo = Instantiate(PlayerUiPrefab);
            _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
        }
        else
        {
            Debug.LogWarning("<Color=Red><a>Missing</a></Color> PlayerUiPrefab reference on player Prefab.", this);
        }
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
        if (photonView.IsMine)
        {
            transform.GetComponent<AudioListener>().enabled = true;
        }
        else
        {
            transform.GetComponent<AudioListener>().enabled = false;
        }
    }

    public override void OnDisable()
    {
        // Always call the base to remove callbacks
        base.OnDisable();
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity on every frame.
        /// </summary>
    public void Update()
    {
        if (photonView.IsMine)
        {
            this.ProcessInputs();
            if (Health <= 0f)
            {
                GameManager.Instance.LeaveRoom();
            }
        }
        laserLine.SetPosition(0, this.beams.transform.position);
        // trigger Beams active state
        if (this.beams != null && this.IsFiring != this.beams.activeInHierarchy)
        {
            this.beams.SetActive(this.IsFiring);
        }
        if (this.beams.activeInHierarchy)
        {
            if (Physics.Raycast(this.beams.transform.position, transform.TransformDirection(Vector3.forward), out hit, 15f, layerMask))
            {
                laserLine.SetPosition(1, hit.point);
                Debug.Log("Did Hit");
                if (hit.collider.gameObject.GetComponent<PlayerManager>() != null)
                {
                    hit.collider.gameObject.GetComponent<PlayerManager>().LoseHealth();
                }
            }
            else
            {
                laserLine.SetPosition(1, beams.transform.position + transform.forward * 15);
            }
        }
    }

    void CalledOnLevelWasLoaded(int level)
    {
        // check if we are outside the Arena and if it's the case, spawn around the center of the arena in a safe zone
        //if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
        //{
            //transform.position = new Vector3(0f, 5f, 0f);
        //}
        GameObject _uiGo = Instantiate(this.PlayerUiPrefab);
        _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
    }

    void LateUpdate()
    {
        if (playerAnimatorManager.animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Jump") && this.beams.activeInHierarchy)
        {
            this.beams.SetActive(false);
        }
    }

    void LoseHealth()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        this.Health -= 0.01f;
    }

    #endregion

    #region Private Methods

    void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode loadingMode)
    {
        this.CalledOnLevelWasLoaded(scene.buildIndex);
    }

    /// <summary>
    /// Processes the inputs. Maintain a flag representing when the user is pressing Fire.
    /// </summary>
    void ProcessInputs()
    {
        if (SceneManager.GetActiveScene().name == "Lobby")
        {
            return;
        }
        if (Input.GetButtonDown("Fire1"))
        {
            if (!this.IsFiring)
            {
                this.IsFiring = true;
            }
            laserSound.Play();
        }
        if (Input.GetButtonUp("Fire1"))
        {
            if (this.IsFiring)
            {
                this.IsFiring = false;
            }
            laserSound.Stop();
        }
    }

    #endregion
}