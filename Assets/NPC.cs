using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public string[] dialogue;
    private int index;

    public GameObject continueButton;
    public GameObject keyF;
    public InputField feedBack;
    public float wordSpeed;
    public bool playerIsClose;

    public bool IsTalking;
    // Thêm các bi?n cho UI nh?p tên
    public GameObject nameInputPanel;
    public TMP_InputField nameInputField;
    public Button submitNameButton;
    public TextMeshProUGUI nameDisplayText;

    public PlayerController playerController;

    public bool canNextLine = true;
    private void Awake()
    {
        playerController = FindFirstObjectByType<PlayerController>();
    }
    private void Start()
    {

        // Ð?t hành ð?ng cho nút g?i tên
        submitNameButton.onClick.AddListener(OnSubmitName);
        // ?n b?ng nh?p tên khi b?t ð?u
        nameInputPanel.SetActive(false);
    }

    private void Update()
    {
        playerController.canAction = !IsTalking;
        if (Input.GetKeyDown(KeyCode.F) && playerIsClose && dialoguePanel.activeInHierarchy == false)
        {
            IsTalking = true;
            if (dialoguePanel.activeInHierarchy)
            {
                zeroText();
            }
            else
            {
                dialoguePanel.SetActive(true);
                StartCoroutine(Typing());
            }
        }
        else if (Input.GetKeyDown(KeyCode.F) && playerIsClose && dialoguePanel.activeInHierarchy)
        {
            if(canNextLine==true)
                NextLine();
        }
    }
    private void FixedUpdate()
    {
        if (index == 1)
        {
            nameInputPanel.SetActive(true);
            canNextLine = false;

        }

    }
    public void zeroText()
    {
        dialogueText.text = "";
        index = 0;
        dialoguePanel.SetActive(false);
        continueButton.SetActive(false);
    }

    IEnumerator Typing()
    {
        foreach (char letter in dialogue[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
        continueButton.SetActive(true);
    }

    public void NextLine()
    {
        if (canNextLine == true)
        {
            if (index < dialogue.Length - 1)
            {
                index++;
                dialogueText.text = " ";
                StartCoroutine(Typing());
            }
            else
            {
                IsTalking = false;
                zeroText();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerIsClose = true;
            keyF.SetActive(true);
            // Hi?n th? b?ng nh?p tên n?u chýa nh?p tên
            if (!PlayerPrefs.HasKey("PlayerName"))
            {
                nameInputPanel.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerIsClose = false;
            keyF.SetActive(false);
            zeroText();
        }
    }

    private void OnSubmitName()
    {
        string playerName = nameInputField.text;
        if (!string.IsNullOrEmpty(playerName))
        {
            // Lýu tên ngý?i chõi vào PlayerPrefs
            PlayerPrefs.SetString("PlayerName", playerName);
            PlayerPrefs.Save();
            // C?p nh?t tên hi?n th? và ?n b?ng nh?p tên
            nameDisplayText.text =   playerName+" ngu :))" ;
            nameInputPanel.SetActive(false);
            canNextLine = true;
            NextLine();
        }
        else
        {
            Debug.Log("Please enter a name.");
        }
    }
}

