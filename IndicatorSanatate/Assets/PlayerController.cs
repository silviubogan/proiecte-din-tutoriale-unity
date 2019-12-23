using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float coolDown;
    public int maxHealth;
    public RectTransform healthTransform;
    public Text healthText;
    public Image visualHealth;

    float cachedY, minXValue, maxXValue;
    bool onCD;
    int currentHealth;

    public int CurrentHealth
    {
        get
        {
            return currentHealth;
        }
        set
        {
            if (value != currentHealth)
            {
                currentHealth = value;
                HandleHealthChange();
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        cachedY = healthTransform.position.y;
        maxXValue = healthTransform.position.x;
        minXValue = maxXValue - healthTransform.rect.width;

        onCD = false;
        CurrentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovementChange();
    }

    float MapValues(float x, float inMin, float inMax, float outMin, float outMax)
    {
        return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }

    void HandleHealthChange()
    {
        healthText.text = CurrentHealth.ToString() + "%";

        float v = MapValues(CurrentHealth, 0, maxHealth, minXValue, maxXValue);
        healthTransform.position = new Vector3(v, cachedY);

        if (CurrentHealth > maxHealth / 2)
        {
            visualHealth.color = new Color32((byte)MapValues(CurrentHealth, maxHealth / 2, maxHealth, 255, 0),
                255,
                0,
                255);
        }
        else
        {
            visualHealth.color = new Color32(255,
                (byte)MapValues(CurrentHealth, 0, maxHealth / 2, 0, 255),
                0,
                255);
        }
    }

    IEnumerator DoCoolDown()
    {
        onCD = true;
        yield return new WaitForSeconds(coolDown);
        onCD = false;
    }

    void HandleMovementChange()
    {
        float translation = speed * Time.deltaTime;

        transform.Translate(new Vector3(
            Input.GetAxis("Horizontal") * translation,
            0,
            Input.GetAxis("Vertical") * translation));
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "HealthSystem")
        {
            if (!onCD && CurrentHealth < maxHealth)
            {
                StartCoroutine(DoCoolDown());
                ++CurrentHealth;
            }
        }
        else if (other.gameObject.name == "DamageSystem")
        {
            if (!onCD && CurrentHealth > 0)
            {
                StartCoroutine(DoCoolDown());
                --CurrentHealth;
            }
        }
    }
}
