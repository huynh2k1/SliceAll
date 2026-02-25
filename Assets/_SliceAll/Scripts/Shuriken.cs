using DynamicMeshCutter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Shuriken : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody _rigidbody;
    private BoxCollider _boxCollider;
    private InputController _inputController;
    private ShurikenSliceHandler _sliceHandler;

    [Header("References")]
    [SerializeField] Transform _modelHolder;

    //Cut Behavior
    Vector3 _from;
    Vector3 _to;

    [Header("Settings")]
    [SerializeField] LayerMask _obstacleLayer;


    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _boxCollider = GetComponent<BoxCollider>();
        _sliceHandler = GetComponentInChildren<ShurikenSliceHandler>();
    }

    public void Initialize(InputController controller, Vector3 velocity, Vector3 form, Vector3 to)
    {
        _inputController = controller;

        if(_rigidbody != null)
        {
            _rigidbody.velocity = velocity;
        }

        _from = form;
        _to = to;   
    }

    private void OnTriggerEnter(Collider other)
    {
        if(((1 << other.gameObject.layer) & _obstacleLayer) != 0)
        {
            StopShuriken();
        }   
    }

    public void Cut(MeshTarget target)
    {
        _inputController.Cut(target, _from, _to);
    }

    private void StopShuriken()
    {
        _rigidbody.isKinematic = true;
    }

    public void ScaleModel(float scale)
    {
        _modelHolder.localScale = Vector3.one * scale;
        _boxCollider.size *= scale;
        _sliceHandler.ScaleModel(scale);
    }
}
