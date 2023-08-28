using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using JetBrains.Annotations;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

// ��ǥ: ���� FSM ���̾�׷��� ���� ���۽�Ű�� �ʹ�.
// �ʿ�Ӽ�1: �� ����

// ��ǥ2: �÷��̾���� �Ÿ��� �����ؼ� Ư�� ���·� ������ش�.
// �ʿ�Ӽ�2: �÷��̾���� �Ÿ�, �÷��̾� Ʈ������

// ��ǥ3 : ���� ���°� Move �϶� �÷��̾���� �Ÿ��� ���ݹ��� ���̸�,���� �÷��̾ ���󰣴�.
// �ʿ�Ӽ�3 : �̵��ӵ�, ���� ���� ĳ���� ��Ʈ�ѷ�,���ݹ���

// ��ǥ4 : ���ݹ������� ������ Ư���ð��� �ѹ��� �����Ѵ�.
// �ʿ�Ӽ�4 : ����ð�, Ư������ ������ 

// ��ǥ 5 : �÷��̾ ���󰡴ٰ� �ʱ���ġ���� �����Ÿ��� �����Return ���·� ��ȯ�Ѵ�.
//�ʿ�Ӽ� 5 : �ʱ���ġ, �����Ÿ� 

// ��ǥ6 : �ʱ� ��ġ�� ���ƿ´�. Ư���Ÿ� �̳���, Idle���·� ��ȯ�Ѵ�.
// �ʿ�Ӽ� : Ư���Ÿ�

// ��ǥ 7 : �÷��̾��� ������ ������ hitdamage ��ƴ ���ʹ��� hp�� ���ҽ�Ų��.
// �ʿ�Ӽ� : hp

// ��ǥ 8 : 2���Ŀ� ���ڽ��� �����ϰڴ�.

// ��ǥ 9 : ���� ���׹��� hp(%) �� �����̴��� �����Ѵ�.

//<Alpha upgrade>
// ��ǥ 10 : Idle ���¿��� Move���·� Animation ���·� ��ȯ�Ѵ�.

// ��ǥ 11 : �׺���̼� ������Ʈ�� �ּҰŸ��� �Է����ְ� �÷��̾ ���� �� �ֵ��� �Ѵ�. 
//�ʿ�Ӽ� : �׺���̼� ������Ʈ 

//��ǥ12 :�׺���̼� ������Ʈ�� �̵��� ���߰� �׺���̼� ��θ� �ʱ�ȭ ���ش�.

//��ǥ 13 :Enemy�� �ʱ�ӵ��� Agent�� �ӵ��� �����ϰ�ʹ�.

//��ǥ 14 : ������Ʈ�� NavMeshLink�� �ö󰡰� ������������ Ȯ�̳��� Ư�� �ִϸ��̼��� �ְ� �ʹ�.


public class EnemyFsm : MonoBehaviour
{
    // �ʿ�Ӽ�: �� ����
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

    // �ʿ�Ӽ�: �÷��̾���� �Ÿ�, �÷��̾� Ʈ������
    public float findDistance = 5f;
    Transform player;

    // �ʿ�Ӽ� : �̵��ӵ�, ���� ���� ĳ���� ��Ʈ�ѷ�,���ݹ���
    public float moveSpeed;
    CharacterController characterController;
    public float attackDistance = 2f;

    // �ʿ�Ӽ�4 : ����ð�, Ư������ ������ 
    float currentTime = 0;
    public float lastAttackTime = 0;
    private float attackDelay = 2f;
    public int attackPower = 2;

    //�ʿ�Ӽ� 5 : �ʱ���ġ, �̵����� ����
    Vector3 originPos;
    public float moveDistance = 20f;

    // �ʿ�Ӽ� 6 : Ư���Ÿ�
    float returnDistance = 0.3f;

    // �ʿ�Ӽ� 7 : hp
    public int hp = 10; 

    //�ʿ�Ӽ� 4 : hp ,max hp, slider
    int maxHP = 10;
    public Slider hpSlider;

    //�ʿ�Ӽ� : Animator
    Animator animator;

