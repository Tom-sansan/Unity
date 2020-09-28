using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerStatus))]
[RequireComponent(typeof(MobAttack))]
public class PlayerController : MonoBehaviour
{
    // Movement speed
    [SerializeField] private float moveSpeed = 3;
    // Jump power
    [SerializeField] private float jumpPower = 3;
    // Aminator
    [SerializeField] private Animator animator;

    private CharacterController _characterController;
    private Transform _transform;
    // Movement speed for character
    private Vector3 _moveVelocity;
    private PlayerStatus _status;
    private MobAttack _attack;
     
    // Start is called before the first frame update
    void Start()
    {
        // Cache to redude load because of accessing each frame
        _characterController = GetComponent<CharacterController>();
        // Cache to reduce load
        _transform = transform;
        _status = GetComponent<PlayerStatus>();
        _attack = GetComponent<MobAttack>();

    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(IsGrounded ? "Ground" : "Air");

        if (CrossPlatformInputManager.GetButtonDown("Fire1"))
        {
            // Attack by Fire1 button (Left click on Mouse by default)
            _attack.AttackIfPossible();
        }

        // Make user input affected for movement if moving is possible
        if (_status.IsMovable)
        {
            // Movement process by input axis (moves sharply as inertia is ignored
            _moveVelocity.x = CrossPlatformInputManager.GetAxis("Horizontal") * moveSpeed;
            _moveVelocity.z = CrossPlatformInputManager.GetAxis("Vertical") * moveSpeed;
            // Turn to movement direction
            _transform.LookAt(_transform.position + new Vector3(_moveVelocity.x, 0, _moveVelocity.z));
        }
        else
        {
            _moveVelocity.x = 0;
            _moveVelocity.z = 0;
        }

        if (IsGrounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                // Jump process
                Debug.Log("Jump!!");
                _moveVelocity.y = jumpPower;
            }
        }
        else
        {
            //　Acceleration by gravity
            _moveVelocity.y += Physics.gravity.y * Time.deltaTime;
        }

        // Moving object
        _characterController.Move(_moveVelocity * Time.deltaTime);

        // Set MoveSpeed to animator
        animator.SetFloat("MoveSpeed", new Vector3(_moveVelocity.x, 0, _moveVelocity.z).magnitude);
    }

    private bool IsGrounded
    {
        get
        {
            var ray = new Ray(_transform.position + new Vector3(0, 0.1f), Vector3.down);
            var raycastHits = new RaycastHit[1];
            var hitCount = Physics.RaycastNonAlloc(ray, raycastHits, 0.2f);
            return hitCount >= 1;
        }
    }
}
