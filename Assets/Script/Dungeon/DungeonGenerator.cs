using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonGenerator : MonoBehaviour
{
    [SerializeField] private RoomPooling roomPooling = null;

    [SerializeField] Vector2 _dungeonSize;
    [SerializeField] int _startPos = 0;

    [SerializeField] GameObject[]  _rooms;
    [SerializeField] Vector2 offset;
    
    List<Cell> _board;
   
    public void Init()
    {
        roomPooling.Init();
    }

    void GenerateDungeon()
    {
        for (int i = 0; i < _dungeonSize.x; i++)
        {
            for (int j = 0; j < _dungeonSize.y; j++)
            {
                Cell currentCell = _board[Mathf.FloorToInt(i + j * _dungeonSize.x)];

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

    public void MazeGenerator()
    {
        //Create Dungeon board
        _board = new List<Cell>();

       /* for(int i = 0; i < _dungeonSize.x; i++)
        {
            for(int j = 0; j < _dungeonSize.y; j++)
            {
                _board.Add(new Cell());
            }
        }*/

        float boardLenght = _dungeonSize.x * _dungeonSize.y;

        for(int i = 0; i < boardLenght; i++)
        {
            _board.Add(new Cell());
        }
      
       //Create Dungeon Maze
       //StarPosition determina el casillero donde el arranca el Dungeon
        int currentCell = _startPos;

        //Generamos la Pila(Stack) donde armaremos el Laberinto
        Stack<int> path = new Stack<int>();
     
        int k = 0;
        while(k < 1000)
        {
            k ++;
          
            //marca la celda actual como visitada
            _board[currentCell].visited = true;

            //si se alcanza la celda de salida
            //ser termina el bucle
            if(currentCell == _board.Count -1)
            {
                break;
            }

            //Check Neighbors cells
            List<int> neighbors = CheckNeighbors(currentCell);

            if(neighbors.Count == 0)
            {
                if(path.Count == 0)
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
                
                int newCell = neighbors[Random.Range(0,neighbors.Count)];

                if(newCell > currentCell)
                {
                    //down or right
                    if( newCell - 1 == currentCell)
                    {
                        _board[currentCell].status[2] = true;
                        currentCell = newCell;
                        _board[currentCell].status[3] = true;
                    }
                    else
                    {
                        _board[currentCell].status[1] = true;
                        currentCell = newCell;
                        _board[currentCell].status[0] = true;
                    }
                }
                else
                {
                     //up or left
                    if( newCell + 1 == currentCell)
                    {
                        _board[currentCell].status[3] = true;
                        currentCell = newCell;
                        _board[currentCell].status[2] = true;
                    }
                    else
                    {
                        _board[currentCell].status[0] = true;
                        currentCell = newCell;
                        _board[currentCell].status[1] = true;
                    }
                }

            }
        }

       //Instantiate rooms
        GenerateDungeon();
      
    }



    
    //Chequea las celdas vecinas
    List<int> CheckNeighbors(int cell)
    {
        List<int> neighbors = new List<int>();
        
        //check Up
        if(cell - _dungeonSize.x >= 0 && !_board[Mathf.FloorToInt(cell - _dungeonSize.x)].visited)
        {
            neighbors.Add(Mathf.FloorToInt(cell - _dungeonSize.x));
        }
        
        //check Down
        if(cell + _dungeonSize.x < _board.Count && !_board[Mathf.FloorToInt(cell + _dungeonSize.x)].visited)
        {
            neighbors.Add(Mathf.FloorToInt(cell + _dungeonSize.x));
        }

        //check Right
        if((cell + 1) % _dungeonSize.x != 0 && !_board[Mathf.FloorToInt(cell + 1)].visited)
        {
            neighbors.Add(Mathf.FloorToInt(cell + 1));
        }

        //check Left
        if(cell % _dungeonSize.x != 0 && !_board[Mathf.FloorToInt(cell - 1)].visited)
        {
            neighbors.Add(Mathf.FloorToInt(cell - 1));
        }

        return neighbors;
    }


    private void OnGUI() 
    {
        float w = Screen.width/2;
        float h = Screen.height - 80;
        if(GUI.Button(new Rect(w,h,250,50), "Regenerate Dungeon"))
        {
            RegenerateDungeon();
        }
    }

    void RegenerateDungeon()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
}
