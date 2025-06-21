using UnityEngine;

namespace SimplestarGame
{
    public class KinematicBehaviour : MonoBehaviour
    {
        [SerializeField, Tooltip("Rotation Speed")] float rotationSpeed = 2.5f;
        void Start()
        {
            this.centor = this.transform.position - Vector3.up;
        }

        void Update()
        {
            this.transform.position = this.centor + new Vector3(0, 
                Mathf.Sin(Time.realtimeSinceStartup * rotationSpeed), 
                Mathf.Cos(Time.realtimeSinceStartup * rotationSpeed));
        }

        Vector3 centor = Vector3.zero;
    }
}