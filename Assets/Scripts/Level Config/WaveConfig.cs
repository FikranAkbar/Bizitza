using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level Config/Wave")]
public class WaveConfig : ScriptableObject
{
    [Header(header: "List Dari Enemy")]
    [Tooltip(tooltip: "Isilah dengan enemy yang akan dimunculkan pada gelombang ini")]
    [SerializeField] List<EnemyBehaviour> enemies;

    [Header(header: "Delay Awal Gelombang")]
    [Tooltip(tooltip: "Delay waktu sebelum gelombang dimulai")]
    [SerializeField] float timeToStartWave = 3f;

    [Header(header: "Jeda Waktu Antara Spawn")]
    [Tooltip(tooltip: "")]
    [SerializeField] float timeBtwnSpawn = 0.2f;

    public List<EnemyBehaviour> GetEnemies() { return enemies; }
    public float GetTimeToStartWave() { return timeToStartWave; }
    public float GetTimeBtwnSpawn() { return timeBtwnSpawn; }
}
