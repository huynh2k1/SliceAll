using DynamicMeshCutter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class InputController : CutterBehaviour
{
    [Header("WEAPON SETTINGS")]
    [SerializeField] Shuriken _shurikenPrefab;
    [SerializeField] float _spawnThreshold = 0.5f; //ngưỡng cho phép bắn 
    [SerializeField] float _shurikenSpeed = 20f; //tốc độ bay của shuriken
    [SerializeField] float _minScale = 0.5f; //tỉ lệ nhỏ nhất của shuriken
    [SerializeField] float _maxScale = 3f; //tỉ lệ lớn nhất của shuriken
    [SerializeField] float _scaleMultiplier = 1f; //hệ số nhân để điều chỉnh tỉ lệ của shuriken

    [Header("LINE RENDERER SETTINGS")]
    [SerializeField] LineRenderer _lineRenderer;
    [SerializeField] int _lineRendererPositionCount = 6; //số lượng điểm trên Line Renderer
    private Vector3 _from;
    private Vector3 _to;

    public bool _isDragging = false;


    public enum InputControlType
    {
        Mouse,
        Shuriken
    }

    [Header("SETTINGS")]
    [SerializeField] InputControlType _inputControlType;

    protected override void Update()
    {
        base.Update();

        if (Input.GetMouseButtonDown(0))
        {
            _isDragging = true;

#if ENABLE_INPUT_SYSTEM
                var mouseScreenPos = UnityEngine.InputSystem.Mouse.current.position.ReadValue();
                var mousePos = new Vector3(mouseScreenPos.x, mouseScreenPos.y, Camera.main.nearClipPlane + 0.05f);
#else
            var mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane + 0.05f);
#endif
            _from = Camera.main.ScreenToWorldPoint(mousePos);
        }

        if (_isDragging)
        {
#if ENABLE_INPUT_SYSTEM
                var mouseScreenPos = UnityEngine.InputSystem.Mouse.current.position.ReadValue();
                var mousePos = new Vector3(mouseScreenPos.x, mouseScreenPos.y, Camera.main.nearClipPlane + 0.05f);
#else
            var mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane + 0.05f);
#endif
            _to = Camera.main.ScreenToWorldPoint(mousePos);
            VisualizeLine(true);
        }
        else
        {
            VisualizeLine(false);
        }

        if (Input.GetMouseButtonUp(0) && _isDragging)
        {
            if (_inputControlType == InputControlType.Mouse)
            {
                Cut();
            }else if (_inputControlType == InputControlType.Shuriken)
            {
                SpawnShuriken();
            }
            _isDragging = false;
        }
    }

    void SpawnShuriken()
    {
        if (_shurikenPrefab == null) return;

        float distance = Vector3.Distance(_from, _to);

        if (distance < _spawnThreshold) return; //nhỏ hơn ngưỡng thì không bắn 

        float scale = Mathf.Clamp(distance * _scaleMultiplier, _minScale, _maxScale); //tính tỉ lệ dựa trên khoảng cách

        Vector3 camPos = Camera.main.transform.position;
        Vector3 lookDir = (_to - _from).normalized;
        Vector3 shootDir = ((_from + _to) * 0.5f - camPos).normalized; //hướng bắn từ camera đến điểm giữa của đường vẽ
        
        var shuriken = Instantiate(_shurikenPrefab, camPos, Quaternion.LookRotation(lookDir, shootDir));
        shuriken.ScaleModel(scale);
        shuriken.Initialize(this, shootDir * _shurikenSpeed, _from, _to);
    }

    public void Cut(MeshTarget target, Vector3 from, Vector3 to)
    {
        Plane plane = new Plane(from, to, Camera.main.transform.position);
        Cut(target, to, plane.normal, null, OnCreated);
    }

    private void Cut()
    {
        Plane plane = new Plane(_from, _to, Camera.main.transform.position);

        var roots = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (var root in roots)
        {
            if (!root.activeInHierarchy)
                continue;
            var targets = root.GetComponentsInChildren<MeshTarget>();
            foreach (var target in targets)
            {
                Cut(target, _to, plane.normal, null, OnCreated);
            }
        }
    }

    void OnCreated(Info info, MeshCreationData cData)
    {
        MeshCreation.TranslateCreatedObjects(info, cData.CreatedObjects, cData.CreatedTargets, Separation);
    }
    private void VisualizeLine(bool value)
    {
        if (_lineRenderer == null)
            return;

        _lineRenderer.enabled = value;

        if (value)
        {
            _lineRenderer.positionCount = 2;
            _lineRenderer.SetPosition(0, _from);
            _lineRenderer.SetPosition(1, _to);
        }
    }
}
