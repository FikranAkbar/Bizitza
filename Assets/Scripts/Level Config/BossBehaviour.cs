using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level Config/Boss Behaviour")]
public class BossBehaviour : ScriptableObject
{
    [Header(header: "List dari gesture")]
    [Tooltip(tooltip: "Berisi gesture yang akan dimunculkan pada bagian atas enemy")]
    [SerializeField] List<GestureRecognizer.GesturePattern> gestures;

    [Header(header: "Animator dari enemy")]
    [Tooltip(tooltip: "Isilah dengan RuntimeAnimatorController dari animasi musuh")]
    [SerializeField] RuntimeAnimatorController bossAnimator;

    [Header(header: "Kecepatan gerak musuh")]
    [Tooltip(tooltip: "Boleh diisi dengan bilangan desimal atau bulat")]
    [SerializeField] float moveSpeed = 1f;

    public List<GestureRecognizer.GesturePattern> GetGestures() { return gestures; }
    public RuntimeAnimatorController GetEnemyAnimator() { return bossAnimator; }
    public float GetMoveSpeed() { return moveSpeed; }
}
