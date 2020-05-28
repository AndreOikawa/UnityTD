using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerVariables : MonoBehaviour
{
    public enum ClickType {
        SHOOT,
        BUILD
    };
    public ClickType CurrentMode = ClickType.SHOOT;
    public int LivesLeft = 20;
    [SerializeField]
    private Texture ShootTexture;
    [SerializeField]
    private Texture BuildTexture;
    
    private Dictionary<ClickType, Texture> Textures;
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        UpdateLives(0);
        Textures = new Dictionary<ClickType, Texture>();
        Textures.Add(ClickType.SHOOT, ShootTexture);
        Textures.Add(ClickType.BUILD, BuildTexture);
    }

    public void UpdateLives(int removeLives) {
        LivesLeft -= removeLives;
        // gameObject.GetComponentInChildren<Camera>().GetComponentInChildren<Canvas>().GetComponentInChildren<Text;
        GameObject.Find("Lives").GetComponent<Text>().text = LivesLeft.ToString();
        if (LivesLeft <= 0) {
            // end game
            GameObject.Find("Lives").GetComponent<Text>().text = "GAME OVER";

        }
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        var prev = CurrentMode;
        if (Input.GetAxis("Mouse ScrollWheel") < 0f) CurrentMode = ClickType.SHOOT;
        else if (Input.GetAxis("Mouse ScrollWheel") > 0f) CurrentMode = ClickType.BUILD;
        if (prev != CurrentMode) {
            Debug.Log(CurrentMode);


            GameObject.Find("Arm").GetComponent<Renderer>().material.mainTexture = Textures[CurrentMode];
            
        }
            
    }
}
