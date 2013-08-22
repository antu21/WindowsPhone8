using Coding4Fun.Toolkit.Audio;
using Coding4Fun.Toolkit.Audio.Helpers;
using Coding4Fun.Toolkit.Controls;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Newtonsoft.Json;
using SoundJabber.Resources;
using SoundJabber.ViewModels;
using System;
using System.IO.IsolatedStorage;
using System.Windows.Navigation;

namespace SoundJabber
{
    public partial class RecordAudio : PhoneApplicationPage
    {
        MicrophoneRecorder recorder = new MicrophoneRecorder();
        IsolatedStorageFileStream audioStream;
        string tempFile = "tempwav.wav";

        public RecordAudio()
        {
            InitializeComponent();

            BuildLocalizedApplicationBar();
        }

        private void BuildLocalizedApplicationBar()
        {
            // Set the page's ApplicationBar to a new instance of ApplicationBar.
            ApplicationBar = new ApplicationBar();

            // Create a new button and set the text value to the localized string from AppResources.
            ApplicationBarIconButton saveButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/save.png", UriKind.Relative));
            saveButton.Text = AppResources.AppBarSaveButtonText;
            saveButton.Click += saveButton_Click;
            ApplicationBar.Buttons.Add(saveButton);
            ApplicationBar.IsVisible = false;
        }

        void saveButton_Click(object sender, EventArgs e)
        {
            InputPrompt prompt = new InputPrompt();
            prompt.Message = "Enter the sound name";
            prompt.Title = "Sound Name";
            prompt.Completed += prompt_Completed;
            prompt.Show();
        }

        void prompt_Completed(object sender, PopUpEventArgs<string, PopUpResult> e)
        {
            if (e.PopUpResult == PopUpResult.Ok)
            {
                SoundData sound = new SoundData();
                sound.FilePath = string.Format("/customAudio/{0}.wav", DateTime.Now.ToFileTime());
                sound.Title = e.Result;

                using (IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (!isoStore.DirectoryExists("/customAudio/"))
                    {
                        isoStore.CreateDirectory("/customAudio/");
                    }
                    isoStore.MoveFile(tempFile, sound.FilePath);
                }

                App.ViewModel.CustomSounds.Items.Add(sound);

                // save to applicationsettings
                var data = JsonConvert.SerializeObject(App.ViewModel.CustomSounds);
                IsolatedStorageSettings.ApplicationSettings[SoundModel.CustomSoundKey] = data;
                IsolatedStorageSettings.ApplicationSettings.Save();

                NavigationService.Navigate(new Uri("/MainPage.xaml?pivotItem=custom", UriKind.RelativeOrAbsolute));
            }

        }

        private void RecordToggleButton_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            PlayAudioButton.IsEnabled = false;
            ApplicationBar.IsVisible = false;
            RotateAudioGrid.Begin();

            recorder.Start();
        }

        private void RecordToggleButton_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            recorder.Stop();

            RotateAudioGrid.Stop();

            SaveAudioFile(recorder.Buffer);

            PlayAudioButton.IsEnabled = true;
            ApplicationBar.IsVisible = true;
        }

        private void SaveAudioFile(System.IO.MemoryStream buffer)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("No sound to save");
            }

            if (audioStream != null)
            {
                AudioPlayer.Stop();
                AudioPlayer.Source = null;
                audioStream.Dispose();
            }

            var bytes = buffer.GetWavAsByteArray(recorder.SampleRate);


            using (IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (isoStore.FileExists(tempFile))
                    isoStore.DeleteFile(tempFile);

                tempFile = string.Format("{0}.wav", DateTime.Now.ToFileTime());

                audioStream = isoStore.CreateFile(tempFile);
                audioStream.Write(bytes, 0, bytes.Length);
                AudioPlayer.SetSource(audioStream);
            }
        }

        private void PlayAudioButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            AudioPlayer.Play();
        }
    }
}