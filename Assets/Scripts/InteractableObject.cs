using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InteractableObject : CollidableObject
{
    private PopUpSystem popUpSystem;
    private bool z_Interacted = false;

    protected override void Start()
    {
        base.Start();
        popUpSystem = FindObjectOfType<PopUpSystem>(); // <- auto-assignation
        if (popUpSystem == null)
        {
            Debug.LogWarning("PopUpSystem non trouvé dans la scène !");
        } 
    }
    
    protected override void OnCollided(GameObject collidedObject)
    {
        Debug.Log("Collided with" + collidedObject.name);
        if (Input.GetKey(KeyCode.E))
        {
            OnInteract();
        }
    }

    protected virtual void OnInteract()
    {
        if (!z_Interacted)
        {
            z_Interacted = true;
            Debug.Log("BOMBARDINO CROCOCILLO" + name);
        }

        if (popUpSystem != null)
        {
            popUpSystem.PopUp("Question de probabilité : ....");
        }
        
    }

}
