using UnityEngine;
using UnityEngine.Pool;

public class BulletObjectPool : MonoBehaviour
{
    private static BulletObjectPool instance;
    [SerializeField] private BulletEntity bulletObject;  // �v�[���ŊǗ�����I�u�W�F�N�g
    private ObjectPool<BulletEntity> bulletPool;

    public static BulletObjectPool Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<BulletObjectPool>();
            }
            return instance;
        }
    }

    private void Start()
    {
         bulletPool = new ObjectPool<BulletEntity>(
            createFunc: () => CreateBullet(),
            actionOnGet: (b) => GetBullet(b),
            actionOnRelease: (b) => OnBulletReleased(b),
            actionOnDestroy: (b) => DestroyBullet(b),
            collectionCheck: true,
            defaultCapacity: 3,
            maxSize: 1000
        );
    }
    
    public BulletEntity GetBullet() => bulletPool.Get(); // �v�[������bullet���擾

    public void ClearBullet() => bulletPool.Clear(); // �v�[���̒��g�����

    /// ////////////////////////////////////////////////////////////////////////////////////////////////////////

    // �v�[���ɓ����bullet�̃C���X�^���X
    private BulletEntity CreateBullet() => Instantiate(bulletObject, transform);

    private void GetBullet(BulletEntity bulletObject) // �v�[������bullet���擾���ꂽ�Ƃ��̏���
    {
        bulletObject.Initialize(() => bulletPool.Release(bulletObject));
        bulletObject.gameObject.SetActive(true);
    }
    
    private void OnBulletReleased(BulletEntity bulletObject) 
    {
        // �v�[����bullet���ԋp���ꂽ�Ƃ��̏���
    }

    private void DestroyBullet(BulletEntity bulletObject) // �v�[������bullet���폜�����Ƃ��̏���
    {
        Destroy(bulletObject.gameObject);
    }
}
