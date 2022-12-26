namespace PlayFab.Networking
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    //using Mirror;
    using UnityEngine.Events;
    using Unity.Netcode;

    public class UnityNetworkManager : NetworkManager
    {
        public static UnityNetworkManager Instance { get; private set; }

        public PlayerEvent OnPlayerAdded = new PlayerEvent();
        public PlayerEvent OnPlayerRemoved = new PlayerEvent();

        public int MaxConnections = 100;
        public int Port = 7777; // overwritten by the code in AgentListener.cs

        public List<UnityNetworkConnection> Connections
        {
            get { return _connections; }
            private set { _connections = value; }
        }
        private List<UnityNetworkConnection> _connections = new List<UnityNetworkConnection>();

        public class PlayerEvent : UnityEvent<string> { }

        // Use this for initialization
        public void Awake()
        {
            Instance = this;
            //NetworkManager.RegisterHandler<ReceiveAuthenticateMessage>(OnReceiveAuthenticate);
        }

        public void StartListen()
        {
            this.Port = (ushort)Port;
            //NetworkManager.Singleton.IsListening.Listen(MaxConnections);
        }

        public void OnApplicationQuit()
        {
            NetworkManager.Singleton.Shutdown();
        }

        private void OnReceiveAuthenticate(NetworkClient nconn, ReceiveAuthenticateMessage message)
        {
            var conn = _connections.Find(c => c.ConnectionId == (int)nconn.ClientId);
            if (conn != null)
            {
                conn.PlayFabId = message.PlayFabId;
                conn.IsAuthenticated = true;
                OnPlayerAdded.Invoke(message.PlayFabId);
            }
        }

        public void OnServerConnect(NetworkClient conn)
        {
            Debug.LogWarning("Client Connected");
            var uconn = _connections.Find(c => c.ConnectionId == (int)conn.ClientId);
            if (uconn == null)
            {
                _connections.Add(new UnityNetworkConnection()
                {
                    Connection = conn,
                    ConnectionId = (int)conn.ClientId,
                    LobbyId = PlayFabMultiplayerAgentAPI.SessionConfig.SessionId
                });
            }
        }

        public void OnServerError(NetworkClient conn, Exception ex)
        {

            Debug.Log(string.Format("Unity Network Connection Status: exception - {0}", ex.Message));
        }
     
        public void OnServerDisconnect(NetworkClient conn)
        {
            var uconn = _connections.Find(c => c.ConnectionId == (int)conn.ClientId);
            if (uconn != null)
            {
                if (!string.IsNullOrEmpty(uconn.PlayFabId))
                {
                    OnPlayerRemoved.Invoke(uconn.PlayFabId);
                }
                _connections.Remove(uconn);
            }
        }
    }

    [Serializable]
    public class UnityNetworkConnection
    {
        public bool IsAuthenticated;
        public string PlayFabId;
        public string LobbyId;
        public int ConnectionId;
        public NetworkClient Connection;
    }

    public class CustomGameServerMessageTypes
    {
        public const short ReceiveAuthenticate = 900;
        public const short ShutdownMessage = 901;
        public const short MaintenanceMessage = 902;
    }

    public struct ReceiveAuthenticateMessage
    {
        public string PlayFabId;
    }

    public struct ShutdownMessage{ }

    [Serializable]
    public struct MaintenanceMessage
    {
        public DateTime ScheduledMaintenanceUTC;
    }
}