using UnityEngine;
using UnityEngine.UI;

public class ScrollListAutoScroller : MonoBehaviour
{
    public ScrollRect scrollRect;
    public RectTransform contentRectTransform;
    public float scrollSpeed = 10f;
    public float delayBetweenItems = 1f;

    private int currentIndex = 0;
    private int numItems;
    private Vector2 lastContentPosition;
    public GameObject testCard;

    void Start()
    {

        for (int j = 0; j < 44; j++)
        {
            GameObject simDatesChildren = Instantiate(testCard);
            simDatesChildren.transform.SetParent(contentRectTransform.transform, false);
        }

            // Get the number of items in the scroll list
            numItems = contentRectTransform.childCount;
        // Set the lastContentPosition to the current content position
        lastContentPosition = scrollRect.content.anchoredPosition;
    }

    void Update()
    {
        // If the current index is less than the number of items in the scroll list
        if (currentIndex < numItems)
        {
            // Get the position of the current item
            RectTransform currentRectTransform = contentRectTransform.GetChild(currentIndex).GetComponent<RectTransform>();

            // Calculate the target position for scrolling
            Vector2 targetPosition = new Vector2(currentRectTransform.anchoredPosition.x, 0f);

            // If the scroll view is not scrolling
            if (scrollRect.content.anchoredPosition == lastContentPosition || scrollRect.velocity == Vector2.zero)
            {
                // If the current position is not equal to the target position, scroll towards the target position
                if (scrollRect.content.anchoredPosition != targetPosition)
                {
                    scrollRect.content.anchoredPosition = Vector2.Lerp(scrollRect.content.anchoredPosition, targetPosition, scrollSpeed * Time.deltaTime);
                }
                // If the current position is equal to the target position, move to the next item
                else
                {
                    currentIndex++;
                    // Delay before scrolling to the next item
                    Invoke("ScrollToNextItem", delayBetweenItems);
                }
            }
            // Update the lastContentPosition
            lastContentPosition = scrollRect.content.anchoredPosition;
        }
    }

    // Scrolls to the next item in the scroll list
    void ScrollToNextItem()
    {
        // Get the position of the next item
        RectTransform nextRectTransform = contentRectTransform.GetChild(currentIndex).GetComponent<RectTransform>();

        // Calculate the target position for scrolling
        Vector2 targetPosition = new Vector2(nextRectTransform.anchoredPosition.x, 0f);

        // Scroll towards the target position
        scrollRect.content.anchoredPosition = targetPosition;
    }
}



