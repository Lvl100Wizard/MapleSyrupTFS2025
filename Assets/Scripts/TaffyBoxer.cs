using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaffyBoxer : BaseProductionMachine
{
    [SerializeField] private AudioClip dropOffClip;
    [SerializeField] private AudioClip pickupClip;
    public MeshRenderer boxMesh; //child component
    public GameObject TaffyBoxPrefab;

    protected override string RequiredItemTag() => "TaffyTray";
    protected override GameObject OutputPrefab() => TaffyBoxPrefab;

    protected override MeshRenderer childMesh() => boxMesh;

    protected override int maxInputRequired => 2;

    protected override AudioClip DropOffClip => dropOffClip;
    protected override AudioClip PickupClip => pickupClip;

}
