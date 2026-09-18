using UnityEngine;

public class Enemy : MonoBehaviour, IHittable
{
    public void TakeHit(HitData hitData)
    {
        Debug.Log("Take Hit");
    }
}
