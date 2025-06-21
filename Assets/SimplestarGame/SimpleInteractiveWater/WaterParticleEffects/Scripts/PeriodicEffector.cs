using UnityEngine;

namespace SimplestarGame.WaterParticle
{
    public class PeriodicEffector : MonoBehaviour
    {
        internal void StartEffect(float period)
        {
            this.ps?.Play();
            this.isPlaying = true;
            this.period = period;
        }

        internal void StartPowerEffect(float period, Vector3 velocity)
        {
            this.StartEffect(period);
        }

        private void Start()
        {
            this.ps = this.GetComponent<ParticleSystem>();
        }

        private void Update()
        {
            if (!this.isPlaying)
            {
                return;
            }

            this.period -= Time.deltaTime;
            if (0 > this.period)
            {
                this.ps?.Stop();
                this.isPlaying = false;
            }
        }

        float period = 2.0f;
        ParticleSystem ps = null;
        bool isPlaying = false;
    }
}