using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseProductionMachine : MonoBehaviour, IDropOffHandler, IPickUpHandler, IStructureUI
{
    [Header("Production Variables")]
    public bool isProducing = false;
    public bool hasOutput = false; // Generic "output" instead of syrup
    public float productionTime = 10.0f;
    protected int currentInputCount = 0;

    [Header("UI Elements")]
    public GameObject timerUIPrefab;
    public GameObject dropOffRequirementUIPrefab;

    [Header("Item Requirement Icon")]
    public Sprite inputIcon;

    protected SapTimerUI timerUI;
    protected DropOffRequirementUI dropOffUI;
    protected Canvas mainCanvas;

    protected abstract AudioClip PickupClip { get; }
    protected abstract AudioClip DropOffClip{ get; }
    protected abstract int maxInputRequired { get; }
    protected abstract string RequiredItemTag(); // Forces child classes to specify required item
    protected abstract GameObject OutputPrefab(); // Forces child classes to define output

    protected abstract MeshRenderer childMesh();


    void Start()
    {
       




     

    }

    public void InitializeUI()
    {
        if (dropOffUI != null)
            return;

        mainCanvas = FindObjectOfType<Canvas>();
        if (!mainCanvas) { Debug.LogError("No Canvas found!"); return; }

        Transform gameUIPanel = mainCanvas.transform.Find("GameUIPanel");
        if (!gameUIPanel) { Debug.LogError("No GameUIPanel found in Canvas!"); return; }

        // Initialize Timer UI
        GameObject timerUIObject = Instantiate(timerUIPrefab, gameUIPanel);
        timerUI = timerUIObject.GetComponent<SapTimerUI>();
        timerUI.Initialize(transform, isProducing);

        // Initialize Drop-Off UI
        GameObject dropOffUIObject = Instantiate(dropOffRequirementUIPrefab, gameUIPanel);
        dropOffUI = dropOffUIObject.GetComponent<DropOffRequirementUI>();
        dropOffUI.Initialize(transform, inputIcon, maxInputRequired);
    }

    public void HandleDropOff(PlayerObjects playerInventory)
    {
        if (isProducing || hasOutput) return;

        int itemsHeld = playerInventory.GetItemCountByTag(RequiredItemTag());

        if (itemsHeld > 0)
        {
            int itemsNeeded = maxInputRequired - currentInputCount;
            int itemsToDeposit = Mathf.Min(itemsHeld, itemsNeeded);

            playerInventory.DropOffItems(itemsToDeposit, RequiredItemTag(), transform);
            currentInputCount += itemsToDeposit;

            SoundFXManager.instance.PlaySoundFXClip(DropOffClip, transform, 1f);
            dropOffUI.UpdateDropOffProgress(currentInputCount, maxInputRequired);

            if (currentInputCount >= maxInputRequired)
            {
                StartProduction();
                dropOffUI.SetVisible(false);
            }
        }
    }

    public virtual void HandlePickup(PlayerObjects playerInventory)
    {
        if (hasOutput)
        {
            SoundFXManager.instance.PlaySoundFXClip(PickupClip, transform, 1f);


            playerInventory.CollectItem(OutputPrefab(), transform);
            hasOutput = false;
            currentInputCount = 0;

            timerUI.SetCheckmarkVisibility(false);
            timerUI.SetSliderValue(0f);
            dropOffUI.UpdateDropOffProgress(currentInputCount, maxInputRequired);
            dropOffUI.SetVisible(true);
            childMesh().enabled = false;
        }
    }

    protected void StartProduction()
    {
        isProducing = true;
        timerUI.StartCooldown(productionTime, FinishProduction);
    }

    protected virtual void FinishProduction()
    {
        isProducing = false;
        hasOutput = true;
        timerUI.SetCheckmarkVisibility(true);
        childMesh().enabled = true;
    }
}
