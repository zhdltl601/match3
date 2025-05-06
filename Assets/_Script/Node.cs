using DG.Tweening;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Node : MonoBehaviour
{
    public EColor ColorType { get; private set; }
    public int X { get; set; }
    public int Y { get; set; }
    private new SpriteRenderer renderer;
    [SerializeField] private Rigidbody2D rigid;

    private Tween currentTween;

    private void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
        Initialization();
    }
    public void Initialization()
    {
        currentTween = InitAnimation();
        rigid.gravityScale = 0;
    }
    public void SetColor(EColor color)
    {
        bool enumFlag = Enum.IsDefined(typeof(EColor), color);
        Debug.Assert(enumFlag, $"enum {color} is not defined");

        renderer.color = color.GetColor();
        ColorType = color;
    }
    private Tween InitAnimation()
    {
        if (currentTween != null)
        {
            currentTween.Complete();
            currentTween.Kill();
            currentTween = null;
        }
        Tween result = transform.DOPunchScale(Vector3.one, 0.2f, 15, 0.5f)
            .SetEase(Ease.Linear)
            .SetLink(gameObject, LinkBehaviour.KillOnDestroy);
        return result;
    }
    private Tween KillAnimation()
    {
        transform.position = Vector3.zero;
        rigid.angularVelocity = 0;
        rigid.linearVelocity = Vector2.zero;

        Tween result = transform.DOShakeScale(0.4f)
            .SetLink(gameObject, LinkBehaviour.KillOnDestroy);
        int dir = Random.Range(-1, 2);
        int xRand = dir;
        int yRand = Random.Range(3, 5);
        Vector2 force = new Vector2(xRand, yRand);
        rigid.AddForce(force, ForceMode2D.Impulse);
        rigid.AddTorque(5 * dir, ForceMode2D.Impulse);
        return result;
    }
}
