﻿using ACCSetupApp.Controls.HUD.Overlay;
using ACCSetupApp.Controls.HUD.Overlay.Internal;
using ACCSetupApp.Controls.HUD.Overlay.OverlayStaticInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
using System.Windows.Shapes;

namespace ACCSetupApp.Controls
{
    /// <summary>
    /// Interaction logic for HudOptions.xaml
    /// </summary>
    public partial class HudOptions : UserControl
    {
        public HudOptions()
        {
            InitializeComponent();

            BuildOverlayStackPanel();

            checkBoxReposition.Checked += (s, e) => SetRepositionMode(true);
            checkBoxReposition.Unchecked += (s, e) => SetRepositionMode(false);
        }

        private void SetRepositionMode(bool enabled)
        {
            stackPanelOverlayCheckboxes.IsEnabled = !enabled;

            lock (Overlays.ActiveOverlays)
                foreach (AbstractOverlay overlay in Overlays.ActiveOverlays)
                {
                    overlay.EnableReposition(enabled);
                }
        }

        private void BuildOverlayStackPanel()
        {
            int screenMiddleX = (int)(System.Windows.SystemParameters.FullPrimaryScreenWidth / 2);
            int screenMiddleY = (int)(System.Windows.SystemParameters.FullPrimaryScreenHeight / 2);

            stackPanelOverlayCheckboxes.Children.Clear();
            foreach (KeyValuePair<string, Type> x in Overlays.AbstractOverlays)
            {
                CheckBox checkBox = new CheckBox() { Content = x.Key };

                checkBox.Checked += (s, e) =>
                {
                    lock (Overlays.ActiveOverlays)
                    {
                        object[] args = new object[] { new System.Drawing.Rectangle((int)System.Windows.SystemParameters.PrimaryScreenWidth / 2, (int)System.Windows.SystemParameters.PrimaryScreenHeight / 2, 300, 150) };
                        AbstractOverlay overlay = (AbstractOverlay)Activator.CreateInstance(x.Value, args);
                        overlay.Start();

                        Overlays.ActiveOverlays.Add(overlay);
                    }
                };

                checkBox.Unchecked += (s, e) =>
                {
                    lock (Overlays.ActiveOverlays)
                    {
                        AbstractOverlay overlay = Overlays.ActiveOverlays.Find(f => f.GetType() == x.Value);
                        overlay?.Stop();
                        Overlays.ActiveOverlays.Remove(overlay);
                    }
                };

                stackPanelOverlayCheckboxes.Children.Add(checkBox);
            }
        }
    }
}
