using UnityEngine;

namespace _Game._Scripts.interfaces
{
    public interface ITeleporter
    {
        public int GetIslandIndex();
        public Vector3 GetTeleportLocation();
    }
}