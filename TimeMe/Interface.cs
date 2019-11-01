using ArnoldVinkCode;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace TimeMe
{
    partial class MainPage
    {
        //Handle tile forward button tap
        void img_Tile_Forward_Tapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                if ((bool)vApplicationSettings["DevStatusMobile"])
                {
                    sv_tab_Tile.ChangeView(sv_tab_Tile.ActualWidth, null, null);
                    sv_tab_Tile.UpdateLayout();
                }
                sv_tab_Tile_Right.ChangeView(null, 0, null);
                sv_tab_Tile_Right.UpdateLayout();
            }
            catch { }
        }

        //Handle tile back button tap
        void img_Tile_Back_Tapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                if ((bool)vApplicationSettings["DevStatusMobile"])
                {
                    sv_tab_Tile.ChangeView(0, null, null);
                    sv_tab_Tile.UpdateLayout();
                }
                sv_tab_Tile_Right.ChangeView(null, 0, null);
                sv_tab_Tile_Right.UpdateLayout();
            }
            catch { }
        }

        //Goto the live tile text display tab
        async void btn_ChangeLiveTileText_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lb_Menu.SelectedIndex = 8;
                lb_Menu_Tapped(null, null);
                await Task.Delay(250);

                lb_Settings.SelectedIndex = 3;
                lb_Settings_Tapped(null, null);
                await Task.Delay(250);

                await AVFunctions.ScrollViewToElement(sv_tab_Settings_Right, btn_TextPositionPreview, true, false);
            }
            catch { }
        }

        //Goto the live tile selection tab
        async void btn_ChangeLiveTileStyle_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lb_Menu.SelectedIndex = 8;
                lb_Menu_Tapped(null, null);
                await Task.Delay(250);

                lb_Settings.SelectedIndex = 0;
                lb_Settings_Tapped(null, null);
            }
            catch { }
        }

        //Goto the weather tile selection tab
        async void btn_ChangeWeatherTileStyle_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lb_Menu.SelectedIndex = 8;
                lb_Menu_Tapped(null, null);
                await Task.Delay(250);

                lb_Settings.SelectedIndex = 1;
                lb_Settings_Tapped(null, null);
            }
            catch { }
        }

        //Goto the battery tile selection tab
        async void btn_ChangeBatteryTileStyle_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lb_Menu.SelectedIndex = 8;
                lb_Menu_Tapped(null, null);
                await Task.Delay(250);

                lb_Settings.SelectedIndex = 2;
                lb_Settings_Tapped(null, null);
            }
            catch { }
        }

        //Render Secondary Live Tile
        Rect GetElementRect(FrameworkElement FrameworkElement) { return new Rect(FrameworkElement.TransformToVisual(null).TransformPoint(new Point()), new Size(FrameworkElement.ActualWidth, FrameworkElement.ActualHeight)); }
    }
}