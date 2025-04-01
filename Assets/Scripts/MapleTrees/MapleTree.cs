using UnityEngine;

public class MapleTree : MonoBehaviour, IPickUpHandler
{
    public bool canTap = true;
    public float tapCooldown = 10.0f;

    //SapTimerUI Prefab
    public GameObject timerUIPrefab;

    //SapPail Prefab
    public GameObject sapPail;

    private SapTimerUI timerUI;
    private Canvas mainCanvas;

    private void Start()
    {
        //Find the main canvas
        mainCanvas = GameObject.FindObjectOfType<Canvas>();
        if (mainCanvas == null)
        {
            Debug.LogError("No Canvas found in the scene!");
            return;
        }

        //Instantiate UI for the MapleTree
        GameObject timerUIObject = Instantiate(timerUIPrefab, mainCanvas.transform);
        timerUI = timerUIObject.GetComponent<SapTimerUI>();

        if (timerUI == null)
        {
            Debug.LogError("SapTimerUI component not found on prefab!");
            return;
        }

        //Initializes UI at the tree's position
        timerUI.Initialize(this.transform, canTap);

        //Start cooldown if tree is not ready
        if (!canTap)
        {
            timerUI.StartCooldown(tapCooldown, () => EndCooldown());
        }
    }

    public void HandlePickup(PlayerObjects playerInventory)
    {
        if (canTap)
        {
            Debug.Log("Harvesting Sap!");
            canTap = false;

            timerUI.SetCheckmarkVisibility(false);
            timerUI.StartCooldown(tapCooldown, () => EndCooldown());

            //Item pickup
            playerInventory.CollectItem(sapPail);
        }
        else if (!canTap)
        {
            Debug.Log("Not ready to harvest!");
        }
    }

    private void EndCooldown()
    {
        canTap = true;
        timerUI.SetCheckmarkVisibility(true);
    }
}