﻿#pragma checksum "..\..\..\Views\CreateManualClickWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "4063353C0BE53D2BE8138F6DA29EC51AD7425CAEDDEDDA73728238FE42037196"
//------------------------------------------------------------------------------
// <auto-generated>
//     O código foi gerado por uma ferramenta.
//     Versão de Tempo de Execução:4.0.30319.42000
//
//     As alterações ao arquivo poderão causar comportamento incorreto e serão perdidas se
//     o código for gerado novamente.
// </auto-generated>
//------------------------------------------------------------------------------

using MouseKeyPlayback.Views;
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


namespace MouseKeyPlayback.Views {
    
    
    /// <summary>
    /// CreateManualClickWindow
    /// </summary>
    public partial class CreateManualClickWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 12 "..\..\..\Views\CreateManualClickWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cbxMouseButton;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\..\Views\CreateManualClickWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cbxMouseAction;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\..\Views\CreateManualClickWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tbxX;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\..\Views\CreateManualClickWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tbxY;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\..\Views\CreateManualClickWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnOk;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\..\Views\CreateManualClickWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnCancel;
        
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
            System.Uri resourceLocater = new System.Uri("/MouseKeyPlayback;component/views/createmanualclickwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Views\CreateManualClickWindow.xaml"
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
            this.cbxMouseButton = ((System.Windows.Controls.ComboBox)(target));
            
            #line 12 "..\..\..\Views\CreateManualClickWindow.xaml"
            this.cbxMouseButton.Initialized += new System.EventHandler(this.CbxMouseButton_Initialized);
            
            #line default
            #line hidden
            return;
            case 2:
            this.cbxMouseAction = ((System.Windows.Controls.ComboBox)(target));
            
            #line 14 "..\..\..\Views\CreateManualClickWindow.xaml"
            this.cbxMouseAction.Initialized += new System.EventHandler(this.CbxMouseAction_Initialized);
            
            #line default
            #line hidden
            return;
            case 3:
            this.tbxX = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.tbxY = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.btnOk = ((System.Windows.Controls.Button)(target));
            
            #line 19 "..\..\..\Views\CreateManualClickWindow.xaml"
            this.btnOk.Click += new System.Windows.RoutedEventHandler(this.BtnOk_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.btnCancel = ((System.Windows.Controls.Button)(target));
            
            #line 20 "..\..\..\Views\CreateManualClickWindow.xaml"
            this.btnCancel.Click += new System.Windows.RoutedEventHandler(this.BtnCancel_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

