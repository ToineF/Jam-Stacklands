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

    private MenuState _state;

    private void Update()
    {
        if (_isTweening)
        {
            return;
        }

        switch (_state)
        {
            case MenuState.TITLE_SCREEN:
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    Application.Quit();
                    return;
                }
                
                if (Input.anyKeyDown)
                {
                    _state = MenuState.MENU;
                    MoveStuff(Direction.Left);
                }
                
                break;
            
            case MenuState.MENU:
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    _state = MenuState.TITLE_SCREEN;
                    MoveStuff(Direction.Right);
                }
                
                break;
            
            case MenuState.CREDITS:
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    _state = MenuState.MENU;
                    dynamicSlowDistance /= 4;
                    dynamicFastDistance /= 4;
                    dynamicSlow.DOLocalMoveX(dynamicSlowDistance, dynamicSpeed, true).OnComplete(() => _isTweening = false);
                    dynamicFast.DOLocalMoveX(dynamicFastDistance, dynamicSpeed, true).OnComplete(() => _isTweening = false);
                }
                
                break;
        }
    }

    public void Play()
    {
        
    }

    public void Credits()
    {
        if (_isTweening)
        {
            return;
        }

        _state = MenuState.CREDITS;
        dynamicSlowDistance *= 4;
        dynamicFastDistance *= 4;
        MoveStuff(Direction.Left);
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
        _isTweening = true;
        dynamicSlow.DOLocalMoveX(dynamicSlowDistance * (1 - (int) direction), dynamicSpeed, true).OnComplete(() => _isTweening = false);
        dynamicFast.DOLocalMoveX(dynamicFastDistance * (1 - (int) direction), dynamicSpeed, true).OnComplete(() => _isTweening = false);
        toHide.DOFade((int) direction, dynamicSpeed);
    }

    enum MenuState
    {
        TITLE_SCREEN,
        MENU,
        CREDITS
    }
}
