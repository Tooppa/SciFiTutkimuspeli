using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMechanics : MonoBehaviour
{
    public Dictionary<string, GameObject> FoundItems { private set; get; }
    public Dictionary<string, GameObject> FoundNotes { private set; get; }
    private int _fuel = 0;
    private int _health = 10;

    private void Start()
    {
        CanvasManager.Instance.SetFuel(_fuel);
        CanvasManager.Instance.SetHealth(_health);
        FoundItems = new Dictionary<string, GameObject>();
        FoundNotes = new Dictionary<string, GameObject>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Pickable")) return;

        var go = other.gameObject;
        var pickables = go.GetComponent<Pickables>();

        if (pickables.HasFuel)
        {
            _fuel += pickables.data.fuel;
            CanvasManager.Instance.SetFuel(_fuel);
        }
        if (pickables.IsNote)
        {
            pickables.ShowInteract();
            FoundNotes.Add(other.name, go);
            return;
        }
        go.SetActive(false);
        FoundItems.Add(other.name, go);
        CanvasManager.Instance.AddNewImage(go.GetComponent<SpriteRenderer>().sprite);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        var pickables = other.gameObject.GetComponent<Pickables>();
        if (!pickables.IsNote || !Input.GetKey(KeyCode.E)) return;
        CanvasManager.Instance.ShowText(pickables.getNote());
        other.gameObject.SetActive(false);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var pickables = other.gameObject.GetComponent<Pickables>();
        if (pickables.IsNote)
            pickables.HideInteract();
    }
}
