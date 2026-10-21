using UnityEngine;
using UnityEngine.UIElements;

public class ItemSpawnerDrag : MonoBehaviour
{
    [Header("Ссылки на компоненты")]
    [SerializeField] private UIDocument uiDocument;
    [SerializeField] private Camera mainCamera;

    [Header("Настройки спавна")]
    [SerializeField] private GameObject prefabToSpawn; 
    [SerializeField] private LayerMask groundLayer;    

    private VisualElement _itemButton;
    private GameObject _previewObject;
    private bool _isDragging = false;

    private void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        if (uiDocument == null)
            uiDocument = GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        VisualElement root = uiDocument.rootVisualElement;

        _itemButton = root.Q<VisualElement>("item-cube");

        if (_itemButton != null)
        {
            _itemButton.RegisterCallback<PointerDownEvent>(OnPointerDown);
            _itemButton.RegisterCallback<PointerMoveEvent>(OnPointerMove);
            _itemButton.RegisterCallback<PointerUpEvent>(OnPointerUp);
        }
    }

    private void OnDisable()
    {
        if (_itemButton != null)
        {
            _itemButton.UnregisterCallback<PointerDownEvent>(OnPointerDown);
            _itemButton.UnregisterCallback<PointerMoveEvent>(OnPointerMove);
            _itemButton.UnregisterCallback<PointerUpEvent>(OnPointerUp);
        }
    }

    // Пользователь нажал на иконку
    private void OnPointerDown(PointerDownEvent evt)
    {
        if (evt.button != 0) return;

        _isDragging = true;

        _itemButton.CapturePointer(evt.pointerId);

        _previewObject = Instantiate(prefabToSpawn);

        Collider col = _previewObject.GetComponent<Collider>();
        if (col != null) col.enabled = false;

        UpdateObjectPosition(evt.position);
    }

    private void OnPointerMove(PointerMoveEvent evt)
    {
        if (!_isDragging || _previewObject == null) return;

        UpdateObjectPosition(evt.position);
    }

    private void OnPointerUp(PointerUpEvent evt)
    {
        if (!_isDragging) return;

        _isDragging = false;
        _itemButton.ReleasePointer(evt.pointerId);

        if (_previewObject == null) return;

        if (GetGroundPoint(evt.position, out Vector3 spawnPoint))
        {
            _previewObject.transform.position = spawnPoint;

            Collider col = _previewObject.GetComponent<Collider>();
            if (col != null) col.enabled = true;

            _previewObject = null;
        }
        else
        {
            Destroy(_previewObject);
            _previewObject = null;
        }
    }

    private void UpdateObjectPosition(Vector2 screenPositionUI)
    {
        if (GetGroundPoint(screenPositionUI, out Vector3 groundPoint))
        {
            _previewObject.SetActive(true);
            _previewObject.transform.position = groundPoint;
        }
        else
        {
            _previewObject.SetActive(false);
        }
    }

    private bool GetGroundPoint(Vector2 uiPos, out Vector3 hitPoint)
    {
        hitPoint = Vector3.zero;

        Vector3 screenPos = new Vector3(uiPos.x, Screen.height - uiPos.y, 0f);

        Ray ray = mainCamera.ScreenPointToRay(screenPos);

        if (Physics.Raycast(ray, out RaycastHit hit, 200f, groundLayer))
        {
            hitPoint = hit.point + Vector3.up * 0.5f;
            return true;
        }

        return false;
    }
}