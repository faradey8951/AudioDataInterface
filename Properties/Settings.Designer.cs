﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AudioDataInterface.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "17.1.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int recDeviceId {
            get {
                return ((int)(this["recDeviceId"]));
            }
            set {
                this["recDeviceId"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int playDeviceId {
            get {
                return ((int)(this["playDeviceId"]));
            }
            set {
                this["playDeviceId"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int langId {
            get {
                return ((int)(this["langId"]));
            }
            set {
                this["langId"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int mainWindowHeight {
            get {
                return ((int)(this["mainWindowHeight"]));
            }
            set {
                this["mainWindowHeight"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int mainWindowWidth {
            get {
                return ((int)(this["mainWindowWidth"]));
            }
            set {
                this["mainWindowWidth"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool mainWindowMaximized {
            get {
                return ((bool)(this["mainWindowMaximized"]));
            }
            set {
                this["mainWindowMaximized"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool invertSignal {
            get {
                return ((bool)(this["invertSignal"]));
            }
            set {
                this["invertSignal"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("500")]
        public int signalHeight {
            get {
                return ((int)(this["signalHeight"]));
            }
            set {
                this["signalHeight"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("256")]
        public int mp3BufferSize {
            get {
                return ((int)(this["mp3BufferSize"]));
            }
            set {
                this["mp3BufferSize"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("82000")]
        public int encoderSampleRate {
            get {
                return ((int)(this["encoderSampleRate"]));
            }
            set {
                this["encoderSampleRate"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("2")]
        public int encoderSignalGain {
            get {
                return ((int)(this["encoderSignalGain"]));
            }
            set {
                this["encoderSignalGain"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("2")]
        public int encoderSilenceSeconds {
            get {
                return ((int)(this["encoderSilenceSeconds"]));
            }
            set {
                this["encoderSilenceSeconds"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("128")]
        public int encoderLeadInOutSubcodesAmount {
            get {
                return ((int)(this["encoderLeadInOutSubcodesAmount"]));
            }
            set {
                this["encoderLeadInOutSubcodesAmount"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("3")]
        public int encoderMpsPlayerSubcodeInterval {
            get {
                return ((int)(this["encoderMpsPlayerSubcodeInterval"]));
            }
            set {
                this["encoderMpsPlayerSubcodeInterval"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("512")]
        public int mpsFftSize {
            get {
                return ((int)(this["mpsFftSize"]));
            }
            set {
                this["mpsFftSize"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("peakHold")]
        public string mpsSpectrumMode {
            get {
                return ((string)(this["mpsSpectrumMode"]));
            }
            set {
                this["mpsSpectrumMode"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("25")]
        public int mpsSpectrumVescosity {
            get {
                return ((int)(this["mpsSpectrumVescosity"]));
            }
            set {
                this["mpsSpectrumVescosity"] = value;
            }
        }
    }
}
