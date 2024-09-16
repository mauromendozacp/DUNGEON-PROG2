using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class RoomFactory : MonoBehaviour
{
    [SerializeField] private Room[] rooms = null;
    [SerializeField] private Transform holder = null;

    private List<Room> roomsList = null;

    public void Init()
    {
        roomsList = new List<Room>();
    }

    public Room GetRoomByType(ROOM_TYPE type)
    {
        List<Room> roomList = rooms.ToList().Where(r => r.Type == type).ToList();

        if (roomList.Count > 0)
        {
            int randomIndex = Random.Range(0, roomList.Count);
            return CreateRoom(randomIndex);
        }

        return null;
    }

    public void ClearRooms()
    {
        foreach (var room in roomsList)
        {
            Destroy(room.gameObject);
        }

        roomsList.Clear();
    }

    private Room CreateRoom(int index)
    {
        Room room = Instantiate(rooms[index], holder);
        room.Init();

        roomsList.Add(room);

        return room;
    }
}
