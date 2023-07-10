using UnityEngine;
using WebSocketSharp; 

public class FlowerAndTreeGenerator : MonoBehaviour
{
    public GameObject treePrefab;
    public GameObject flowerPrefab;
    public float radius = 10f;
    public int numTrees = 10;
    public int numFlowers = 20;
    public float checkInterval = 5f;
    public LayerMask terrainLayerMask;
    public WebSocket ws;
    public int state = 0; 
    private float lastCheckTime = 0f; 
    private float value;

    void Start() { 
        ws = new WebSocket("ws://10.19.26.73:8080");
        ws.Connect();
        ws.Send("Helo");
        ws.OnMessage += (sender, e) =>
        {
            Debug.Log("Message Received from "+((WebSocket)sender).Url+", Data : "+e.Data);
        };
    } 

    void Update()
    {
        if (Time.time - lastCheckTime >= checkInterval)
        {
            lastCheckTime = Time.time;
            
            ws.Connect();
            Debug.Log("running update");
            ws.Send("send data pls");
            ws.OnMessage += (sender, e) => 
            {
                state = int.Parse(e.Data); 
                Debug.Log("data received: " + state);  
            };
                        
            value = state; 
            Debug.Log("value being received as the state is " + value);

            if (value == 1f)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, terrainLayerMask))
                {
                    for (int i = 0; i < numTrees; i++)
                    {
                        Vector3 randomPosition = hit.point + Random.insideUnitSphere * radius;
                        randomPosition.y = 0; 
                        GameObject tree = Instantiate(treePrefab, randomPosition, Quaternion.identity);
                        tree.transform.parent = hit.transform;
                    }

                    for (int i = 0; i < numFlowers; i++)
                    {
                        Vector3 randomPosition = hit.point + Random.insideUnitSphere * radius;
                        randomPosition.y = 0;
                        GameObject flower = Instantiate(flowerPrefab, randomPosition, Quaternion.identity);
                        flower.transform.parent = hit.transform;
                    }
                }
            }
        }
    }
}