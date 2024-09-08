using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private DungeonGenerator dungeonGenerator = null;

    void Start()
    {
        dungeonGenerator.Init();
        dungeonGenerator.MazeGenerator();
    }
}
