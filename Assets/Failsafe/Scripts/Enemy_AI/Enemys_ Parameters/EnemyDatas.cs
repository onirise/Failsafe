using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDatas", menuName = "Data/EnemyDatas", order = 1)]
public class EnemyDatas : ScriptableObject
{
    public EnemyData[] enemyDatas; // ������ ������ ������
}

[System.Serializable]
public class EnemyData
{
    public string Name; // ��� �����
    public float Accelaration; // ��������� �����, ��� ������ �� ����� ������� ������������ �������� ��������� ����� � �������
    public int AngelarSpeed ; // ���� �������� �����, ��� ������ �� ����� ����������� ��������� ������ � �������
    public float StoppingDistance; // ��������� ��������� �����
    [Header("Enemy Chase")]
    public float ChaseSpeed; // �������� �������������
    public float LostPlayerTime; // ����� ������ ������
    [Header("Enemy Patroling")]
    public float PatrolingSpeed; // �������� ��������������
    public float PatrolingWaitTime; // ����� �������� ��� ��������������
    [Header("Enemy Searching")]
    public float SearchingSpeed; // �������� ������
    public float SearchingDuration; // ����� �������� ��� ������
    public float SearchRadius; // ������ ������
    public float offsetSearchingPoint; // ������ ������� ������
    [Header("Enemy Health")]
    public float Health; // �������� �����
    [Header("Enemy Damage")]
    public float Damage; // ���� �����
    [Header("Enemy Attack Range")]
    public bool isRangeAttack = true; // ����, �����������, �������� �� ����� �������� ���
    public float AttackRange; // ��������� ����� �����
    [Header("Enemy Attack Speed")]
    public float AttackSpeed; // �������� ����� �����
}
