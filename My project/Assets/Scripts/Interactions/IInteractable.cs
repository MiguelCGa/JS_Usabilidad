using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]

public abstract class IInteractable : MonoBehaviour
{
    Transform trans;
    SpriteRenderer sprite;
    // Start is called before the first frame update
    void Start() {
        trans = transform;
        sprite = GetComponent<SpriteRenderer>();
        InputReader.Instance.onUse += ClickCheck;
    }

    bool IsInside(InputReader.MousePos pos) {
        float left = trans.position.x - sprite.size.x * trans.localScale.x / 2f;
        float top = trans.position.y - sprite.size.y * trans.localScale.y / 2f;
        float right = trans.position.x + sprite.size.x * trans.localScale.x / 2f;
        float bottom = trans.position.y + sprite.size.y * trans.localScale.y / 2f;
        return pos.x > left && pos.x < right && pos.y > top && pos.y < bottom;
    }

    public abstract void OnClick();

    // Update is called once per frame
    void ClickCheck() {
        if (IsInside(InputReader.Instance.mousePos)) {
            OnClick();
        }
    }
}
