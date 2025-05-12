using Custom.Audio;
using DG.Tweening;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Node : MonoBehaviour
{
    public EColor ColorType { get; private set; }
    [field: SerializeField] public int X { get; private set; }
    [field: SerializeField] public int Y { get; private set; }
    public bool IsSelected { get; private set; }

    [Header("Settings")]
    [SerializeField] private BaseAudioSO aud_init;
    //refs
    [SerializeField] private Rigidbody2D rigid;
    [SerializeField] private GameObject selectOutlineGO;

    private new SpriteRenderer renderer;
    private Tween currentTween;

    private void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
        rigid.gravityScale = 0;
    }
    public void NodeActive(bool active)
    {
        IsSelected = active;
        selectOutlineGO.SetActive(active);
    }
    public void Initialization()
    {
        AudioManager.PlayWithInit(aud_init, true);
        currentTween = InitAnimation();
    }
    //public void MovePosition(EDirection eDirection)
    //{
    //    Vector2 vector = eDirection.GetVector();
    //    int x = (int)vector.x;
    //    int y = (int)vector.y;
    //    MovePosition(x, y);
    //}
    public void MovePosition(int xDir, int yDir)
    {
        int resultX = X + xDir;
        int resultY = Y + yDir;
        Debug.Assert(NodeManager.IsNodePositionValid(resultX, resultY), "node is not valid");

        Vector3 result = new Vector3(xDir, yDir) + transform.position;
        SetPosition(resultX, resultY, result);
    }
    public void SetPosition(int x, int y, Vector3 position)
    {
        transform.position = position;
        X = x;
        Y = y;
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
        Tween result = transform.DOPunchScale(Vector3.one, 0.2f, 20, 0.25f)
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
