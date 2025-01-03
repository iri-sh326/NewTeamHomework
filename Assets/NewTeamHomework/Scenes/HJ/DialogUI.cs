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
    #endregion

    private void Start()
    {
        
        //xml 데이터 파일 읽기
        LoadDialogXml(xmlFile);

        dialogs = new Queue<Dialog>();
        InitDialog();

        StartDialog(0);
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
    public void StartDialog(int dialogIndex)
    {
        //현재 대화씬(dialogIndex) 내용을 큐에 입력
        foreach (XmlNode node in allNodes)
        {
            int num = int.Parse(node["number"].InnerText);
            if (num == dialogIndex)
            {
                Dialog dialog = new Dialog();
                dialog.number = num;
                dialog.character = int.Parse(node["character"].InnerText);
                dialog.name = node["name"].InnerText;
                dialog.sentence = node["sentence"].InnerText;

                dialogs.Enqueue(dialog);
            }
        }

        //첫번째 대화를 보여준다
        DrawNextDialog();
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

        if(dialog.character == 1&& dialog.number ==1 )
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
                boardtext.text = " H + O + O ";
            }
            if (dialog.sentence == "이제 여러분이 직접 수소 원자와 산소 원자를 가져와서 결합을 만들어보세요.")
            {
                currentAnimIndex++;
                Anim(currentAnimIndex);
            }




        }
            
        nameText.text = dialog.name;
        StartCoroutine(typingSentence(dialog.sentence));
        //sentenceText.text = dialog.sentence;
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
        InitDialog();

        //대화 종료시 이벤트 처리
        //...
    }

    private void Anim(int index)
    {
        teacher.SetInteger("a",index);
    }
}
