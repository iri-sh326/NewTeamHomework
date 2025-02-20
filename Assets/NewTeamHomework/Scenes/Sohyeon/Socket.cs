using TMPro;
using UnityEngine;

public class Socket : MonoBehaviour
{
    public Transform _transform;
    public Canvas canvas;
    public TextMeshProUGUI text;
    public GameObject ball_1;
    public GameObject ball_2;
    public GameObject ball_3;

    public bool isCorrect_1;
    public bool isCorrect_2;
    public bool isCorrect_3;
    
    public DialogUI dialogUI;
    public GameObject dialogUiCanvas;
    private bool hasActivatedDialog = false; // 상태 플래그
    
    private void Update()
    {
        if(isCorrect_1 && isCorrect_2 && isCorrect_3 && !hasActivatedDialog)
        {
            hasActivatedDialog = true;
            dialogUI.waitForPlayerAction = false;
            dialogUI.stopFlag = false;
            dialogUiCanvas.SetActive(true); // 캔버스를 다시 활성화
            dialogUiCanvas.transform.position = (_transform.position);
            dialogUI.DrawNextDialog();
        }
        else
        {
            Debug.Log("오답");
        }
    }
}
