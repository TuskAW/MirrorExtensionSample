using UnityEngine;
using Mirror;

namespace MirrorExtension
{
    [RequireComponent(typeof(NetworkManager))]
    public class NetworkManagerHost : MonoBehaviour
    {
        NetworkManager manager;

        void Awake()
        {
            manager = GetComponent<NetworkManager>();
        }

        void Start()
        {
            bool noConnection = (manager.client == null || manager.client.connection == null ||
                                 manager.client.connection.connectionId == -1);

            if (!manager.IsClientConnected() && !NetworkServer.active)
            {
                if (noConnection)
                {
                    if (Application.platform != RuntimePlatform.WebGLPlayer)
                    {
                        manager.StartHost();
                    }
                }
            }
        }
    }
}
