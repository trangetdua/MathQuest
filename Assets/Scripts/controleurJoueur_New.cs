using System.Collections;
using UnityEngine;

public class controleurJoueur : MonoBehaviour
{
    public float moveSpeed;
    public bool isMoving;
    private Vector2 input;
    private Animator animator;

    public GameObject panelQuestion;



    private void Awake()
    {
        animator = GetComponent<Animator>();
           // Initialiser le panel au lancement
        if (panelQuestion != null)
        {
            panelQuestion.SetActive(false); //  cache le panel au début
        }
        else
        {
            Debug.LogWarning("PanelQuestion n’est pas assigné dans l’Inspector !");
        }
    }

    private void Update()
    {
        if (!isMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");
            Debug.Log("Input x " + input.x);
            Debug.Log("Input y " + input.y);


            if(input.x != 0) input.y = 0;

            if(input != Vector2.zero)
            {

                animator.SetFloat("moveX", input.x);
                animator.SetFloat("moveY", input.y);

                var targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;

                StartCoroutine(Move(targetPos));
            }
        }
        animator.SetBool("isMoving", isMoving);
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

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("collision ok");

        //  Active le canvas quand le joueur entre en collision
        if (panelQuestion != null)
        {
            panelQuestion.SetActive(true);
        }
    }
    
}
