using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

// 목적 : 폭탄이 물체에 부딫히면 이펙트를 만들고 파괴된다.
//필요속성 : 폭발 이펙트

// 목적2 : 폭발효과 반경내에서 레이어가 'Enemy '인 모든 게임 오브젝트의 Collider를 저장하여,해당 적 게임 오브젝트에게 수류탄 데미지를 준다.
//필요속성 : 폭발 효과 반경 

public class Bomb : MonoBehaviour
{
    //필요속성 : 폭발 이펙트

    public GameObject bombeffect;

    //필요속성 : 폭발 효과 반경 
    public float explosionRadius = 5f;
    //public damage = 5;

    // 목적 : 폭탄이 물체에 부딫히면 파괴된다.

    private void OnCollisionEnter(Collision collision)
    {
        // 목적2 : 폭발효과 반경내에서 레이어가 'Enemy '인 모든 게임 오브젝트의 Collider를 저장하여,해당 적 게임 오브젝트에게 수류탄 데미지를 준다.
        Collider[] cols= Physics.OverlapSphere(transform.position, explosionRadius, 1 << 8 | 6);
        //Collider[] cols = Physics.OverlapSphere(transform.position, explosionRadius, LayerMask.GetMask("Enemy")| LayerMask.GetMask("Player"));

        //해당 적 게임 오브젝트에게 수류탄 데미지를 준다.
        for(int i=0; i<cols.Length; i++)
        {
            //cols[i].GetComponent<EnemyFsm>().DamageAction(damage);
        }
        
        //이펙트를 만든다.
        GameObject bombGo = Instantiate(bombeffect);
        bombGo.transform.position = transform.position;
        
        //이펙트 위치를 나(폭탄)의 위치시켜준다.
        Destroy(gameObject);
    }
}
