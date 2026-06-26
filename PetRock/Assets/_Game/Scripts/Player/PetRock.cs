using UnityEngine;
using UnityEngine.InputSystem;

public class PetRock : MonoBehaviour
{

    // Note: This would be better solved with a state machine, but that's a bit too advanced for now
    
    [SerializeField] float _moveSpeed;
    [SerializeField] float _wanderRadius;

    [Space]

    [SerializeField] InputActionReference _whistleInput;
    [SerializeField] Transform _waitingPos;
    [SerializeField] Transform _holdingPos;
    public Transform HoldingPos => _holdingPos;

    [Space]

    [Header("Audio")]
    [SerializeField] SoundInfoSO _whistleSound;
    [SerializeField] SoundInfoSO _whistlingSound;
    AudioSource _whistlingAudioSource;

    public static event System.Action<bool> OnWhistled;
    public static event System.Action<bool> OnIsWaiting;
    public static event System.Action OnArrivedAtSpecificPosition;
    public static System.Action<State, Transform> OnChangeState;
    // to show that we can also invoke from another script
    // use with utmost caution


    public enum State { Wandering, Waiting, Eating, Playing }
    bool _movingToSpecificPos = false;
    State _currentState;
    Vector3 _startPos;
    Vector3 _goalPos;
    Transform _playT;


    void OnEnable()
    {
        OnChangeState += NotifyChangeState;
    }

    void OnDisable()
    {
        OnChangeState -= NotifyChangeState;
    }

    void Start()
    {
        _startPos = transform.position;
        _currentState = State.Wandering;
        _goalPos = NewGoalPos();

        _whistlingAudioSource = AudioManagerWithPool.Instance.Play(_whistlingSound, transform.position);
        _whistlingAudioSource.transform.SetParent(transform);
    }

    void Update()
    {
        // we'll change this later to using Events, which is more performant
        if (!_movingToSpecificPos && _whistleInput.action.WasPerformedThisFrame())
            ToggleWaiting();
    }

    void FixedUpdate()
    {
        if (!CanMove())
            return;

        if (_currentState == State.Playing && _playT != null)
            _goalPos = new(_playT.position.x, transform.position.y, _playT.position.z);

        transform.LookAt(new Vector3(_goalPos.x, transform.position.y, _goalPos.z));
        transform.position = Vector3.MoveTowards(transform.position, _goalPos, _moveSpeed * (_currentState == State.Wandering ? 1 : 3) * Time.deltaTime);
        
        if (Vector3.Distance(transform.position, _goalPos) < 0.01f)
        {
            transform.position = _goalPos;

            if (_movingToSpecificPos)
            {
                _movingToSpecificPos = false;
                OnArrivedAtSpecificPosition?.Invoke();
                if (_currentState == State.Waiting)
                {
                    transform.LookAt(Camera.main.transform);
                    OnIsWaiting?.Invoke(true);
                }
            }
            else if (_currentState == State.Wandering)
            {
                _goalPos = NewGoalPos();
            }
        }
    }

    bool CanMove() => _currentState == State.Wandering || _currentState == State.Playing || _movingToSpecificPos;

    void ToggleWaiting()
    {
        _currentState = _currentState == State.Waiting ? State.Wandering : State.Waiting;

        AudioManagerWithPool.Instance.Play(_whistleSound, Vector3.zero);
        AudioManagerWithPool.Instance.Pause(_whistlingAudioSource, _currentState == State.Waiting);
        
        OnWhistled?.Invoke(_currentState == State.Waiting);

        if (_currentState == State.Waiting)
        {
            _goalPos = new(_waitingPos.position.x, transform.position.y, _waitingPos.position.z);
            _movingToSpecificPos = true;
            _currentState = State.Waiting;
        }
        else
        {
            OnIsWaiting?.Invoke(false);
            _goalPos = NewGoalPos();
        }
    }

    Vector3 NewGoalPos()
    {
        Vector3 randomPos = Random.insideUnitSphere * _wanderRadius;
        randomPos += _startPos;
        return new(randomPos.x, transform.position.y, randomPos.z);
    }

    void NotifyChangeState(State newState, Transform goalT)
    {
        _currentState = newState;
        _playT = null;
        Debug.Log($"Change state to {newState}, goalT: {goalT}");

        switch (newState)
        {
            case State.Wandering:
                _goalPos = NewGoalPos();
                break;
            case State.Playing:
                _playT = goalT;
                break;
            case State.Waiting:
                _goalPos = new(_waitingPos.position.x, transform.position.y, _waitingPos.position.z);
                _movingToSpecificPos = true;
                break;
            default:
                _goalPos = new(goalT.position.x, transform.position.y, goalT.position.z);;
                _movingToSpecificPos = true;
                break;
        }   
    }


}
