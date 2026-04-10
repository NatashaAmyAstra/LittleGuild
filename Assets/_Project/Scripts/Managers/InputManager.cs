using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class InputManager : MonoBehaviour
{
    public static InputManager main;

    [SerializeField] private Transform testObject;
    private CustomInput _input;

    private void Awake() {
        SetSingleton();
    }

    private void SetSingleton() {
        if(main == null)
            main = this;
        else
            Destroy(this);
    }

    private void OnEnable() {
        _input = new CustomInput();
        _input.Enable();

        _input.Player.Press.performed += Press;
        _input.Player.Press.canceled += Release;
        _input.Player.Drag.performed += Drag;
    }

    private void OnDisable() {
        _input.Disable();
    }

    private MoveableObjectBase _object;
    private Vector3 _clickOffset;

    private void Press(InputAction.CallbackContext value) {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        Collider2D[] hits = Physics2D.OverlapPointAll(mousePosition);

        foreach(Collider2D hit in hits)
        {
            if(hit.TryGetComponent(out MoveableObjectBase moveableObject))
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
