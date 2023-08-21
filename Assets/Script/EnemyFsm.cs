using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

// ��ǥ: ���� FSM ���̾�׷��� ���� ���۽�Ű�� �ʹ�.
// �ʿ�Ӽ�1: �� ����

// ��ǥ2: �÷��̾���� �Ÿ��� �����ؼ� Ư�� ���·� ������ش�.
// �ʿ�Ӽ�2: �÷��̾���� �Ÿ�, �÷��̾� Ʈ������

// ��ǥ3 : ���� ���°� Move �϶� �÷��̾���� �Ÿ��� ���ݹ��� ���̸�,���� �÷��̾ ���󰣴�.
// �ʿ�Ӽ� : �̵��ӵ�, ���� ���� ĳ���� ��Ʈ�ѷ�,���ݹ���
public class EnemyFSM : MonoBehaviour
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



    // Start is called before the first frame update
    void Start()
    {
        enemyState = EnemyState.Idle;

        player = GameObject.Find("Player").transform;

        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
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
                Damaged();
                break;
            case EnemyState.Die:
                Die();
                break;
        }
    }

    private void Die()
    {
        throw new NotImplementedException();
    }

    private void Damaged()
    {
        throw new NotImplementedException();
    }

    private void Return()
    {
        throw new NotImplementedException();
    }

    private void Attack()
    {
        throw new NotImplementedException();
    }

    private void Move()
    {
        // ��ǥ3 : ���� ���°� Move �϶� �÷��̾���� �Ÿ��� ���ݹ��� ���̸�,���� �÷��̾ ���󰣴�.
        float distanceToPlayer = (player.position - transform.position).magnitude;
        if (distanceToPlayer > attackDistance)
        {
            Vector3 dir = (player.position - transform.position).normalized;

            //�÷��̾ ���󰣴�.
            characterController.Move(dir * moveSpeed * Time.deltaTime);
        }
        else
        {
            //���� �������� ������ �������� ���¸� ��ȯ�Ѵ�.
            enemyState = EnemyState.Idle;
            print("������ȯ -> Attack");
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
        }
    }
}