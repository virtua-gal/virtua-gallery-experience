using SimplestarGame.WaterParticle;
using UnityEngine;
// using Valve.VR;

namespace SimplestarGame.Wave
{
    /// <summary>
    /// If collision object has WaveSimulator, Add wave at the ClosestPoint with its velocity.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class WaterInterferer : MonoBehaviour
    {
        [SerializeField] PeriodicEffector waterDrop;
        [SerializeField] PeriodicEffector waterSplash;

        //public void SetPosition(SteamVR_Behaviour_Pose pose, SteamVR_Input_Sources sources)
        //{
        //    this.transform.position = pose.transform.position;
        //    this.transform.rotation = Quaternion.identity;
        //}

        void Start()
        {
            this.lastPoint = transform.position;
        }

        void FixedUpdate()
        {
            // For reading the delta time it is recommended to use Time.deltaTime instead.
            // Because it automatically returns the right delta time if you are inside a FixedUpdate function or Update function.
            this.velocity = (this.transform.position - this.lastPoint) / Time.deltaTime;
            this.lastPoint = transform.position;    
        }

        void OnTriggerEnter(Collider other)
        {
            var waveComputer = other.gameObject.GetComponent<WaveSimulator>();
            if (null == waveComputer)
            {
                return;
            }
            waveComputer.AddWave(other.ClosestPoint(transform.position), this.velocity);

            if (null != this.waterSplash)
            {
                this.waterSplash.transform.position = new Vector3(transform.position.x, other.transform.position.y + 0.051f, transform.position.z);
                this.waterSplash.StartPowerEffect(0.2f, this.velocity);
            }
        }

        void OnTriggerExit(Collider other)
        {
            var waveComputer = other.gameObject.GetComponent<WaveSimulator>();
            if (null == waveComputer)
            {
                return;
            }
            waveComputer.AddWave(other.ClosestPoint(transform.position), this.velocity);

            if (null != this.waterDrop)
            {
                this.waterDrop.StartEffect(0.2f);
            }
        }

        Vector3 velocity;
        Vector3 lastPoint;
    }
}
