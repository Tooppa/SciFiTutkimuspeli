using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager Instance { get; private set; }
    public GameObject uiButton;
    public GameObject notesByLocation;
    public GameObject floatingText;
    public GameObject hud;
    public GameObject noteScreen;
    public GameObject rocketButton;
    public Transform storedNotesScreen;
    public Transform upgradeGrid;
    public TextMeshProUGUI upgradeText;
    public Slider fuelSlider;
    private Transform _currentNoteScreen;
    private float _currentNoteScreenHeight;
    private string _currentLocation;
    private GameObject _interact;

    private void Awake()
    {
        Instance = this;
        _currentNoteScreenHeight = 0;
        rocketButton.SetActive(false);
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
        hud.SetActive(true);
        noteScreen.SetActive(true);
        noteScreen.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = note;
        PauseGame();
    }

    public void AddNewNote(Sprite sprite, string note, string location)
    {
        if (!_currentNoteScreen || location != _currentLocation)
        {
            // special case for rocket area
            if(location == "Rocket")
                rocketButton.SetActive(true);
            var current = Instantiate(notesByLocation, storedNotesScreen);
            _currentNoteScreen = current.GetComponentInChildren<HorizontalLayoutGroup>().transform;
            _currentLocation = location;
            
            // get height of spawned notesbylocation element
            var heightNewElement = current.GetComponent<RectTransform>().sizeDelta.y;
            // calculate new height of children on stored notes screen
            _currentNoteScreenHeight += heightNewElement;
            // get current height of stored note screen
            var heightStored = storedNotesScreen.GetComponent<RectTransform>().sizeDelta.y;
            // if children's combined height exceeds current height of note screen calculate new height to fit all children
            if (_currentNoteScreenHeight > heightStored)
            {
                var widthStored = storedNotesScreen.GetComponent<RectTransform>().sizeDelta.x;
                storedNotesScreen.GetComponent<RectTransform>().sizeDelta = new Vector2(widthStored, _currentNoteScreenHeight);
            }
            current.GetComponentInChildren<TextMeshProUGUI>().text = _currentLocation;
        }
        var obj = Instantiate(uiButton, _currentNoteScreen);
        obj.GetComponent<Image>().sprite = sprite;
        var btn = obj.GetComponent<Button>();
        btn.onClick.AddListener(() => { ShowText(note);});
    }

    public void HideInteract()
    {
        _interact.SetActive(false);
    }
    public void ShowInteract(Transform goT)
    {
        if (!_interact || !_interact.activeSelf)
        {
            _interact = Instantiate(floatingText, goT.position + Vector3.up, quaternion.identity, goT);
            return;
        }
        _interact.SetActive(true);
    }
}
