using UnityEngine;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using CymaticLabs.Unity3D.Amqp.SimpleJSON;
using System.Collections.Generic;


[System.Serializable]
public class ClownDataWrapper
{
    public ClownData[] prompts;
    public string user;
    public string topic;
}


[System.Serializable]
public class ClownData
{
    public string name;
    public string content;
    public string sound_bytearray;
}

public class ServerLogic : MonoBehaviour
{
    public GameLogic gameLogic;

    public int promptIdx = 0;

    public List<ClownDataWrapper> clownDataList = new List<ClownDataWrapper>();
    public ClownData[] clownDataArray;
    ClownDataWrapper wrapper;

    public string ServerAddress = "192.168.229.129";
    public string Username = "guest";
    public string Password = "guest";
    public string OutputQueueName = "output_queue";

    private IConnection connection;
    private IModel channel;
    private EventingBasicConsumer consumer;

    private void Start()
    {

        //InitTest();

        DontDestroyOnLoad(gameObject);
        ConnectToRabbitMQ();
    }

    private void OnDestroy()
    {
        DisconnectFromRabbitMQ();
    }

    private void ConnectToRabbitMQ()
    {
        ConnectionFactory factory = new ConnectionFactory()
        {
            HostName = ServerAddress,
            UserName = Username,
            Password = Password
        };

        try
        {
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            channel.QueueDeclare(OutputQueueName, true, false, false, null);

            consumer = new EventingBasicConsumer(channel);
            consumer.Received += OnMessageReceived;

            channel.BasicConsume(OutputQueueName, true, consumer);
            Debug.LogError("connected to RabbitMQ: ");
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to connect to RabbitMQ: " + e.Message);
        }
    }

    private void DisconnectFromRabbitMQ()
    {
        if (channel != null && channel.IsOpen)
            channel.Close();

        if (connection != null && connection.IsOpen)
            connection.Close();
    }

    private void OnMessageReceived(object sender, BasicDeliverEventArgs e)
    {
        ReadOnlyMemory<byte> body = e.Body;
        byte[] byteArray = body.ToArray();
        string message = Encoding.UTF8.GetString(byteArray);

        Debug.Log("Received message from RabbitMQ: " + message);
        // Deserialize the JSON data into an array of ClownData objects


        wrapper = JsonUtility.FromJson<ClownDataWrapper>(message);
        Debug.Log("User: " + wrapper.user);
        Debug.Log("Topic: " + wrapper.topic);
        Debug.Log("Data: " + wrapper.prompts.Length.ToString());
        // Access the data from the clownDataArray
        foreach (ClownData clownData in wrapper.prompts)
        {
            Debug.Log("Name: " + clownData.name);
            Debug.Log("Content: " + clownData.content);
            Debug.Log("Sound Byte Array: " + clownData.sound_bytearray);
        }

        // Process the received message
        //Debug.Log("Received message from RabbitMQ: " + message);

        clownDataList.Add(wrapper);
        gameLogic.OnReceiveMessageFromServer();
    }

    public ClownDataWrapper GetClownData()
    {
        if(clownDataList.Count == 0 || promptIdx == clownDataList.Count)
        {
            return null;
        }
        return clownDataList[promptIdx++];
        //gameLogic.AddPromptData(clownDataList[promptIdx]);
    }

    void InitTest()
    {
        for(int i = 0; i < 3; i++)
        {
            ClownDataWrapper data = new ClownDataWrapper();
            ClownData[] item = generateClownData(i);
            data.prompts = item;
            data.topic = "";
            clownDataList.Add(data);
        }
    }

    ClownData[] generateClownData(int j)
    {
        ClownData[] clowns = new ClownData[5];
        for (int i = 0; i < 5; i ++)
        {
            int x = i + j;
            ClownData data = new ClownData();
            data.name = "name" + x.ToString();
            data.content = "content is this way" + x.ToString();
            data.sound_bytearray = x.ToString();

            clowns[i] = data;
        }
        return clowns;
    }

    public void AddTest()
    {
        ClownDataWrapper data = new ClownDataWrapper();

        ClownData[] item = generateClownData(4);
        data.prompts = item;
        clownDataList.Add(data);
        gameLogic.OnReceiveMessageFromServer();
        Debug.Log("Received message from RabbitMQ: ----");

    }
}
