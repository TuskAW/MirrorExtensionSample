using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MirrorExtension.Examples
{
    public class TransformSynchronizer : MonoBehaviour
    {
        public Transform SourceTransform;
        public bool SyncRotation = false;

        void Update()
        {
            transform.position = SourceTransform.position;
            if (SyncRotation)
            {
                transform.rotation = SourceTransform.rotation;
            }
        }
    }
}