using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    public GameObject damageTextPrefab;
    public GameObject healthTextPrefab;

    public Canvas gameCanvas;
    private void Awake()
    {
        gameCanvas=FindObjectOfType<Canvas>();
        
    }

    private void OnEnable()
    {
        CharacterEvents.characterDamaged+=CharacterTookDamage;
        CharacterEvents.characterHealed+=CharacterHealed;   
    }

    private void OnDisable()
    {
        CharacterEvents.characterDamaged-=CharacterTookDamage;
        CharacterEvents.characterHealed-=CharacterHealed;
    }
    public void CharacterTookDamage(GameObject character,int damageReceived)
    {
        //creat text
        Vector3 spawnPosition=Camera.main.WorldToScreenPoint(character.transform.position);

        TMP_Text tmpText = Instantiate(damageTextPrefab, spawnPosition, Quaternion.identity, gameCanvas.transform)
            .GetComponent<TMP_Text>();
        tmpText.text=damageReceived.ToString();
    }
    public void CharacterHealed(GameObject character,int healRestored) 
    {
        //creat text
        Vector3 spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);

        TMP_Text tmpText = Instantiate(healthTextPrefab, spawnPosition, Quaternion.identity, gameCanvas.transform).GetComponent<TMP_Text>();
        tmpText.text = healRestored.ToString();
    }
    public void OnExitGame(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            #if (UNITY_EDITOR|| DEVLOPMENT_BUILD)
                Debug.Log(this.name+" : "+this.GetType()+" : "+System.Reflection.MethodBase.GetCurrentMethod().Name);
            #endif
            #if (UNITY_EDITOR)
                UnityEditor.EditorApplication.isPlaying = false;

            #elif (UNITY_STANDALONE)
                  Application.Quit();
            #elif (UNITY_WEBGL)
                SeceneManager.LoadScene("QuitScene");
            #endif
        }
    }
}
