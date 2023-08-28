using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ����: ���콺 ������ ��ư�� ���� ��ź�� Ư�� �������� �߻��ϰ� �ʹ�.
// �ʿ�Ӽ�: ��ź ���ӿ�����Ʈ, �߻� ��ġ, ����
// 1-1. ���콺 ������ ��ư�� ������.
// 1-2. ��ź ���ӿ�����Ʈ�� �����ϰ� firePosition�� ��ġ��Ų��.
// 1-3. ��ź ������Ʈ�� rigidBody�� �����ͼ� ī�޶� ���� �������� ���� ���Ѵ�.

// ����2: ���콺 ���� ��ư�� ������ �ü� �������� ���� �߻��ϰ� �ʹ�.
// 2-1. ���콺 ���� ��ư�� ������.
// 2-2. ���̸� �����ϰ� �߻� ��ġ�� �߻� ������ �����Ѵ�.
// 2-3. ���̰� �ε��� ����� ������ ������ �� �ִ� ������ �����.
// 2-4. ���̸� �߻��ϰ�, �ε��� ��ü�� ������ �� ��ġ�� �ǰ� ȿ���� �����.
// �ʿ�Ӽ�: �ǰ�ȿ�� ���ӿ�����Ʈ, ����Ʈ�� ��ƼŬ �ý���

// ����3 : ���̰� �΋H�� ����� Enemy��� Enemy ���� �������� �ְڴ�.
// ����4 : �̵� ���� Ʈ���� �Ķ���Ͱ��� 0�϶� Attack Trigger�� ���� �ϰڴ�.
// �ʿ�Ӽ�: �ڽ� ������Ʈ�� �ִϸ�����

// ����5 : Ű������ Ư�� Ű �Է����� �����带 ��ȯ�ϰ� �ʹ�.
// �ʿ�Ӽ� 5: ���� ��� ������ ���� , ��Ȯ�� ���� 
// 5-1 ��ָ�� : ���콺 ������ ��ư�� ������ ����ź�� ������ �ʹ�.
// 5-2 �������� ��� : ���콺 ������ ��ư�� ������ ȭ���� Ȯ���ϰ� �ʹ�.

// ���� 6 : ���� �߻��� �� ���� �ð� �Ŀ� ������� �ѱ� ����Ʈ�� Ȱ��ȭ �Ѵ�.
// �ʿ�Ӽ� : �ѱ� ����Ʈ �迭
public class PlayerFire : MonoBehaviour
{
    // �ʿ�Ӽ�: ��ź ���ӿ�����Ʈ, �߻� ��ġ, ����
    public GameObject bomb;
    public GameObject firePosition;
    public float power;
    private PlayerFire playerFire;
    private Transform myTransform;

    // �ʿ�Ӽ�: �ǰ�ȿ�� ���ӿ�����Ʈ, ����Ʈ�� ��ƼŬ �ý���
    public GameObject hitEffect;
    ParticleSystem particleSystem;

    // �ʿ�Ӽ�: �ڽ� ������Ʈ�� �ִϸ�����
    Animator animator;

    //�ʿ�Ӽ�5 : ������ ������ ����, ��Ȯ�� ����

    public enum WeaponMode
    {
        Nomal,
        Sniper
    }
    WeaponMode weaponMode = WeaponMode.Nomal;
    bool isZoomMode = false;

    // �ʿ�Ӽ� : �ѱ� ����Ʈ �迭
    public GameObject[] fireflashEffs;

    void Awake()
    {
        playerFire = GameObject.Find("Player").GetComponent<PlayerFire>();
    }

    private void Start()
    {
        particleSystem = hitEffect.GetComponent<ParticleSystem>();
        animator = GetComponentInChildren<Animator>();
        int x = 3;
        int y = 4;
        Swap(ref x, ref y);
        print(string.Format("x: {0}, y: {1}", x, y));

        int a = 7;
        int b = 3;
        int quotient;

        quotient = Divide(a, b, out remainder);
        print(string.Format("��: {0}, ������: {1}", quotient, remainder));
    }
    int remainder;

    public int weaponPower = 2;

    public object weaponModeTxt { get; internal set; }

