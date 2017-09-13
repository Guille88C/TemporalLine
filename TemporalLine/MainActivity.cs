using Android.App;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using TemporalLine.Components;

namespace TemporalLine
{
    [Activity(Label = "Temporal Line", MainLauncher = true)]
    public class MainActivity : Activity
    {
        int count = 1;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            AvailabilityTemporalLine temporalLine = this.FindViewById<AvailabilityTemporalLine>(Resource.Id.main_temporal_line_atl);
            if (temporalLine != null)
            {
			    List<TemporalLinePoint> lBusyPoints = new List<TemporalLinePoint>();
			    lBusyPoints.Add(new TemporalLinePoint { TemporalLineStart = 0.0f / 1399.0f, TemporalLineEnd = 300.0f / 1399.0f } );
			    lBusyPoints.Add(new TemporalLinePoint { TemporalLineStart = 400.0f / 1399.0f, TemporalLineEnd = 460.0f / 1399.0f });
			    lBusyPoints.Add(new TemporalLinePoint { TemporalLineStart = 1000.0f / 1399.0f, TemporalLineEnd = 1399.0f / 1399.0f });
                temporalLine.BusyArea = lBusyPoints;
			}

            AvailabilityTemporalLineGroup temporalLineGroup = this.FindViewById<AvailabilityTemporalLineGroup>(Resource.Id.main_temporal_line_group_atlg);
            if (temporalLineGroup != null)
            {
                temporalLineGroup.positionEvent += TemporalLineGroup_positionEvent;
            }

            Button button = FindViewById<Button>(Resource.Id.myButton);
            button.Click += (sender, e) => 
            {
                if (temporalLineGroup != null)
                {
                    temporalLineGroup.AddSelectedTemporalLine(new TemporalLinePoint { TemporalLineStart = 400.0f / 1399.0f, TemporalLineEnd = 600.0f / 1399.0f });
                }
            };
        }

        private void TemporalLineGroup_positionEvent(object sender, TemporalLinePoint e)
        {
        }
    }
}

