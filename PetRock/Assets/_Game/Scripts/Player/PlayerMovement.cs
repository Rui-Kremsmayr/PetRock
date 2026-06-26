using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    
    [SerializeField] CharacterController _characterController;
    [SerializeField] InputActionReference _moveAction;

    [Space]
    
    [SerializeField] float _speed = 5f;
    [SerializeField] float _rotateDamping = 0.1f;
    float _turnSmoothingVel;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked; 
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        Vector2 input = _moveAction.action.ReadValue<Vector2>();
        Vector3 dir = new Vector3(input.x, 0, input.y).normalized;

        if (dir.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            float smoothTargetAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothingVel, _rotateDamping);

            transform.rotation = Quaternion.Euler(0f, smoothTargetAngle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            _characterController.Move(_speed * Time.deltaTime * moveDir.normalized);
        }
    }

}

