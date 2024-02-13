using UnityEngine;

public class MoveBetweenPoints : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private Transform limit1, limit2;
    [SerializeField] private bool movingRight = true;
    private Vector3 pos1, pos2;
    private void Start()
    {
        pos1 = limit1.position;
        pos2 = limit2.position;
    }
    void Update()
    {
        // MOVER EL OBJETO
        MoveObject();

        // COMPROBAR DIRECCION
        CheckLimits();
    }
    void MoveObject()
    {
        float dir = movingRight ? 1f : -1f;
        float movement = dir * speed * Time.deltaTime;
        transform.Translate(new Vector3(movement, 0f, 0f));
    }

    void CheckLimits()
    {
        if (movingRight && transform.position.x >= pos2.x)
        {
            movingRight = false;
        }
        else if (!movingRight && transform.position.x <= pos1.x)
        {
            movingRight = true;
        }
    }
}
