using UnityEngine;

public class TeleportRefresh : MonoBehaviour
{
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        //Physics.IgnoreCollision(player.GetComponent<Collider>(), GetComponent<Collider>());
    }

    private void OnTriggerEnter(Collider collision)
    {
        // check if you hit an enemy
        if (collision.gameObject.GetComponent<PlayerMovement>() != null)
        {
            PlayerMovement playerScript = collision.gameObject.GetComponent<PlayerMovement>();
            
            playerScript.numberOfTeleports = 3;
            gameObject.SetActive(false);
        }
    }
    private void OnDisable()
    {
        Invoke("Respawn", 2f);
    }

    void Respawn()
    {
        gameObject.SetActive(true);
    }
}
