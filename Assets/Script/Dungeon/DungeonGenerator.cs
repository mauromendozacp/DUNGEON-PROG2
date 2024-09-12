using System.Collections.Generic;

using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    [Header("Dungeon Configuration")]
    [SerializeField] private Vector2 dungeonSize;
    [SerializeField] private int startPos = 0;
    [SerializeField] private Vector2 offset;
    [SerializeField] private int maxIterator = 0;
    
    [Header("Seed Configuration")]
    [SerializeField] private int seed = 0;
    [SerializeField] private bool useRandomSeed = false;

    private RoomPooling roomPooling = null;

    private List<Cell> board = null;
    private int lastCell = 0;

    private void Awake()
    {
        roomPooling = GetComponent<RoomPooling>();
    }

    public void Init()
    {
        roomPooling.Init();

        board = new List<Cell>();
    }

    public void MazeGenerator()
    {
        InitRandomSeed();
        InitializeBoard();
        GenerateDungeon();
    }

    public void RegenerateDungeon()
    {
        roomPooling.ReleaseAllRooms();
        MazeGenerator();
    }

    private void InitializeBoard()
    {
        float boardLenght = dungeonSize.x * dungeonSize.y;

        for (int i = 0; i < boardLenght; i++)
        {
            board.Add(new Cell());
        }

        int currentCell = startPos;
        Stack<int> path = new Stack<int>();

        for (int i = 0; i < maxIterator; i++)
        {
            board[currentCell].visited = true;

            if (currentCell == board.Count - 1)
            {
                break;
            }

            List<int> neighbors = CheckNeighbors(currentCell);

            if (neighbors.Count == 0)
            {
                if (path.Count == 0)
                {
                    break;
                }
                else
                {
                    currentCell = path.Pop();
                }
            }
            else
            {
                path.Push(currentCell);

                int newCell = neighbors[Random.Range(0, neighbors.Count)];

                if (newCell > currentCell)
                {
                    //down or right
                    if (newCell - 1 == currentCell)
                    {
                        board[currentCell].status[2] = true;
                        currentCell = newCell;
                        board[currentCell].status[3] = true;
                    }
                    else
                    {
                        board[currentCell].status[1] = true;
                        currentCell = newCell;
                        board[currentCell].status[0] = true;
                    }
                }
                else
                {
                    //up or left
                    if (newCell + 1 == currentCell)
                    {
                        board[currentCell].status[3] = true;
                        currentCell = newCell;
                        board[currentCell].status[2] = true;
                    }
                    else
                    {
                        board[currentCell].status[0] = true;
                        currentCell = newCell;
                        board[currentCell].status[1] = true;
                    }
                }
            }
        }

        lastCell = currentCell;
    }

    private void GenerateDungeon()
    {
        for (int i = 0; i < dungeonSize.x; i++)
        {
            for (int j = 0; j < dungeonSize.y; j++)
            {
                Cell currentCell = board[Mathf.FloorToInt(i + j * dungeonSize.x)];

                if (currentCell.visited)
                {
                    GameObject newRoom = roomPooling.GetRoomObject();
                    newRoom.transform.position = new Vector3(i * offset.x, 0f, -j * offset.y);
                    newRoom.transform.rotation = Quaternion.identity;

                    RoomBehaviour rb = newRoom.GetComponent<RoomBehaviour>();
                    rb.UpdateRoom(currentCell.status);

                    newRoom.name += " " + i + "-" + j;
                }
            }
        }
    }

    private List<int> CheckNeighbors(int cell)
    {
        List<int> neighbors = new List<int>();
        
        //check Up
        if(cell - dungeonSize.x >= 0 && !board[Mathf.FloorToInt(cell - dungeonSize.x)].visited)
        {
            neighbors.Add(Mathf.FloorToInt(cell - dungeonSize.x));
        }
        
        //check Down
        if(cell + dungeonSize.x < board.Count && !board[Mathf.FloorToInt(cell + dungeonSize.x)].visited)
        {
            neighbors.Add(Mathf.FloorToInt(cell + dungeonSize.x));
        }

        //check Right
        if((cell + 1) % dungeonSize.x != 0 && !board[Mathf.FloorToInt(cell + 1)].visited)
        {
            neighbors.Add(Mathf.FloorToInt(cell + 1));
        }

        //check Left
        if(cell % dungeonSize.x != 0 && !board[Mathf.FloorToInt(cell - 1)].visited)
        {
            neighbors.Add(Mathf.FloorToInt(cell - 1));
        }

        return neighbors;
    }

    private void InitRandomSeed()
    {
        if (useRandomSeed)
        {
            int randomSeed = System.DateTime.Now.GetHashCode();
            Random.InitState(randomSeed);

            Debug.Log("Seed Generated: " + randomSeed);
        }
        else
        {
            Random.InitState(seed);
        }
    }
}
