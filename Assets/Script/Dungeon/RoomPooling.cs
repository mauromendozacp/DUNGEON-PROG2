using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Pool;

public class RoomPooling : MonoBehaviour
{
    [SerializeField] private GameObject[] rooms = null;
    [SerializeField] private Transform holder = null;

    private ObjectPool<GameObject> roomPool = null;
    private List<GameObject> roomList = null;

    public void Init()
    {
        roomPool = new ObjectPool<GameObject>(CreateRoom, GetRoom, ReleaseRoom, DestroyRoom);
        roomList = new List<GameObject>();
    }

    public GameObject GetRoomObject()
    {
        GameObject roomGO = roomPool.Get();
        roomList.Add(roomGO);

        return roomGO;
    }

    public void ReleaseAllRooms()
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            roomPool.Release(roomList[i]);
        }

        roomList.Clear();
    }

    private GameObject CreateRoom()
    {
        int randomIndex = Random.Range(0, rooms.Length);
        GameObject roomGO = Instantiate(rooms[randomIndex], holder);

        return roomGO;
    }

    private void GetRoom(GameObject room)
    {
        room.SetActive(true);
    }

    private void ReleaseRoom(GameObject room)
    {
        room.SetActive(false);
    }

    private void DestroyRoom(GameObject room)
    {
        Destroy(room);
    }
}
