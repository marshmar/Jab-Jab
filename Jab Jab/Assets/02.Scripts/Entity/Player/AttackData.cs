using UnityEngine;

[CreateAssetMenu(menuName = "JabJab/AttackData")]
public class AttackData : ScriptableObject
{
    public string AnimStateName;            // Animator State 이름
    public float BusyDuration;              // 이 공격이 끝났다고 칠 시간
    public float CancelStartRatio;          // 몇 % 지점부터 다음 입력을 받을지
    public ComboLink[] ComboLinks;          // 다음 이어질 공격에 대한 정보들

    public float HitStartRatio;             // 공격 판정 시작 시간 비율
    public float HitEndRatio;               // 공격 판정 종료 시간 비율
    public Vector3 HitBoxOffset;
    public Vector3 HitBoxSize;

    public float Damage;
    public HitStrength HitStrength;

    public int HitStopFrames;
}
