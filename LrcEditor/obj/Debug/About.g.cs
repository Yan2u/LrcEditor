﻿#pragma checksum "..\..\About.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "FA95788CB9ED385CFDE0B50C9861A13AC4F3D261"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using LrcEditor;
using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Converters;
using MaterialDesignThemes.Wpf.Transitions;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace LrcEditor {
    
    
    /// <summary>
    /// About
    /// </summary>
    public partial class About : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 17 "..\..\About.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button UIBtn_Copy;
        
        #line default
        #line hidden
        
        
        #line 58 "..\..\About.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button UIBtn;
        
        #line default
        #line hidden
        
        
        #line 61 "..\..\About.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button PlayerModuleBtn;
        
        #line default
        #line hidden
        
        
        #line 64 "..\..\About.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button CSCoreBtn;
        
        #line default
        #line hidden
        
        
        #line 67 "..\..\About.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button TrnsBtn;
        
        #line default
        #line hidden
        
        
        #line 70 "..\..\About.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button MusicUIBtn;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/LrcEditor;component/about.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\About.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.UIBtn_Copy = ((System.Windows.Controls.Button)(target));
            return;
            case 2:
            this.UIBtn = ((System.Windows.Controls.Button)(target));
            
            #line 58 "..\..\About.xaml"
            this.UIBtn.Click += new System.Windows.RoutedEventHandler(this.UIBtn_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.PlayerModuleBtn = ((System.Windows.Controls.Button)(target));
            
            #line 61 "..\..\About.xaml"
            this.PlayerModuleBtn.Click += new System.Windows.RoutedEventHandler(this.PlayerModuleBtn_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.CSCoreBtn = ((System.Windows.Controls.Button)(target));
            
            #line 64 "..\..\About.xaml"
            this.CSCoreBtn.Click += new System.Windows.RoutedEventHandler(this.CSCoreBtn_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.TrnsBtn = ((System.Windows.Controls.Button)(target));
            
            #line 67 "..\..\About.xaml"
            this.TrnsBtn.Click += new System.Windows.RoutedEventHandler(this.TrnsBtn_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.MusicUIBtn = ((System.Windows.Controls.Button)(target));
            
            #line 70 "..\..\About.xaml"
            this.MusicUIBtn.Click += new System.Windows.RoutedEventHandler(this.MusicUIBtn_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
