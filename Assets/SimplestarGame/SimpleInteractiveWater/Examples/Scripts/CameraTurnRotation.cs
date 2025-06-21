using UnityEngine;

namespace SimplestarGame
{
    public class CameraTurnRotation : MonoBehaviour
    {
        [SerializeField] GameObject rotationTarget;
        [SerializeField] Vector3 rotationCenter = new Vector3(0, 0.8f, 0);
        [SerializeField] float rotationSpeed = 0.001f;
        [SerializeField] float rotationRudius = 3;
        [SerializeField] float offsetHeight = 0.2f;

        // Update is called once per frame
        void Update()
        {
            this.currentRadian += this.rotationSpeed;
            this.rotationTarget.transform.position = this.rotationCenter 
                + new Vector3(Mathf.Cos(this.currentRadian), this.offsetHeight, Mathf.Sin(this.currentRadian)) * this.rotationRudius;
            this.rotationTarget.transform.LookAt(this.rotationCenter, Vector3.up);
        }

        float currentRadian = 0;
    }
}