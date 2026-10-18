using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraScript : MonoBehaviour
{
    Camera selfCamera;
    [Header("Controls")]
    InputAction moveAction, scrollAction, mousePosAction, rkmAction;
    [SerializeField] float moveSpeed = 0.1f;
    [SerializeField] float scrollSpeed = 1.0f;
                     Vector2 previousMousePosition;
                     bool wasrkmpressed = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        selfCamera = GetComponent<Camera>();

        moveAction = InputSystem.actions.FindAction("Move");
        scrollAction = InputSystem.actions.FindAction("Mouse Scroll");
        mousePosAction = InputSystem.actions.FindAction("Mouse Position");
        rkmAction = InputSystem.actions.FindAction("Right Mouse Button");
    }

    // Update is called once per frame
    void Update()
    {
        //Zoom
        float currentZoom = selfCamera.orthographicSize;
        Vector2 scrollValue = scrollAction.ReadValue<Vector2>();
        currentZoom -= scrollValue[1] * scrollSpeed;
        currentZoom = Math.Clamp(currentZoom, 0, float.PositiveInfinity);
        selfCamera.orthographicSize = currentZoom;

        //Key movement
        Vector3 moveValue = moveAction.ReadValue<Vector2>();
        transform.Translate(moveValue * moveSpeed * Time.deltaTime * currentZoom);

        //Mouse movement
        if (rkmAction.IsPressed() && wasrkmpressed)
        {
            Vector3 mousePosDiviation = mousePosAction.ReadValue<Vector2>() - previousMousePosition;
            previousMousePosition = mousePosAction.ReadValue<Vector2>();
            mousePosDiviation[0] = mousePosDiviation[0] * -1 / Screen.width * currentZoom * 2;
            mousePosDiviation[1] = mousePosDiviation[1] * -1 / Screen.height * (currentZoom * 2 / selfCamera.aspect);
            //Mouse movement feels unright
            transform.Translate(mousePosDiviation);
        } else if (!wasrkmpressed)
        {
            previousMousePosition = mousePosAction.ReadValue<Vector2>();
            wasrkmpressed = true;
        } else
        {
            wasrkmpressed = false;
        }
    }
}