    // Update is called once per frame
    void Update()
    {
        //����7 : GameManager�� Ready �����ϴ�� �÷��̾�, ���� ������ �� ������ �Ѵ�.
        if (GameManager.Instance.state != GameManager.Gamestate.Start)
            return;
        // ����5 : Ű������ Ư�� Ű �Է����� �����带 ��ȯ�ϰ� �ʹ�.
        // �ʿ�Ӽ� 5: ���� ��� ������ ���� , ��Ȯ�� ���� 
        // 5-1 ��ָ�� : ���콺 ������ ��ư�� ������ ����ź�� ������ �ʹ�.

        switch (weaponMode)
        {
            case WeaponMode Nomal:
                // ����2. ��ź ���ӿ�����Ʈ�� �����ϰ� firePosition�� ��ġ��Ų��.
                GameObject bombGO = Instantiate(bomb);
                bombGO.transform.position = firePosition.transform.position;
                // ����3. ��ź ������Ʈ�� rigidBody�� �����ͼ� ī�޶� ���� �������� ���� ���Ѵ�.
                Rigidbody rigidbody = bombGO.GetComponent<Rigidbody>();
                rigidbody.AddForce(Camera.main.transform.forward * power, ForceMode.Impulse);
                break;
            // 5-2 �������� ��� : ���콺 ������ ��ư�� ������ ȭ���� Ȯ���ϰ� �ʹ�.
            //case WeaponMode Sniper:
               // if (isZoomMode)
               // {
                //    Camera.main.fieldOfView = 15;
                //    isZoomMode = true;
               // }
               // break;


        }
        // ����1. ���콺 ������ ��ư�� ������.
        if (Input.GetMouseButtonDown(1)) // ����(0), ������(1), ��(2)
        {
        }

        // 2-1. ���콺 ���� ��ư�� ������.
        if (Input.GetMouseButtonDown(0))
        {

            //����4: �̵� ���� Ʈ���� �Ķ���� ���� 0�϶� Attack Trigger�� �����ϰڴ�.
            if (animator.GetFloat("MoveMotion") == 0)
            {
                animator.SetTrigger("Attack");

            }
            // 2-2. ���̸� �����ϰ� �߻� ��ġ�� �߻� ������ �����Ѵ�.
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

            // 2-3. ���̰� �ε��� ����� ������ ������ �� �ִ� ������ �����.
            RaycastHit hitInfo = new RaycastHit();

            // 2-4. ���̸� �߻��ϰ�, 
            if (Physics.Raycast(ray, out hitInfo)) // ref & out 
            {
                print("�浹ü���� �Ÿ�: " + hitInfo.distance);

                // �ε��� ��ü�� ������ �� ��ġ�� �ǰ� ȿ����(���� ���� ��������) �����.
                hitEffect.transform.position = hitInfo.point;
                hitEffect.transform.forward = hitInfo.normal;

                // �ǰ� ����Ʈ�� ����Ѵ�.
                particleSystem.Play();

                // ����3 : ���̰� �΋H�� ����� Enemy��� Enemy ���� �������� �ְڴ�.
                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    EnemyFsm enemyFsm = hitInfo.transform.gameObject.GetComponent<EnemyFsm>();
                    enemyFsm.DamageAction(1);
                }
            }

            //�ѱ� ����Ʈ ������ ���� �ڷ�ƾ ����
            StartCoroutine(ShootEffOn(0.05f));
        }

        //Ű���� ���� 1���� ������, �����带 ��ָ��� ��ȯ�Ѵ�.
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            weaponMode = WeaponMode.Nomal;
            //ī�޶� FoV�� ó�� ���·� �ٲ��ش�.
            Camera.main.fieldOfView = 60;
        }
        //Ű���� ���� 2���� ������, �����带 ���� ���� �����Ѵ�.
        else if (Input.GetKeyDown(KeyCode.Alpha2))

        {
            weaponMode = WeaponMode.Sniper;
        }

    }

    // ���� 6 : ���� �߻��� �� ���� �ð� �Ŀ� ������� �ѱ� ����Ʈ�� �������� Ȱ��ȭ �Ѵ�.
    IEnumerator ShootEffOn(float duration)
    {
        //�����ð� �Ŀ� ������� �ѱ� ����Ʈ�� Ȱ��ȭ �Ѵ�.
        int randNum =  Random.Range(0,fireflashEffs.Length-1);
        fireflashEffs[randNum].SetActive(true);

        //�����ð� ��ٸ���.
        yield return new WaitForSeconds(duration);

        //�����ð��� ������ ��Ȱ��ȭ
        fireflashEffs[randNum].SetActive(true);
    }

    private static int Divide(int a, int b, out int remainder)
    {
        remainder = a % b;

        return a / b;
    }

    private static void Swap(ref int a, ref int b)
    {
        int temp = a;
        a = b;
        b = temp;
    }
}