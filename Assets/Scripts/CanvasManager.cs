using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager Instance { get; private set; }
    public GameObject uiButton;
    public GameObject notesByLocation;
    public GameObject hud;
    public GameObject noteScreen;
    public Transform storedNotesScreen;
    public Transform upgradeGrid;
    public TextMeshProUGUI upgradeText;
    public Slider fuelSlider;
    private Transform _currentNoteScreen; 

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        gameObject.GetComponent<Canvas>().worldCamera = Camera.main;
        _currentNoteScreen = null;
    }

    public void SetHudActive()
    {
        hud.SetActive(!hud.activeSelf);
        if(hud.activeSelf)
            PauseGame();
        else
            ResumeGame();
    }
    public void SetFuel(int fuel)
    {
        fuelSlider.value = fuel;
    }
    public void AddNewUpgrade(Sprite sprite, string stats)
    {
        var upgrade = Instantiate(uiButton, upgradeGrid);
        upgrade.GetComponent<Image>().sprite = sprite;
        var btn = upgrade.GetComponent<Button>();
        btn.onClick.AddListener(() => { upgradeText.text = stats; });
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        if (hud.activeSelf)return;
        Time.timeScale = 1;
    }

    public void ShowText(string note)
    {
        noteScreen.SetActive(true);
        noteScreen.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = note;
        PauseGame();
    }

    public void AddNewNote(Sprite sprite, string note)
    {
        if (!_currentNoteScreen)
        {
            var current = Instantiate(notesByLocation, storedNotesScreen);
            _currentNoteScreen = current.GetComponentInChildren<HorizontalLayoutGroup>().transform;
            current.GetComponentInChildren<TextMeshProUGUI>().text = "TestScene";
        }
        var obj = Instantiate(uiButton, _currentNoteScreen);
        obj.GetComponent<Image>().sprite = sprite;
        var btn = obj.GetComponent<Button>();
        btn.onClick.AddListener(() => { ShowText(note);});
    }
}
