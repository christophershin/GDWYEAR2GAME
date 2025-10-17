using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Ordering : MonoBehaviour
{
    [SerializeField] private Image front, middle, back, card1, card2, card3;
    private RectTransform _rFront, _rMiddle, _rBack,  _rCard1, _rCard2, _rCard3;
    private string _nameCard1, _nameCard2, _nameCard3;

    private int _state = 1;
    private string _card;

    public string GetCard()
    {
        return _card;
    }

    public void AddCard(string card)
    {
        
    }

    public void UseCard()
    {
        
    }
    
    void Start()
    {
        // Assigning Names
        _nameCard1 = "null";
        _nameCard2 = "null";
        _nameCard3 = "null";
        
        // Transforms
        _rFront = front.rectTransform;
        _rMiddle = middle.rectTransform;
        _rBack = back.rectTransform;
        _rCard1 = card1.rectTransform;
        _rCard2 = card2.rectTransform;
        _rCard3 = card3.rectTransform;
        
        // Assigning Positions
        _rCard1.anchoredPosition = new Vector2(_rFront.anchoredPosition.x, _rFront.anchoredPosition.y);
        _rCard2.anchoredPosition = new Vector2(_rMiddle.anchoredPosition.x, _rMiddle.anchoredPosition.y);
        _rCard3.anchoredPosition = new Vector2(_rBack.anchoredPosition.x, _rBack.anchoredPosition.y);
        
        // Assigning Rotation
        _rCard1.localRotation = _rFront.localRotation;
        _rCard2.localRotation = _rMiddle.localRotation;
        _rCard3.localRotation = _rBack.localRotation;
        
        // Assigning colors
        card1.color = front.color;
        card2.color = middle.color;
        card3.color = back.color;
    }

    void Update()
    {
        // Card inputs
        float scroll = Input.mouseScrollDelta.y;

        if (scroll != 0f)
            (scroll > 0f ? (System.Action)SwapRight : SwapLeft)();

        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E))
            (Input.GetKeyDown(KeyCode.Q) ? (System.Action)SwapLeft : SwapRight)();
        
        // Card values
        RectTransform[] cards = { _rCard1, _rCard2, _rCard3 };
        RectTransform[] targets = { _rFront, _rMiddle, _rBack };
        float speed = 10f;
        
        // Lerp cards
        for (int i = 0; i < cards.Length; i++)
        {
            cards[i].anchoredPosition = Vector2.Lerp(cards[i].anchoredPosition, targets[i].anchoredPosition, Time.deltaTime * speed);
            cards[i].localRotation = Quaternion.Lerp(cards[i].localRotation, targets[i].localRotation, Time.deltaTime * speed);
        }

    }

    public void SwapLeft()
    {
        // Switch values
        RectTransform temp = _rFront;
        _rFront = _rBack;
        _rBack = _rMiddle;
        _rMiddle = temp;
        
        _state += 1;
        if (_state > 3)
        {
            _state = 1;
        }
        
        SetState();
    }

    private void SwapRight()
    {
        // Switch valyues
        RectTransform temp = _rFront;
        _rFront = _rMiddle;
        _rMiddle = _rBack;
        _rBack = temp;

        _state -= 1;
        if (_state < 1)
        {
            _state = 3;
        }
        
        SetState();
    }

    private void SetState()
    {
        switch (_state)
        {
            case 1:
                _rCard3.SetSiblingIndex(1); 
                _rCard2.SetSiblingIndex(2); 
                _rCard1.SetSiblingIndex(3); 
                break;
            case 2:
                _rCard3.SetSiblingIndex(2); 
                _rCard2.SetSiblingIndex(3); 
                _rCard1.SetSiblingIndex(1); 
                break;
            case 3:
                _rCard3.SetSiblingIndex(3); 
                _rCard2.SetSiblingIndex(1); 
                _rCard1.SetSiblingIndex(2); 
                break;
        }
    }
}
