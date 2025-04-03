using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAnimator : MonoBehaviour
{

   // Reference to the Animator component
    [SerializeField] public Animator animator;

    // Public references to the two animation controllers
    [SerializeField] public RuntimeAnimatorController movehold;
    [SerializeField] public RuntimeAnimatorController standhold;
    [SerializeField] RuntimeAnimatorController stand;
    [SerializeField] RuntimeAnimatorController move;



    [SerializeField]public playerController pc;
    [SerializeField]public PlayerObjects po;

    // Start is called before the first frame update
    void Start()
    {
        // Get the Animator component attached to this GameObject
        animator = GetComponentInChildren<Animator>();

        // Default controller (optional)
        animator.runtimeAnimatorController = stand;
    }

    // Method to switch between animation controllers
    void SwitchController(RuntimeAnimatorController ac)
    {
        
            animator.runtimeAnimatorController = ac;
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (po.GetHeldItemCount() > 0)
        {
            if (pc.IsMoving())
            {
                SwitchController(movehold);
            }
            else
            {
                SwitchController(standhold);
            }
        }else
        {
            if (pc.IsMoving())
            {
                SwitchController(move);

            }
            else
            {
                SwitchController(stand);
            }
        }
    }
}

