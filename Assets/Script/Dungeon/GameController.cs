using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private DungeonGenerator dungeonGenerator = null;
    [SerializeField] private PlayerController playerController = null;

    private Vector3 startPlayerPosition = Vector3.zero;

    void Start()
    {
        startPlayerPosition = playerController.transform.position;

        dungeonGenerator.Init();
        dungeonGenerator.MazeGenerator();
    }

    private void OnGUI()
    {
        float w = Screen.width / 2;
        float h = Screen.height - 80;

        if (GUI.Button(new Rect(w, h, 250, 50), "Regenerate Dungeon"))
        {
            playerController.ResetPlayer(startPlayerPosition);
            dungeonGenerator.RegenerateDungeon();
        }
    }
}
