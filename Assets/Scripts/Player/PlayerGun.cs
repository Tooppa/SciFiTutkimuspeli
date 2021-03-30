using UnityEngine;

namespace Player
{
    public class PlayerGun : MonoBehaviour
    {
        private ParticleSystem _gun;
        public bool HasGun  { private set; get; }

        private GameObject _audioController;

        public float cooldown;
        public float counter;

        private void Start()
        {
            _gun = GetComponentInChildren<ParticleSystem>();
            _audioController = GameObject.Find("AudioController");
            counter = cooldown;
            HasGun = false;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R) && _gun.gameObject.activeSelf && counter > cooldown && HasGun)
            {
                CameraEffects.Instance.ShakeCamera(1.5f, .1f);
                _gun.Play();
                counter = 0;

                _audioController.GetComponent<SFX>().PlayGunShot();
            }
            else
                _gun.Stop();

            counter += Time.deltaTime;

        }

        public void EquipGun()
        {
            HasGun = true;
        }
    }
}
