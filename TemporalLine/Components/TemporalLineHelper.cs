using Android.Views;
using System;
using static Android.Views.View;

namespace TemporalLine.Components
{
    public class TemporalLineHelper
    {
        /// <summary>
        /// With this method we calculate the width and the height of a view.
        /// 
        /// Note: We must use this method in the OnMeasure method of a View.
        /// </summary>
        /// <param name="widthMeasureSpec">OnMeasure param.</param>
        /// <param name="heightMeasureSpec">OnMeasure param.</param>
        /// <param name="width">The width of the view.</param>
        /// <param name="height">The height of the view.</param>
        public static void MeasureView(int widthMeasureSpec, int heightMeasureSpec, out int width, out int height)
        {
            int desiredWidth = 100;
            int desiredHeight = 100;

            MeasureSpecMode widthMode = MeasureSpec.GetMode(widthMeasureSpec);
            int widthSize = MeasureSpec.GetSize(widthMeasureSpec);
            MeasureSpecMode heightMode = MeasureSpec.GetMode(heightMeasureSpec);
            int heightSize = MeasureSpec.GetSize(heightMeasureSpec);

            //Measure Width
            if (widthMode == MeasureSpecMode.Exactly)
            {
                //Must be this size
                width = widthSize;
            }
            else if (widthMode == MeasureSpecMode.AtMost)
            {
                //Can't be bigger than...
                width = Math.Min(desiredWidth, widthSize);
            }
            else
            {
                //Be whatever you want
                width = desiredWidth;
            }

            //Measure Height
            if (heightMode == MeasureSpecMode.Exactly)
            {
                //Must be this size
                height = heightSize;
            }
            else if (heightMode == MeasureSpecMode.AtMost)
            {
                //Can't be bigger than...
                height = Math.Min(desiredHeight, heightSize);
            }
            else
            {
                //Be whatever you want
                height = desiredHeight;
            }
        }
    }
}