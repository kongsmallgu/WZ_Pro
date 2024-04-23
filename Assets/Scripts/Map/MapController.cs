using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct RoomCoord 
{
    public int x;
    public int y;
    public RoomCoord(int _x,int _y) 
    {
        x = _x;
        y = _y;
    }

    public static bool operator != (RoomCoord _c1, RoomCoord _c2) 
    {
        return !(_c1 == _c2);
    }
    public static bool operator ==(RoomCoord _c1, RoomCoord _c2)
    {
        return !(_c1.x == _c2.x) && (_c1.y == _c2.y);
    }
}
public class MapController : MonoBehaviour
{
    [Header("地图基本设置")]
    public GameObject tilePrefab;
    public int mapSize = 30;//地图大小
    private List<RoomCoord> allRoomCoord;
    public Transform mapManager;

    [Header("地图障碍物")]
    public GameObject obsPrefab;
    public int obsPrefabNumber = 30;//障碍物的数量
    private Queue<RoomCoord> shuffleQueue;//保存洗牌后的数据

    private void Start()
    {
        allRoomCoord = new List<RoomCoord>();
        //shuffleQueue = new Queue<RoomCoord>();

        GenerateMap();
        Generate0bstacle();
    }
    private void GenerateMap() 
    {
        for (int i = 0; i < mapSize; i++) 
        {
            for (int j = 0; j < mapSize; j++) 
            {
                Vector3 Pos = new Vector3(transform.position.x + i, transform.position.y, transform.position.z + j);
                GameObject spawnTile = Instantiate(tilePrefab, Pos, Quaternion.Euler(0, 0, 0));
                spawnTile.transform.SetParent(mapManager);
                allRoomCoord.Add(new RoomCoord(i, j));
            }
               
        }
          
    }

    private RoomCoord GetRandomCoord() 
    {
        RoomCoord randomCoord = shuffleQueue.Dequeue();//队列:先进先出
        shuffleQueue.Enqueue(randomCoord);//将移出的元素放在队列的最后一个，保证队列完整性，大小不变
        return randomCoord;
    
    }
    //生成的障碍物预制体
    private void Generate0bstacle() 
    {
        //int obsCount = 0;//记录已经生成了多少个障碍物
        shuffleQueue = new Queue<RoomCoord>(MapShuffle.Knuth(allRoomCoord.ToArray()));//将洗牌后的数字转化后放进数组
        Debug.Log("障碍物品的数量----------------"+ obsPrefabNumber);
        for (int obsCount = 0; obsCount < obsPrefabNumber; obsCount++)
        {
            RoomCoord randomCoord = GetRandomCoord();
            Vector3 obsPos = new Vector3(transform.position.x + randomCoord.x, 0.5f, transform.position.z + randomCoord.y);
            GameObject spawnObstacle = Instantiate(obsPrefab, obsPos, Quaternion.identity);
            spawnObstacle.transform.SetParent(mapManager);//将生成出来的障碍物进行统一管理
        }



    }

}
