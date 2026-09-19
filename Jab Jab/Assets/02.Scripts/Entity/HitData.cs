using UnityEngine;

[System.Serializable]
public struct HitData
{
    public HitData(float damage, Vector3 hitDirection, HitStrength hitStrength, int hitStopFrames)
    {
        Damage = damage;
        HitDirection = hitDirection;
        HitStrength = hitStrength;
        HitStopFrames = hitStopFrames;
    }

    public float Damage;
    public Vector3 HitDirection;
    public HitStrength HitStrength;
    public int HitStopFrames;
}
