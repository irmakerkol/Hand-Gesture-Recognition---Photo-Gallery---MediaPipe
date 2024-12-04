using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class GestureReceiver : MonoBehaviour
{
    private UdpClient udpClient;
    private IPEndPoint remoteEndPoint;

    public RectTransform cursor; // Drag the cursor UI element here
    public RectTransform canvasRect; // Drag the canvas here for scaling

    void Start()
    {
        udpClient = new UdpClient(5052); // Port to listen on
        remoteEndPoint = new IPEndPoint(IPAddress.Any, 5052);
    }

    void Update()
    {
        if (udpClient.Available > 0)
        {
            byte[] data = udpClient.Receive(ref remoteEndPoint);
            string message = Encoding.ASCII.GetString(data);
            string[] coords = message.Split(',');

            if (coords.Length == 2)
            {
                float x = float.Parse(coords[0]);
                float y = float.Parse(coords[1]);

                // Convert normalized coordinates to canvas coordinates
                Vector2 canvasPos = new Vector2(x * canvasRect.sizeDelta.x, y * canvasRect.sizeDelta.y);

                // Update cursor position
                cursor.anchoredPosition = canvasPos;
            }
        }
    }

    void OnApplicationQuit()
    {
        udpClient.Close();
    }
}
