﻿#pragma checksum "..\..\LTranlate.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "E65A387CB6E6B244900A5442C34FC936945E8F57"
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
    /// LTranlate
    /// </summary>
    public partial class LTranlate : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 23 "..\..\LTranlate.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox InputBox;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\LTranlate.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BtnSearch;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\LTranlate.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BtnCopy;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\LTranlate.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox OutputBox;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\LTranlate.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MaterialDesignThemes.Wpf.Snackbar SnakeB;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\LTranlate.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MaterialDesignThemes.Wpf.SnackbarMessage SnakeBarMessage;
        
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
            System.Uri resourceLocater = new System.Uri("/LrcEditor;component/ltranlate.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\LTranlate.xaml"
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
            
            #line 9 "..\..\LTranlate.xaml"
            ((LrcEditor.LTranlate)(target)).Closing += new System.ComponentModel.CancelEventHandler(this.Window_Closing);
            
            #line default
            #line hidden
            return;
            case 2:
            this.InputBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.BtnSearch = ((System.Windows.Controls.Button)(target));
            
            #line 25 "..\..\LTranlate.xaml"
            this.BtnSearch.Click += new System.Windows.RoutedEventHandler(this.BtnSearch_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.BtnCopy = ((System.Windows.Controls.Button)(target));
            
            #line 28 "..\..\LTranlate.xaml"
            this.BtnCopy.Click += new System.Windows.RoutedEventHandler(this.BtnCopy_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.OutputBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            this.SnakeB = ((MaterialDesignThemes.Wpf.Snackbar)(target));
            return;
            case 7:
            this.SnakeBarMessage = ((MaterialDesignThemes.Wpf.SnackbarMessage)(target));
            
            #line 35 "..\..\LTranlate.xaml"
            this.SnakeBarMessage.ActionClick += new System.Windows.RoutedEventHandler(this.SnakeBarMessage_ActionClick);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

