using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class PlayerAnimatorManager : MonoBehaviourPun
{
    #region Private Fields

    [SerializeField]
    private float directionDampTime = 0.25f;
    public Animator animator;

    PlayerManager playerManager;

    #endregion

    #region MonoBehaviour Callbacks

    void Start()
    {
        playerManager = GetComponent<PlayerManager>();
        animator = GetComponent<Animator>();
        if (!animator)
        {
            Debug.LogError("PlayerAnimatorManager is Missing Animator Component", this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }
        if (!animator)
        {
            return;
        }
        if (SceneManager.GetActiveScene().name == "Lobby")
        {
            return;
        }
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        // only allow jumping if we are running.
        if (stateInfo.IsName("Base Layer.Run"))
        {
            // When using trigger parameter
            if (Input.GetButtonDown("Fire2") && !playerManager.IsFiring)
            {
                animator.SetTrigger("Jump");
            }
        }
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        if (v < 0)
        {
            v = 0;
        }
        animator.SetFloat("Speed", h * h + v * v);
        animator.SetFloat("Speed", h * h + v * Mathf.Abs(v));
        animator.SetFloat("Direction", h, directionDampTime, Time.deltaTime);
    }

    #endregion
}