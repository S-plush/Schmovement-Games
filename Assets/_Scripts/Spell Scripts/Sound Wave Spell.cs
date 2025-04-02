using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundWaveSpell : MonoBehaviour
{
    public Spell spell;
    public Rigidbody rb;

    [HideInInspector] public Alpha alpha;

    private int bounce = 3;
    private Vector3 aimingDirection;

    private void Awake()
    {
        alpha = FindObjectOfType<Alpha>();
    }

    public void Aiming(Vector3 direction)
    {
        aimingDirection = direction;
        rb.velocity = new Vector3(aimingDirection.x, aimingDirection.y, 0) * 20f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("starting bounce " + bounce);
        bounce--;
        spell.damageValue++;
        spell.knockbackValue = spell.knockbackValue + 5;

        if (bounce < 0)
        {
            Destroy(gameObject);
            spell.damageValue = 1;
            spell.knockbackValue = 15;
            return;
        }

        var contact = collision.contacts[0];
        Vector3 newVelocity = Vector3.Reflect(aimingDirection.normalized, contact.normal);
        Aiming(newVelocity);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {

        }
        else if (other.gameObject.tag == "Player")
        {
            if (bounce < 3)
            {
                Destroy(gameObject, 0.2f);

                if (alpha != null)
                {
                    Debug.Log("alpha is not null");
                    Vector3 pushDirection = aimingDirection.normalized;
                    float distanceToPlayer = Vector3.Distance(transform.position, alpha.transform.position);

                    if (alpha.TryGetComponent<CharacterController>(out CharacterController controller))
                    {
                        Vector3 push = pushDirection * spell.knockbackValue;
                        StartCoroutine(Push(controller, push));
                    }
                }

                spell.knockbackValue = 15;
            }
        }
    }

    private IEnumerator Push(CharacterController controller, Vector3 pushDirection)
    {
        Debug.Log("i'm in!");
        float elapsedTime = 0f;
        float pushDuration = .2f;
        Vector3 startingPosition = alpha.transform.position;

        while (elapsedTime < pushDuration)
        {
            controller.Move(Vector3.Lerp(Vector3.zero, pushDirection, elapsedTime / pushDuration) * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
