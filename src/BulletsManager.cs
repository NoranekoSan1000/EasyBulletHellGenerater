using UnityEngine;

public class BulletsManager : MonoBehaviour
{
    public enum ShotFormation { StraightShot, ExplosionShot, SpreadShot, SiegeShot, WaveShot } //�����e�A�����e�A�g�U�e�A��͒e�A�E�F�[�u�A�i�����E�ǉ��\��j
    private ShotFormation formation = ShotFormation.StraightShot; //���ˌ`��
    private GameObject bullet;
    private GameObject target;
    private float interval; //���s����
    private int executionCount; //���s��
    private Vector3 direction;
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
    public void InitialBulletStatus(bool ismissile, float speed, float acceleration, float existtime)
    {
        isMissile = ismissile;
        this.speed = speed;
        this.acceleration = acceleration;
        existTime = existtime;

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
    public void StraightShot(int executioncount, float interval, Vector3 direction)
    {
        executionCount = executioncount;
        this.interval = interval;
        formation = ShotFormation.StraightShot;
        this.direction = direction;
        numBullets = 1;
    }

    /// <summary> �����e </summary>
    public void ExplosionShot(int executioncount, float interval, Vector3 direction, int numbullets)
    {
        executionCount = executioncount;
        this.interval = interval;
        formation = ShotFormation.ExplosionShot;
        this.direction = direction;
        numBullets = numbullets;
    }

    /// <summary> �g�U�e </summary>
    public void SpreadShot(int executioncount, float interval, float spreadangle, Vector3 direction, int numbullets)
    {
        executionCount = executioncount;
        this.interval = interval;
        formation = ShotFormation.SpreadShot;
        spreadAngle = spreadangle;
        this.direction = direction;
        numBullets = numbullets;
    }

    /// <summary> ��͒e </summary>
    public void SiegeShot(int executioncount, float interval, float radius, Vector3 direction, int numbullets)
    {
        executionCount = executioncount;
        this.interval = interval;
        formation = ShotFormation.SiegeShot;
        this.direction = direction;
        numBullets = numbullets;
        siegeRadius = radius;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /// <summary> �ǉ��ݒ�@�����p�xX�ύX </summary>
    public void SetAngleOffsetX(float angle)
    {
        rotationOffset = Quaternion.AngleAxis(angle, Vector3.right) * rotationOffset;
    }

    /// <summary> �ǉ��ݒ�@�����p�xY�ύX </summary>
    public void SetAngleOffsetY(float angle)
    {
        rotationOffset = Quaternion.AngleAxis(angle, Vector3.up) * rotationOffset;
    }

    /// <summary> �ǉ��ݒ�@�����p�xZ�ύX </summary>
    public void SetAngleOffsetZ(float angle)
    {
        rotationOffset = Quaternion.AngleAxis(angle, Vector3.forward) * rotationOffset;
    }

    /// <summary> �ǉ��ݒ�@�����ʒu�ύX </summary>
    public void SetPositionOffset(Vector3 addpos)
    {
        positionOffset = addpos;
    }

    /// <summary> �ǉ��ݒ�@1�����Ƃɉ�] </summary>
    public void SetRotate(float angle)
    {
        rotationIncrement = angle;
    }

    /// <summary> �ǉ��ݒ�@�w��b����^�[�Q�b�g�֌����ύX </summary>
    public void SetChangeTarget(GameObject newtarget, float changetime)
    {
        target = newtarget;
        changeTargetTime = changetime;
    }

    /// <summary> �ǉ��ݒ�@�w��b����d��ON </summary>
    public void SetChangeGravity(float changetime)
    {
        changeGravityTime = changetime;
    }


    /// <summary> �ǉ��ݒ�@�w��b����ꎞ��~ </summary>
    public void SetStartPauseTime(float startpausetime)
    {
        startpauseTime = startpausetime;
    }

    /// <summary> �ǉ��ݒ�@�w��b����ꎞ��~�I�� </summary>
    public void SetEndPauseTime(float endpausetime)
    {
        endPauseTime = endpausetime;
    }

    /// <summary> �ǉ��ݒ�@�S�e���ˌ�ꎞ��~�I�� </summary>
    public void SetEndPauseAllFire()
    {
        endPauseAllFire = true;  
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private void straightShot()
    {  
        BulletEntity shotA = BulletObjectPool.Instance.GetBullet();
        shotA.Initialize(direction, transform.position + positionOffset, isMissile, target, speed, 
            acceleration, existTime, changeTargetTime, changeGravityTime, startpauseTime, endPauseTime);
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

                BulletEntity shotA = BulletObjectPool.Instance.GetBullet();
                shotA.Initialize(newDirection, transform.position + positionOffset, isMissile, target, speed, 
                    acceleration, existTime, changeTargetTime, changeGravityTime, startpauseTime, endPauseTime);
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

                BulletEntity shotA = BulletObjectPool.Instance.GetBullet();
                shotA.Initialize(newDirection, transform.position + positionOffset, isMissile, target, 
                    speed, acceleration, existTime, changeTargetTime, changeGravityTime, startpauseTime, endPauseTime);
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

            BulletEntity shotA = BulletObjectPool.Instance.GetBullet();
            shotA.Initialize((target.transform.position - siegePosition).normalized, siegePosition + positionOffset, isMissile, target,
                speed, acceleration, existTime, changeTargetTime, changeGravityTime, startpauseTime, endPauseTime);
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

