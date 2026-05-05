using System.Collections.Generic;
using UnityEngine;

public class BoatEnemyManager : MonoBehaviour
{
    Transform player;
    float spacing = 1.5f;

    public List<BoatEnemyMove> enemies = new List<BoatEnemyMove>();

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
    }
    public void Register(BoatEnemyMove enemy)
    {
        if (!enemies.Contains(enemy))
            enemies.Add(enemy);
    }
    public void Unregister(BoatEnemyMove enemy)
    {
        enemies.Remove(enemy);
    }
    void Update()
    {
        AssignSlots();
    }
    void AssignSlots()
    {
        int count = enemies.Count;

        for (int i = 0; i < count; i++)
        {
            int slotIndex = GetBalancedIndex(i);

            float targetX = player.position.x + slotIndex * spacing;
            enemies[i].SetTargetX(targetX);
        }
    }
    int GetBalancedIndex(int i)
    {
        if (i == 0) return 1;

        int n = (i + 1) / 2;
        return (i % 2 == 0) ? -n : n;
    }
}
