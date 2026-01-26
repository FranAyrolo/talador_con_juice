using UnityEngine;

public class pickupFlotante : MonoBehaviour
{
    [Tooltip("Collider para disparar el pickup")]
    public Collider itemCollider;


    void Start()
    {
        if (!itemCollider)
        {
            itemCollider = GetComponent<Collider>();
            Debug.LogError("Tuve que buscar el collider con getComponent!!\nQue en realidad por ahora todo bien pero si tuviese varios colliders, qué hacemo' ahí eh?");
        }
        itemCollider.isTrigger = true;
    }


    void Update()
    {
        transform.position += Vector3.up * Mathf.Sin(Time.time) * 0.00025f;
        transform.Rotate(0, 50 * Time.deltaTime, 0);
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<ThirdPersonController>()?.ObtainItem(this.gameObject.transform.GetChild(0));
            //other.GetParent().ObtainItem(this.gameObject.transform.GetChild(0));
            Destroy(gameObject); // Pickup example
        }
    }
}
