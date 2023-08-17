using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 목적 : 폭탄이 물체에 부딫히면 이펙트를 만들고 파괴된다.
//필요속성 : 폭발 이펙트

public class Bomb : MonoBehaviour
{
    //필요속성 : 폭발 이펙트

    public GameObject bombeffect;

    // 목적 : 폭탄이 물체에 부딫히면 파괴된다.

    private void OnCollisionEnter(Collision collision)
    {
        //이펙트를 만든다.
        GameObject bombGo = Instantiate(bombeffect);
        bombGo.transform.position = transform.position;
        
        //이펙트 위치를 나(폭탄)의 위치시켜준다.
        Destroy(gameObject);
    }
}
