using UnityEngine;

public class SnowflakeScript : MonoBehaviour
{
    private LogicScript logicScript;
    public int score = 10;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //logicScript = GameObject.FindGameObjectWithTag("LogicScore").GetComponent<LogicScript>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        logicScript.addScore(score);
    }
}
