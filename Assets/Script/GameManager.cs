using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

// 목적 : 게임의 상태(Ready, start, Game Over)를 구별하고, 게임의 시작과 끝을 TEXTUI로 표현하고 싶다.
// 필요속성 : 게임상태, 열거형 변수,TextUI

// 목적2 : 2초후 게임이 시작 된다 .Ready 상태에서 Start상태로 변경되며 게임이 시작된다.

// 목적 3 : 플레이어의 Hp가 0보다 작으면 상태 텍스트를 GAmeOver로 바꿔주고 상태또한 GameOver로바꿔준다.
// 필요속성3 : hp가 들어가 있는 playerMove

//목적 4 :  플레이어의 hp가 0이하라면, 플레이어의 애니메이션을 멈춘다.
//필요속성 : 플레이어의 애니메이터 컴포넌트

// 목적5 : Setting  버튼을 누르면 Option UI 가 켜진다. 동시에 세임 속도를 조절한다 (0 or 1)
//필요속성 : Option UI게임오브젝트. 일시정지 상태

// 목적6 : 게임오버시 Retry 와 Quit 버튼을 활성화한다. 
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    // 필요속성 :게임상태, 열거형 변수,TextUI

    public enum Gamestate
    {
        Ready, 
        start, 
        GameOver,
        Start,
        Pause
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

    //필요속성 : 플레이어의 애니메이터 컴포넌트
    public GameObject optionUI;

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

            // 목적6 : 게임오버시 Retry 와 Quit 버튼을 활성화한다. 
            GameObject retryBtn = stateText.transform.GetChild(0).gameObject;
            GameObject quitBtn = stateText.transform.GetChild(0).gameObject;
            retryBtn.SetActive(true);
            quitBtn.SetActive(true);

            //목적7 : 게임 오버시 HP Bar와 WEapon Mode Text를 비활성화한다.
            player.hpSlider.gameObject.SetActive(true);
            //player.GetComponent<PlayerFire>().weaponModeTxt.gameObject.SetActive(false);

            state = Gamestate.GameOver;

            
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheakGameOver();
    }

    // 목적5 : Option  버튼을 누르면 Option UI 가 켜진다. 동시에 세임 속도를 조절한다 (0 or 1)
    // option 화면 켜기
    public void OpenOptionWindow()
    {
        //Option UI 가 켜진다.
        optionUI.SetActive(true);   
        //동시에 세임 속도를 조절한다 (00
        Time.timeScale = 0;
        state = Gamestate.Pause;
    }
    //계속하기
    public void CloseOptionWindow()
    {
        //Option UI 가 꺼진다.
        optionUI.SetActive(false);
        //동시에 세임 속도를 조절한다 (1)
        Time.timeScale = 1;
        state = Gamestate.Start;
    }
    // 다시하기 옵션
    public void Restart()
    {
        //동시에 세임 속도를 조절한다 (1)
        Time.timeScale = 1;

        //씬 번호를 다시 로드
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //게임종료 옵션
    public void QuitGame()
    {
        Application.Quit();
    }
}
