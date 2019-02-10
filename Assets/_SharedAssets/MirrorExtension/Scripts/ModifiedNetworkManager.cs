// ========================================================
// ModifiedNetworkManager.cs
//
// Reference:
// http://motoyama.hateblo.jp/entry/unet-networkdiscovery
// ========================================================
using UnityEngine;
using Mirror;

namespace MirrorExtension
{
    [RequireComponent(typeof(ModifiedNetworkDiscovery))]
    public class ModifiedNetworkManager : NetworkManager
    {
        [SerializeField] bool useDiscovery;
        [SerializeField] bool startAsHost;

        ModifiedNetworkDiscovery discovery;

        void Start()
        {
            discovery = GetComponent<ModifiedNetworkDiscovery>();

            if (startAsHost)
            {
                StartHost();
                if (useDiscovery)
                {
                    discovery.Initialize();
                    discovery.StartAsServer();
                }
            }
            else
            {
                StartClient();
                if (useDiscovery)
                {
                    discovery.Initialize();
                    discovery.StartAsClient();
                }
            }
        }

        public override void OnStopServer()
        {
            if (useDiscovery)
            {
                discovery.StopBroadcast();
            }
        }
    }
}
