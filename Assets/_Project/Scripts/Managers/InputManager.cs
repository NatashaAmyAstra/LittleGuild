using UnityEngine;
using UnityEngine.InputSystem;

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

    #region Enable and disable input system
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
    #endregion

    #region Mouse controlls
    private MoveableObjectBase _object;
    private Vector3 _clickOffset;

    private void Press(InputAction.CallbackContext value) {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        Collider2D[] hits = Physics2D.OverlapPointAll(mousePosition);

        foreach(Collider2D hit in hits)
        {
            if(hit.TryGetComponent(out MoveableObjectBase moveableObject))
            {
                _object = moveableObject.GrabObject();
                if(_object == null)
                    continue;

                _clickOffset = moveableObject.transform.position - mousePosition;

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
    #endregion
}
