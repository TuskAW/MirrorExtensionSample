using UnityEngine;
using Mirror;

namespace MirrorExtension
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Network/NetworkHumanPose")]
    [RequireComponent(typeof(NetworkIdentity))]
    [RequireComponent(typeof(Animator))]
    public class NetworkHumanPose : NetworkBehaviour
    {
        public bool m_LerpEnabled = true;
        public bool m_FixedBodyPosition = false;

        // Sync variables
        [SyncVar] Vector3 bodyPosition;
        [SyncVar] Quaternion bodyRotation;
        SyncListFloat muscles;

        private float lastClientSendTime;
    
        private Avatar m_Avatar;
        private HumanPoseHandler m_PoseHandler;
        private HumanPose m_NextPose;
        private HumanPose m_PreviousPose;
        private HumanPose m_CurrentPose;
        private float m_LerpWeight;

        private int m_SynchronizeMusclesCount;
        private Vector3 m_InitialBodyPosition;
        private Quaternion m_InitialBodyRotation;

        void Start()
        {
            var animator = GetComponent<Animator>();
            if (animator != null && animator.avatar != null)
            {
                m_Avatar = animator.avatar;
                m_PoseHandler = new HumanPoseHandler(m_Avatar, transform);
                m_PoseHandler.GetHumanPose(ref m_NextPose);
                m_PoseHandler.GetHumanPose(ref m_PreviousPose);
                m_PoseHandler.GetHumanPose(ref m_CurrentPose);

                m_InitialBodyPosition = m_CurrentPose.bodyPosition;
                m_InitialBodyRotation = m_CurrentPose.bodyRotation;

                m_SynchronizeMusclesCount = m_NextPose.muscles.Length;
                if(isServer)
                {
                    for (int i = 0; i < m_SynchronizeMusclesCount; i++)
                    {
                        muscles.Add(0.0f);
                    }
                }
            }
        }

        void Update()
        {   
            // if server then always sync to others.
            if (isServer)
            {
                UpdateSyncVars();
            }

            // no 'else if' since host mode would be both
            if (isClient)
            {
                if (!hasAuthority)
                {
                    UpdateHumanPoseUsingSyncVars(m_LerpEnabled);
                }
            }
        }

        void UpdateSyncVars()
        {
            m_PoseHandler.GetHumanPose(ref m_NextPose);

            bodyPosition = m_NextPose.bodyPosition;
            bodyRotation = m_NextPose.bodyRotation;
            for (int i = 0; i < m_SynchronizeMusclesCount; i++)
            {
                muscles[i] = m_NextPose.muscles[i];
            }
        }

        void UpdateHumanPoseUsingSyncVars(bool lerpEnabled)
        {
            m_PreviousPose.bodyPosition = m_NextPose.bodyPosition;
            m_PreviousPose.bodyRotation = m_NextPose.bodyRotation;
            for (int i = 0; i < m_SynchronizeMusclesCount; i++)
            {
                m_PreviousPose.muscles[i] = m_NextPose.muscles[i];
            }

            m_NextPose.bodyPosition = bodyPosition;
            m_NextPose.bodyRotation = bodyRotation;
            for (int i = 0; i < m_SynchronizeMusclesCount; i++)
            {
                m_NextPose.muscles[i] = muscles[i];
            }

            // check only each 'syncInterval'
            if (Time.time - lastClientSendTime >= syncInterval)
            {
                m_LerpWeight = 0.0f;
                lastClientSendTime = Time.time;
            }

            if (m_LerpEnabled)
            {
                float fps = Mathf.Round(1.0f / Time.unscaledDeltaTime); // [frame/second]
                float serializationRate = 1.0f / syncInterval; // [times/second]
                m_LerpWeight += (serializationRate / fps);

                Vector3 InterpolatedPosition = Vector3.Lerp(m_PreviousPose.bodyPosition, m_NextPose.bodyPosition, m_LerpWeight);
                m_CurrentPose.bodyPosition = (m_FixedBodyPosition) ? m_InitialBodyPosition : InterpolatedPosition;

                m_CurrentPose.bodyRotation = Quaternion.Lerp(m_PreviousPose.bodyRotation, m_NextPose.bodyRotation, m_LerpWeight);
                for (int i = 0; i < m_SynchronizeMusclesCount; i++)
                {
                    m_CurrentPose.muscles[i] = Mathf.Lerp(m_PreviousPose.muscles[i], m_NextPose.muscles[i], m_LerpWeight);
                }

                m_PoseHandler.SetHumanPose(ref m_CurrentPose);
            }
            else
            {
                if (m_FixedBodyPosition)
                {
                    m_NextPose.bodyPosition = m_InitialBodyPosition;
                }
                m_PoseHandler.SetHumanPose(ref m_NextPose);
            }
        }
    }
}