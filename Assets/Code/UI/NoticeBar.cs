using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoticeBar : MonoBehaviour
{
    private SpriteRenderer sprite;
    private SpriteRenderer parentSprite;
    private EnemyController enemy;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        parentSprite = transform.parent.GetComponent<SpriteRenderer>();
        enemy = GetComponentInParent<EnemyController>();

        enemy.onNoticeClockChange += UpdateBar;
        enemy.onNoticeClockReset += HideBar;
    }

    void Update()
    {
        
    }

    void UpdateBar()
    {
        sprite.enabled = true;
        parentSprite.enabled = true;
        sprite.size = new Vector2(enemy.normalizedNoticeClock, sprite.size.y);
    }

    void HideBar()
    {
        sprite.enabled = false;
        parentSprite.enabled = false;
    }
}
