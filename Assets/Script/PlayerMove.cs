using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

// 목적 : W,A,S,D 키를 누르면 캐릭터를 그 방향으로 이동 시키고 싶다.
// 필요속성 : 이동속도
// 순서1 사용자의 입력을 받는다.
// 순서2 이동방향을 설정한다.
// 순서3 이동속도에 따라 나를 이동시킨다. 

// 목적2 : 스페이스를 누르면 수직으로 점프하고 싶다.
// 필요속성 : 캐릭터 컨트롤러, 중력변수, 수직 속력 변수
// 목적2 순서-1. 캐릭터 수직 속도에 중력을 적용하고 싶다.
// 목적2 순서-2 캐릭터 컨트롤러로 나를 이동시키고 싶다.
// 2-3 스페이스 키를 누르면 수직속도에 점프파워를 적용 하고싶다.

// 목적 3 점프중인지 확인하고, 점프중이면 점프전 상태로 초기화 하고싶다. 
// 순서 3-1

// 목적3 : 플레이어가 피격을 당하면 hp를 damage 만큼 깎는다.
// 필요속성 : hp

// 목적 4 : 플레이어 hp(%) 슬라이더에 적용한다.
// 필요속성 4 : hp, max hp,Slider

// 목적 5 : 적의공격을 받았을때 hitimage를 켰다가 꺼준다.
// 필요속성 5 : hitimage 게임오브젝트

//필요속성 6 : 현재시간, hitImage 종료시간

// 목적 7 :GameManager가 Ready 상태일때는 플레이어와 적이 움직일 수 없도록 한다. 
public class PlayerMove : MonoBehaviour
{
    // 필요속성 : 이동속도
    public float speed = 10;

    // 필요속성 : 캐릭터 컨트롤러, 중력변수, 수직 속력 변수, 점프파워, 점프상태 변수
    CharacterController characterController;

    float gravity = -20f;
    float yVelocity = 0;
    public float jumpPower = 10; 
    public bool isJumping = false;

    // 필요속성3 : hp
    public int hp = 10;
    public Slider hpslider;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // 필요속성 4 : hp, max hp ,Slider
    int maxHP = 10;

    // 필요속성 5 : hitimage 게임오브젝트
    public GameObject HitImage;

    //필요속성 6 : 현재시간, hitImage 종료시간
    float currentTime;
    public float HitImageEndTime;


    void Update()
    {
        // 목적 7 :GameManager가 Ready 상태일때는 플레이어와 적이 움직일 수 없도록 한다. 
        if (GameManager.Instance.state != GameManager.Gamestate.start)
            return;

        // 순서1 사용자의 입력을 받는다.
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        //만약 점프중이였다면 점프전 상태로 초기화 하고싶다.

        if( isJumping && characterController.collisionFlags == CollisionFlags.Below)
        {
        
            isJumping = false;

            yVelocity = 0;
        }

        //바닥에 닿아 있을때, 수직 속도를 초기화 하고싶다.

        else if (characterController.collisionFlags == CollisionFlags.Below)
        {
            yVelocity = 0;
        }

        if (Input.GetButtonDown("Jump") && !isJumping )
        {
            yVelocity = jumpPower;
            isJumping = true;
        }

        // 순서2 이동방향을 설정한다.
        Vector3 dir = new Vector3(h, 0 ,v);
        dir = Camera.main.transform.TransformDirection(dir);

        //2-1 캐릭터 수직속도에 중력을 적용하고 싶다.
        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;


        // 순서3 이동속도에 따라 나를 이동시킨다. 
        //transform.position += dir * speed * Time.deltaTime;

        // 2-2 캐릭터 컨트롤러로 나를 이동시키고 싶다.
        characterController.Move(dir * speed * Time.deltaTime);

        // 목적 4 : 플레이어 hp(%) 슬라이더에 적용한다.
        hpslider.value = (float)hp / maxHP;

    }
    // 목적3 : 플레이어가 피격을 당하면 hp를 damage 만큼 깎는다.

    public void DamageAction(int damage)
    {
        hp -= damage;

        // 목적 5 : 적의공격을 받았을때 hitimage를 켰다가 꺼준다.
        if (hp > 0)
        {
            StartCoroutine(PlayHitEffect());
        }
        else
        {
            StartCoroutine(DamageEffect());
        }

        // 목적 6 : 플레이어가 죽으면 hitImage 의 알파값을 현재 값에서 255로 만들어준다.


        
    }

    IEnumerator DamageEffect()
    {
        //hitimage 활성화
        HitImage.gameObject.SetActive(true);
        Color HitImageColor = HitImage.GetComponent<Image>().color;

        while (true)
        {
            currentTime += Time.deltaTime;

            yield return null;

            HitImageColor.a = Mathf.Lerp(0, 1, currentTime / HitImageEndTime);

            HitImage.GetComponent<Image>().color= HitImageColor;

            if (currentTime > HitImageEndTime)
            {
                currentTime = 0;
                break;
            }
                    
        }

        yield return null;
    }
    IEnumerator PlayHitEffect()
    {
        //hitimage 활성화
        HitImage.gameObject.SetActive(true);

        //0.5 초간 기다린다.
        yield return new WaitForSeconds(0.2f);

        //hitImage 비활성화
        HitImage.gameObject.SetActive(false);
    }

    
}
