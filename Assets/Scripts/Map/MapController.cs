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
    [Header("��ͼ��������")]
    public GameObject tilePrefab;
    public int mapSize = 30;//��ͼ��С
    private List<RoomCoord> allRoomCoord;
    public Transform mapManager;

    [Header("��ͼ�ϰ���")]
    public GameObject obsPrefab;
    public int obsPrefabNumber = 30;//�ϰ��������
    private Queue<RoomCoord> shuffleQueue;//����ϴ�ƺ������

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
        RoomCoord randomCoord = shuffleQueue.Dequeue();//����:�Ƚ��ȳ�
        shuffleQueue.Enqueue(randomCoord);//���Ƴ���Ԫ�ط��ڶ��е����һ������֤���������ԣ���С����
        return randomCoord;
    
    }
    //���ɵ��ϰ���Ԥ����
    private void Generate0bstacle() 
    {
        //int obsCount = 0;//��¼�Ѿ������˶��ٸ��ϰ���
        shuffleQueue = new Queue<RoomCoord>(MapShuffle.Knuth(allRoomCoord.ToArray()));//��ϴ�ƺ������ת����Ž�����
        Debug.Log("�ϰ���Ʒ������----------------"+ obsPrefabNumber);
        for (int obsCount = 0; obsCount < obsPrefabNumber; obsCount++)
        {
            RoomCoord randomCoord = GetRandomCoord();
            Vector3 obsPos = new Vector3(transform.position.x + randomCoord.x, 0.5f, transform.position.z + randomCoord.y);
            GameObject spawnObstacle = Instantiate(obsPrefab, obsPos, Quaternion.identity);
            spawnObstacle.transform.SetParent(mapManager);//�����ɳ������ϰ������ͳһ����
        }



    }

}
