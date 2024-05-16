using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private Transform dynamicSlow;
    [SerializeField]
    private Transform dynamicFast;
    [SerializeField]
    private Image toHide;

    [SerializeField]
    private float dynamicSlowDistance;
    [SerializeField]
    private float dynamicFastDistance;
    
    [SerializeField]
    private float dynamicSpeed;

    private bool _isTweening;
    private bool _hasPressedAnyKey;

    private void Update()
    {
        if (_isTweening)
        {
            return;
        }

        if (_hasPressedAnyKey)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                MoveStuff(Direction.Right);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
                return;
            }

            if (Input.anyKeyDown)
            {
                MoveStuff(Direction.Left);
            }
        }
    }

    public void Play()
    {
        
    }

    public void Credits()
    {
        
    }

    public void Quit()
    {
        Application.Quit();
    }

    enum Direction
    {
        Left,
        Right,
    }

    private void MoveStuff(Direction direction)
    {
        _hasPressedAnyKey = direction == Direction.Left;
        _isTweening = true;
        dynamicSlow.DOLocalMoveX(dynamicSlowDistance * (1 - (int) direction), dynamicSpeed, true).OnComplete(() => _isTweening = false);
        dynamicFast.DOLocalMoveX(dynamicFastDistance * (1 - (int) direction), dynamicSpeed, true).OnComplete(() => _isTweening = false);
        toHide.DOFade((int) direction, dynamicSpeed);
    }
}
