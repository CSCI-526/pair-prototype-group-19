using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Player player;
    // Start is called before the first frame update

    private Image bar;

    void Start()
    {
        bar = this.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        bar.fillAmount = (float) player.getHealth()/100;
    }
}
