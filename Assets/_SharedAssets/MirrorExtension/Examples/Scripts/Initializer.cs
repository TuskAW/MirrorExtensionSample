using System.Collections;
using UnityEngine;
using UniHumanoid;
using Mirror;

namespace MirrorExtension.Examples
{
    public class Initializer : MonoBehaviour
    {
        [SerializeField] GameObject HumanPoseSynchronizerPrefab;
        [SerializeField] HumanPoseTransfer m_source;

        void Start()
        {
            StartCoroutine(Initialize());
        }

        IEnumerator Initialize()
        {
            yield return new WaitWhile(() => NetworkServer.active);

            GameObject go = Instantiate(HumanPoseSynchronizerPrefab);
            NetworkServer.Spawn(go);

            HumanPoseTransfer m_target = go.GetComponent<HumanPoseTransfer>();
            if (m_target != null)
            {
                m_target.Source = m_source;
                m_target.SourceType = UniHumanoid.HumanPoseTransfer.HumanPoseTransferSourceType.HumanPoseTransfer;
            }
        }
    }
}
