using UnityEngine;

public class XRotate : MonoBehaviour {
    // 目标对象
    public GameObject target;
    // 是否可以旋转
    private bool m_rotate = false;
    public float speed = 20;

    private float screenY;
    // Use this for initialization
    void Start () {
        screenY = Screen.height;
        if (target == null) {
            target = this.gameObject;
        }

        if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android) {
            speed = speed / 60;
        }
    }

    // Update is called once per frame
    private void Update () {

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown (0)) {
            m_rotate = true;
        }

        if (Input.GetMouseButtonUp (0)) {
            m_rotate = false;
        }

        if (m_rotate) {
            float mouseX = Input.GetAxis ("Mouse X") * -100f;
            target.transform.Rotate (Vector3.up, mouseX * Time.deltaTime * speed);
        }

#endif

#if UNITY_IOS || UNITY_ANDROID
        
        //if (!isTouchUI()) return;
        if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Began) {
            m_rotate = true;
        }

        if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Ended) {
            m_rotate = false;
        }

        if (m_rotate && Input.touchCount == 1 && Input.GetTouch (0).phase == TouchPhase.Moved && Input.GetTouch(0).position.y > screenY*0.25f && Input.GetTouch(0).position.y < screenY * 0.75f) {
            //Debug.LogWarning("------Input.GetTouch (0).---" + Input.GetTouch(0).position.y);
            Vector2 deltaPos = Input.GetTouch (0).deltaPosition;
            target.transform.Rotate (Vector3.down * deltaPos.x * speed, Space.World);
        }
#endif

    }

    /// <summary>判断是否点击在UI上面</summary>
    private bool isTouchUI()
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            if (Input.touchCount < 1) return false;
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) return true;
        }
        else
        {
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return true;
        }

        return false;
    }
}