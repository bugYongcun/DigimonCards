﻿

#pragma checksum "f:\users\王永淳\documents\visual studio 2012\Projects\DigimonCard\DigimonCard\RoomCard.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "18FABEF4AB24F5D8034CFB9A1830E53C"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DigimonCard
{
    partial class RoomCard : global::Windows.UI.Xaml.Controls.UserControl
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        public global::Windows.UI.Xaml.Controls.Grid ClickBox; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.Image HostPhoto; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.Image ChallengerPhoto; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.TextBlock roomNum; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private bool _contentLoaded;

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent()
        {
            if (_contentLoaded)
                return;

            _contentLoaded = true;
            global::Windows.UI.Xaml.Application.LoadComponent(this, new global::System.Uri("ms-appx:///RoomCard.xaml"), global::Windows.UI.Xaml.Controls.Primitives.ComponentResourceLocation.Application);
 
            ClickBox = (global::Windows.UI.Xaml.Controls.Grid)this.FindName("ClickBox");
            HostPhoto = (global::Windows.UI.Xaml.Controls.Image)this.FindName("HostPhoto");
            ChallengerPhoto = (global::Windows.UI.Xaml.Controls.Image)this.FindName("ChallengerPhoto");
            roomNum = (global::Windows.UI.Xaml.Controls.TextBlock)this.FindName("roomNum");
        }
    }
}


