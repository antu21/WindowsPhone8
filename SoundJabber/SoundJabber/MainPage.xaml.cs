using Coding4Fun.Toolkit.Controls;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using SoundJabber.Resources;
using SoundJabber.ViewModels;
using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace SoundJabber
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;

            // Sample code to localize the ApplicationBar
            BuildLocalizedApplicationBar();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
        }

        private void LongListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LongListSelector selector = sender as LongListSelector;
            if (selector == null)
                return;

            SoundData data = selector.SelectedItem as SoundData;
            if (data == null)
                return;

            if (File.Exists(data.FilePath))
            {
                AudioPlayer.Source = new Uri(data.FilePath, UriKind.RelativeOrAbsolute);
            }
            else
            {
                try
                {
                    using (var storageFolder = IsolatedStorageFile.GetUserStoreForApplication())
                    {
                        using (var stream = new IsolatedStorageFileStream(data.FilePath, FileMode.Open, storageFolder))
                        {
                            AudioPlayer.SetSource(stream);
                        }
                    }
                }
                catch (Exception ex)
                {
                    string s = ex.Message;
                }
            }

            // to replay the sound if same tile click multiple times
            selector.SelectedItem = null;
        }

        private void BuildLocalizedApplicationBar()
        {
            // Set the page's ApplicationBar to a new instance of ApplicationBar.
            ApplicationBar = new ApplicationBar();

            // Create a new button and set the text value to the localized string from AppResources.
            ApplicationBarIconButton recordButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/microphone.png", UriKind.Relative));
            recordButton.Text = AppResources.AppBarRecordButtonText;
            recordButton.Click += recordButton_Click;
            ApplicationBar.Buttons.Add(recordButton);

            // Create a new menu item with the localized string from AppResources.
            ApplicationBarMenuItem aboutMenuItem = new ApplicationBarMenuItem(AppResources.AppBarAboutMenuItemText);
            aboutMenuItem.Click += aboutMenuItem_Click;
            ApplicationBar.MenuItems.Add(aboutMenuItem);
        }

        void aboutMenuItem_Click(object sender, EventArgs e)
        {
            AboutPrompt aboutMe = new AboutPrompt();
            aboutMe.Show("Prasad Honrao", "PrasadHonrao", "Honrao.Prasad@hotmail.com", "http://PrasadHonrao.com");
        }

        void recordButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/RecordAudio.xaml", UriKind.Relative));
        }

        private void Pin_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void Delete_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            LongListSelector selector = sender as LongListSelector;
            if (selector == null)
                return;

            SoundData data = selector.SelectedItem as SoundData;
            if (data == null)
                return;

            if (File.Exists(data.FilePath))
            {
                File.Delete(data.FilePath);
            }
        }
    }
}