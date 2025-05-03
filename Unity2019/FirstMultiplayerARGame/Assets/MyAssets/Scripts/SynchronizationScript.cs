using Photon.Pun;
using UnityEngine;

/// <summary>
/// SynchronizationScript Class
/// </summary>
public class SynchronizationScript : MonoBehaviour, IPunObservable
{
    #region Nested Class

    #endregion Nested Class

    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField

    #endregion SerializeField

    #region Protected Variables

    #endregion Protected Variables

    #region Public Variables

    #region Public Const Variables

    #endregion Public Const Variables

    #region Public Properties

    #endregion Public Properties

    #endregion Public Variables

    #region Private Variables
    /// <summary>
    /// Rigidbody
    /// </summary>
    private Rigidbody rb;
    /// <summary>
    /// PhotonView
    /// </summary>
    private PhotonView photonView;
    /// <summary>
    /// 
    /// </summary>
    private Vector3 networkedPosition;
    /// <summary>
    /// 
    /// </summary>
    private Quaternion networkedRotation;

    #region Private Const Variables

    #endregion Private Const Variables

    #region Private Properties

    #endregion Private Properties

    #endregion Private Variables

    #endregion Variables

    #region Methods

    #region Unity Methods

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        photonView = GetComponent<PhotonView>();
        networkedPosition = new Vector3();
        networkedRotation = new Quaternion();
    }
    void Start()
    {
        Init();
    }
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine)
        {
            // Change the remote player's position and rotation
            rb.position = Vector3.MoveTowards(rb.position, networkedPosition, Time.fixedDeltaTime);
            rb.rotation = Quaternion.RotateTowards(rb.rotation, networkedRotation, Time.fixedDeltaTime * 100);
        }
    }
    #endregion Unity Methods

    #region Public Methods
    /// <summary>
    /// OnPhotonSerializeView callback method
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="info"></param>
    /// <exception cref="System.NotImplementedException"></exception>
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // True if the client is owner of this PhotonView which is Attached to Player GameObject
        if (stream.IsWriting)
        {
            // PhotonView is mine and I'm the one who controls this player
            // should send position, velocity etc. data to the other players
            stream.SendNext(rb.position);
            stream.SendNext(rb.rotation);
        }
        else
        {
            // Called on my player GameObject hat exists in remote player's GameObject
            networkedPosition = (Vector3)stream.ReceiveNext();
            networkedRotation = (Quaternion)stream.ReceiveNext();
        }
    }
    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Initialize this class
    /// </summary>
    private void Init()
    {

    }
    #endregion Private Methods

    #endregion Methods

    #region For Debug

#if DEBUG

#endif

    #endregion For Debug
}
