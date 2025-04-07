using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]

public class ShaderPosition : MonoBehaviour
{
    [SerializeField] private float radius = 1f;

    private void FixedUpdate()
    {
        Shader.SetGlobalVector("_Position", transform.position);
        Shader.SetGlobalFloat("_Radius", radius);
    }
}
