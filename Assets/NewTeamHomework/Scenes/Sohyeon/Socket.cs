using UnityEngine;

public class Socket : MonoBehaviour
{
    public GameObject ball_1;
    public GameObject ball_2;
    public GameObject ball_3;

    public bool isCorrect_1;
    public bool isCorrect_2;
    public bool isCorrect_3;

    private void Update()
    {
        if(isCorrect_1 && isCorrect_2 && isCorrect_3)
        {
            Debug.Log("정답");
        }
        else
        {
            Debug.Log("오답");
        }
    }
}
