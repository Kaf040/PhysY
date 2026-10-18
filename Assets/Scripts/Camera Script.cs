using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraScript : MonoBehaviour
{
    Camera selfCamera;
    [Header("Controls")]
    InputAction moveAction, scrollAction;
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float scrollSpeed = 1.0f;
    [Header("Grid")]
    [SerializeField] int targetVerticalBarsOnScreen = 30;
    [SerializeField] int aceptableDiviation = 15;
                     float barsStep = 1;
                     int actualBars;
    [SerializeField] float barThikness = 0.05f;
    [SerializeField] float coordinateAxisThiknessMultiplyer = 2.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        selfCamera = GetComponent<Camera>();
        moveAction = InputSystem.actions.FindAction("Move");
        scrollAction = InputSystem.actions.FindAction("Mouse Scroll");
    }

    void OnGUI()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveValue = moveAction.ReadValue<Vector2>();
        transform.Translate(moveValue * moveSpeed * Time.deltaTime);
        float currentZoom = selfCamera.orthographicSize;
        Vector2 scrollValue = scrollAction.ReadValue<Vector2>();
        selfCamera.orthographicSize = currentZoom + scrollValue[0] * scrollSpeed;
    }
}
