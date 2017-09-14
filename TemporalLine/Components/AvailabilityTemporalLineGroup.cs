using System;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace TemporalLine.Components
{
    /// <summary>
    /// This component is a ViewGroup that contains the time line and the selected area.
    /// 
    /// The timeline is a view that is shown in a green and red color (green color indicates that the device is free and
    /// red color indicates that the device is busy).
    /// 
    /// The selected area is a view that indicates the time region that the user wants to book the device.
    /// </summary>
    [Register("temporalLine.components.availabilityTemporalLineGroup")]
    public class AvailabilityTemporalLineGroup : FrameLayout
	{
        /// <summary>
        /// This event returns the final position of the selected area when we finish of sliding the selected area.
        /// </summary>
        public event EventHandler<TemporalLinePoint> positionEvent;

        /// <summary>
        /// Width and height of this ViewGroup (dp).
        /// </summary>
		private int mWidth, mHeight;

        /// <summary>
        /// When we touch the selected area, we are touching in a (x, y) coordinates of the screen. The initial coordinates are "origX" and "origY".
        /// </summary>
        private float origX, origY;

        private Color mSelectedColor;

        // CONSTRUCTOR

        public AvailabilityTemporalLineGroup(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            this.Init(attrs);
        }

        public AvailabilityTemporalLineGroup(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
		{
			this.Init(attrs);
        }

		public AvailabilityTemporalLineGroup(Context context, IAttributeSet attrs) : base(context, attrs)
		{
			this.Init(attrs);
        }

		public AvailabilityTemporalLineGroup(Context context) : base(context)
		{
			this.Init(null);
		}

        private void Init(IAttributeSet attrs)
		{
			if (this.Context == null || attrs == null)
			{
				return;
			}

			var a = this.Context.ObtainStyledAttributes(attrs, Resource.Styleable.TemporalLineColor);

            this.mSelectedColor = a.GetColor(Resource.Styleable.TemporalLineColor_temopral_line_color_selected, Color.Orange);

			a.Recycle();
        }

        // ====

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
		{
            TemporalLineHelper.MeasureView(widthMeasureSpec, heightMeasureSpec, out this.mWidth, out this.mHeight);
            base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
		}

        /// <summary>
        /// This method places a selected area over the temporal line,
        /// </summary>
        /// <param name="selectedArea">The percentage star point and the percentage end point of the time line.</param>
        public void AddSelectedTemporalLine(TemporalLinePoint selectedArea)
        {
            if (selectedArea != null)
            {
                // We calculate the width of the selected area (dp).
                int width = (int)((selectedArea.TemporalLineEnd - selectedArea.TemporalLineStart) * this.mWidth);

                // We create and place the selected area view over the timeline in the position where it must be, and with the
                // width that it must have.
                SelectedTemporalLine selectedLine = new SelectedTemporalLine(this.Context);
                selectedLine.SelectedColor = this.mSelectedColor;
                selectedLine.SetX(this.mWidth * selectedArea.TemporalLineStart);
                this.AddView(selectedLine, new ViewGroup.LayoutParams(width, ViewGroup.LayoutParams.MatchParent));

                // With the touch event we move el selected area view.
                selectedLine.Touch += (sender, e) => 
                {
                    switch (e.Event.Action)
                    {
                        // We save the coordinates where we are touching the screen (x, y) just when we touch the selected area.
                        case MotionEventActions.Down:
                            this.origX = e.Event.RawX;
                            this.origY = e.Event.RawY;
                            break;

                        // We move the selected area view.
                        case MotionEventActions.Move:
                            // Distance that we have moved the selected area.
                            float deltaX = e.Event.RawX - this.origX;
                            float deltaY = e.Event.RawY - this.origY;

                            // We calculate the new position of the selected area view.
                            float posViewX = selectedLine.GetX() + deltaX;
                            if (posViewX > this.mWidth - width) posViewX = this.mWidth - width;
                            else if (posViewX < 0) posViewX = 0;

                            // We place the selected area view in the new position, and we repaint it ("Invalidate").
                            selectedLine.SetX(posViewX);
                            selectedLine.Invalidate();

                            // We calculate the new "origX" and "origY" values. They have to be changed to calculate a right value of
                            // "deltaX" and "deltaY".
                            this.origX = e.Event.RawX;
                            this.origY = e.Event.RawY;

                            break;

                        // When we finish touching the selected area, we invoke an event with the percentage start point and the percentage end point where it is palced.
                        case MotionEventActions.Up:
                        case MotionEventActions.Cancel:
                            if (this.positionEvent != null)
                            {
                                this.positionEvent.Invoke(this, new TemporalLinePoint { TemporalLineStart = selectedLine.GetX() / this.mWidth, TemporalLineEnd = (selectedLine.GetX() + width) / this.mWidth }) ;
                            }
                            break;
                    }

#if (DEBUG)
                    Log.Info("ATLG :: pos", "(" + (selectedLine.GetX() / this.mWidth).ToString() + " , " + ((selectedLine.GetX() + width) / this.mWidth).ToString() + ")");
#endif
                };
            }
        }
    }
}
