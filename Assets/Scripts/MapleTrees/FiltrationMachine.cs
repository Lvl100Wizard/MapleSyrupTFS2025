using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiltrationMachine : BaseProductionMachine
{
    [SerializeField] private AudioClip dropOffClip;
    [SerializeField] private AudioClip pickupClip;
    public MeshRenderer filteredSyrupMesh; //child component
    public GameObject FilteredSyrupPrefab;

    protected override string RequiredItemTag() => "SyrupUnfiltered";
    protected override GameObject OutputPrefab() => FilteredSyrupPrefab;

    protected override MeshRenderer childMesh() => filteredSyrupMesh;

    protected override int maxInputRequired => 2;

    protected override AudioClip DropOffClip => dropOffClip;
    protected override AudioClip PickupClip => pickupClip;
}
