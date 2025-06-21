using UnityEngine;

namespace SimplestarGame
{
    public class TrackObject : MonoBehaviour
    {
        [SerializeField] GameObject trackTarget;
        [SerializeField] GameObject trackObject;

        // Update is called once per frame
        void Update()
        {
            this.trackObject.transform.position = this.trackTarget.transform.position;
        }
    }
}
