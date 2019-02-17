using UnityEngine;
using Mirror;

namespace MirrorExtension
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Network/NetworkLocalTransform")]
    public class NetworkLocalTransform : NetworkBehaviour
    {
        [SerializeField] string ParentObjectTagName = "SharingRoot";
        [SerializeField] bool SyncLocalRotation = false;

        // Sync variables
        [SyncVar] Vector3 localPosition;
        [SyncVar] Quaternion localRotation;

        // [SerializeField] bool LerpEnabled = true;
        // Vector3 lastPosition;
        // Quartenion lastRotation;
        // float lastClientSendTime;

        void Start()
        {
            GameObject parentObj = GameObject.FindWithTag(ParentObjectTagName);
            transform.parent = parentObj.transform;

            localPosition = Vector3.zero;
            localRotation = Quaternion.identity;
        }

        void Update()
        {
            // if server then always sync to others.
            if (isServer && hasAuthority)
            {
                UpdateSyncVars();
            }

            // no 'else if' since host mode would be both
            if (isClient)
            {
                if (!isServer && hasAuthority)
                {
                    CmdUpdateSyncVars(transform.localPosition, transform.localRotation);
                }
                if (!hasAuthority)
                {
                    UpdateLocalTransformUsingSyncVars();
                }
            }
        }

        void UpdateSyncVars()
        {
            localPosition = transform.localPosition;
            localRotation = (SyncLocalRotation) ? transform.localRotation : Quaternion.identity;
        }

        [Command]
        void CmdUpdateSyncVars(Vector3 position, Quaternion rotation)
        {
            localPosition = position;
            localRotation = (SyncLocalRotation) ? rotation : Quaternion.identity;
        }

        void UpdateLocalTransformUsingSyncVars()
        {
            transform.localPosition = localPosition;
            transform.localRotation = localRotation;
        }
    }
}
