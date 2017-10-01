using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace CustomRenderer
{
	public partial class MapPage : ContentPage
	{
		public MapPage ()
		{
			InitializeComponent ();

            var pin = new CustomPin
            {
                Pin = new Pin
                {
                    Type = PinType.Place,
                    Position = new Position(37.79752, -121.40183),
                    Label = "a",
                    Address = "1"
                },
                Id = "Xamarin",
                Url = "http://xamarin.com/about/",
                Estado=true
            };

            var pin2 = new CustomPin
            {
                Pin = new Pin
                {
                    Type = PinType.Place,
                    Position = new Position(36.79752, -121.40183),
                    Label = "b",
                    Address = "2"
                },
                Id = "Google",
                Url = "http://google.com/about/",
                Estado=false
            };

            customMap.CustomPins = new List<CustomPin> { pin,pin2 };
			customMap.Pins.Add (pin.Pin);
            customMap.Pins.Add (pin2.Pin);
            customMap.MoveToRegion (MapSpan.FromCenterAndRadius (new Position (37.79752, -122.40183), Distance.FromMiles (1.0)));
		}
	}
}
