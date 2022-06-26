using UnityEngine;
public class pause : MonoBehaviour
{
    bool pauseButton;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (pauseButton == true) Time.timeScale = 0f;
        else if (pauseButton == false) Time.timeScale = 1f;
    }

    public void PauseButton()
    {
        pauseButton = !pauseButton;
    }
}
