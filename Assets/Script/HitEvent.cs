using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ����: �÷��̾�� �������� ������.
public class HitEvent : MonoBehaviour
{
    public EnemyFsm eFsm;

    public void HitPlayer()
    {
        eFsm.AttackAction();
    }

}