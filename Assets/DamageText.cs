using UnityEngine;

public class DamageText : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float fadeSpeed = 1f;

    private TextMesh damageTextMesh;
    private Color textColor;

    void Start()
    {
        damageTextMesh = GetComponent<TextMesh>();
        textColor = damageTextMesh.color;
    }

    void Update()
    {
        // Move the damage text upwards
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

        // Fade out the damage text
        textColor.a -= fadeSpeed * Time.deltaTime;
        damageTextMesh.color = textColor;

        // Destroy the damage text when it's fully transparent
        if (textColor.a <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void SetDamageText(int damageAmount)
    {
        // Set the damage amount as text
        damageTextMesh.text = "-" + damageAmount.ToString();
    }
}
