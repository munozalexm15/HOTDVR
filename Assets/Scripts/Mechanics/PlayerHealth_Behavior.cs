using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth_Behavior : MonoBehaviour
{
    public int health;
    private int maxHealth;
    private int minHealth;

    public List<Material> hurtMaterials;

    public GameObject leftHand;
    public GameObject rightHand;

    public bool isHurt;

    private bool isRecovering;

    private float duration = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 3;
        minHealth = 3;
        isHurt = false;
        isRecovering = false;
    }

    private void Update()
    {
        if (health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
