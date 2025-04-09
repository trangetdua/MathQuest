using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class CollidableObject : MonoBehaviour
{

    private Collider2D z_Collider;
    [SerializeField]
    private ContactFilter2D z_Filter;
    private List<Collider2D> z_CollidedObjects = new List<Collider2D>(1);
    

    protected virtual void Start()
    {
        z_Collider = GetComponent<Collider2D>();
    }

    protected virtual void Update()
    {
        z_Collider.Overlap(z_Filter, z_CollidedObjects);
        foreach(var o in z_CollidedObjects)
        {
            // Ignorer les collisions avec Camera Confiner
            if (o.name == "Camera Confiner")
            {
                continue; // Ignore cette collision
            }
            OnCollided(o.gameObject);

        }
    }

    protected virtual void OnCollided(GameObject collidedObject)
    {
        
    }

}
