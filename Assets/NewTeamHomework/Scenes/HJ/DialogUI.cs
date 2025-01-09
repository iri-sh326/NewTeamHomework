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
        
        //xml ������ ���� �б�
        LoadDialogXml(xmlFile);

        dialogs = new Queue<Dialog>();
        InitDialog();

        StartDialog(0, 8);
    }

    //Xml ������ �о� ���̱�
    private void LoadDialogXml(string path)
    {
        TextAsset xmlFile = Resources.Load<TextAsset>(path);

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlFile.text);
        allNodes = xmlDoc.SelectNodes("root/Dialog");
    }

    //��ȭ �����ϱ�
    public void StartDialog(int dialogIndex, int dialogRange = 1)
    {
        // ť �ʱ�ȭ
        dialogs.Clear();

        Debug.Log($"Dialog ����: dialogIndex = {dialogIndex}, dialogRange = {dialogRange}");

        // XML ������ ���͸� �� ��� ���
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

        // ť�� ��ϵ� ��� ������ ���
        Debug.Log($"ť�� �߰��� ��� ����: {dialogs.Count}");

        // ù ��° ��� ���
        if (dialogs.Count > 0)
        {
            DrawNextDialog();
        }
        else
        {
            Debug.LogWarning($"DialogIndex {dialogIndex}�� �ش��ϴ� ��簡 �����ϴ�.");
        }
    }

    //�ʱ�ȭ
    private void InitDialog()
    {
        dialogs.Clear();

        //npcImage.SetActive(false);
        nameText.text = "";
        sentenceText.text = "";
        boardtext.text = "";
        nextButton.SetActive(false);
        
    }


    //���� ��ȭ�� �����ش� - (ť)dialogs���� �ϳ� ������ �����ش�
    public void DrawNextDialog()
    {
        Debug.Log(dialogs.Count.ToString());
        //dialogs üũ
        if (dialogs.Count == 0)
        {
            EndDialog();
            return;
        }

        //dialogs���� �ϳ� �����´�
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
        
        // Ƽ�� �ൿ ���� ���� �κ�
        if(dialog.character == 1 && dialog.number ==1 )
        { 
            if(dialog.sentence == "��, ���� ���̴� ���� ��ü���� ���� ���ڵ��̿���.")
            {
                currentAnimIndex++;
                Anim(currentAnimIndex);
            }
            if (dialog.sentence == "�̷� ������ ���� �����̶�� �ϴµ�, ���ڵ��� ���� ���ڸ� ������ ���ϰ� ����Ǵ� �Ŷ��ϴ�.")
            {
                currentAnimIndex++;
                Anim(currentAnimIndex);
                boardtext.text = " H + O + H ";
            }
            if (dialog.sentence == "���� �������� ���� ���� ���ڿ� ��� ���ڸ� �����ͼ� ������ ��������.")
            {
                currentAnimIndex++;
                Anim(currentAnimIndex);
            }
        }

        if (dialog.character == 1 && dialog.number == 3)
        {
            if(dialog.sentence == "����")
            {
                stopFlag = true;
                waitForPlayerAction = true;
                dialogUI.SetActive(false);
                Anim(0);
                socket1.SetActive(true);
                return; // �� �������� DrawNextDialog�� �ߴ�
            }
            
            if (dialog.sentence == "������! ���� ���� ���� �� ���� ��� ���� �ϳ��� �����ؼ� �Ϻ��� **�� ����(H\u2082O)**�� ����������.")
            {
                Anim(3);
            }
        }

        if (dialog.character == 1 && dialog.number == 5)
        {
            if (dialog.sentence == "�̻�ȭź�Ҵ� ���� ������ ������ �־��. �׷��ϱ� ��� ���� �� ���� ź���� ��Ȯ�� �翷�� ��ġ�ؾ� �ؿ�.")
            {
                boardtext.text = " O + C + O ";
            }
            
            if (dialog.sentence == "����")
            {
                stopFlag = true;
                waitForPlayerAction = true;
                dialogUI.SetActive(false);
                Anim(0);
                socket2.SetActive(true);
                return; // �� �������� DrawNextDialog�� �ߴ�
            }
        }

        if (!stopFlag)
        {
            nameText.text = dialog.name;
            StartCoroutine(typingSentence(dialog.sentence));
            //sentenceText.text = dialog.sentence;
        }
    }

    //�ؽ�Ʈ Ÿ���� ����
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

    //��ȭ ����
    private void EndDialog()
    {

        //��ȭ ����� �̺�Ʈ ó��
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
