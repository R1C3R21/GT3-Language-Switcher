using Microsoft.Win32;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GT3_Language_Switcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Save save;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Original_File_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.FileName = "BESCES-50294GAMEDATA";
            ofd.DefaultExt = "";
            Nullable<bool> result = ofd.ShowDialog();

            if (result == true)
            {
                try
                {
                    Button_Convert.IsEnabled = false;
                    Button_English.IsEnabled = false;
                    Button_French.IsEnabled = false;
                    Button_Italian.IsEnabled = false;
                    Button_German.IsEnabled = false;
                    Button_Spanish.IsEnabled = false;
                    Button_English.IsChecked = false;
                    Button_French.IsChecked = false;
                    Button_Italian.IsChecked = false;
                    Button_German.IsChecked = false;
                    Button_Spanish.IsChecked = false;

                    byte[] saveData = File.ReadAllBytes(ofd.FileName);
                    save = new Save(saveData);

                    if (save.Verify() == false)
                    {
                        throw new Exception("The save file is damaged or invalid, make sure to check it is a valid one.");
                    }
                    else
                    {
                        switch(save.GetLanguage())
                        {
                            case 1:
                                Button_French.IsEnabled = true;
                                Button_Italian.IsEnabled = true;
                                Button_German.IsEnabled = true;
                                Button_Spanish.IsEnabled = true;
                                break;
                            case 2:
                                Button_English.IsEnabled = true;
                                Button_Italian.IsEnabled = true;
                                Button_German.IsEnabled = true;
                                Button_Spanish.IsEnabled = true;
                                break;
                            case 3:
                                Button_English.IsEnabled = true;
                                Button_French.IsEnabled = true;
                                Button_German.IsEnabled = true;
                                Button_Spanish.IsEnabled = true;
                                break;
                            case 4:
                                Button_English.IsEnabled = true;
                                Button_French.IsEnabled = true;
                                Button_Italian.IsEnabled = true;
                                Button_Spanish.IsEnabled = true;
                                break;
                            case 5:
                                Button_English.IsEnabled = true;
                                Button_French.IsEnabled = true;
                                Button_Italian.IsEnabled = true;
                                Button_German.IsEnabled = true;
                                break;
                            case -1:
                                throw new Exception("The save file is damaged or invalid, make sure to check it is a valid one.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Button_Convert_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = "";
            sfd.DefaultExt = "";
            Nullable<bool> result = sfd.ShowDialog();

            if (result == true)
            {
                try
                {
                    if (Button_English.IsChecked == true)
                    {
                        save.Convert(sfd.FileName, 1);
                    }
                    else if (Button_French.IsChecked == true)
                    {
                        save.Convert(sfd.FileName, 2);
                    }
                    else if (Button_Italian.IsChecked == true)
                    {
                        save.Convert(sfd.FileName, 3);
                    }
                    else if (Button_German.IsChecked == true)
                    {
                        save.Convert(sfd.FileName, 4);
                    }
                    else if (Button_Spanish.IsChecked == true)
                    {
                        save.Convert(sfd.FileName, 5);
                    }
                    MessageBox.Show("Conversion completed successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred during conversion: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                Button_Convert.IsEnabled = false;
                Button_English.IsEnabled = false;
                Button_French.IsEnabled = false;
                Button_Italian.IsEnabled = false;
                Button_German.IsEnabled = false;
                Button_Spanish.IsEnabled = false;
                Button_English.IsChecked = false;
                Button_French.IsChecked = false;
                Button_Italian.IsChecked = false;
                Button_German.IsChecked = false;
                Button_Spanish.IsChecked = false;
            }
        }

        private void Button_English_Checked(object sender, RoutedEventArgs e)
        {
            if (Button_English.IsChecked == true)
            {
                Button_Convert.IsEnabled = true;
            }
            else
            {
                Button_Convert.IsEnabled = false;
            }
        }

        private void Button_French_Checked(object sender, RoutedEventArgs e)
        {
            if (Button_French.IsChecked == true)
            {
                Button_Convert.IsEnabled = true;
            }
            else
            {
                Button_Convert.IsEnabled = false;
            }
        }

        private void Button_Italian_Checked(object sender, RoutedEventArgs e)
        {
            if (Button_Italian.IsChecked == true)
            {
                Button_Convert.IsEnabled = true;
            }
            else
            {
                Button_Convert.IsEnabled = false;
            }
        }

        private void Button_German_Checked(object sender, RoutedEventArgs e)
        {
            if (Button_German.IsChecked == true)
            {
                Button_Convert.IsEnabled = true;
            }
            else
            {
                Button_Convert.IsEnabled = false;
            }
        }

        private void Button_Spanish_Checked(object sender, RoutedEventArgs e)
        {
            if (Button_Spanish.IsChecked == true)
            {
                Button_Convert.IsEnabled = true;
            }
            else
            {
                Button_Convert.IsEnabled = false;
            }
        }
    }
}