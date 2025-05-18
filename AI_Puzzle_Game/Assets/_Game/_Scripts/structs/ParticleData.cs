using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ParticleData {
    [SerializeField] public ParticleType type;
    [SerializeField] public GameObject particlePrefab;
    public List<GameObject> particles {get; private set; }

    public GameObject GetParticle() {
        for (int i = 0; i < particles.Count; i++) {
            var p = particles[i];
            if (!p.activeSelf) return p; 
        }
        return null;
    }

    public void AddParticle(GameObject particle) {
        particles.Add(particle);
    }

    public static bool operator true(ParticleData v) {
        return v != null;
    }

    public static bool operator false(ParticleData v) {
        return v == null;
    }
}