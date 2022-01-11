using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitScript : MonoBehaviour
{
    private bool clickedBefore = false;

    public float quitTimer = 3.0f; //뒤로가기 키 3초간 체크
    public string firstMessage = "한번 더 누를 경우 앱이 종료됩니다.";
    public int firstMessageTime = 0; //0: LENGTH_SHORT, 1: LENGTH_LONG (토스트 표현 시간)

    void Update()
    {
        //Check input for the first time
        if (Input.GetKeyDown(KeyCode.Escape) && !clickedBefore)
        {
            // [Android] Toast메세지 실행
            gameObject.GetComponent<Toast>().Show( firstMessage, firstMessageTime );

            Debug.Log("Back Button pressed for the first time");
            // 한번 더 눌렀을 경우 확인하기 위한 변수
            clickedBefore = true;

            // 한번 더 누를 경우를 체크하기 위한 코루틴 생성
            StartCoroutine( quitingTimer(quitTimer) );
        }
    }

    IEnumerator quitingTimer(float timer)
    {
        //Wait for a frame so that Input.GetKeyDown is no longer true
        yield return null;

        float counter = 0;

        while (counter < timer)
        {
            // 경과 시간 누적
            counter += Time.deltaTime;

            // 뒤로 가기 키 체크 (Windows: ESC, Android: 뒤로가기)
            if ( Input.GetKeyDown(KeyCode.Escape) )
            {
                Quit();
            }

            //Wait for a frame so that Unity does not freeze
            yield return null;
        }

        // timer(ex: 3초)가 경과했을 경우 원상 복구
        clickedBefore = false;
    }

    void Quit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        #endif
    }
}
