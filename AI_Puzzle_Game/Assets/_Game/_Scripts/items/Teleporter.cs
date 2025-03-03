using System.Collections;
using System.Collections.Generic;
using _Game._Scripts.interfaces;
using UnityEngine;

public class Teleporter : MonoBehaviour, ITeleporter
{
    [SerializeField] private int teleportToIslandIndex;
    [SerializeField] private Transform teleportLocation;
    public int GetIslandIndex()
    {
        return teleportToIslandIndex;
    }

    public Vector3 GetTeleportLocation()
    {
        return teleportLocation.position;
    } 
}
