using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionSpell : MonoBehaviour
{
    public Spell spell;
    public Alpha alpha;

    public float radius = 5f;
    public bool pushed = false;
    public bool preventMoving = false;
    public bool pushedRight = false;
    public bool pushedLeft = false;

    private void Awake()
    {
        alpha = FindObjectOfType<Alpha>();

        if(alpha == null)
        {
            Debug.LogError("Alpha object not in scene");
        }
    }

    private void Start()
    {
        Destroy(gameObject, 0.5f);
    }

    public void Aiming(Vector3 direction)
    {
        if (alpha != null)
        {
            Debug.Log("alpha is not null");
            Vector3 pushDirection = -direction.normalized;
            float distanceToPlayer = Vector3.Distance(transform.position, alpha.transform.position);

            if (alpha.TryGetComponent<CharacterController>(out CharacterController controller))
            {
                Vector3 push = pushDirection * spell.knockbackValue;
                StartCoroutine(Push(controller, push));
            }
        }
        else if (alpha == null)
        {
            Debug.Log("alpha is null");
        }
    }

    private IEnumerator Push(CharacterController controller, Vector3 pushDirection)
    {
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