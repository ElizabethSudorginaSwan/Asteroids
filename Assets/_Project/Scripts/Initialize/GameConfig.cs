using SpaceShooter.Player;
using SpaceShooter.Score;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameConfig : ScriptableObject
{
    public float MinSizeUFO;
    public float MaxSizeUFO;
    public float SpawnIntervalUFO;

    public float MinSizeAsteroid;
    public float MaxSizeAsteroid;
    public float MinRotateAsteroid;
    public float MaxRotateAsteroid;
    public float SpawnIntervalAsteroid;

    public GameObject[] BulletPrefabs;
    public GameObject[] LazerPrefabs;
    public GameObject[] UfoPrefabs;
    public GameObject[] AsteroidPrefabs;
    public GameObject[] SmallAsteroidPrefabs;

    public GameObject BackgroundPrefab;
    public GameObject ScoreManagerPrefab;
    public GameObject ButtonPlayAgainPrefab;

    public GameObject UIPlayerPrefab;
    public GameObject UIShooterPrefab;

    public GameObject PlayerPrefab;

    public int BulletPoolSize;
    public int LazerPoolSize;
    public int UfoPoolSize;
    public int AsteroidPoolSize;
    public int SmallAsteroidPoolSize;
}
