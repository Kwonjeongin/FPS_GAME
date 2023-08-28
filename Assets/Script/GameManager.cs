using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// 목적 : 게임의 상태(Ready, start, Game Over)를 구별하고, 게임의 시작과 끝을 TEXTUI로 표현하고 싶다.
// 필요속성 : 게임상태, 열거형 변수,TextUI

// 목적2 : 2초후 게임이 시작 된다 .Ready 상태에서 Start상태로 변경되며 게임이 시작된다.

// 목적 3 : 플레이어의 Hp가 0보다 작으면 상태 텍스트를 GAmeOver로 바꿔주고 상태또한 GameOver로바꿔준다.
// 필요속성3 : hp가 들어가 있는 playerMove

//목적 4 :  플레이어의 hp가 0이하라면, 플레이어의 애니메이션을 멈춘다.
//필요속성 : 플레이어의 애니메이터 컴포넌트
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    // 필요속성 :게임상태, 열거형 변수,TextUI

    public enum Gamestate
    {
        Ready, 
        start, 
        GameOver,
        Start
    }

    public Gamestate state = Gamestate.Ready;
    public TMP_Text stateText;

    // 필요속성3 : hp가 들어가 있는 playerMove
    PlayerMove player;

    //필요속성 : 플레이어의 애니메이터 컴포넌트
    private Animator animator;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        stateText.text = "Ready";
        stateText.color = new Color32 (225, 185, 0, 255);

        StartCoroutine(GameStart());

        player = GameObject.Find("Player").GetComponent<PlayerMove>();
        animator = player.GetComponentInChildren<Animator>();
    }
    // 목적2 : 2초후 게임이 시작 된다 .Ready 상태에서 Start상태로 변경되며 게임이 시작된다.
    IEnumerator GameStart()
    {
        //2초를 기다린다.
        yield return new WaitForSeconds(2f);

        stateText.text = "Game Start";

        //0.5초를 기다린다.
        yield return new WaitForSeconds(0.5f);

        //상태 text 비활성
        stateText.gameObject.SetActive(false);

        //상태변경
        state = Gamestate.start;
        
    }

    // 목적3: 플레이어의 hp가 0보다 작으면 상태텍스트와 상태를 GameOver로 바꿔주고

    void CheakGameOver() 
    {
        if(player.hp <= 0) 
        {
            // 목적4: 플레이어의 hp가 0 이하라면, 플레이어의 애니메이션을 멈춘다.
            animator.SetFloat("MoveMotion", 0f);

            //상태 텍스트ON
            stateText.gameObject.SetActive(true);

            //상태 텍스트를 GameOver로 변경
            stateText.text = "GameOver";

            //상태 텍스트의 컬러를 빨색으로 변경
            stateText.color = new Color32(225, 0, 0, 255);

            state = Gamestate.GameOver;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheakGameOver();
    }
}
