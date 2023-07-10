using UnityEngine;
using WebSocketSharp;
public class WsClient : MonoBehaviour
{
    WebSocket ws;
    public int state = 0; 
    private float checkInterval = 3f; 
    private float lastCheckTime = 0f; 

    void Start()
    {
        ws = new WebSocket("ws://10.19.26.73:8080");
        ws.Connect();
        ws.Send("Helo");
        ws.OnMessage += (sender, e) =>
        {
            Debug.Log("Message Received from "+((WebSocket)sender).Url+", Data : "+e.Data);
        };
    }
    
    public void Update()
        {
            if (Time.time - lastCheckTime >= checkInterval)
            {
                ws.Connect();
                lastCheckTime = Time.time;
                Debug.Log("running update");
                ws.Send("send data pls");
                ws.OnMessage += (sender, e) => 
                {
                    state = int.Parse(e.Data); 
                    Debug.Log("data received: " + state);  
                };
    
                if(ws == null)
                {
                    return;
                }  
            }
            
        }
}