using UnityEngine;
using TMPro;

public class DamagePopupAnimation : MonoBehaviour
{
    public float moveUpSpeed = 30f;
    public float lifetime = 1.0f;
    public float fadeDuration = 0.5f;

    private TextMeshProUGUI text;
    private float timer = 0f;
    private Color originalColor;

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        originalColor = text.color;
    }

    void Update()
    {
        timer += Time.deltaTime;

        // Monter verticalement
        transform.position += Vector3.up * moveUpSpeed * Time.deltaTime;

        // Fondu (sur la fin de vie)
        if (timer > lifetime - fadeDuration)
        {
            float fadeAmount = 1 - ((timer - (lifetime - fadeDuration)) / fadeDuration);
            text.color = new Color(originalColor.r, originalColor.g, originalColor.b, fadeAmount);
        }

        // Détruire après la durée de vie
        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }
}