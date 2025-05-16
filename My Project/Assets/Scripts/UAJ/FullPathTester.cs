using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullPathTester : MonoBehaviour
{
    public static FullPathTester Instance { get; private set; }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartTester()
    {

    }
}
