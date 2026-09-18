using UnityEngine;

[System.Serializable]
public struct ComboLink
{
    public InputButton  InputButton;     // 콤보 공격에 필요한 입력 버튼
    public AttackData   Next;            // 다음 콤보 공격 데이터
}
