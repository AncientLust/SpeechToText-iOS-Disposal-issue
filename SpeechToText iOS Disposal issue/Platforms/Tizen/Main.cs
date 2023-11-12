using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using System;

namespace SpeechToText_iOS_Disposal_issue
{
    internal class Program : MauiApplication
    {
        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

        static void Main(string[] args)
        {
            var app = new Program();
            app.Run(args);
        }
    }
}
