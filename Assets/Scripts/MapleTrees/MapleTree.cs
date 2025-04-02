using UnityEngine;
using UnityEngine.UIElements;

public class MapleTree : MonoBehaviour, IPickUpHandler
{
    public bool canTap = true;
    public float tapCooldown = 10.0f;

    //Pickup Audio
    [SerializeField] private AudioClip pickupClip;

    //SapTimerUI Prefab
    public GameObject timerUIPrefab;

    //SapPail Prefab
    public GameObject sapPail;

    public MeshRenderer bucketMesh;

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

        //Finds the GameUIPanel inside the Canvas for UI placement
        Transform gameUIPanelTransform = mainCanvas.transform.Find("GameUIPanel");
        if (gameUIPanelTransform == null)
        {
            Debug.LogError("No GameUIPanel found inside the Canvas!");
            return;
        }

        //Instantiate UI for the MapleTree
        GameObject timerUIObject = Instantiate(timerUIPrefab, gameUIPanelTransform);
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
            //Play SoundFX
            SoundFXManager.instance.PlaySoundFXClip(pickupClip, transform, 1f);
            Debug.Log("Harvesting Sap!");
            canTap = false;

            timerUI.SetCheckmarkVisibility(false);
            timerUI.StartCooldown(tapCooldown, () => EndCooldown());

            //Item pickup
            playerInventory.CollectItem(sapPail, this.transform);

            bucketMesh.enabled = false;
        }
        else if (!canTap)
        {
            Debug.Log("Not ready to harvest!");
        }
    }

    private void EndCooldown()
    {
        canTap = true;
        bucketMesh.enabled = true;

        timerUI.SetCheckmarkVisibility(true);
    }
}