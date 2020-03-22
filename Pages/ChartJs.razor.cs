using BlazorJSDemo.TypeScriptDefinitions;
using LivingThing.TCCS.Core;
using LivingThing.TCCS.Definitions.Javascript;
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
    public partial class ChartJs : ComponentBase, IScriptExecutor
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
                //of course we can avoid this literal by converting the definition for dom to C#
                var context = scope.Literal<HTMLCanvasElement>("document.getElementById('myChart').getContext('2d')");
                var chart = await scope.GetDefinition<TypeScriptDefinitions.Chart>(context, scope.Instantiate<TypeScriptDefinitions.Chart.ChartConfiguration>((scope, options) =>
                {
                    options.type = TypeScriptDefinitions.Chart.ChartType.line;
                    options.data = scope.Instantiate<TypeScriptDefinitions.Chart.ChartData>((scope, data) =>
                    {
                        data.labels = new Union<string, string[], double, double[], DateTime, DateTime[], moment.Moment, moment.Moment[]>[]
                        {
                            "Red", "Blue", "Yellow", "Green", "Purple", "Orange"
                        };
                        data.datasets = new TypeScriptDefinitions.Chart.ChartDataSets[]
                        {
                            scope.Instantiate<TypeScriptDefinitions.Chart.ChartDataSets>((scope, data) =>{
                                data.label = "# of Votes";
                                data.data = new Union<double, object, undefined>[]
                                {
                                    12, 19, 3, 5, 2, 3
                                };
                                data.backgroundColor = new Union<TypeScriptDefinitions.Chart.ChartColor, TypeScriptDefinitions.Chart.ChartColor[], TypeScriptDefinitions.Chart.Scriptable<TypeScriptDefinitions.Chart.ChartColor>>(new TypeScriptDefinitions.Chart.ChartColor[]{
                                    new Union<string, CanvasGradient, CanvasPattern, string[]>("rgba(255, 99, 132, 0.2)"),
                                    new Union<string, CanvasGradient, CanvasPattern, string[]>("rgba(54, 162, 235, 0.2)"),
                                    new Union<string, CanvasGradient, CanvasPattern, string[]>("rgba(255, 206, 86, 0.2)"),
                                    new Union<string, CanvasGradient, CanvasPattern, string[]>("rgba(75, 192, 192, 0.2)"),
                                    new Union<string, CanvasGradient, CanvasPattern, string[]>("rgba(153, 102, 255, 0.2)"),
                                    new Union<string, CanvasGradient, CanvasPattern, string[]>("rgba(255, 159, 64, 0.2)")
                                });
                                data.borderColor = new Union<TypeScriptDefinitions.Chart.ChartColor, TypeScriptDefinitions.Chart.ChartColor[], TypeScriptDefinitions.Chart.Scriptable<TypeScriptDefinitions.Chart.ChartColor>>(new TypeScriptDefinitions.Chart.ChartColor[]{
                                    new Union<string, CanvasGradient, CanvasPattern, string[]>("rgba(255, 99, 132, 1)"),
                                    new Union<string, CanvasGradient, CanvasPattern, string[]>("rgba(54, 162, 235, 1)"),
                                    new Union<string, CanvasGradient, CanvasPattern, string[]>("rgba(255, 206, 86, 1)"),
                                    new Union<string, CanvasGradient, CanvasPattern, string[]>("rgba(75, 192, 192, 1)"),
                                    new Union<string, CanvasGradient, CanvasPattern, string[]>("rgba(153, 102, 255, 1)"),
                                    new Union<string, CanvasGradient, CanvasPattern, string[]>("rgba(255, 159, 64, 1)")
                                });
                                data.borderWidth = new Union<TypeScriptDefinitions.Chart.BorderWidth, TypeScriptDefinitions.Chart.BorderWidth[], TypeScriptDefinitions.Chart.Scriptable<TypeScriptDefinitions.Chart.BorderWidth>>(new TypeScriptDefinitions.Chart.BorderWidth(1));
                            })
                        };
                    });
                    options.options = scope.Instantiate<TypeScriptDefinitions.Chart.ChartOptions>((scope, options) =>
                    {
                        options.scales = new Union<TypeScriptDefinitions.Chart.ChartScales, TypeScriptDefinitions.Chart.LinearScale, TypeScriptDefinitions.Chart.LogarithmicScale, TypeScriptDefinitions.Chart.TimeScale>(scope.Instantiate<TypeScriptDefinitions.Chart.ChartScales>((scope, options) =>
                        {
                            options.yAxes = new TypeScriptDefinitions.Chart.ChartYAxe[]
                            {
                                scope.Instantiate<TypeScriptDefinitions.Chart.ChartYAxe>((scope, options)=>{
                                    options.ticks = scope.Instantiate<TypeScriptDefinitions.Chart.TickOptions>((scope, options) =>
                                    {
                                        options.beginAtZero = true;
                                    });
                                })
                            };
                        }));
                    });
                }));
            });
            _ = scope.Execute<object>(null, true);
            //scope.ParameterBag.Set<TypeScriptDefinitions.Chart.ChartConfiguration>(scope, c => c.type, TypeScriptDefinitions.Chart.ChartType.bar);
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
