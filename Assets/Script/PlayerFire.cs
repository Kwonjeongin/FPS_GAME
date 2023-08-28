using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 목적: 마우스 오른쪽 버튼을 눌러 폭탄을 특정 방향으로 발사하고 싶다.
// 필요속성: 폭탄 게임오브젝트, 발사 위치, 방향
// 1-1. 마우스 오른쪽 버튼을 누른다.
// 1-2. 폭탄 게임오브젝트를 생성하고 firePosition에 위치시킨다.
// 1-3. 폭탄 오브젝트의 rigidBody를 가져와서 카메라 정면 방향으로 힘을 가한다.

// 목적2: 마우스 왼쪽 버튼을 누르면 시선 방향으로 총을 발사하고 싶다.
// 2-1. 마우스 왼쪽 버튼을 누른다.
// 2-2. 레이를 생성하고 발사 위치와 발사 방향을 설정한다.
// 2-3. 레이가 부딪힌 대상의 정보를 저장할 수 있는 변수를 만든다.
// 2-4. 레이를 발사하고, 부딪힌 물체가 있으면 그 위치에 피격 효과를 만든다.
// 필요속성: 피격효과 게임오브젝트, 이펙트의 파티클 시스템

// 목적3 : 레이가 부딫힌 대상이 Enemy라면 Enemy 에게 데미지를 주겠다.
// 목적4 : 이동 블렌드 트리의 파라메터값이 0일때 Attack Trigger를 시전 하겠다.
// 필요속성: 자식 오브젝트의 애니메이터

// 목적5 : 키보드의 특정 키 입력으로 무기모드를 전환하고 싶다.
// 필요속성 5: 무기 모드 열거형 변수 , 줌확인 변수 
// 5-1 노멀모드 : 마우스 오른쪽 버튼을 누르면 수류탄을 던지고 싶다.
// 5-2 스나이퍼 모드 : 마우스 오른족 버튼을 누르면 화면을 확대하고 싶다.

// 목적 6 : 총을 발사할 때 일정 시간 후에 사라지는 총구 이펙트를 활성화 한다.
// 필요속성 : 총구 이펙트 배열
public class PlayerFire : MonoBehaviour
{
    // 필요속성: 폭탄 게임오브젝트, 발사 위치, 방향
    public GameObject bomb;
    public GameObject firePosition;
    public float power;
    private PlayerFire playerFire;
    private Transform myTransform;

    // 필요속성: 피격효과 게임오브젝트, 이펙트의 파티클 시스템
    public GameObject hitEffect;
    ParticleSystem particleSystem;

    // 필요속성: 자식 오브젝트의 애니메이터
    Animator animator;

    //필요속성5 : 무기모드 열거형 변수, 줌확인 변수

    public enum WeaponMode
    {
        Nomal,
        Sniper
    }
    WeaponMode weaponMode = WeaponMode.Nomal;
    bool isZoomMode = false;

    // 필요속성 : 총구 이펙트 배열
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
        print(string.Format("몫: {0}, 나머지: {1}", quotient, remainder));
    }
    int remainder;

    public int weaponPower = 2;

    // Update is called once per frame
    void Update()
    {
        //목적7 : GameManager가 Ready 상태일대는 플레이어, 적이 움직일 수 없도록 한다.
        if (GameManager.Instance.state != GameManager.Gamestate.Start)
            return;
        // 목적5 : 키보드의 특정 키 입력으로 무기모드를 전환하고 싶다.
        // 필요속성 5: 무기 모드 열거형 변수 , 줌확인 변수 
        // 5-1 노멀모드 : 마우스 오른쪽 버튼을 누르면 수류탄을 던지고 싶다.

        switch (weaponMode)
        {
            case WeaponMode Nomal:
                // 순서2. 폭탄 게임오브젝트를 생성하고 firePosition에 위치시킨다.
                GameObject bombGO = Instantiate(bomb);
                bombGO.transform.position = firePosition.transform.position;
                // 순서3. 폭탄 오브젝트의 rigidBody를 가져와서 카메라 정면 방향으로 힘을 가한다.
                Rigidbody rigidbody = bombGO.GetComponent<Rigidbody>();
                rigidbody.AddForce(Camera.main.transform.forward * power, ForceMode.Impulse);
                break;
            // 5-2 스나이퍼 모드 : 마우스 오른족 버튼을 누르면 화면을 확대하고 싶다.
            //case WeaponMode Sniper:
               // if (isZoomMode)
               // {
                //    Camera.main.fieldOfView = 15;
                //    isZoomMode = true;
               // }
               // break;


        }
        // 순서1. 마우스 오른쪽 버튼을 누른다.
        if (Input.GetMouseButtonDown(1)) // 왼쪽(0), 오른쪽(1), 휠(2)
        {
        }

        // 2-1. 마우스 왼쪽 버튼을 누른다.
        if (Input.GetMouseButtonDown(0))
        {

            //목적4: 이동 블렌드 트리의 파라메터 값이 0일때 Attack Trigger를 시전하겠다.
            if (animator.GetFloat("MoveMotion") == 0)
            {
                animator.SetTrigger("Attack");

            }
            // 2-2. 레이를 생성하고 발사 위치와 발사 방향을 설정한다.
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

            // 2-3. 레이가 부딪힌 대상의 정보를 저장할 수 있는 변수를 만든다.
            RaycastHit hitInfo = new RaycastHit();

            // 2-4. 레이를 발사하고, 
            if (Physics.Raycast(ray, out hitInfo)) // ref & out 
            {
                print("충돌체와의 거리: " + hitInfo.distance);

                // 부딪힌 물체가 있으면 그 위치에 피격 효과를(법선 벡터 방향으로) 만든다.
                hitEffect.transform.position = hitInfo.point;
                hitEffect.transform.forward = hitInfo.normal;

                // 피격 이펙트를 재생한다.
                particleSystem.Play();

                // 목적3 : 레이가 부딫힌 대상이 Enemy라면 Enemy 에게 데미지를 주겠다.
                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    EnemyFsm enemyFsm = hitInfo.transform.gameObject.GetComponent<EnemyFsm>();
                    enemyFsm.DamageAction(1);
                }
            }

            //총구 이펙트 실행을 위한 코루틴 시작
            StartCoroutine(ShootEffOn(0.05f));
        }

        //키보드 숫자 1번을 누르면, 무기모드를 노멀모드로 전환한다.
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            weaponMode = WeaponMode.Nomal;
            //카메라 FoV를 처음 상태로 바꿔준다.
            Camera.main.fieldOfView = 60;
        }
        //키보드 숫자 2번을 누르면, 무기모드를 저격 모드로 설정한다.
        else if (Input.GetKeyDown(KeyCode.Alpha2))

        {
            weaponMode = WeaponMode.Sniper;
        }

    }

    // 목적 6 : 총을 발사할 때 일정 시간 후에 사라지는 총구 이펙트를 랜덤으로 활성화 한다.
    IEnumerator ShootEffOn(float duration)
    {
        //일정시간 후에 사라지는 총구 이펙트를 활성화 한다.
        int randNum =  Random.Range(0,fireflashEffs.Length-1);
        fireflashEffs[randNum].SetActive(true);

        //일정시간 기다린다.
        yield return new WaitForSeconds(duration);

        //일정시간이 지나면 비활성화
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