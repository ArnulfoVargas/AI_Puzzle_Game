using System.Collections.Generic;
using UnityEngine;

public class ParticlesManager : MonoBehaviour {
    public static ParticlesManager Instance {get; private set;}
    [SerializeField] List<ParticleData> particlesData;

    void Awake()
    {
        Instance ??= this;

        if (Instance != this) {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(this);
    }

    public void SpawnParticle(ParticleType particleType, Vector3 postition) {
        SpawnParticle(particleType, postition, Quaternion.identity);
    }

    public void SpawnParticle(ParticleType particleType, Vector3 postition, Quaternion rotation) {
        ParticleData data = null;

        for (int i = 0; i < particlesData.Count; i++) {
            var pd = particlesData[i];
            if (pd.type == particleType) {
                data = pd;
                break;
            }
        }

        if (data == null) {
            return;
        }

        var particle = data.GetParticle();

        if (particle == null) {
            particle = Instantiate(data.particlePrefab, postition, rotation);
            data.AddParticle(particle);
        }

        particle.SetActive(true);

    }
}