using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;



public class DialogUI : MonoBehaviour
{
    #region Variables
    //XML
    public string xmlFile = "Dialog/Dialog";    //Path
    private XmlNodeList allNodes;

    private Queue<Dialog> dialogs;

    //UI
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI sentenceText;
    //public GameObject npcImage;
    public GameObject nextButton;


    public Animator teacher;
    public int currentAnimIndex = 0;
    public TextMeshProUGUI boardtext;
    
    public bool waitForPlayerAction = false;
    public bool stopFlag = false;
    public GameObject dialogUI;

    public GameObject socket1;
    public GameObject socket2;
    #endregion

    private void Start()
    {
        
        //xml 데이터 파일 읽기
        LoadDialogXml(xmlFile);

        dialogs = new Queue<Dialog>();
        InitDialog();

        StartDialog(0, 8);
    }

    //Xml 데이터 읽어 들이기
    private void LoadDialogXml(string path)
    {
        TextAsset xmlFile = Resources.Load<TextAsset>(path);

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlFile.text);
        allNodes = xmlDoc.SelectNodes("root/Dialog");
    }

    //대화 시작하기
    public void StartDialog(int dialogIndex, int dialogRange = 1)
    {
        // 큐 초기화
        dialogs.Clear();

        Debug.Log($"Dialog 시작: dialogIndex = {dialogIndex}, dialogRange = {dialogRange}");

        // XML 데이터 필터링 및 대사 등록
        foreach (XmlNode node in allNodes)
        {
            Debug.Log($"Processing XML Node: number={node["number"]?.InnerText}, character={node["character"]?.InnerText}, name={node["name"]?.InnerText}, sentence={node["sentence"]?.InnerText}");

            if (int.TryParse(node["number"].InnerText, out int num) && num >= dialogIndex && num < dialogIndex + dialogRange)
            {
                Dialog dialog = new Dialog
                {
                    number = num,
                    character = int.Parse(node["character"].InnerText),
                    name = node["name"].InnerText,
                    sentence = node["sentence"].InnerText
                };

                dialogs.Enqueue(dialog);
                Debug.Log($"Dialog Enqueued: number={dialog.number}, character={dialog.character}, name={dialog.name}, sentence={dialog.sentence}");
            }
        }

        // 큐에 등록된 대사 개수를 출력
        Debug.Log($"큐에 추가된 대사 개수: {dialogs.Count}");

        // 첫 번째 대사 출력
        if (dialogs.Count > 0)
        {
            DrawNextDialog();
        }
        else
        {
            Debug.LogWarning($"DialogIndex {dialogIndex}에 해당하는 대사가 없습니다.");
        }
    }

    //초기화
    private void InitDialog()
    {
        dialogs.Clear();

        //npcImage.SetActive(false);
        nameText.text = "";
        sentenceText.text = "";
        boardtext.text = "";
        nextButton.SetActive(false);
        
    }


    //다음 대화를 보여준다 - (큐)dialogs에서 하나 꺼내서 보여준다
    public void DrawNextDialog()
    {
        Debug.Log(dialogs.Count.ToString());
        //dialogs 체크
        if (dialogs.Count == 0)
        {
            EndDialog();
            return;
        }

        //dialogs에서 하나 꺼내온다
        Dialog dialog = dialogs.Dequeue();

        //if (dialog.character > 0)
        //{
        //    npcImage.SetActive(true);
        //    npcImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Dialog/Npc/Npc0" + dialog.character.ToString());
        //}
        //else //dialog.character<=0
        //{
        //    npcImage.SetActive(false);
        //}

        nextButton.SetActive(false);
        
        // 티쳐 행동 로직 설정 부분
        if(dialog.character == 1 && dialog.number ==1 )
        { 
            if(dialog.sentence == "자, 여기 보이는 작은 구체들이 각각 원자들이에요.")
            {
                currentAnimIndex++;
                Anim(currentAnimIndex);
            }
            if (dialog.sentence == "이런 결합을 공유 결합이라고 하는데, 원자들이 서로 전자를 나누며 강하게 연결되는 거랍니다.")
            {
                currentAnimIndex++;
                Anim(currentAnimIndex);
                boardtext.text = " H + O + H ";
            }
            if (dialog.sentence == "이제 여러분이 직접 수소 원자와 산소 원자를 가져와서 결합을 만들어보세요.")
            {
                currentAnimIndex++;
                Anim(currentAnimIndex);
            }
        }

        if (dialog.character == 1 && dialog.number == 3)
        {
            if(dialog.sentence == "공백")
            {
                stopFlag = true;
                waitForPlayerAction = true;
                dialogUI.SetActive(false);
                Anim(0);
                socket1.SetActive(true);
                return; // 이 시점에서 DrawNextDialog를 중단
            }
            
            if (dialog.sentence == "멋져요! 이제 수소 원자 두 개와 산소 원자 하나가 결합해서 완벽한 **물 분자(H\u2082O)**가 만들어졌어요.")
            {
                Anim(3);
            }
        }

        if (dialog.character == 1 && dialog.number == 5)
        {
            if (dialog.sentence == "이산화탄소는 직선 구조를 가지고 있어요. 그러니까 산소 원자 두 개가 탄소의 정확히 양옆에 위치해야 해요.")
            {
                boardtext.text = " O + C + O ";
            }
            
            if (dialog.sentence == "공백")
            {
                stopFlag = true;
                waitForPlayerAction = true;
                dialogUI.SetActive(false);
                Anim(0);
                socket2.SetActive(true);
                return; // 이 시점에서 DrawNextDialog를 중단
            }
        }

        if (!stopFlag)
        {
            nameText.text = dialog.name;
            StartCoroutine(typingSentence(dialog.sentence));
            //sentenceText.text = dialog.sentence;
        }
    }

    //텍스트 타이핑 연출
    IEnumerator typingSentence(string typingText)
    {
        sentenceText.text = "";

        foreach (char latter in typingText)
        {
            sentenceText.text += latter;
            yield return new WaitForSeconds(0.03f);
        }

        nextButton.SetActive(true);
    }

    //대화 종료
    private void EndDialog()
    {

        //대화 종료시 이벤트 처리
        //...
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void Anim(int index)
    {
        teacher.SetInteger("a",index);
    }
}
