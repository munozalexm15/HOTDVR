using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth_Behavior : MonoBehaviour
{
    public int health;

    public List<Material> hurtMaterials;

    public GameObject leftHand;
    public GameObject rightHand;
    public TMP_Text hud_health;

    public bool isHurt;

    // Start is called before the first frame update
    void Start()
    {
        isHurt = false;
  
    }

    private void Update()
    {
        if (health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
