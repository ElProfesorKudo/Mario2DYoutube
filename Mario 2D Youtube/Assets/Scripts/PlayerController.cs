using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [Header("Physics Objects")]
    [SerializeField] private Rigidbody2D _rigidbody2DPlayer;
    [SerializeField] private BoxCollider2D _boxCollider2DPlayer;

    [Header("Player Components")]
    [SerializeField] private SpriteRenderer _spriteRendererPlayer;
    [SerializeField] private AnimationPlayerController _animationPlayerController;

    [Header("Input action references")]
    [SerializeField] private InputActionReference _playerInputActionReferenceMove;
    [SerializeField] private InputActionReference _playerInputActionReferenceJump;

    [Header("Layers")]
    [SerializeField] private LayerMask _groundPipeBrickLayerMask;
    [SerializeField] private LayerMask _brickLayerMask;
    [SerializeField] private LayerMask _pipeLayerMask;


    private float _speedPlayer = 5f;
    private float _gravitySpeedFall;
    private float _maxJumpHeight = 5.0f;
    private float _maxJumpTime = 1.0f;
    private float _distanceCheckGroundDefault = 0.1f;
    private float _distanceCheckGround = 0.1f;


    private Vector2 _velocity;
    private Vector2 _getInputValue;
    private Vector2 _previousInputValue;

    private bool _isPlayerGrounded = false;
    private bool _isPlayerFacingAnything = false;
    private bool _isPlayerHeadTouchingBricks = false;
    private bool _isPlayerJumping = false;
    private bool _isPlayerRunningFast = false;

    private float Gravity => ((-2.0f * _maxJumpHeight) / Mathf.Pow(_maxJumpTime / 2.0f, 2));
    private float JumpForce => (2.0f * _maxJumpHeight) / (_maxJumpTime / 2.0f);

    private void Update()
    {
        //TODO: handle the state of player and the game
        CheckPlayerGroundedAndFacingPipe();
        ApplyGravity();
    }

    private void FixedUpdate()
    {
        //TODO: handle the state of player and the game
        //TODO: handle the physics of player
        MovePlayer();
    }


    private void MovePlayer()
    {
        _rigidbody2DPlayer.linearVelocity = _velocity;
    }


    private void CheckPlayerGroundedAndFacingPipe()
    {
        _isPlayerGrounded = Physics2D.BoxCast(_boxCollider2DPlayer.bounds.center, _boxCollider2DPlayer.bounds.size, 0f, Vector2.down, _distanceCheckGround, _groundPipeBrickLayerMask);
        _isPlayerFacingAnything = Physics2D.BoxCast(_boxCollider2DPlayer.bounds.center, _boxCollider2DPlayer.bounds.size, 0f, !_spriteRendererPlayer.flipX ? Vector2.right : Vector2.left, _distanceCheckGroundDefault, _groundPipeBrickLayerMask);
        RaycastHit2D hit2D = Physics2D.BoxCast(_boxCollider2DPlayer.bounds.center, _boxCollider2DPlayer.bounds.size, 0f, Vector2.up, _distanceCheckGroundDefault, _groundPipeBrickLayerMask);
        _isPlayerHeadTouchingBricks = hit2D;

        if (_isPlayerGrounded)
        {
            _velocity.y = Mathf.Max(_velocity.y, 0f);
            _isPlayerJumping = false;
            // TODO: Resize of the collider
            _animationPlayerController.SetBoolIsGroundedParameter(true);
        }
        else
        {
            _isPlayerJumping = true;
            // TODO: Resize of the collider
            _animationPlayerController.SetBoolIsGroundedParameter(false);
        }

        // Todo: Handle the collision of the brick head

        if (_isPlayerFacingAnything)
        {
            _velocity.x = 0f;
            _animationPlayerController.SetBoolIsRunningParameter(false);
            _animationPlayerController.SetFloatAnimationSpeedMultiplierParameter(1);
            _animationPlayerController.SetBoolIsSlidingParameter(false);
        }
        else
        {
            _velocity.x = _getInputValue.x * _speedPlayer;
        }

        if (_isPlayerGrounded && !_isPlayerFacingAnything && _getInputValue != Vector2.zero)
        {
            _animationPlayerController.SetBoolIsRunningParameter(true);
            if (_isPlayerRunningFast)
            {
                _animationPlayerController.SetFloatAnimationSpeedMultiplierParameter(2);
            }
        }

        //Todo: Check pipe interaction
    }

    private void CheckDirectionToFace(Vector2 inputValue)
    {
        if (inputValue.x > 0 && _spriteRendererPlayer.flipX)
        {
            _spriteRendererPlayer.flipX = false;
            //TODO: hAndle the fire ball place position
        }
        else if (inputValue.x < 0 && !_spriteRendererPlayer.flipX)
        {
            _spriteRendererPlayer.flipX = true;
            //TODO: hAndle the fire ball place position
        }
    }

    private void ApplyGravity()
    {
        _velocity.y = _velocity.y + Gravity * _gravitySpeedFall * Time.deltaTime;
        _velocity.y = Mathf.Max(_velocity.y, Gravity / 2.0f);
    }
    #region Input System

    public void OnMove(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started)
        {
            Vector2 tempMoveDirection = callbackContext.ReadValue<Vector2>();
            CheckDirectionToFace(tempMoveDirection);
            _animationPlayerController.SetBoolIsRunningParameter(true);
        }


        _getInputValue = callbackContext.ReadValue<Vector2>();
        if (callbackContext.performed)
        {
            _previousInputValue = _getInputValue;
            _animationPlayerController.SetBoolIsRunningParameter(true);
        }

        if (callbackContext.canceled)
        {
            _animationPlayerController.SetBoolIsRunningParameter(false);
            //TODO: Handle this anition of mario
            //Todo : Handle slide of mario when realse the key
        }
    }

    public void OnJump(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started && _isPlayerGrounded)
        {
            _isPlayerJumping = true;
            _gravitySpeedFall = 1.0f;
            _velocity.y = JumpForce;
            //TODO : Handle audio when mario jump
        }

        if (callbackContext.canceled && !_isPlayerGrounded)
        {
            UpdateSpeedFallValueAfterHoldOrCancelJump();
        }
    }

    public void OnCrouch(InputAction.CallbackContext callbackContext)
    {

    }

    public void OnFire(InputAction.CallbackContext callbackContext)
    {

    }
    #endregion Input System

    private void UpdateSpeedFallValueAfterHoldOrCancelJump()
    {
        _gravitySpeedFall = 3.0f;
    }
}

public enum PlayerState
{
    Small = 0,
    Grown = 1,
    PowerUp = 2,
    SuperPower = 3,
    Dead = 4
}
