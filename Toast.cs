using UnityEngine;

public class Toast: MonoBehaviour
{
    private DefaultToast _instance;

    void Start() 
    {
        #if !UNITY_EDITOR && UNITY_IOS
            _instance = new iOSToast();
        #elif !UNITY_EDITOR && UNITY_ANDROID
            _instance = new AndroidToast();
        #else
            _instance = new DefaultToast();
        #endif
    }

    /*
    * message: 표현될 메세지
    * time: 0: LENGTH_SHORT, 1: LENGTH_LONG (Other)
    */
    public void Show(string message, int time)
    {
        _instance.Show(message, time);
    }
}

public class DefaultToast {
    public virtual void Show(string message, int time) {
        Debug.Log("DefaultToast > Show() : Untiy3d editor can not do anything.");
    }
}

public class AndroidToast: DefaultToast {
    AndroidJavaObject toast;

    public override void Show(string message, int time) {
        using (AndroidJavaClass ajc = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
            using (AndroidJavaObject activity = ajc.GetStatic<AndroidJavaObject>("currentActivity")) {
                using (AndroidJavaObject global = activity.Call<AndroidJavaObject>("getApplicationContext")) {
                    AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
                    AndroidJavaObject msg = new AndroidJavaObject("java.lang.String", message);
                    int opt = toastClass.GetStatic<int>( time == 0 ? "LENGTH_SHORT" : "LENGTH_LONG" );

                    // Toast가 이미 실행 중일 경우, 실행되던 Toast는 취소 시킨 후 다시 생성. (토스트 겹침 현상 때문)
                    if (toast != null) {
                        toast.Call("cancel");
                    }

                    // Toast 객체 생성
                    toast = toastClass.CallStatic<AndroidJavaObject>(
                        "makeText",
                        global,
                        msg,
                        opt
                    );

                    // 실행
                    toast.Call("show");
                }
            }
        }
    }
}

public class iOSToast: DefaultToast {
    //iOS는 뒤로가기 버튼이 없으므로 제외
    public override void Show(string message, int time) {
        Debug.Log("iOSToast > Show() : Untiy3d editor can not do anything.");
    }
}