using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;


namespace WpfYoutubetutorial
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        #region Constructor
        public MainWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region onLoader

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Gets every Logiccal drive on the machine
           foreach ( var drive in Directory.GetLogicalDrives())
            {
                // create a new item for it
                var item = new TreeViewItem()
                {
                    //Set the header
                    Header = drive,
                    //And the full Path
                    Tag = drive
                };

      
            

                //add Dummby Item
                item.Items.Add(null);

                //Lissten out for item being expanded
                item.Expanded += Folder_Expanded;


                //Add it to the main tree-view
                FolderView.Items.Add(item);
            }
        }

        private void Folder_Expanded(object sender, RoutedEventArgs e)
        {
            #region Inital Cechks
                 var item = (TreeViewItem)sender;

                 //If the item only contains the dummy data
                 if (item.Items.Count != 1 || item.Items[0] != null)
                     return;

                 //Clear dummmy data

                 item.Items.Clear();

                 // Get Full Path
                 var fullPath = (string)item.Tag;

            #endregion

            #region Get Folders

            //Create a Blank List
            var directorys = new List<string>();

            // Try and get the directories from the folder
            // ignoring any issues doing so
            try
            {
                var dirs = Directory.GetDirectories(fullPath);

                if (dirs.Length > 0)
                    directorys.AddRange(dirs);
             
            }
            catch { }

            directorys.ForEach(
                directoryPath =>
                {
                    // Create directory item
                    var subItem = new TreeViewItem()
                    {
                        //Set header as folder name
                        Header = GetFileFolderName(directoryPath),
                        Tag = directoryPath
                    };

                    //Add dummy item so we can Expand folder
                    subItem.Items.Add(null);

                    //Hanel expandeing;
                    subItem.Expanded += Folder_Expanded;
                    item.Items.Add(subItem);


                });

            #endregion

            #region GetFiles


            //Create a Blank List
            var files = new List<string>();

            // Try and get the  files from the folder
            // ignoring any issues doing so
            try
            {
                var fs = Directory.GetFiles(fullPath);

                if (fs.Length > 0)
                    files.AddRange(fs);

            }
            catch { }

            files.ForEach(
                filePath =>
                {
                    // Create File item
                    var subItem = new TreeViewItem()
                    {
                        //Set header as file name
                        Header = GetFileFolderName(filePath),
                        Tag = filePath
                    };


                    item.Items.Add(subItem);


                });


            #endregion




        }

        public static string GetFileFolderName(string path)
        {
            //if we have no path,return empty
            if (string.IsNullOrEmpty(path))
                return string.Empty;

            // make all slashes back slashes
            var normalizedPath = path.Replace('/', '\\');

            // Find the last backslash in the path
            var lastIndex = normalizedPath.LastIndexOf('\\');

            // if we don-t find a backslash,return the path itself
            if (lastIndex <= 0)
                return path;

            //return the name after the last BackSlahs
            return path.Substring(lastIndex + 1);

        }


        #endregion



    }
}
