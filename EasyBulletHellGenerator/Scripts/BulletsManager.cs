using UnityEngine;

namespace EasyBulletHellGenerator
{
    public class BulletsManager : MonoBehaviour
    {
        public enum ShotFormation
        {
            StraightShot = 100,
            ExplosionShot = 200,
            SpreadShot = 300,
            SiegeShot = 400,
        } //�����e�A�����e�A�g�U�e�A��͒e
        private ShotFormation formation = ShotFormation.StraightShot; //���ˌ`��
        private GameObject bullet;
        private GameObject target;
        private float interval; //�ˌ��Ԋu
        private int executionCount; //���s��
        private Vector3 direction;//�ړ��̌���
        private ObjectDirection objDirection; //�I�u�W�F�N�g�̌���
        public enum ObjectDirection
        {
            Front = 100,
            Back = 200,
            Left = 300,
            Right = 400,
            Up = 500,
            Down = 600,
            Direction_of_movement = 700, //�i�s����
        }
        private int numBullets;
        private float spreadAngle; //���g�U�̂�
        private float siegeRadius; //����͂̂�
        private float rotationIncrement; // �e���ˎ��̊p�x������

        private Quaternion rotationOffset = Quaternion.identity;
        private Vector3 positionOffset = new Vector3(0, 0, 0);

        private bool isMissile; //�U�����邩�ۂ�
        private float speed; //���x
        private float acceleration; //�����x
        private float existTime; //���ݎ���
        private float changeTargetTime;//�^�[�Q�b�g�ύX����
        private float changeGravityTime;//�d��ON����
        private float startpauseTime;//pause�J�n����
        private float endPauseTime;//pause�I������

        private float elapsedTime = 999f;
        private int currentExecutionCount = 999;
        private float currentRotationAngle = 0;
        private bool endPauseAllFire;

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void Awake()
        {
            formation = ShotFormation.StraightShot;
            rotationOffset = Quaternion.identity;
            positionOffset = new Vector3(0, 0, 0);
            rotationIncrement = 0;
        }

        /// <summary> �I�u�W�F�N�g�ݒ�(�K�{) </summary>
        public void Initialize(GameObject bullet, GameObject target)
        {
            this.bullet = bullet;
            this.target = target;
        }

        /// <summary> �e�̋����ݒ�(�K�{) </summary>
        public void InitialBulletStatus(bool ismissile, float speed, float acceleration, float existtime, ObjectDirection objdirection)
        {
            isMissile = ismissile;
            this.speed = speed;
            this.acceleration = acceleration;
            existTime = existtime;
            objDirection = objdirection;

            this.direction = Vector3.back;
            changeTargetTime = 9999999999999;
            changeGravityTime = 9999999999999;
            startpauseTime = 9999999999999;
            endPauseTime = 9999999999999;
            elapsedTime = 0f;
            currentExecutionCount = 0;
            currentRotationAngle = 0;
            endPauseAllFire = false;
        }

        /// <summary> �����e </summary>
        public void StraightShot(int executioncount, float interval)
        {
            executionCount = executioncount;
            this.interval = interval;
            formation = ShotFormation.StraightShot;
            numBullets = 1;
        }

        /// <summary> �����e </summary>
        public void ExplosionShot(int executioncount, float interval, int numbullets)
        {
            executionCount = executioncount;
            this.interval = interval;
            formation = ShotFormation.ExplosionShot;
            numBullets = numbullets;
        }

        /// <summary> �g�U�e </summary>
        public void SpreadShot(int executioncount, float interval, float spreadangle, int numbullets){
            executionCount = executioncount;
            this.interval = interval;
            formation = ShotFormation.SpreadShot;
            spreadAngle = spreadangle;
            numBullets = numbullets;
        }

