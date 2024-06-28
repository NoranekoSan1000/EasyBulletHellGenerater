using System;
using UnityEngine;

public class BulletEntity : MonoBehaviour
{
    private Action onDisable;  // ��A�N�e�B�u�����邽�߂̃R�[���o�b�N

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
        float existtime, float changedirectiontime, float changegravitytime, float startpausetime, float endpausetime)
    {
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
        }
        else
        {
            // �^�[�Q�b�g�̌�����ς���^�C�~���O�ɒB������A�^�[�Q�b�g�̕����Ɍ�����
            if (!hasChangedDirection && elapsedTime >= changeDirectionTime && missileTarget != null)
            {
                ChangeDirectionToTarget();
            }
            else
            {
                // �^�[�Q�b�g�̕����Ɍ���������͏����̔��˕����ɐi��
                transform.position += direction * currentSpeed * Time.deltaTime;
            }
        } 
    }

    private void ChangeDirectionToTarget()
    {
        Vector3 directionToTarget = (missileTarget.transform.position - transform.position).normalized;
        direction = directionToTarget; // �������^�[�Q�b�g�̕����ɕύX
        transform.rotation = Quaternion.LookRotation(directionToTarget); // �e�̉�]���ύX �s�v�����H
        hasChangedDirection = true;
    }
   
}