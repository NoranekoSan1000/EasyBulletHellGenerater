using UnityEngine;

namespace EasyBulletHellGenerator
{
    public class BulletsLauncher : MonoBehaviour
    {
        private GameObject bulletsManagerObject;
        private GameObject bulletsPool;
        public GameObject targetObject;  

        private void Awake()
        {
            bulletsManagerObject = new GameObject("bulletsManagerObject");
            bulletsPool = new GameObject("bulletsPool");
            bulletsPool.AddComponent<BulletsPool>();
        }

        /// <summary>
        /// ScriptableObject��p���Đݒ�
        /// </summary>
        public BulletsManager GenerateBulletHell(BulletPattern bulletpattern)
        {
            GameObject bullets = Instantiate(bulletsManagerObject, transform.position, transform.rotation);
            BulletsManager s = bullets.AddComponent<BulletsManager>();
            s.Initialize(bulletpattern.bulletObject, targetObject); // �I�u�W�F�N�g�ݒ�
            s.InitialBulletStatus(bulletpattern.isMissile, bulletpattern.speed, bulletpattern.acceleration, bulletpattern.existTime); // �e�̋����ݒ�

            if (bulletpattern.formation == BulletsManager.ShotFormation.StraightShot)
            {
                s.StraightShot(bulletpattern.executionCount, bulletpattern.interval); // �e���ݒ�
            }
            else if (bulletpattern.formation == BulletsManager.ShotFormation.ExplosionShot)
            {
                s.ExplosionShot(bulletpattern.executionCount, bulletpattern.interval, bulletpattern.numBullets); // �e���ݒ�
            }
            else if (bulletpattern.formation == BulletsManager.ShotFormation.SpreadShot)
            {
                s.SpreadShot(bulletpattern.executionCount, bulletpattern.interval, bulletpattern.spreadAngle, bulletpattern.numBullets); // �e���ݒ�
            }
            else if (bulletpattern.formation == BulletsManager.ShotFormation.SiegeShot)
            {
                s.SiegeShot(bulletpattern.executionCount, bulletpattern.interval, bulletpattern.siegeRadius ,bulletpattern.numBullets); // �e���ݒ�
            }
            else
            {
                Debug.LogError("BulletPattern is Missing");
                s.StraightShot(bulletpattern.executionCount, bulletpattern.interval); // �e���ݒ�
            }
            return s;
        }
    }
}