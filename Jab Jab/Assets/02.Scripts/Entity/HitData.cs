using UnityEngine;

[System.Serializable]
public struct HitData
{
    public HitData(float damage, Vector3 hitDirection)
    {
        Damage = damage;
        HitDirection = hitDirection;
    }

    public float Damage;
    public Vector3 HitDirection;
    // 경직 정도


}
