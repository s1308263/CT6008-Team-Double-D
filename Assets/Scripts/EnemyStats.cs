using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "Scriptable Objects/EnemyStats")]
public class EnemyStats : ScriptableObject
{   
    [SerializeField] public float speed = 3;
    [SerializeField] public int health = 10;
    [SerializeField] public float attackSpeed = 10;
    [SerializeField] public int damage = 10;
    [SerializeField] public int compartmentHealth = 5;
    [SerializeField] public int playerHealth = 1;
    [SerializeField] public int missileDamge = 2;
    [SerializeField] public int boatEnemyHealth = 20;
    [SerializeField] public int advancedEnemyHealth = 50;
}
