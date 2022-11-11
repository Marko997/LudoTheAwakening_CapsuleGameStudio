using UnityEngine;
using Unity.Netcode;

namespace CustomNetworkVariables
{
    public class CustomNetworkVariables : MonoBehaviour
    {
        
    }

    public struct NetworkString : INetworkSerializable
    {
        private string info;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref info);
        }

        public override string ToString()
        {
            return info.ToString();
        }
        public static implicit operator string(NetworkString s) => s.ToString();
        public static implicit operator NetworkString(string s) => new NetworkString() { info = new string(s) };

    }

}