using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInitializer : MonoBehaviour
{
    [SerializeField]
    string levelID;
    // Start is called before the first frame update
    void Start() {
        ConversationManager.Instance.SetCurrentLevel(levelID);
        Destroy(gameObject);
    }
}
