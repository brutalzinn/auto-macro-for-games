﻿#pragma checksum "..\..\..\Views\CreateWaitWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "45F3A47FCDEED8AF066F24A2F392AE09DE8183A376F13DA672BFCEF8458D9220"
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
    /// CreateWaitWindow
    /// </summary>
    public partial class CreateWaitWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 15 "..\..\..\Views\CreateWaitWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tbxWait;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\..\Views\CreateWaitWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnOk;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\..\Views\CreateWaitWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnCancel;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\..\Views\CreateWaitWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Expander HeaderControl;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\..\Views\CreateWaitWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox RandomMin;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\..\Views\CreateWaitWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox RandomMax;
        
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
            System.Uri resourceLocater = new System.Uri("/MouseKeyPlayback;component/views/createwaitwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Views\CreateWaitWindow.xaml"
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
            
            #line 8 "..\..\..\Views\CreateWaitWindow.xaml"
            ((MouseKeyPlayback.Views.CreateWaitWindow)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.tbxWait = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.btnOk = ((System.Windows.Controls.Button)(target));
            
            #line 16 "..\..\..\Views\CreateWaitWindow.xaml"
            this.btnOk.Click += new System.Windows.RoutedEventHandler(this.BtnOk_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.btnCancel = ((System.Windows.Controls.Button)(target));
            
            #line 17 "..\..\..\Views\CreateWaitWindow.xaml"
            this.btnCancel.Click += new System.Windows.RoutedEventHandler(this.BtnCancel_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.HeaderControl = ((System.Windows.Controls.Expander)(target));
            
            #line 19 "..\..\..\Views\CreateWaitWindow.xaml"
            this.HeaderControl.Collapsed += new System.Windows.RoutedEventHandler(this.HeaderControl_Collapsed);
            
            #line default
            #line hidden
            
            #line 19 "..\..\..\Views\CreateWaitWindow.xaml"
            this.HeaderControl.TouchDown += new System.EventHandler<System.Windows.Input.TouchEventArgs>(this.HeaderControl_TouchDown);
            
            #line default
            #line hidden
            
            #line 19 "..\..\..\Views\CreateWaitWindow.xaml"
            this.HeaderControl.FocusableChanged += new System.Windows.DependencyPropertyChangedEventHandler(this.HeaderControl_FocusableChanged);
            
            #line default
            #line hidden
            
            #line 19 "..\..\..\Views\CreateWaitWindow.xaml"
            this.HeaderControl.LayoutUpdated += new System.EventHandler(this.HeaderControl_LayoutUpdated);
            
            #line default
            #line hidden
            return;
            case 6:
            this.RandomMin = ((System.Windows.Controls.TextBox)(target));
            return;
            case 7:
            this.RandomMax = ((System.Windows.Controls.TextBox)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

