using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Prolog/Scenario")]
public class PrologScenario : ScriptableObject
{
    [Header(header: "Comic Picture")]
    [SerializeField] Sprite comicPicture;

    [Header(header: "Starting Point")]
    [SerializeField] Vector2 startingPoint;

    [Header(header: "End Point")]
    [SerializeField] Vector2 endPoint;

    public Sprite GetComicPicutre() { return comicPicture; }

    public Vector2 GetStartingPoint() { return startingPoint; }

    public Vector2 GetEndPoint() { return endPoint; }
}
