using UnityEngine;
using UnityEngine.InputSystem; 
using UnityEngine.UIElements;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask selectableLayer; 

    private VisualElement _objectMenu;
    private Button _closeButton;

    private void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        if (uiDocument == null)
            uiDocument = GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        var root = uiDocument.rootVisualElement;

        _objectMenu = root.Q<VisualElement>("object-menu");

        _closeButton = root.Q<Button>("close-btn");
        if (_closeButton != null)
        {
            _closeButton.clicked += CloseMenu;
        }
    }

    private void Update()
    {
        if (Mouse.current == null) return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            HandleClick();
        }
    }

    private void HandleClick()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector2 uiPos = new Vector2(mousePos.x, Screen.height - mousePos.y);

        var pickedElement = uiDocument.rootVisualElement.panel.Pick(uiPos);
        if (pickedElement != null && pickedElement != uiDocument.rootVisualElement && pickedElement.name != "inventory-bar")
        {
            return;
        }

        Ray ray = mainCamera.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out RaycastHit hit, 200f, selectableLayer))
        {
            if (hit.collider.gameObject.name.Contains("Cube"))
            {
                OpenMenu();
                return;
            }
        }

        CloseMenu();
    }

    public void OpenMenu()
    {
        if (_objectMenu == null) return;
        _objectMenu.style.display = DisplayStyle.Flex;
    }

    public void CloseMenu()
    {
        if (_objectMenu == null) return;
        _objectMenu.style.display = DisplayStyle.None;
    }
}