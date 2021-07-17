using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level Config/Boss Wave")]
public class BossWaveConfig : ScriptableObject
{
    [SerializeField] List<EnemyBehaviour> bosses;
    [SerializeField] Vector2 spawnPoint;
    [SerializeField] float timeToStartWave = 3f;

    public List<EnemyBehaviour> GetBosses() { return bosses; }
    public Vector2 GetSpawnPoint() { return spawnPoint; }
    public float GetTimeToStartWave() { return timeToStartWave; }
}
