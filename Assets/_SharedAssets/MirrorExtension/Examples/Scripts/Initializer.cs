using System.Collections;
using UnityEngine;
using UniHumanoid;
using Mirror;

namespace MirrorExtension.Examples
{
    public class Initializer :MonoBehaviour
    {
        [SerializeField] GameObject HumanPoseSynchronizerPrefab;
        [SerializeField] GameObject SourceObject;

        void Start()
        {
            StartCoroutine(Initialize());
        }

        IEnumerator Initialize()
        {
            yield return new WaitUntil(() => NetworkServer.active);

            GameObject go = Instantiate(HumanPoseSynchronizerPrefab);
            NetworkServer.SpawnWithClientAuthority(go, NetworkServer.localConnection);

            HumanPoseTransfer m_target = go.GetComponent<HumanPoseTransfer>();
            HumanPoseTransfer m_source = SourceObject.GetComponent<HumanPoseTransfer>();
            if (m_target != null)
            {
                m_target.Source = m_source;
                m_target.SourceType = UniHumanoid.HumanPoseTransfer.HumanPoseTransferSourceType.HumanPoseTransfer;
            }

            TransformSynchronizer synchronizer = go.AddComponent<TransformSynchronizer>();
            synchronizer.SourceTransform = SourceObject.transform;
        }
    }
}