        /// <summary> ��͒e </summary>
        public void SiegeShot(int executioncount, float interval, float radius, int numbullets)
        {
            executionCount = executioncount;
            this.interval = interval;
            formation = ShotFormation.SiegeShot;
            numBullets = numbullets;
            siegeRadius = radius;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary> �ǉ��ݒ�@�����p�xX�ύX </summary>
        public BulletsManager SetAngleOffsetX(float angle)
        {
            rotationOffset = Quaternion.AngleAxis(angle, Vector3.right) * rotationOffset;
            return this;
        }

        /// <summary> �ǉ��ݒ�@�����p�xY�ύX </summary>
        public BulletsManager SetAngleOffsetY(float angle)
        {
            rotationOffset = Quaternion.AngleAxis(angle, Vector3.up) * rotationOffset;
            return this;
        }

        /// <summary> �ǉ��ݒ�@�����p�xZ�ύX </summary>
        public BulletsManager SetAngleOffsetZ(float angle)
        {
            rotationOffset = Quaternion.AngleAxis(angle, Vector3.forward) * rotationOffset;
            return this;
        }

        /// <summary> �ǉ��ݒ�@�����ʒu�ύX </summary>
        public BulletsManager SetPositionOffset(Vector3 addpos)
        {
            positionOffset = addpos;
            return this;
        }

        /// <summary> �ǉ��ݒ�@1�����Ƃɉ�] </summary>
        public BulletsManager SetRotate(float angle)
        {
            rotationIncrement = angle;
            return this;
        }

        /// <summary> �ǉ��ݒ�@�w��b����^�[�Q�b�g�֌����ύX </summary>
        public BulletsManager SetChangeTarget(GameObject newtarget, float changetime)
        {
            target = newtarget;
            changeTargetTime = changetime;
            return this;
        }

        /// <summary> �ǉ��ݒ�@�w��b����d��ON </summary>
        public BulletsManager SetChangeGravity(float changetime)
        {
            changeGravityTime = changetime;
            return this;
        }


        /// <summary> �ǉ��ݒ�@�w��b����ꎞ��~ </summary>
        public BulletsManager SetStartPauseTime(float startpausetime)
        {
            startpauseTime = startpausetime;
            return this;
        }

        /// <summary> �ǉ��ݒ�@�w��b����ꎞ��~�I�� </summary>
        public BulletsManager SetEndPauseTime(float endpausetime)
        {
            endPauseTime = endpausetime;
            return this;
        }

        /// <summary> �ǉ��ݒ�@�S�e���ˌ�ꎞ��~�I�� </summary>
        public BulletsManager SetEndPauseAllFire()
        {
            endPauseAllFire = true;
            return this;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void straightShot()
        {
            BulletEntity shotA = BulletsPool.Instance.InstBullet(bullet);
            shotA.Initialize(direction, transform.position + positionOffset, isMissile, target, speed,
            acceleration, existTime, changeTargetTime, changeGravityTime, startpauseTime, endPauseTime, objDirection);
        }

        private void spreadShot()
        {

            if (numBullets == 1) straightShot();
            else
            {
                for (int i = 0; i < numBullets; i++)
                {
                    var angle = -spreadAngle / 2f + i * (spreadAngle / (numBullets - 1)) + currentRotationAngle;
                    var newDirection = Quaternion.AngleAxis(angle, Vector3.up) * direction;
                    newDirection = rotationOffset * newDirection;

                    BulletEntity shotA = BulletsPool.Instance.InstBullet(bullet);
                    shotA.Initialize(newDirection, transform.position + positionOffset, isMissile, target, speed,
                      acceleration, existTime, changeTargetTime, changeGravityTime, startpauseTime, endPauseTime, objDirection);
                }

                // ���̔��ˎ��̊p�x�����ʂ�ݒ�
                currentRotationAngle += rotationIncrement;
            }
        }

        private void explosionShot()
        {
            float angleIncrement = 360f / numBullets;

            if (numBullets == 1) straightShot();
            else
            {
                for (int i = 0; i < numBullets; i++)
                {
                    var angle = i * angleIncrement + currentRotationAngle;
                    var newDirection = Quaternion.Euler(0, angle, 0) * direction;
                    newDirection = rotationOffset * newDirection;

                    BulletEntity shotA = BulletsPool.Instance.InstBullet(bullet);
                    shotA.Initialize(newDirection, transform.position + positionOffset, isMissile, target,
                    speed, acceleration, existTime, changeTargetTime, changeGravityTime, startpauseTime, endPauseTime, objDirection);
                }

                // ���̔��ˎ��̊p�x�����ʂ�ݒ�
                currentRotationAngle += rotationIncrement;

            }
        }

        private void siegeShot()
        {
            float angleIncrement = 360f / numBullets;
            Vector3 baseOffset = Vector3.up * siegeRadius;

            for (int i = 0; i < numBullets; i++)
            {
                var angle = i * angleIncrement + currentRotationAngle;
                var offset = Quaternion.AngleAxis(angle, Vector3.forward) * baseOffset;
                offset = rotationOffset * offset;
                var siegePosition = target.transform.position + offset;

                BulletEntity shotA = BulletsPool.Instance.InstBullet(bullet);
                shotA.Initialize((target.transform.position - siegePosition).normalized, siegePosition + positionOffset, isMissile, target,
                speed, acceleration, existTime, changeTargetTime, changeGravityTime, startpauseTime, endPauseTime, objDirection);
            }

            // ���̔��ˎ��̊p�x�����ʂ�ݒ�
            currentRotationAngle += rotationIncrement;

        }

        private void Update()
        {
            elapsedTime -= Time.deltaTime;
            if (elapsedTime <= 0f && currentExecutionCount < executionCount)
            {
                //�ꎞ��~�I�����Ԃ�ݒ�
                if (endPauseAllFire) endPauseTime = startpauseTime + interval * executionCount - interval * currentExecutionCount;

                if (formation == ShotFormation.StraightShot) straightShot();
                if (formation == ShotFormation.SpreadShot) spreadShot();
                if (formation == ShotFormation.ExplosionShot) explosionShot();
                if (formation == ShotFormation.SiegeShot) siegeShot();

                elapsedTime = interval;
                currentExecutionCount++;
            }

            if (currentExecutionCount >= executionCount)
            {
                Destroy(gameObject);
            }
        }

    }
}
