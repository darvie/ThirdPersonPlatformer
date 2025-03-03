using UnityEngine;
using UnityEngine.Audio;

public class Coin : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 50f;
    [SerializeField] private float floatSpeed = 0.5f;
    [SerializeField] private float floatHeight = 0.2f;
    [SerializeField] private AudioClip collectSound;
    private AudioSource audioSource;
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            CollectCoin();
        }
    }

    private void CollectCoin()
    {

        if (collectSound != null)
        {
            AudioSource.PlayClipAtPoint(collectSound, transform.position);
        }

        Debug.Log("Coin Collected!");
        Destroy(gameObject); 
    }
}