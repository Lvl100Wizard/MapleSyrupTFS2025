using UnityEngine;

public class SnowGlobe : BaseProductionMachine
{
    [SerializeField] private AudioClip dropOffClip;
    [SerializeField] private AudioClip pickupClip;
    public MeshRenderer trayMesh; //child component
    public GameObject TaffyTrayPrefab;

    protected override string RequiredItemTag() => "SyrupUnfiltered";
    protected override GameObject OutputPrefab() => TaffyTrayPrefab;

    protected override MeshRenderer childMesh() => trayMesh;

    protected override int maxInputRequired => 1;

    protected override AudioClip DropOffClip => dropOffClip;
    protected override AudioClip PickupClip => pickupClip;
}

    