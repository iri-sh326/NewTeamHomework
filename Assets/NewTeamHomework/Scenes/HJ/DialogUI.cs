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
        
        //xml ������ ���� �б�
        LoadDialogXml(xmlFile);

        dialogs = new Queue<Dialog>();
        InitDialog();

        StartDialog(0);
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
    public void StartDialog(int dialogIndex)
    {
        //���� ��ȭ��(dialogIndex) ������ ť�� �Է�
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

        //ù��° ��ȭ�� �����ش�
        DrawNextDialog();
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

        if(dialog.character == 1&& dialog.number ==1 )
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
                boardtext.text = " H + O + O ";
            }
            if (dialog.sentence == "���� �������� ���� ���� ���ڿ� ��� ���ڸ� �����ͼ� ������ ��������.")
            {
                currentAnimIndex++;
                Anim(currentAnimIndex);
            }




        }
            
        nameText.text = dialog.name;
        StartCoroutine(typingSentence(dialog.sentence));
        //sentenceText.text = dialog.sentence;
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
        InitDialog();

        //��ȭ ����� �̺�Ʈ ó��
        //...
    }

    private void Anim(int index)
    {
        teacher.SetInteger("a",index);
    }
}
