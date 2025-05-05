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
    /// <summary>
    /// 
    /// </summary>
    [SerializeField]
    private bool synchronizeVelocity = true;
    /// <summary>
    /// 
    /// </summary>
    [SerializeField]
    private bool synchronizeAngularVelocity = true;
    /// <summary>
    /// 
    /// </summary>
    [SerializeField]
    private bool isTeleportEnabled = true;
    /// <summary>
    /// 
    /// </summary>
    [SerializeField]
    private float teleportIfDistanceGreaterThan = 1.0f;
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
    /// Networked Position
    /// </summary>
    private Vector3 networkedPosition;
    /// <summary>
    /// Networked Rotation
    /// </summary>
    private Quaternion networkedRotation;
    /// <summary>
    /// Distance
    /// </summary>
    private float distance;
    /// <summary>
    /// Angle
    /// </summary>
    private float angle;
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
            rb.position = Vector3.MoveTowards(rb.position, networkedPosition, distance * (1.0f / PhotonNetwork.SerializationRate));
            rb.rotation = Quaternion.RotateTowards(rb.rotation, networkedRotation, angle * (1.0f / PhotonNetwork.SerializationRate));
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
            if (synchronizeVelocity) stream.SendNext(rb.velocity);
            if (synchronizeAngularVelocity) stream.SendNext(rb.angularVelocity);
        }
        else
        {
            // Called on my player GameObject hat exists in remote player's GameObject
            networkedPosition = (Vector3)stream.ReceiveNext();
            networkedRotation = (Quaternion)stream.ReceiveNext();
            if (isTeleportEnabled)
            {
                if (Vector3.Distance(rb.position, networkedPosition) > teleportIfDistanceGreaterThan)
                    rb.position = networkedPosition;
            }
            if (synchronizeVelocity || synchronizeAngularVelocity)
            {
                // PhotonNetwork.Time: Used to synchronize time of all players in a room.
                // Actually, it's the server time.
                float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
                if (synchronizeVelocity)
                {
                    rb.velocity = (Vector3)stream.ReceiveNext();
                    networkedPosition += rb.velocity * lag;
                    distance = Vector3.Distance(rb.position, networkedPosition);
                }
                if (synchronizeAngularVelocity)
                {
                    rb.angularVelocity = (Vector3)stream.ReceiveNext();
                    networkedRotation = Quaternion.Euler(rb.angularVelocity * lag) * networkedRotation;
                    angle = Quaternion.Angle(rb.rotation, networkedRotation);
                }
            }
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
