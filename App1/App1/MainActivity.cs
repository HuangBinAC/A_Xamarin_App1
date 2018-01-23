using Android.App;
using Android.Widget;
using Android.OS;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace App1
{
    [Activity(Label = "App1", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            AppCenter.SetLogUrl("https://in-staging-south-centralus.staging.avalanch.es");
            AppCenter.Start("3519e059-856f-4e62-a320-c8dc5d28db94",
                   typeof(Analytics), typeof(Crashes));

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);

            // New code will go here

            // Get our UI controls from the loaded layout
            EditText phoneNumberText = FindViewById<EditText>(Resource.Id.PhoneNumberText);
            TextView translatedPhoneWord = FindViewById<TextView>(Resource.Id.TranslatedPhoneWord);
            Button translateButton = FindViewById<Button>(Resource.Id.TranslateButton);

            // Add code to translate number
            translateButton.Click += (sender, e) =>
            {
                // Translate user’s alphanumeric phone number to numeric
                string translatedNumber = Core.PhonewordTranslator.ToNumber(phoneNumberText.Text);
                if (string.IsNullOrWhiteSpace(translatedNumber))
                {
                    translatedPhoneWord.Text = string.Empty;
                }
                else
                {
                    translatedPhoneWord.Text = translatedNumber;
                }
            };
        }
    }
}

