using GestureRecognizer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level Config/Enemy Behaviour")]
public class EnemyBehaviour : ScriptableObject
{
    [Header(header: "List dari gesture")]
    [Tooltip(tooltip: "Berisi gesture yang akan dimunculkan pada bagian atas enemy")]
    [SerializeField] List<GestureRecognizer.GesturePattern> gestures;

    [Header(header: "Lokasi Spawn")]
    [Tooltip(tooltip: "Berisi dua titik koordinat X dan Y untuk memunculkan enemy")]
    [SerializeField] Vector2 spawnPoint;

    [Header(header: "Aset Gambar")]
    [Tooltip(tooltip: "Isilah dengan aset dari musuh yang akan muncul")]
    [SerializeField] Sprite enemySprite;

    [Header(header: "Animator dari enemy")]
    [Tooltip(tooltip: "Isilah dengan RuntimeAnimatorController dari animasi musuh")]
    [SerializeField] RuntimeAnimatorController enemyAnimator;

    [Header(header: "Kecepatan gerak musuh")]
    [Tooltip(tooltip: "Boleh diisi dengan bilangan desimal atau bulat")]
    [SerializeField] float moveSpeed = 1f;

    public List<GesturePattern> GetGestures() { return gestures; }
    public Vector2 GetSpawnPoint() { return spawnPoint; }
    public Sprite GetEnemySprite() { return enemySprite; }
    public RuntimeAnimatorController GetEnemyAnimator() { return enemyAnimator; }
    public float GetMoveSpeed() { return moveSpeed; }
}
