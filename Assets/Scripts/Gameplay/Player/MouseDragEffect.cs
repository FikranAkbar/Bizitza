using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDragEffect : MonoBehaviour
{

    public ParticleSystem drawEffect;

    private void OnMouseDrag()
    {
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
        Vector3 objPos = Camera.main.ScreenToWorldPoint(mousePos);

        transform.position = objPos;
    }

    private void OnMouseDown()
    {
        drawEffect.Play();
    }

    private void OnMouseUp()
    {
        drawEffect.Stop();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
