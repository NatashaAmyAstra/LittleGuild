using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Transform testObject;
    private CustomInput _input;

    private void Awake() {
        _input = new CustomInput();
        _input.Enable();

        _input.Player.Press.performed += Press;
        _input.Player.Press.canceled += Release;
        _input.Player.Drag.performed += Drag;
    }

    private MoveableObject _object;
    private Vector3 _clickOffset;

    private void Press(InputAction.CallbackContext value) {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        Collider2D[] hits = Physics2D.OverlapPointAll(mousePosition);

        foreach(Collider2D hit in hits)
        {
            if(hit.TryGetComponent(out MoveableObject moveableObject))
            {
                _object = moveableObject;
                _clickOffset = moveableObject.transform.position - mousePosition;

                moveableObject.GrabObject();
                return;
            }
        }
    }

    private void Release(InputAction.CallbackContext value) {
        if(_object == null)
            return;

        _object.ReleaseObject();
        _object = null;

    }

    private void Drag(InputAction.CallbackContext value) {
        if(_object == null)
            return;

        Vector3 newPosition = Camera.main.ScreenToWorldPoint(value.ReadValue<Vector2>());
        newPosition += _clickOffset;
        _object.DragObject(newPosition);
    }
}
