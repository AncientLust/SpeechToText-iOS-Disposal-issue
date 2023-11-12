using System.Globalization;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Media;

namespace SpeechToText_iOS_Disposal_issue;

public partial class MainPage : ContentPage
{
    private readonly ISpeechToText speechToText;

    public MainPage(ISpeechToText speechToText)
    {
        InitializeComponent();
        this.speechToText = speechToText;
    }

    public async void OnListenClicked(object sender, EventArgs args)
    {

        var isGranted = await speechToText.RequestPermissions(CancellationToken.None);
        if (!isGranted)
        {
            await Toast.Make("Permission not granted").Show(CancellationToken.None);
            return;
        }

        await Listen50Times();

        RecognizedTextLabel.Text = "Say a phrase: ";

        try
        {
            var recognitionResult = await speechToText.ListenAsync(
                CultureInfo.GetCultureInfo("en-US"),
                new Progress<string>(recognitionResult => RecognizedTextLabel.Text += " " + recognitionResult),
                CancellationToken.None);

            if (recognitionResult.Exception is not null)
            {
                RecognizedTextLabel.Text = "Exception:" + recognitionResult.Exception.Message;
            }
        }
        catch (Exception a)
        {
            Console.WriteLine(a);
        }

        //await speechToText.DisposeAsync();
    }

    public async void OnStopListenClicked(object sender, EventArgs e)
    {
        await speechToText.StopListenAsync(CancellationToken.None);
    }

    private async Task Listen50Times()
    {
        for (int i = 0; i < 50; i++)
        {
            var ctSource = new CancellationTokenSource();

            RecognizedTextLabel.Text = $"ListenAsync switches: {i}";

            _ = speechToText.ListenAsync(
                CultureInfo.GetCultureInfo("en-US"),
                new Progress<string>(),
                ctSource.Token);

            await speechToText.StopListenAsync(CancellationToken.None);
            await Task.Delay(100, CancellationToken.None);
            //await speechToText.DisposeAsync();
        }
    }
}