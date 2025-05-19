using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    private PlayerInputsReader inputs;
    void OnEnable()
    {
        inputs ??= Resources.Load<PlayerInputsReader>("PlayerInputsReader");
        inputs.OnMove += OnMove;  
    }

    private void OnMove(Vector3 d) {
        ParticlesManager.Instance.SpawnParticle(ParticleType.MOVE, transform.position);
    }
}
