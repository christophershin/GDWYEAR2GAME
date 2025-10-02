using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    // Orientation Game Object
        public Transform orientation;
        
        // Public Variables
        public float moveSpeed;
        public float groundDrag;
        
        // Input Variables
        private float _horizontalInput;
        private float _verticalInput;
        
        // Movement and rigidbody
        private Vector3 _moveDirection;
        private Rigidbody _rb;
        
        // Check if on ground
        public float playerHeight;
        public LayerMask whatIsGround;
        private bool _isGrounded;
        
        
        // Start is called before the first frame update
        void Start()
        {
            if (!IsOwner) return;
            _rb = GetComponent<Rigidbody>();
        }
    
        // Update is called once per frame
        void Update()
        {
            if (!IsOwner) return;
            // Ground Check
            _isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * .5f + .2f, whatIsGround);
            
            MyInput();
            SpeedControl();
            
            // Handle Drag
    
            if (_isGrounded)
            {
                _rb.linearDamping = groundDrag;
            }
            else
            {
                _rb.linearDamping = 0;
            }
        }
    
        void FixedUpdate()
        {
            MovePlayer();
        }
    
        private void MyInput()
        {
            _horizontalInput = Input.GetAxisRaw("Horizontal");
            _verticalInput = Input.GetAxisRaw("Vertical");
        }
    
        private void MovePlayer()
        {
            _moveDirection = orientation.forward * _verticalInput + orientation.right * _horizontalInput;
            
            _rb.AddForce(moveSpeed * 10f * _moveDirection.normalized, ForceMode.Force);
        }
    
        private void SpeedControl()
        {
            Vector3 flatVelocity = new Vector3(_rb.linearVelocity.x, 0, _rb.linearVelocity.z);
            
            // limit velocity if needed
            if (flatVelocity.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVelocity.normalized * moveSpeed;
                _rb.linearVelocity = new Vector3(limitedVel.x, _rb.linearVelocity.y, limitedVel.z);
            }
        }
}
