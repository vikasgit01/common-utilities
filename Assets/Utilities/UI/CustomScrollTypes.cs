using GameManagers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace GameUtills
{
    public class CustomScrollTypes : ScrollRect
    {
        [SerializeField] private ScrollType scrollType;
        [SerializeField] private RectTransform indicatorContent;
        [SerializeField] private ScrollAxis axis;
        [SerializeField] private float moveSpeed;

        int totalChildren;
        float values;
        protected override void Start()
        {
            base.Start();
            if (indicatorContent != null)
            {
                onValueChanged.AddListener((vector2) => OnValueChanged(vector2));
                SetUp();
            }
        }

        void SetUp()
        {
            totalChildren = indicatorContent.childCount;
            if (totalChildren > 0) values = 1 / totalChildren;
            SetStartingIndicator();
        }

        void SetStartingIndicator()
        {
            if (totalChildren > 0 && content != null)
            {
                if (UIUtills.IsRightPivot(content) || UIUtills.IsBottomPivot(content))
                    indicatorContent.GetChild(totalChildren - 1).GetComponent<IScrollIndicatorSelect>().Selected();

                if (UIUtills.IsLeftPivot(content) || UIUtills.IsTopPivot(content))
                    indicatorContent.GetChild(0).GetComponent<IScrollIndicatorSelect>().Selected();

                if (UIUtills.IsCenterPivot(content))
                    indicatorContent.GetChild((totalChildren - 1) / 2).GetComponent<IScrollIndicatorSelect>().Selected();
            }
        }

        int currentSelected = 0;
        void OnValueChanged(Vector2 vector2)
        {
            if (CanShowIndicator())
            {
                if ((horizontal && !vertical) || axis == ScrollAxis.X) SetSelected(vector2.x, false);
                if ((vertical && !horizontal) || axis == ScrollAxis.Y) SetSelected(vector2.y, true);
            }
        }

        bool CanShowIndicator() => (scrollType == ScrollType.INDICATORCLICK && !isMoveToIndex) || scrollType == ScrollType.INDICATOR || scrollType == ScrollType.TRAVERSEINDICATOR;

        void SetSelected(float value, bool isVerticle)
        {
            currentSelected = (int)(value * totalChildren);

            if (currentSelected < 0) currentSelected = 0;

            if (isVerticle) currentSelected = ((totalChildren - 1) - currentSelected);

            if (currentSelected > (totalChildren - 1)) currentSelected = totalChildren - 1;

            if (currentSelected < totalChildren && currentSelected >= 0)
            {
                indicatorContent.GetChild(currentSelected).GetComponent<IScrollIndicatorSelect>().Selected();
                if (currentSelected - 1 >= 0) indicatorContent.GetChild(currentSelected - 1).GetComponent<IScrollIndicatorSelect>().NotSelected();
                if (currentSelected + 1 < totalChildren) indicatorContent.GetChild(currentSelected + 1).GetComponent<IScrollIndicatorSelect>().NotSelected();
            }
        }

        void Move(int index, float itemWidth, int direction, float currentPos, bool scrollX)
        {
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
                moveCoroutine = null;
            }

            currentPosition = new Vector2(content.anchoredPosition.x, content.anchoredPosition.y);

            if (index > previousIndex)
            {
                previousIndex = index;
                destinationPoint = index * (itemWidth);
            }
            else if (index < previousIndex)
            {
                previousIndex = index;
                destinationPoint = index * (itemWidth);
            }

            if (canCallScrollLimitReached) scrollLimitReached?.Invoke();

            moveCoroutine = SmoothScrolling(currentPos, destinationPoint * direction, 1f, scrollX);
            StartCoroutine(moveCoroutine);
        }

        Vector2 currentPosition;
        IEnumerator SmoothScrolling(float currentPosition, float destinationPosition, float totalTime, bool scrollX)
        {
            float time = 0f;
            float normalizedTime = 0f;
            isScrolling = true;
            totalTime = Mathf.Abs(destinationPosition - currentPosition) * totalTime;
            while (time <= totalTime)
            {
                normalizedTime = time / totalTime;

                if (scrollX) this.currentPosition.x = Mathf.Lerp(currentPosition, destinationPosition, normalizedTime * moveSpeed);
                else this.currentPosition.y = Mathf.Lerp(currentPosition, destinationPosition, normalizedTime * moveSpeed);

                content.anchoredPosition = this.currentPosition;
                time += totalTime * Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }


            isScrolling = false;
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
                moveCoroutine = null;
            }
        }

        float destinationPoint;
        IEnumerator moveCoroutine = null;
        float previousIndex = 0;
        int index = 0;
        bool isScrolling;
        bool canCallScrollLimitReached;
        Action scrollLimitReached;
        public void Scroll(ScrollDirection direction,Action<int> scrollingToIndex = null, Action scrollLimitReached = null)
        {
            this.scrollLimitReached = scrollLimitReached;
            if (scrollType != ScrollType.TRAVERSE && scrollType != ScrollType.TRAVERSEINDICATOR) return;

            if (isScrolling) return;

            if (axis == ScrollAxis.Y && axis == ScrollAxis.X) return;

            float itemSize = 0f;
            float spacing = 0f;
            float currentPos = axis == ScrollAxis.X ? content.anchoredPosition.x : content.anchoredPosition.y;

            itemSize = GetItemSize();

            index = Mathf.RoundToInt(Mathf.Abs(currentPos / (itemSize)));

            if (direction.positive)
            {
                index++;
                if ((index > content.childCount - 1)) index = content.childCount - 1;
            }
            if (direction.negative)
            {
                index--;
                if ((index < 0)) index = 0;
            }

            int moveDir = (UIUtills.IsRightPivot(content) || UIUtills.IsTopPivot(content)) ? 1 : -1;

            bool canMove = !(index == 0 && previousIndex == 0 && direction.negative) && !(index == content.childCount - 1 && previousIndex == content.childCount - 1 && direction.positive);
            LogsManager.Print($"moveDir : {moveDir}, currentPos : {currentPos}, index : {index}, previousIndex : {previousIndex}, canMove : {canMove}");

            canCallScrollLimitReached = index == 0 || index == content.childCount - 1; 

            if (canMove)
            {
                scrollingToIndex?.Invoke(index);
                Move(index, itemSize, moveDir, currentPos, axis == ScrollAxis.X);
            }
            else
            {
                scrollLimitReached?.Invoke();
            }
        }

        bool isMoveToIndex;
        public void MoveToIndex(int index)
        {
            if (scrollType != ScrollType.INDICATORCLICK || index >= totalChildren) return;

            isMoveToIndex = true;
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
                moveCoroutine = null;
            }

            float itemSize = GetItemSize();

            float destinationPosition = index * itemSize;
            float currentPos = axis == ScrollAxis.X ? content.anchoredPosition.x : content.anchoredPosition.y;
            int currentIndex = Mathf.RoundToInt(Mathf.Abs(currentPos / (itemSize)));
            LogsManager.Print($"currentIndex : {currentIndex}, currentPos : {currentPos}");
            //currentIndex = (UIUtills.IsLeftPivot(content) || UIUtills.IsTopPivot(content)) ? currentIndex : (totalChildren - 1) - currentIndex;
            LogsManager.Print($"index : {index}, currentIndex : {currentIndex}, totalChildren : {totalChildren - 1}");

            if (index == currentIndex) return;

            if (indicatorContent)
            {
                LogsManager.Print($"MoveIndex : {(UIUtills.IsRightPivot(content) || UIUtills.IsTopPivot(content))}");

                int previousIndex = (UIUtills.IsLeftPivot(content) || UIUtills.IsTopPivot(content)) ? currentIndex : (totalChildren - 1) - currentIndex;
                if (indicatorContent.GetChild(previousIndex)) indicatorContent.GetChild(previousIndex).GetComponent<IScrollIndicatorSelect>().NotSelected();
                LogsManager.Print($"previousIndex : {previousIndex}");

                int moveToIndex = (UIUtills.IsLeftPivot(content) || UIUtills.IsTopPivot(content)) ? index : (totalChildren - 1) - index;
                if (indicatorContent.GetChild(moveToIndex)) indicatorContent.GetChild(moveToIndex).GetComponent<IScrollIndicatorSelect>().Selected();
                LogsManager.Print($"MoveIndex : {moveToIndex}");
            }

            int moveDir = (UIUtills.IsRightPivot(content) || UIUtills.IsTopPivot(content)) ? 1 : -1;
            LogsManager.Print($" itemSize : {itemSize}, destinationPosition : {destinationPosition * moveDir}, currentPos : {currentPos}");

            moveCoroutine = SmoothScrolling(currentPos, destinationPosition * moveDir, 1f, axis == ScrollAxis.X);
            StartCoroutine(moveCoroutine);
        }

        float GetItemSize()
        {
            float itemSize = 0;
            float spacing = 0;
            if (content.childCount > 0 && content.GetChild(0) != null)
            {
                if (content.GetChild(0).TryGetComponent(out RectTransform rectTransform))
                    itemSize = axis == ScrollAxis.X ? rectTransform.rect.width : axis == ScrollAxis.Y ? rectTransform.rect.height : 0f;

                if (content.TryGetComponent(out HorizontalLayoutGroup horizontalLayoutGroup))
                    spacing = horizontalLayoutGroup.spacing;

                if (content.TryGetComponent(out VerticalLayoutGroup verticalLayoutGroup))
                    spacing = verticalLayoutGroup.spacing;

                itemSize += spacing;

            }

            return itemSize;
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);
            isMoveToIndex = false;
            isScrolling = false;
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
                moveCoroutine = null;
            }
        }
    }

    interface IScrollIndicatorSelect
    {
        public void Selected();
        public void NotSelected();
    }

    public class ScrollDirection
    {
        public bool positive;
        public bool negative;

        public ScrollDirection(bool positive, bool negative)
        {
            this.positive = positive;
            this.negative = negative;
        }
    }

    public enum ScrollType
    {
        NONE,
        TRAVERSE,
        INDICATOR,
        TRAVERSEINDICATOR,
        INDICATORCLICK
    }

    enum ScrollAxis
    {
        X,
        Y
    }
}  