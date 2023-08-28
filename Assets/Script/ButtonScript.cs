using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

//��ǥ: �÷��̾ ��ư�� ������ �ٸ��� ������ �׺���̼� �Žø� �ٽ� �����.
//�ʿ�Ӽ� : �ٸ� ���ӿ�����Ʈ, �׺���̼�Surface

public class ButtonScript : MonoBehaviour
{
    public GameObject bridge;
    public NavMeshSurface navMeshSurface;

    void Start()
    {
        bridge.SetActive(false);
    }

    
    private void OnTriggerEnter(Collider other)
    {   //��ǥ: �÷��̾ ��ư�� ������ 
        if (other.CompareTag("Player"))
        {
           //�ٸ��� ������ 
            bridge.SetActive(true);
            //�׺���̼� �Žø� �ٽ� �����.
            navMeshSurface.BuildNavMesh();
        }
    }
}