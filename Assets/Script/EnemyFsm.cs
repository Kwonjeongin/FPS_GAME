using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

// 목표: 적을 FSM 다이어그램에 따라 동작시키고 싶다.
// 필요속성1: 적 상태

// 목표2: 플레이어와의 거리를 측정해서 특정 상태로 만들어준다.
// 필요속성2: 플레이어와의 거리, 플레이어 트랜스폼

// 목표3 : 적의 상태가 Move 일때 플레이어와의 거리가 공격범위 밖이면,적이 플레이어를 따라간다.
// 필요속성3 : 이동속도, 적을 위한 캐릭터 컨트롤러,공격범위

// 목표4 : 공격범위내에 들어오면 특정시간에 한번씩 공격한다.
// 필요속성4 : 현재시간, 특정공격 딜레이 

// 목표 5 : 플레이어를 따라가다가 초기위치에서 일정거리를 벗어나면Return 상태로 전환한다.
//필요속성 5 : 초기위치, 일정거리 

// 목표6 : 초기 위치로 돌아온다. 특정거리 이내면, Idle상태로 전환한다.
// 필요속성 : 특정거리

// 목표 7 : 플레이어의 공격을 받으면 hitdamage 만틈 에너미의 hp를 감소시킨다.
// 필요속성 : hp

// 목표 8 : 2초후에 나자신을 제거하겠다.

// 목표 9 : 현재 에네미의 hp(%) 를 슬라이더에 적용한다.

//<Alpha upgrade>
// 목표 10 : Idle 상태에서 Move상태로 Animation 상태로 전환한다.


