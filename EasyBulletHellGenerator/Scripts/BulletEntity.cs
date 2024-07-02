using System;
using UnityEngine;

namespace EasyBulletHellGenerator
{
    public class BulletEntity : MonoBehaviour
    {
        private Action onDisable;

        private BulletsManager.ObjectDirection objDirection;
        private Vector3 direction; //���˕��� o
        private bool isMissile; //�U�����邩�ۂ� o
        private GameObject missileTarget; //�U���^�[�Q�b�g o
        private float speed; //���x o
        private float acceleration; //�����x o
        private float existTime; //���ݎ��� o  
        private float changeDirectionTime;// �^�[�Q�b�g�ύX����
        private float changeGravityTime;
        private float startPauseTime;//pause�J�n����
        private float endPauseTime;//pause�I������

        private float elapsedTime = 0f;
        private bool hasChangedDirection = false; // ������ς������ǂ����̃t���O
        private bool isPaused = false; //�ꎞ��~�����ǂ����̃t���O

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public void Initialize(Action ondisable)
        {
            onDisable = ondisable;
        }

        public void Initialize(Vector3 direction, Vector3 launchpos, bool ismissile, GameObject missiletarget, float speed, float acceleration,
            float existtime, float changedirectiontime, float changegravitytime, float startpausetime, float endpausetime,
            BulletsManager.ObjectDirection objdirection)
        {
            objDirection = objdirection;
            transform.rotation = SetQuaternionFromObjectDirection(objdirection);
            this.direction = direction;            
            isMissile = ismissile;
            missileTarget = missiletarget;
            this.speed = speed;
            this.acceleration = acceleration;
            existTime = existtime;
            changeDirectionTime = changedirectiontime;
            changeGravityTime = changegravitytime;
            startPauseTime = startpausetime;
            endPauseTime = endpausetime;

            transform.position = launchpos;
            elapsedTime = 0f;
            hasChangedDirection = false;
            isPaused = false;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void Update()
        {
            elapsedTime += Time.deltaTime;// ���Ԍo�߂̒ǐ�
            if (elapsedTime >= existTime)
            {
                onDisable?.Invoke();
                gameObject.SetActive(false);
            }

            if (elapsedTime >= startPauseTime && elapsedTime < endPauseTime) isPaused = true;
            else isPaused = false;
            if (isPaused) return;

            float currentSpeed = speed + acceleration * elapsedTime; // ���x�̍X�V

            if (elapsedTime >= changeGravityTime && missileTarget != null)
            {
                direction += Vector3.down * 9.81f * Time.deltaTime; // �d�͂̉e����ǉ�
            }

            if (isMissile && missileTarget != null) //�U���e
            {
                Vector3 directionToTarget = (missileTarget.transform.position - transform.position).normalized;
                transform.position += directionToTarget * currentSpeed * Time.deltaTime;
                if (direction != Vector3.zero)
                {
                    transform.rotation = Quaternion.LookRotation(directionToTarget);
                }
                    
            }
            else
            {
                // �^�[�Q�b�g�̌�����ς���^�C�~���O�ɒB������A�^�[�Q�b�g�̕����Ɍ�����
                if (!hasChangedDirection && elapsedTime >= changeDirectionTime && missileTarget != null)
                {
                    ChangeDirectionToTarget();
                }

                // �^�[�Q�b�g�̕����Ɍ���������̓^�[�Q�b�g�̕����ɐi��
                transform.position += direction * currentSpeed * Time.deltaTime;
                if (hasChangedDirection && direction != Vector3.zero)
                {
                    transform.rotation = Quaternion.LookRotation(direction);
                }
                else if (!hasChangedDirection && direction != Vector3.zero)
                {
                    transform.rotation = SetQuaternionFromObjectDirection(objDirection);
                }
            }
        }

        private void ChangeDirectionToTarget()
        {
            Vector3 directionToTarget = (missileTarget.transform.position - transform.position).normalized;
            direction = directionToTarget; // �������^�[�Q�b�g�̕����ɕύX
            transform.rotation = Quaternion.LookRotation(directionToTarget);
            hasChangedDirection = true;
        }

        private Quaternion SetQuaternionFromObjectDirection(BulletsManager.ObjectDirection objdirection)
        {
            if(objdirection == BulletsManager.ObjectDirection.Front) return Quaternion.LookRotation(Vector3.forward);
            else if (objdirection == BulletsManager.ObjectDirection.Back) return Quaternion.LookRotation(Vector3.back);
            else if (objdirection == BulletsManager.ObjectDirection.Right) return Quaternion.LookRotation(Vector3.right);
            else if (objdirection == BulletsManager.ObjectDirection.Left) return Quaternion.LookRotation(Vector3.left);
            else if (objdirection == BulletsManager.ObjectDirection.Up) return Quaternion.LookRotation(Vector3.up);
            else if (objdirection == BulletsManager.ObjectDirection.Down) return Quaternion.LookRotation(Vector3.down);
            else if (objdirection == BulletsManager.ObjectDirection.Direction_of_movement) return Quaternion.LookRotation(direction);
            else return Quaternion.LookRotation(Vector3.forward);

        }

    }
}