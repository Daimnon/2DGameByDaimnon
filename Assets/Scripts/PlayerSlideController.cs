using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSlideController : MonoBehaviour
{
    private SlidingPlayer_Actions _controls; // set PlayerInput to c# events, then generate c# script, and this is it.
    private InputAction _moveAction; // action reference.
    
    private Vector2 _moveInputValue = new Vector2(); // we will use this to cash the inputs value.
    private bool _isCalculatingFlips = true;

    [Header("Systems")]
    [SerializeField] private ScoreManager _scoreManager;
    [SerializeField] private AudioManager _audioManager;
    [SerializeField] private CrashDetector _crashDetector;

    [Header("Settings")]
    [SerializeField] private string _groundTag = "LevelCollider"; // the tag of the player
    
    [Header("Data")]
    [SerializeField] private float _moveSpeed = 10.0f; // _sE2D base speed.
    [SerializeField] private float _accelerateSpeed = 15.0f; // _sE2D accelerated speed.
    [SerializeField] private float _deccelerateSpeed = 5.0f; // _sE2D deccelerated speed.
    [SerializeField] private float _torqueAmount = 10.0f; // rotation force.
    private float _fullRot = 360.0f;
    private float _flipOffset = 45.0f;
    private float _prevRot = 0.0f;
    private float _totalRot = 0.0f;
    private int _flipCount = 0;

    [Header("SFX")]
    [SerializeField] private AudioClip _slideSFX;
    [SerializeField] private AudioClip _landSFX;
    [SerializeField] private AudioClip _airUp;


    [Header("Components")]
    [SerializeField] private Rigidbody2D _rb2D;
    [SerializeField] private SpriteRenderer _sR; public SpriteRenderer SR => _sR;
    [SerializeField] private SurfaceEffector2D _sE2D; public SurfaceEffector2D SE2D => _sE2D;

    [Header("Animations")]
    //[SerializeField] private ParticleSystem _powerupActivatedParticles;
    [SerializeField] private ParticleSystem _powerupActiveParticles;
    [SerializeField] private ParticleSystem _powerupDeactivatedParticles;

    private void Awake()
    {
        _controls = new SlidingPlayer_Actions(); // we need to create a new instance of the generated input script for it to work.
    }
    private void Start()
    {
        _moveAction = _controls.Player.Move; // this is how we hook the actual action to our reference.
        _moveAction.Enable(); // we need to enable the action so it works.
        _crashDetector.OnCrash += Crashed;
    }
    private void OnDestroy()
    {
        _crashDetector.OnCrash -= Crashed;
    }

    // we do not necessarily do the actual movement here because the frame rate is not in sync with the physical frames.
    private void Update()
    {
        GetMoveVector(); // we read input each frame for a nice lag-less feeling.
        CalculateFlips();
    }

    // we probably will to any force addition here for the collision to match the actual physical movement.
    private void FixedUpdate()
    {
        Move(_moveInputValue, _rb2D);
        Rotate(_moveInputValue, _rb2D);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(_groundTag) && !_crashDetector.IsCrashing)
        {
            AudioSource currentlyPlayedSource = _audioManager.PlaySound(_slideSFX);
            currentlyPlayedSource.volume = _audioManager.SoundsVolume / 1.5f;
            _audioManager.PlayRepeatedly(currentlyPlayedSource);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(_groundTag) && !_crashDetector.IsCrashing)
        {
            AudioSource currentlyPlayedSource = _audioManager.PlaySound(_airUp);
            currentlyPlayedSource.volume = _audioManager.SoundsVolume / 1.5f;
        }
    }

    /* wraped the actual methods in use case methods for additional changes of outside control, for instance if I want to change 
    animation when input is disabled or enabled I will put it in here and use this instead of the actual method.*/
    public void EnableInputs()
    {
        _moveAction.Enable();
    }
    public void DisableInputs()
    {
        _moveAction.Disable();
    }

    private void GetMoveVector()
    {
        // we do not want to read the value if inputs are disabled, it's losing performance over nothing.
        if (!_moveAction.enabled) return; 

        Vector2 moveVector = _moveAction.ReadValue<Vector2>(); // actual input reading from the assigned action.
        _moveInputValue = moveVector; // caching the input value.
    }

    // note that I take the Vector2 and Rigidbody2D as arguments in the method for a possible future repurpose.
    private void Move(Vector2 moveVector, Rigidbody2D rb2D)
    {
        float moveInput = moveVector.y; // we cach the value of specifically the y in case we need to use it in more than one place.

        if (moveInput > 0.1f) // pressing forward input.
            _sE2D.speed = _accelerateSpeed;
        else if (moveInput < -0.1f) // pressing backwards input.
            _sE2D.speed = _deccelerateSpeed;
        else // not pressing anything, aka moveInput > -0.1 && moveInput < 0.1
            _sE2D.speed = _moveSpeed;

    }
    private void Rotate(Vector2 moveVector, Rigidbody2D rb2D)
    {
        float rotateInput = moveVector.x;

        if (rotateInput < 0)
            rb2D.AddTorque(_torqueAmount * Time.fixedDeltaTime);
        else if (rotateInput > 0)
            rb2D.AddTorque(-_torqueAmount * Time.fixedDeltaTime);
    }
    
    private void CalculateFlips()
    {
        if (!_isCalculatingFlips) return;

        float currentRot = transform.rotation.eulerAngles.z;
        _totalRot += Mathf.DeltaAngle(_prevRot, currentRot);

        if (_totalRot > _fullRot - _flipOffset || _totalRot < -_fullRot + _flipOffset)
        {
            _flipCount++;
            _scoreManager.AddFlipScore(_flipCount);
            _totalRot = 0.0f;
        }
        _prevRot = currentRot;
    }
    private void Crashed()
    {
        DisableInputs();
        _sE2D.speed = 0.0f;
        _sE2D.useFriction = true;
        _isCalculatingFlips = false;
    }

    public void ActivatePowerup(PowerupSO powerup)
    {
        _powerupDeactivatedParticles.Stop();
        _powerupActiveParticles.Play();
        _audioManager.PlaySound(powerup.PowerupActivatedSFX);
        if (powerup.PowerupType == PowerupType.Torque)
        {
            _torqueAmount += powerup.Value;
        }
    }
    public void DeactivatePowerup(PowerupSO powerup)
    {
        _powerupActiveParticles.Stop();
        _powerupDeactivatedParticles.Play();
        _audioManager.PlaySound(powerup.PowerupEndedSFX);
        if (powerup.PowerupType == PowerupType.Torque)
        {
            _torqueAmount -= powerup.Value;
        }
    }
}
