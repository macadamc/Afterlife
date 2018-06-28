using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SimpleInputNamespace
{
	public class ButtonInputUI : Selectable, IPointerDownHandler, IPointerUpHandler
	{
		public SimpleInput.ButtonInput button = new SimpleInput.ButtonInput();

		protected override void Awake()
		{
            base.Awake();

			Graphic graphic = GetComponent<Graphic>();
			if( graphic != null )
				graphic.raycastTarget = true;
		}

        protected override void OnEnable()
		{
            base.OnEnable();
			button.StartTracking();
		}

        protected override void OnDisable()
		{
            base.OnDisable();
            button.StopTracking();
		}

        public override void OnPointerDown( PointerEventData eventData )
		{
            base.OnPointerDown(eventData);
			button.value = true;
		}

        public override void OnPointerUp( PointerEventData eventData )
		{
            base.OnPointerUp(eventData);
            button.value = false;
		}
	}
}