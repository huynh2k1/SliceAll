using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class BtnShoot : MonoBehaviour, IPointerDownHandler
{
    public static Action OnPointerDownAction;
    public static Action OnPointerUpAction;

    [SerializeField] Transform player;
    [SerializeField] float rotateSpeed = 0.15f;
    [SerializeField] float smooth = 12f;

    bool isDragging;
    Vector2 lastPos;

    float initRotX;
    float initRotY;
    float rotX;
    float rotY;

    private void Awake()
    {
        PlayerCtrl.OnPlayerInit += Init;
    }

    private void OnDestroy()
    {
        PlayerCtrl.OnPlayerInit -= Init;
    }

    public void Init()
    {
        Vector3 e = player.eulerAngles;
        initRotX = e.x;
        initRotY = e.y;
        rotX = e.x;
        rotY = e.y;
        lastPos = Vector3.zero;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
        lastPos = eventData.position;

        OnPointerDownAction?.Invoke();
    }

    void Update()
    {
        if(isDragging && Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            OnPointerUpAction?.Invoke();
            rotX = initRotX;
            rotY = initRotY;
        }


        if (!isDragging)
            return;

        Vector2 currentPos = Input.mousePosition;

        Vector2 delta = currentPos - lastPos;
        lastPos = currentPos;

        rotY += delta.x * rotateSpeed;
        rotX -= delta.y * rotateSpeed;

        rotX = Mathf.Clamp(rotX, -60f, 60f);

        Quaternion targetRot = Quaternion.Euler(rotX, rotY, 0f);
        player.rotation = Quaternion.Slerp(
            player.rotation,
            targetRot,
            Time.deltaTime * smooth
        );
    }

}