using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public bool isMoving;
    private Vector2 input;
    private Animator animator;

    public LayerMask interactableLayer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void HandleUpdate()
    {
        if (Input.GetKeyDown(KeyCode.W))        // Z = W en AZERTY
        {
            Interact();
            Debug.Log(">>> INTERACT CALLED <<<");
            return;
        }

        if (!isMoving)
        {
            input = Vector2.zero;

            if (Input.GetKey(KeyCode.LeftArrow)) input.x = -1;
            if (Input.GetKey(KeyCode.RightArrow)) input.x = 1;
            if (Input.GetKey(KeyCode.UpArrow)) input.y = 1;
            if (Input.GetKey(KeyCode.DownArrow)) input.y = -1;

            Debug.Log("Input x " + input.x);
            Debug.Log("Input y " + input.y);

            if (input.x != 0) input.y = 0;

            if (input != Vector2.zero)
            {
                animator.SetFloat("moveX", input.x);
                animator.SetFloat("moveY", input.y);

                Vector3 targetPos = transform.position + new Vector3(input.x, input.y);

                if (IsWalkable(targetPos))
                {
                    StartCoroutine(Move(targetPos));
                }
            }
        }

        animator.SetBool("isMoving", isMoving);
        GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(-transform.position.y * 100);
    }



    void Interact()
    {
        var facingDir = new Vector3(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
        var interactPos = transform.position + facingDir;
        var colliders = Physics2D.OverlapCircleAll(interactPos, 0.5f, interactableLayer);

        foreach (var collider in colliders)
        {
            var monos = collider.GetComponents<MonoBehaviour>();
            foreach (var mono in monos)
            {
                if (mono is Interactable interactable)
                {
                    interactable.Interact();
                    Debug.Log(">>> INTERACT CALLED <<<");
                    return;
                }
            }
        }

        Debug.Log("Trying to interact at: " + interactPos);
    }


    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;
        isMoving = false;
    }

    private bool IsWalkable (Vector3 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, 0.2f,interactableLayer) != null)
        {
            return false;
        }

        return true;
    }
}
