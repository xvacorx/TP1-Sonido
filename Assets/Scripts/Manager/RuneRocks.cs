using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RuneRocks : MonoBehaviour, IInteractable
{
    [SerializeField] private FenceGate fenceGate;
    [SerializeField] private Spawner spawner;
    [SerializeField] private int eventEnemiesAmount;
    [SerializeField] private GateColour gateColour;
    [SerializeField] private GameObject runeObtainedEffect;
    [SerializeField] private Transform canvas;

    public enum GateColour {greenGate, purpleGate, blueGate}

    private GameManager gameManager;
    private string runeColour;

    private bool interactable = true;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void Interact()
    {
        if (interactable && !gameManager.eventActive)
            StartCoroutine(StartEvent());
    }

    private void Update()
    {
        if (gameManager.killCounter == eventEnemiesAmount)
            EndEvent();
    }

    private IEnumerator StartEvent()
    {
        FindObjectOfType<AudioManager>().Play("Event");

        interactable = false;
        gameManager.eventActive = true;

        runeColour = gateColour.ToString();

        if (runeColour == "greenGate")        
            gameManager.greenRune = true;
        
        else if (runeColour == "purpleGate")        
            gameManager.purpleRune = true;
        
        else       
            gameManager.blueRune = true;
        

        for (int i = 0; i < eventEnemiesAmount; i++)
        {
            spawner.Spawn();
            yield return new WaitForSeconds(15f);
        }
    }

    private void EndEvent()
    {
        FindObjectOfType<AudioManager>().Play("Completed");
        FindObjectOfType<PlayerActions>().FullHeal();

        GameObject runeEffect = Instantiate(runeObtainedEffect, canvas);
        Destroy(runeEffect, 2f);

        gameManager.eventActive = false;
        gameManager.killCounter = 0;

        gameManager.UpdateRunesUI();
    }
}