public class EnemyFsm : MonoBehaviour
{
    // 필요속성: 적 상태
    enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Return,
        Damaged,
        Die
    }
    EnemyState enemyState;

    // 필요속성: 플레이어와의 거리, 플레이어 트랜스폼
    public float findDistance = 5f;
    Transform player;

    // 필요속성 : 이동속도, 적을 위한 캐릭터 컨트롤러,공격범위
    public float moveSpeed;
    CharacterController characterController;
    public float attackDistance = 2f;

    // 필요속성4 : 현재시간, 특정공격 딜레이 
    float currentTime = 0;
    public float lastAttackTime = 0;
    private float attackDelay = 2f;
    public int attackPower = 2;

    //필요속성 5 : 초기위치, 이동가능 범위
    Vector3 originPos;
    public float moveDistance = 20f;

    // 필요속성 6 : 특정거리
    float returnDistance = 0.3f;

    // 필요속성 7 : hp
    public int hp = 10; 

    //필요속성 4 : hp ,max hp, slider
    int maxHP = 10;
    public Slider hpSlider;

    //필요속성 : Animator
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        enemyState = EnemyState.Idle;

        player = GameObject.Find("Player").transform;

        characterController = GetComponent<CharacterController>();

        originPos = transform.position;

        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        // 목적 7 :GameManager가 Ready 상태일때는 플레이어와 적이 움직일 수 없도록 한다. 
        if (GameManager.Instance.state != GameManager.Gamestate.start)
            return;

        // 목표: 적을 FSM 다이어그램에 따라 동작시키고 싶다.
        switch (enemyState)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Move:
                Move();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Return:
                Return();
                break;
            case EnemyState.Damaged:
               // Damaged();
                break;
            case EnemyState.Die:
               // Die();
                break;
        }

        // 목적 9 : 현재 에네미의 hp(%) 를 슬라이더에 적용한다.
        hpSlider.value = (float)hp / maxHP;
    }



    private void Die()
    {
        StopAllCoroutines();
        StartCoroutine(DieProcess());
    }

    //목적 : 2초후에 나자신을 제거하겠다.
    IEnumerator DieProcess()
    {
        animator.SetTrigger("Die");
        yield return new WaitForSeconds(2);
        print("사망");
        Destroy(gameObject);
    }

    // 목표 7 : 플레이어의 공격을 받으면 hitdamage 만틈 에너미의 hp를 감소시킨다.
    // 목표 8 : 에네미의 체력이 0보타 크면 피격 상태로 전환
    // 목표 9 : 그렇지 않으면 죽음 상태로 전환
    public void DamageAction(int damage)
    {
        //만약 , 이미 에네미가 피격 됐거나, 사망 상태라면 데미지를 주지않는다.
        if (enemyState == EnemyState.Damaged || enemyState == EnemyState.Die)
            return;
        
        //플레이어의 공격력 만큼 hp 를감소.
        hp -= damage;

        // 목표 8 : 에네미의 체력이 0보타 크면 피격 상태로 전환
        if(hp > 0)
        {
            enemyState = EnemyState.Damaged;
            print("상태전환 : any state -> Damaged ");
            Damaged();
        }

        // 목표 9 : 그렇지 않으면 죽음 상태로 전환
        else
        {
            enemyState = EnemyState.Die;
            print("상태전환 : any state -> Die ");
            Die();
        }


    }

    private void Damaged()
    {
        //피격모션 5.0

        //피격 상태 처리를 위한 코루틴 실행
        StartCoroutine(DamageProcess());
        animator.SetTrigger("Damaged");
    }

    //데미지 처리용
    IEnumerator DamageProcess()
    {
        // 피격 모션 시간만큼 기다린다.
        yield return new WaitForSeconds(0.5f);

        //현재 상태를 이동 상태로 전환한다.
        enemyState = EnemyState.Move;
        print("상태전환 : Dmage -> Move");
        animator.SetTrigger("Damaged2Move");
    }

    private void Return()
    {
        float distanceToOriginPos = (originPos - transform.position).magnitude;
        //특정거리 이내면(0.1), Idle상태로 전환한다.
        if ( distanceToOriginPos > returnDistance)
        {
            Vector3 dir = (originPos - transform.position).normalized;
            characterController.Move(dir *moveSpeed * Time.deltaTime);
        }

        else
        {
            enemyState = EnemyState.Idle;
            print("상태전환 Return -> Idle");
            animator.SetTrigger("Return2Idle");


        }
    }

    private void Attack()
    {
        // 목표4 : 공격범위내에 들어오면 특정시간에 한번씩 공격한다.
        float distanceToPlayer = (player.position - transform.position).magnitude;
        if (distanceToPlayer < attackDistance)
        {
            currentTime += Time.deltaTime;
            //특정시간에 한번씩 공격한다.
            if(currentTime > attackDelay)
            {
                if(player.GetComponent<PlayerMove>() != null) { }
                player.GetComponent<PlayerMove>().DamageAction(attackPower);
                print("공격!");
                currentTime = 0;

                animator.SetTrigger("Attack2attackDelay");
            }
        }
        else
        {
            //그렇지 않으면 move 로 상태를 전환한다.
            enemyState = EnemyState.Move;
            print("상태전환 : Attack -> Move ");
            currentTime = attackDelay;
            animator.SetTrigger("Attack2Move");
        }
    }

    private void Move()
    {
        // 목표3 : 적의 상태가 Move 일때 플레이어와의 거리가 공격범위 밖이면,적이 플레이어를 따라간다.
        float distanceToPlayer = (player.position - transform.position).magnitude;

        //목표 5 : 플레이어를 따라가다가 초기위치에서 일정거리를 벗어나면 초기위치로 돌아온다.

        float distanceToOriginPos = (originPos - transform.position).magnitude;
        if (distanceToOriginPos > moveDistance)
        {
            enemyState = EnemyState.Return;
            print("상태전환 : move -> Return");
            transform.forward = (originPos - transform.position).normalized;
            animator.SetTrigger("Attack2Move");
        }
        else
        {
            if (distanceToPlayer > attackDistance)
            {
                Vector3 dir = (player.position - transform.position).normalized;

                //플레이어를 따라간다.
                characterController.Move(dir * moveSpeed * Time.deltaTime);

               
            }
            else
            {
                //공격 범위내로 들어오면 공격으로 상태를 전환한다.
                enemyState = EnemyState.Attack;
                print("상태전환 -> Attack");

                animator.SetTrigger("Move2Attack");
            }
        }
    }

    private void Idle()
    {
        float distanceToPlayer = (player.position - transform.position).magnitude;
        // float tempDist = Vector3.Distance(transform.position, player.position);

        // 현재 플레이어와의 거리가 특정 범위 내면 상태를 Move로 바꿔준다.
        if (distanceToPlayer < findDistance)
        {
            enemyState = EnemyState.Move;
            print("상태전환: Idle -> Move");

            // 목표 10 : Idle 상태에서 Move상태로 Animation 상태로 전환한다.
            animator.SetTrigger("Idle2Move");
        }
    }
}