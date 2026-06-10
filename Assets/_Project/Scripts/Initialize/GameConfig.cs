using SpaceShooter.Ammunition;
using SpaceShooter.Asteroids;
using SpaceShooter.Player;
using SpaceShooter.ShooterPlayer;
using SpaceShooter.Score;
using SpaceShooter.UFOs;
using UnityEngine;
using UnityEngine.UI;

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

    public Bullet[] BulletPrefabs;
    public Lazer[] LazerPrefabs;
    public UFOEnemy[] UfoPrefabs;
    public AsteroidsEnemy[] AsteroidPrefabs;
    public SmallAsteroidEnemy[] SmallAsteroidPrefabs;

    public GameObject BackgroundPrefab;
    public ScoreManagerUI ScoreManagerPrefab;
    public Button ButtonPlayAgainPrefab;

    public PlayerUIView UIPlayerPrefab;
    public ShooterUIView UIShooterPrefab;

    public PlayerGameOverUIView GameOverUIPrefab; 

    public PlayerMovement PlayerPrefab;

    public int BulletPoolSize;
    public int LazerPoolSize;
    public int UfoPoolSize;
    public int AsteroidPoolSize;
    public int SmallAsteroidPoolSize;
}