    //�ʿ�Ӽ� : �׺���̼� ������Ʈ 
    NavMeshAgent navMeshAgent;

    // Start is called before the first frame update
    void Start()
    {
        enemyState = EnemyState.Idle;

        player = GameObject.Find("Player").transform;

        characterController = GetComponent<CharacterController>();

        originPos = transform.position;

        animator = GetComponentInChildren<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        //��ǥ 13 :Enemy�� �ʱ�ӵ��� Agent�� �ӵ��� �����ϰ�ʹ�.
        navMeshAgent.speed = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {

        // ���� 7 :GameManager�� Ready �����϶��� �÷��̾�� ���� ������ �� ������ �Ѵ�. 
        if (GameManager.Instance.state != GameManager.Gamestate.start)
            return;

        // ��ǥ: ���� FSM ���̾�׷��� ���� ���۽�Ű�� �ʹ�.
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

        // ���� 9 : ���� ���׹��� hp(%) �� �����̴��� �����Ѵ�.
        hpSlider.value = (float)hp / maxHP;
    }



    private void Die()
    {
        StopAllCoroutines();
        StartCoroutine(DieProcess());
    }

    //���� : 2���Ŀ� ���ڽ��� �����ϰڴ�.
    IEnumerator DieProcess()
    {
        animator.SetTrigger("Die");
        yield return new WaitForSeconds(2);
        print("���");
        Destroy(gameObject);
    }

    // ��ǥ 7 : �÷��̾��� ������ ������ hitdamage ��ƴ ���ʹ��� hp�� ���ҽ�Ų��.
    // ��ǥ 8 : ���׹��� ü���� 0��Ÿ ũ�� �ǰ� ���·� ��ȯ
    // ��ǥ 9 : �׷��� ������ ���� ���·� ��ȯ
    public void DamageAction(int damage)
    {
        //���� , �̹� ���׹̰� �ǰ� �ưų�, ��� ���¶�� �������� �����ʴ´�.
        if (enemyState == EnemyState.Damaged || enemyState == EnemyState.Die)
            return;
        
        //�÷��̾��� ���ݷ� ��ŭ hp ������.
        hp -= damage;

        // ��ǥ 8 : ���׹��� ü���� 0��Ÿ ũ�� �ǰ� ���·� ��ȯ
        if(hp > 0)
        {
            enemyState = EnemyState.Damaged;
            print("������ȯ : any state -> Damaged ");
            Damaged();
        }

        // ��ǥ 9 : �׷��� ������ ���� ���·� ��ȯ
        else
        {
            enemyState = EnemyState.Die;
            print("������ȯ : any state -> Die ");
            Die();
        }


    }

    private void Damaged()
    {
        //�ǰݸ�� 5.0

        //�ǰ� ���� ó���� ���� �ڷ�ƾ ����
        StartCoroutine(DamageProcess());
        animator.SetTrigger("Damaged");
    }

    //������ ó����
    IEnumerator DamageProcess()
    {
        // �ǰ� ��� �ð���ŭ ��ٸ���.
        yield return new WaitForSeconds(0.5f);

        //���� ���¸� �̵� ���·� ��ȯ�Ѵ�.
        enemyState = EnemyState.Move;
        print("������ȯ : Dmage -> Move");
        animator.SetTrigger("Damaged2Move");
    }

    private void Return()
    {
        //�ʱ���ġ�� ���ƿ´�.
        float distanceToOriginPos = (originPos - transform.position).magnitude;

       
        if ( distanceToOriginPos > returnDistance)
        {
            Vector3 dir = (originPos - transform.position).normalized;
            characterController.Move(dir *moveSpeed * Time.deltaTime);
            transform.forward = dir;
        }

        else  //Ư���Ÿ� �̳���(0.1), Idle���·� ��ȯ�Ѵ�.
        {
            enemyState = EnemyState.Idle;
            print("������ȯ Return -> Idle");
            animator.SetTrigger("Return2Idle");


        }
    }

    private void Attack()
    {
        // ��ǥ4 : ���ݹ������� ������ Ư���ð��� �ѹ��� �����Ѵ�.
        float distanceToPlayer = (player.position - transform.position).magnitude;
        if (distanceToPlayer < attackDistance)
        {
            currentTime += Time.deltaTime;
            //Ư���ð��� �ѹ��� �����Ѵ�.
            if(currentTime > attackDelay)
            {
                if (player.GetComponent<PlayerMove>() != null)
                    return;
                //player.GetComponent<PlayerMove>().DamageAction(attackPower);
                print("����!");
                currentTime = 0;

                animator.SetTrigger("AttackDelay2Attack");
            }
        }
        else
        {
            //�׷��� ������ move �� ���¸� ��ȯ�Ѵ�.
            enemyState = EnemyState.Move;
            print("������ȯ : Attack -> Move ");
            currentTime = attackDelay;
            animator.SetTrigger("Attack2Move");
        }
    }

    public void AttackAction()
    {
        player.GetComponent<PlayerMove>().DamageAction(attackPower);
    }

    private void Move()
    {
        // ��ǥ3 : ���� ���°� Move �϶� �÷��̾���� �Ÿ��� ���ݹ��� ���̸�,���� �÷��̾ ���󰣴�.
        float distanceToPlayer = (player.position - transform.position).magnitude;

        //��ǥ 5 : �÷��̾ ���󰡴ٰ� �ʱ���ġ���� �����Ÿ��� ����� �ʱ���ġ�� ���ƿ´�.

        float distanceToOriginPos = (originPos - transform.position).magnitude;
        if (distanceToOriginPos > moveDistance)
        {
            enemyState = EnemyState.Return;
            print("������ȯ : Move -> Return");
            transform.forward = (originPos - transform.position).normalized;
            animator.SetTrigger("Move2Return");
        }
        else
        {
            if (distanceToPlayer > attackDistance)
            {
                //Vector3 dir = (player.position - transform.position).normalized;

                //�÷��̾ ���󰣴�.
                //characterController.Move(dir * moveSpeed * Time.deltaTime);

                //transform.forward = dir;
                navMeshAgent.stoppingDistance = attackDelay;

                navMeshAgent.SetDestination(player.position);

                //�̵��� ���߰� ��θ� �ʱ�ȭ�Ѵ�.
                navMeshAgent.isStopped = true;
                navMeshAgent.ResetPath();

                // ��ǥ 11 : �׺���̼� ������Ʈ�� �ּҰŸ��� �Է����ְ� �÷��̾ ���� �� �ֵ��� �Ѵ�. 
                navMeshAgent.stoppingDistance = attackDistance;
                navMeshAgent.SetDestination(player.position);
            }
            //��ǥ 14 : ������Ʈ�� NavMeshLink�� �ö󰡰� ������������ Ȯ�̳��� Ư�� �ִϸ��̼��� �ְ� 
            if (navMeshAgent.isOnOffMeshLink)
            {
                object navMeshOwner = navMeshAgent.newMeshOwner;
                GameObject navMeshGo = (navMeshAgent as Component).gameobject;
                print(navMeshGo.name + "navMeshGo.name�� �̵��ϰ� �ֽ��ϴ�.");
                //OffMeshLinkData offdata = navMeshAgent.currentOffMeshLinkData;
                //print(offdata.offMeshLink.name);
                
            }
            else
            {
                //���� �������� ������ �������� ���¸� ��ȯ�Ѵ�.
                enemyState = EnemyState.Attack;
                print("������ȯ -> Attack");

                animator.SetTrigger("Move2AttackDelay");
            }
        }
    }

    private void Idle()
    {
        float distanceToPlayer = (player.position - transform.position).magnitude;
        // float tempDist = Vector3.Distance(transform.position, player.position);

        // ���� �÷��̾���� �Ÿ��� Ư�� ���� ���� ���¸� Move�� �ٲ��ش�.
        if (distanceToPlayer < findDistance)
        {
            enemyState = EnemyState.Move;
            print("������ȯ: Idle -> Move");

            // ��ǥ 10 : Idle ���¿��� Move���·� Animation ���·� ��ȯ�Ѵ�.
            animator.SetTrigger("Idle2Move");
        }
    }
}