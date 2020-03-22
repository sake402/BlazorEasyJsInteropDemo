using BlazorJSDemo.TypeScriptDefinitions;
using LivingThing.TCCS.Core;
using LivingThing.TCCS.Definitions.Util;
using LivingThing.TCCS.Interface;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorJSDemo.Pages
{
    public partial class Map : ComponentBase, IScriptExecutor
    {
        [Inject] public IJSRuntime JS { get; set; }

        public Task<T> ExecuteAsync<T>(string javascript, params object[] parameters)
        {
            return JS.InvokeAsync<T>(javascript, parameters).AsTask();
        }

        async void CreateMap()
        {
            var generator = new Generator(this, new GeneratorOptions()
            {
                //DisableParameterBag = true
            });
            var scope = await generator.StoredProcedure(async scope =>
            {
                var L = await scope.GetDefinition<Leaflet>();
                var map = L.map("map", scope.Instantiate<Leaflet.MapOptions>((scope, op) =>
                {
                    op.dragging = true;
                    op.tap = false;
                    op.center = new Union<Leaflet.LatLng, Leaflet.LatLngLiteral, Leaflet.LatLngTuple>(scope.Instantiate<Leaflet.LatLngLiteral>((scope, ll) =>
                    {
                        ll.lat = 51.5;
                        ll.lng = -0.09;
                    }, "centerOptions", true));
                }, "mapOptions", true))
                .setView(new Union<Leaflet.LatLng, Leaflet.LatLngLiteral, Leaflet.LatLngTuple>(scope.Instantiate<Leaflet.LatLngLiteral>((scope, ll) =>
                {
                    ll.lat = 51.5;
                    ll.lng = -0.09;
                }, null, true)), 13, null);
                var tile = L.tileLayer("https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png", scope.Instantiate<Leaflet.TileLayerOptions>((scope, op) =>
                {
                    op.attribution = "&copy; <a href=\"https://www.openstreetmap.org/copyright\">OpenStreetMap</a> contributors";
                }, "options", true)).addTo(map);
                for (int i = 0; i < 2; i++)
                {
                    L.marker(new Union<Leaflet.LatLng, Leaflet.LatLngLiteral, Leaflet.LatLngTuple>(scope.Instantiate<Leaflet.LatLngLiteral>((scope, ll) =>
                    {
                        ll.lat = 51.5 + i * 0.05;
                        ll.lng = -0.09 + i * 0.05;
                    }, null, true)), null)
                    .addTo(map)
                    .bindPopup(new Union<Func<Leaflet.Layer, Leaflet.Content>, Leaflet.Content, Leaflet.Popup>(new Leaflet.Content("A pretty CSS3 popup.<br> Easily customizable.")), null)
                    .openPopup(null);
                }
            });
            _ = scope.Execute<object>(null, true);
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                CreateMap();
            }
            base.OnAfterRender(firstRender);
        }
    }
}
