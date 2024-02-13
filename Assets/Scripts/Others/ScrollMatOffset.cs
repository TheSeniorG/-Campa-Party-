using UnityEngine;

public class ScrollMatOffset : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 0.5f;
    private MeshRenderer mRenderer;
    private void Start()
    {
        //OBTENER RENDERER
        mRenderer = GetComponent<MeshRenderer>();
    }
    void Update()
    {
        //CALCULAR NUEVO TILING
        float offset = Time.time * scrollSpeed;

        //CREAR VECTOR NUEVO
        Vector2 newOffset = new Vector2(offset, 0);

        //APLCIAR OFFSET
        mRenderer.material.mainTextureOffset = newOffset;
    }
}

