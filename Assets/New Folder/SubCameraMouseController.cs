using UnityEngine;

public class SubCameraMouseController : MonoBehaviour
{
    [Header("回転設定")]
    public float rotateSensitivity = 120f; // 左右回転感度（度/秒）

    [Header("上下移動設定")]
    public float moveSensitivity = 2f;     // 上下移動速度
    public float minHeight = 0.5f;         // 最低高さ
    public float maxHeight = 3.0f;         // 最高高さ

    void Update()
    {
        Rotate();
        MoveVertical();
    }

    void Rotate()
    {
        float mouseX = Input.GetAxis("Mouse X");

        if (mouseX != 0f)
        {
            transform.Rotate(
                0f,
                mouseX * rotateSensitivity * Time.deltaTime,
                0f
            );
        }
    }

    void MoveVertical()
    {
        float mouseY = Input.GetAxis("Mouse Y");

        if (mouseY != 0f)
        {
            Vector3 pos = transform.position;
            pos.y += mouseY * moveSensitivity * Time.deltaTime;
            pos.y = Mathf.Clamp(pos.y, minHeight, maxHeight);
            transform.position = pos;
        }
    }
}
