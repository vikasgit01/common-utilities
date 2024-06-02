using GameManagers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameUtills
{
    public class ButtonAnimation : Button
    {
        [SerializeField] private bool enableAnimation = true;
        [SerializeField] private Transform animateGraphics;
        [SerializeField] private Vector3 scaleSize = new Vector3(.8f, .8f, .8f);
        [SerializeField] private string buttonEventName = "None";

        private Vector3 actualScaleSize;
        protected override void Awake()
        {
            base.Awake();

            if (animateGraphics == null) animateGraphics = transform;

            if (animateGraphics && enableAnimation)
                actualScaleSize = animateGraphics.transform.localScale;
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);

            if (IsInteractable())
                LogsManager.Print("Button Event -> " + buttonEventName);
        }

        public override void OnPointerDown(PointerEventData pointerEventData)
        {
            if (animateGraphics && enableAnimation && IsInteractable())
            {
                animateGraphics.transform.localScale = scaleSize;
            }
        }

        public override void OnPointerUp(PointerEventData pointerEventData)
        {
            if (animateGraphics && enableAnimation && IsInteractable())
            {
                animateGraphics.transform.localScale = actualScaleSize;
            }
        }

        public void SetUpButtonEvent(string eventName) => buttonEventName = eventName;
    }

}