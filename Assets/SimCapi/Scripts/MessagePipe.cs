using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SimCapi
{
    [System.Serializable]
    class UM
    {
        public string m;

        public UM(string m)
        {
            this.m = m;

        }
    }

    public abstract class MessagePipe
    {
        public Transporter transporter;

        public abstract void sendMessage(string message);
        public abstract void receiveMessage(string message);
    }


    public class IFramePipe : MessagePipe
    {
        public override void receiveMessage(string message)
        {
            if (transporter == null)
                return;

            SimCapiConsole.log(message, "Recived:");

            transporter.reciveJsonMessage(message);
        }


        public override void sendMessage(string message)
        {
            SimCapiConsole.log(message, "Sent:");

            if (ExternalCalls.isInIFrame() == false)
                return;

            ExternalCalls.postMessage(message);
        }
    }

    /*
    public class EditorPipe : MessagePipe
    {

        SocketIOComponent _socketIOComponent;


        public EditorPipe()
        {
            _socketIOComponent = SocketIOComponent.getInstance();
            _socketIOComponent.On("unityRecive", socketRecive);


            _socketIOComponent.On("connect", clientConnected);
        }

        void clientConnected(SocketIOEvent socketIOEvent)
        {
            Debug.Log("Connected to server");

        }


        void socketRecive(SocketIOEvent socketIOEvent)
        {
            string message = socketIOEvent.data["m"].ToString();

            Debug.Log(message);
        }

        public override void sendMessage(string message)
        {
            Debug.Log(message);
            SimCapiConsole.log(message, "Sent:");

            _socketIOComponent.Emit("unitySend", new JSONObject(JsonUtility.ToJson(new UM(message))));
        }
    }
    */

}