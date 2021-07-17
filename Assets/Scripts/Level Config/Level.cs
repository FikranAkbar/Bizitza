using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Level Config/Level")]
public class Level : ScriptableObject
{
    [Header(header: "List dari gelombang musuh")]
    [Tooltip(tooltip: "Isilah dengan gelombang musuh yang akan dimunculkan pada level ini")]
    [SerializeField] List<WaveConfig> waves;

    [SerializeField] WaveConfig bossWave;

    public List<WaveConfig> GetWaves() { return waves; }
    public WaveConfig GetBossWave() { return bossWave; }
}
