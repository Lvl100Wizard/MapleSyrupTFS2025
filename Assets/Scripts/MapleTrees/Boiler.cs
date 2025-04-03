using UnityEngine;

public class Boiler : BaseProductionMachine
{
    [SerializeField] private AudioClip dropOffClip;
    [SerializeField] private AudioClip pickupClip;
    public MeshRenderer bucketMesh;
    public GameObject syrupPrefab;

    protected override string RequiredItemTag() => "Sap";
    protected override GameObject OutputPrefab() => syrupPrefab;

    protected override MeshRenderer childMesh() => bucketMesh;

    protected override int maxInputRequired => 4;

    protected override AudioClip DropOffClip => dropOffClip;
    protected override AudioClip PickupClip => pickupClip;
}

    