using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private DungeonGenerator dungeonGenerator = null;

    void Start()
    {
        dungeonGenerator.Init();
        dungeonGenerator.MazeGenerator();
    }

    private void OnGUI()
    {
        float w = Screen.width / 2;
        float h = Screen.height - 80;

        if (GUI.Button(new Rect(w, h, 250, 50), "Regenerate Dungeon"))
        {
            dungeonGenerator.RegenerateDungeon();
        }
    }
}
