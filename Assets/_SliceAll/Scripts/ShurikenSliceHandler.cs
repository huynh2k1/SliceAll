using DynamicMeshCutter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenSliceHandler : MonoBehaviour
{
    [SerializeField] bool isCut = false;
    [SerializeField] BoxCollider _boxCollider;

    public Shuriken _shurikenObject;

    private void OnTriggerEnter(Collider other)
    {
        if (isCut) return;

        DynamicRagdoll dynamicRagdoll = other.GetComponentInParent<DynamicRagdoll>();
        if (dynamicRagdoll != null)
        {
            MeshTarget target = dynamicRagdoll.GetComponentInChildren<MeshTarget>();
            if (target == null) return;
            _shurikenObject.Cut(target);    
        } 

        MeshTarget meshTarget = other.GetComponent<MeshTarget>();
        if (meshTarget != null)
        {
            _shurikenObject.Cut(meshTarget);
        }

        isCut = true;
    }

    private void OnTriggerExit(Collider other)
    {
        isCut = false;
    }
    
    public void ScaleModel(float scale)
    {
        _boxCollider.size *= scale;
    }
}
