using UnityEngine;

public class BottlingStation : BaseProductionMachine
{
    // Instead of using the base RequiredItemTag and maxInputRequired, we'll have two separate inputs.
    // For a 1:1 input, we expect exactly one item to be processed.
    public enum SyrupInputType { Normal, Premium }

    // Define the two input tags and corresponding outputs.
    // You can also make these abstract in BaseProductionMachine if other machines share similar patterns.
    [SerializeField] private string normalInputTag = "UnfilteredSyrupBucket";
    [SerializeField] private string premiumInputTag = "FilteredSyrupBucket";

    [SerializeField] private GameObject normalOutputPrefab;  // e.g. normal syrup bottle
    [SerializeField] private GameObject premiumOutputPrefab; // e.g. premium syrup bottle

    [SerializeField] private MeshRenderer normalBottleMesh; //child meshes
    [SerializeField] private MeshRenderer premiumBottleMesh;//child meshes


    // We'll use this to record which input was processed.
    private SyrupInputType currentInputType;

    // We won't use these two from the base class in the same way, so we can simply implement a dummy version.
    protected override string RequiredItemTag() { return ""; }
    protected override int maxInputRequired { get { return 1; } }
    protected override GameObject OutputPrefab()
    {
        // Return the correct output based on the input processed.
        return currentInputType == SyrupInputType.Premium ? premiumOutputPrefab : normalOutputPrefab;
    }

    // Child-specific mesh for visual cues (if needed)
    protected override MeshRenderer childMesh()
    {
        // Return the appropriate MeshRenderer (e.g., of a bucket or other visual element)
        return currentInputType == SyrupInputType.Premium ? premiumBottleMesh : normalBottleMesh;
        //GetComponentInChildren<MeshRenderer>();
    }

    // Override the drop-off behavior to check for two possible inputs.
    public override void HandleDropOff(PlayerObjects playerInventory)
    {
        if (isProducing || hasOutput) return;
        if (playerInventory == null) return;

        // Check for premium input first
        if (playerInventory.GetItemCountByTag(premiumInputTag) > 0)
        {
            playerInventory.DropOffItems(1, premiumInputTag, transform);
            currentInputType = SyrupInputType.Premium;
        }
        // Otherwise, check for normal input
        else if (playerInventory.GetItemCountByTag(normalInputTag) > 0)
        {
            playerInventory.DropOffItems(1, normalInputTag, transform);
            currentInputType = SyrupInputType.Normal;
        }
        else
        {
            Debug.Log("No valid input available for Syrup Bottler");
            return;
        }

        SoundFXManager.instance.PlaySoundFXClip(DropOffClip, transform, 1f);
        StartProduction();
        dropOffUI.SetVisible(false);
    }

    // The base HandlePickup (from BaseProductionMachine) can remain unchanged
    // so that when the player picks up the output, it will use OutputPrefab()
    // which we now override based on currentInputType.

    // Provide the sound clips via abstract properties:
    protected override AudioClip PickupClip => /* assign your pickup clip here, e.g.: */ pickupClip;
    [SerializeField] private AudioClip pickupClip;
    protected override AudioClip DropOffClip => /* assign your drop-off clip here, e.g.: */ dropOffClip;
    [SerializeField] private AudioClip dropOffClip;
}
