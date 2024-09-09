using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonGenerator : MonoBehaviour
{
    [SerializeField] private RoomPooling roomPooling = null;

    [SerializeField] private Vector2 dungeonSize;
    [SerializeField] private int startPos = 0;
    [SerializeField] private Vector2 offset;
    [SerializeField] private int maxIterator = 0;
    
    private List<Cell> board;
    private int lastCell = 0;
   
    public void Init()
    {
        roomPooling.Init();
    }

    public void MazeGenerator()
    {
        InitializeBoard();
        GenerateDungeon();
    }

    public void RegenerateDungeon()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void InitializeBoard()
    {
        //Create Dungeon board
        board = new List<Cell>();

        float boardLenght = dungeonSize.x * dungeonSize.y;

        for (int i = 0; i < boardLenght; i++)
        {
            board.Add(new Cell());
        }

        //Create Dungeon Maze
        //StarPosition determina el casillero donde el arranca el Dungeon
        int currentCell = startPos;

        //Generamos la Pila(Stack) donde armaremos el Laberinto
        Stack<int> path = new Stack<int>();

        for (int i = 0; i < maxIterator; i++)
        {
            //marca la celda actual como visitada
            board[currentCell].visited = true;

            //si se alcanza la celda de salida
            //ser termina el bucle
            if (currentCell == board.Count - 1)
            {
                break;
            }

            //Check Neighbors cells
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
}
