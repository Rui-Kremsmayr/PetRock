using UnityEngine;
using UnityEngine.InputSystem;

public class InteractablesManager : MonoBehaviour
{

    [SerializeField] InputActionReference _mouseClick;
    [SerializeField] InputActionReference _mousePos;
    [SerializeField] GameObject _toysArea;

    bool _blockInput;
    IInteractable _currentInteractable;


    void OnEnable()
    {
        _mouseClick.action.started += HandleClick;
        _mouseClick.action.canceled += HandleClick;
        _mouseClick.action.performed += HandleClick;
        PetRock.OnIsWaiting += ActivateToys;
    }

    void OnDisable()
    {
        _mouseClick.action.started -= HandleClick;
        _mouseClick.action.canceled -= HandleClick;
        _mouseClick.action.performed -= HandleClick;
        PetRock.OnIsWaiting -= ActivateToys;
    }

    void Start()
    {
        ActivateToys(false);
        _currentInteractable = null;
    }

    void ActivateToys(bool activate)
    {
        _toysArea.SetActive(activate);
    }

    void HandleClick(InputAction.CallbackContext context)
    {
        if (_blockInput)
        {
            if (context.performed)
                _currentInteractable?.OnClick();
            else if (context.canceled)
                _currentInteractable?.OnPointerUp();

            return;
        }
        else if (context.started)
        {
            ChooseToy();
        }
    }

    void ChooseToy()
    {
        Ray ray = Camera.main.ScreenPointToRay(_mousePos.action.ReadValue<Vector2>());
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform.TryGetComponent(out IInteractable interactable))
            {
                _blockInput = true;
                ActivateToys(false);
                _currentInteractable = interactable;
                _currentInteractable.OnFinishedInteraction += FreeInput;
                interactable.OnPointerDown();
            }
        }
    }

    void FreeInput()
    {
        _blockInput = false;
        _currentInteractable.OnFinishedInteraction -= FreeInput;
    }


}