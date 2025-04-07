using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] private GameObject visual;
    [SerializeField] private Collider col;

    public void OnOpenGate() {
        visual.SetActive(false);
        col.enabled = false;
    }
}
