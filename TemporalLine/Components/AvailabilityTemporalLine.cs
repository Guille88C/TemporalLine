using System.Collections.Generic;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using Android.Views;

namespace TemporalLine.Components
{
    /// <summary>
    /// This component is used as the temporal line. Grenn and red colors are used here.
    /// 
    /// Green color let us know when the device is available.
    /// 
    /// Red color let us kenow when the device is busy.
    /// </summary>
    [Register("temporalLine.components.availabilityTemporalLine")]
    public class AvailabilityTemporalLine : View
	{
        private Color mBusyColor, mAvailableColor;

        private int mWidth, mHeight;
		private Paint mPaint;

        private List<TemporalLinePoint> mListBusyArea;

		public AvailabilityTemporalLine(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
			this.Init(attrs);
		}

		public AvailabilityTemporalLine(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
			this.Init(attrs);
		}

		public AvailabilityTemporalLine(Context context) : base(context)
        {
			this.Init(null);
		}

		public AvailabilityTemporalLine(Context context, IAttributeSet attrs) : base(context, attrs)
        {
			this.Init(attrs);
		}

        /// <summary>
        /// This property stores the busy point in the temporal line. They are painted in red.
        /// </summary>
        public List<TemporalLinePoint> BusyArea
        {
            get
            {
                return this.mListBusyArea;
            }
            set
            {
                this.mListBusyArea = value;
            }
        }

		protected override void OnDraw(Canvas canvas)
		{
			this.mPaint = new Paint();

            // White border of the component.
            //this.mPaint.Color = Color.White;
            //this.mPaint.StrokeWidth = 3;
            //canvas.DrawRect(0, 0, this.mWidth, this.mHeight, this.mPaint);

            // Green color for all the component.
			this.mPaint.StrokeWidth = 0;

            this.mPaint.Color = this.mAvailableColor;
            
			//canvas.DrawRect(3, 3, this.mWidth - 3, this.mHeight - 3, this.mPaint);
			canvas.DrawRect(0, 0, this.mWidth, this.mHeight, this.mPaint);

            // Red color fot the busy areas.
            if (this.mListBusyArea != null && this.mListBusyArea.Count > 0)
            {
                foreach (TemporalLinePoint item in this.mListBusyArea)
                {
                    if (item != null)
                    {
                        // The "TemporalLinePoint" reperesent the percentages of the temporal line.
                        float percStart = item.TemporalLineStart;
                        float percEnd = item.TemporalLineEnd;

                        // We calculate the real points.
                        //int pointStart = (int)(this.mWidth * percStart) + 3;
                        //int pointEnd = (int)(this.mWidth * percEnd) - 3;
                        int pointStart = (int)(this.mWidth * percStart);
                        int pointEnd = (int)(this.mWidth * percEnd);

                        // We paint the rectangles.
                        this.mPaint.Color = this.mBusyColor;
                        //canvas.DrawRect(pointStart, 3, pointEnd, this.mHeight - 3, this.mPaint);
                        canvas.DrawRect(pointStart, 0, pointEnd, this.mHeight, this.mPaint);
                    }
                }
            }
		}

		protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            TemporalLineHelper.MeasureView(widthMeasureSpec, heightMeasureSpec, out this.mWidth, out this.mHeight);
            this.SetMeasuredDimension(this.mWidth, this.mHeight);
        }

		private void Init(IAttributeSet attrs)
		{
			if (this.Context == null || attrs == null)
			{
				return;
			}

			var a = this.Context.ObtainStyledAttributes(attrs, Resource.Styleable.TemporalLineColor);

            if (a != null)
            {
                this.mBusyColor = a.GetColor(Resource.Styleable.TemporalLineColor_temopral_line_color_busy, Color.Red);
                this.mAvailableColor = a.GetColor(Resource.Styleable.TemporalLineColor_temopral_line_color_available, Color.Green);

                a.Recycle();
            }
		}

        public void Update()
        {
            this.Invalidate();
        }
    }
}
