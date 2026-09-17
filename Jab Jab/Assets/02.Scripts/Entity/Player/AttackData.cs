using UnityEngine;

[CreateAssetMenu(menuName = "JabJab/AttackData")]
public class AttackData : ScriptableObject
{
    public string animName;     // Animator State 이름
    public float length;        // 이 공격이 끝났다고 칠 시간
    public float cancelRatio;   // 몇 % 지점부터 다음 입력을 받을지
}
