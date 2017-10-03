using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Plugin.Geolocator;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Net;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using Xamarin.Forms.Xaml;
using Xamarin.Auth;
using Xamarin.Forms.Xaml;
using System.Net.Http;

namespace CustomRenderer
{
   
    public partial class MapPage : ContentPage
	{
        public CustomMap mapa = new CustomMap();

        string datosOcupacion;

        string obtenerDatosOcupacion()
        {
            string url = "http://tesis2017.000webhostapp.com/webservice.php";

            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = client.GetAsync(url).Result)
                {
                    using (HttpContent content = response.Content)
                    {
                        datosOcupacion = content.ReadAsStringAsync().Result;
                        return datosOcupacion;
                    }
                }
            }
        }        

        private async void findMe()
        {
            var locator = CrossGeolocator.Current;
            Plugin.Geolocator.Abstractions.Position position = new Plugin.Geolocator.Abstractions.Position();

            position = await locator.GetPositionAsync();
            customMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude),Distance.FromMiles(1)));
        }

        private async void requestGPSAsync()
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                    {
                        await DisplayAlert("Need location", "Gunna need that location", "OK");
                    }

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
                    //Best practice to always check that the key exists
                    if (results.ContainsKey(Permission.Location))
                        status = results[Permission.Location];
                }
                if (status == PermissionStatus.Granted)
                {
                    var results = await CrossGeolocator.Current.GetPositionAsync();
                }
                else if (status != PermissionStatus.Unknown)
                {
                    await DisplayAlert("Location Denied", "Can not continue, try again.", "OK");
                }
            }
            catch 
            {

            }
        }

        public MapPage ()
		{
            
            InitializeComponent ();

            //customMap.IsShowingUser = true;
            //requestGPSAsync();
            var datoOcupacion=obtenerDatosOcupacion();
            var datosResultadoOcupacion = JArray.Parse(datosOcupacion);
            //findMe();
            string estado = "";
            bool estadoB;
            ///////////////////////////////////////////////////
            if ((int)datosResultadoOcupacion[0]["estado"] == 1)
            {
                estado = "ocupado";
                estadoB = false;
            }
            else
            {
                estado = "desocupado";
                estadoB = true;
            }
            //////////////////////////
            var pin = new CustomPin
            {
                Pin = new Pin
                {
                    Type = PinType.Place,
                    Position = new Position((double)datosResultadoOcupacion[0]["coordenadas_lat"],(double)datosResultadoOcupacion[0]["coordenadas_lon"]),
                    Label = estado,
                    Address = datosResultadoOcupacion[0]["calle"].ToString(),
                },
                Id = "Xamarin",
                Url = "",
                Estado=false//estadoB
            };
            //////////////////////////////////////////////////////
            if ((int)datosResultadoOcupacion[1]["estado"] == 1)
            {
                estado = "ocupado";
                estadoB = false;
            }
            else
            {
                estado = "desocupado";
                estadoB = true;
            }
            ///////////////////////////////
            var pin2 = new CustomPin
            {
                Pin = new Pin
                {
                    Type = PinType.Place,
                    Position = new Position((double)datosResultadoOcupacion[1]["coordenadas_lat"], (double)datosResultadoOcupacion[1]["coordenadas_lon"]),
                    Label = estado,
                    Address = datosResultadoOcupacion[1]["calle"].ToString(),
                },
                Id = "Google",
                Url = "",
                Estado=true//estadoB
            };
            /////////////////////////////////////////////////////////
            customMap.CustomPins = new List<CustomPin> { pin,pin2 };
			customMap.Pins.Add (pin.Pin);
            customMap.Pins.Add (pin2.Pin);            
        }
	}
}
