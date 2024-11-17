using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    // Declaración de títulos y textos
    public Button tutorialButton;
    public GameObject tit1Tutorial;
    public GameObject tit2Tutorial;
    public GameObject tit3Tutorial;
    public GameObject tit4Tutorial;
    public GameObject tit5Tutorial;
    public GameObject text1Tutorial;
    public GameObject text2Tutorial;
    public GameObject text3Tutorial;
    public GameObject text4Tutorial;
    public GameObject text5Tutorial;
    public GameObject UI1Tutorial;
    public GameObject UI2Tutorial;
    public GameObject UI3Tutorial;
    public GameObject UI4Tutorial;
    public GameObject UI5Tutorial;

    //Estado
    private int _state;

    private void Awake()
    {
        _state = 0;

        //Inicializar el estado inicial
        tit1Tutorial.SetActive(true);
        text1Tutorial.SetActive(true);
        tit2Tutorial.SetActive(false);
        text2Tutorial.SetActive(false);
        tit3Tutorial.SetActive(false);
        text3Tutorial.SetActive(false);
        tit4Tutorial.SetActive(false);
        text4Tutorial.SetActive(false);
        tit5Tutorial.SetActive(false);
        text5Tutorial.SetActive(false);

        UI1Tutorial.SetActive(true);
        UI2Tutorial.SetActive(false);
        UI3Tutorial.SetActive(false);
        UI4Tutorial.SetActive(false);
        UI5Tutorial.SetActive(false);

        // Vincular el botón al método OnButtonPressed
        tutorialButton.onClick.AddListener(OnButtonPressed);
    }

    public void OnButtonPressed()
    {
        _state++;
        if (_state == 1)
        {
            tit1Tutorial.SetActive(false);
            text1Tutorial.SetActive(false);
            UI1Tutorial.SetActive(false);
            tit2Tutorial.SetActive(true);
            text2Tutorial.SetActive(true);
            UI2Tutorial.SetActive(true);
        }
        else if (_state == 2)
        {
            tit2Tutorial.SetActive(false);
            text2Tutorial.SetActive(false);
            UI2Tutorial.SetActive(false);
            tit3Tutorial.SetActive(true);
            text3Tutorial.SetActive(true);
            UI3Tutorial.SetActive(true);
        }
        else if (_state == 3)
        {
            tit3Tutorial.SetActive(false);
            text3Tutorial.SetActive(false);
            UI3Tutorial.SetActive(false);
            tit4Tutorial.SetActive(true);
            text4Tutorial.SetActive(true);
            UI4Tutorial.SetActive(true);
        }
        else if (_state == 4)
        {
            tit4Tutorial.SetActive(false);
            text4Tutorial.SetActive(false);
            UI4Tutorial.SetActive(false);
            tit5Tutorial.SetActive(true);
            text5Tutorial.SetActive(true);
            UI5Tutorial.SetActive(true);
        }
        else if (_state == 5)
        {
            // Cargar la escena del menú principal
            SceneManager.LoadScene("MainMenuScene");
        }
    }
}
