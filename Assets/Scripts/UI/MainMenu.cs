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

    private void Update()
    {
        if (_isTweening)
        {
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.A))
        {
            _isTweening = true;
            dynamicSlow.DOLocalMoveX(dynamicSlowDistance, dynamicSpeed, true).OnComplete(() => _isTweening = false);
            dynamicFast.DOLocalMoveX(dynamicFastDistance, dynamicSpeed, true).OnComplete(() => _isTweening = false);
            toHide.DOFade(0, dynamicSpeed);
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _isTweening = true;
            dynamicSlow.DOLocalMoveX(0, dynamicSpeed, true).OnComplete(() => _isTweening = false);
            dynamicFast.DOLocalMoveX(0, dynamicSpeed, true).OnComplete(() => _isTweening = false);
            toHide.DOFade(1, dynamicSpeed);
        }
    }
}
