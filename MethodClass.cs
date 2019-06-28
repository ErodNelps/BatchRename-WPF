﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WindowsProgamming;

namespace WindowsProgramming
{
    //Method arguments interface
    public interface IMethodArgs
    {
        string methodType { get; }
    }
    //New Case Args
    [Serializable]
    public class NewCaseArgs : IMethodArgs
    {
        public string methodType
        {
            get
            {
                return "NewCase";
            }
        }
        public List<string> Style {
            get { return new List<string>() { "1", "2", "3" }; }
        }
    }
    //Replace Args
    [Serializable]
    public class ReplaceArgs : IMethodArgs
    {
        public string methodType
        {
            get
            {
                return "Replace";
            }
        }
        public string Target { get; set; }
        public string Replacer { get; set; }
    }
    //Trim Args
    [Serializable]
    public class TrimArgs : IMethodArgs
    {
        public string methodType
        {
            get
            {
                return "Trim";
            }
        }
        public string trimChars { get; set; }
    }
    //Remove Args
    [Serializable]
    public class RemovePatternArgs : IMethodArgs
    {
        public string methodType
        {
            get
            {
                string result = "RemovePattern";
                return result;
            }
        }
        public string Pattern { get; set; }
    }
    //Move Args
    [Serializable]
    public class MoveArgs : IMethodArgs
    {
        public string methodType
        {
            get
            {
                return "Move";
            }
        }
        public int FromPos { get; set; }
        public int Length { get; set; }
        public int ToPos { get; set; }
    }
    //New Name Args
    [Serializable]
    public class NewNameArgs : IMethodArgs
    {
        public string methodType
        {
            get
            {
                return "NewName";
            }
        }
        public string NewName { get; set; }
    }

    //Method action interface
    public interface IMethodAction
    {
        string MethodName { get; set; }
        bool IsChecked { get; set; }
        IMethodArgs methodArgs { get; set; }
        string Description { get; }
        string Process(string origin);
        void ShowUpdateDetailWindow();
    }

    [Serializable]
    public class ReplaceAction : IMethodAction, INotifyPropertyChanged
    {
        public string MethodName { get; set; }
        public bool IsChecked { get; set; }

        public IMethodArgs methodArgs { get; set; }
        public string Process(string origin)
        {
            var args = methodArgs as ReplaceArgs;
            var result = origin.Replace(args.Target, args.Replacer);
            return result;

        }

        public string Description
        {
            get
            {
                var args = methodArgs as ReplaceArgs;
                var result = $"Replace {args.Target} with {args.Replacer}";
                return result;
            }
        }

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        void RaiseChangeEvent(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public void ShowUpdateDetailWindow()
        {
            var window = new DetailUpdateWindow(
                methodArgs as ReplaceArgs);

            if (window.ShowDialog() == true)
            {
                RaiseChangeEvent("Description");
            }
        }
    }

    [Serializable]
    public class NewCaseAction : IMethodAction, INotifyPropertyChanged
    {
        public string MethodName { get; set; }
        public bool IsChecked { get; set; }
        public IMethodArgs methodArgs { get; set; }

        public string Process(string origin)
        {
            var args = methodArgs as NewCaseArgs;
            var result = "NewCase";
            return result;
        }
        public string Description
        {
            get
            {
                var args = methodArgs as NewCaseArgs;
                var result = "Case: ";
                return result;
            }
        }

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        void RaiseChangeEvent(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
        public void ShowUpdateDetailWindow()
        {
            var window = new DetailUpdateWindow(
                methodArgs as NewCaseArgs);

            if (window.ShowDialog() == true)
            {
                RaiseChangeEvent("Description");
            }
        }
    }

    [Serializable]
    public class RemoveAction : IMethodAction, INotifyPropertyChanged
    {
        public string MethodName { get; set; }
        public bool IsChecked { get; set; }
        public IMethodArgs methodArgs { get; set; }
        public string Process(string origin)
        {
            var args = methodArgs as RemovePatternArgs;
            string result = origin.Replace(args.Pattern, "");
            return result;
        }
        public string Description
        {
            get
            {
                var args = methodArgs as RemovePatternArgs;
                var result = $"Remove {args.Pattern}";
                return result;
            }
        }

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        void RaiseChangeEvent(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
        public void ShowUpdateDetailWindow()
        {
            var window = new DetailUpdateWindow(
                 methodArgs as RemovePatternArgs);

            if (window.ShowDialog() == true)
            {
                RaiseChangeEvent("Description");
            }
        }
    }

    [Serializable]
    public class TrimAction : IMethodAction, INotifyPropertyChanged
    {
        public string MethodName { get; set; }
        public bool IsChecked { get; set; }
        public IMethodArgs methodArgs { get; set; }
        public string Process(string origin)
        {
            var args = methodArgs as TrimArgs;
            var result = origin;
            if (args.trimChars != null) {
                foreach (var token in args.trimChars)
                {
                    result.Trim(token);
                }
            }
            return result;
        }
        public string Description
        {
            get
            {
                var args = methodArgs as TrimArgs;
                var result = $"Trim character(s): " + args.trimChars;
                return result;
            }
        }

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        void RaiseChangeEvent(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
        public void ShowUpdateDetailWindow()
        {
            var window = new DetailUpdateWindow(
                 methodArgs as TrimArgs);

            if (window.ShowDialog() == true)
            {
                RaiseChangeEvent("Description");
            }
        }
    }

    [Serializable]
    public class MoveAction : IMethodAction, INotifyPropertyChanged
    {
        public string MethodName { get; set; }
        public bool IsChecked { get; set; }
        public IMethodArgs methodArgs { get; set; }
        public string Process(string origin)
        {
            var args = methodArgs as MoveArgs;
            var result = origin;
            if(args.Length > 0 && args.FromPos > 0 && args.ToPos > 0 && args.ToPos < result.Length)
            {
                //result[args.FromPos]
            }
            return result;
        }
        public string Description
        {
            get
            {
                var args = methodArgs as MoveArgs;
                var result = $"Move {args.Length} character(s) from position {args.FromPos} to position {args.ToPos}";
                return result;
            }
        }

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        void RaiseChangeEvent(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
        public void ShowUpdateDetailWindow()
        {
            var window = new DetailUpdateWindow(
                 methodArgs as MoveArgs);

            if (window.ShowDialog() == true)
            {
                RaiseChangeEvent("Description");
            }
        }
    }

    [Serializable]
    public class NewNameAction : IMethodAction, INotifyPropertyChanged
    {
        public string MethodName { get; set; }
        public bool IsChecked { get; set; }
        public IMethodArgs methodArgs { get; set; }
        public string Process(string origin)
        {
            var args = methodArgs as NewNameArgs;
            var result = args.NewName;
            return result;
        }
        public string Description
        {
            get
            {
                var args = methodArgs as NewNameArgs;
                var result ="New item's name is:"+ args.NewName;
                return result;
            }
        }

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        void RaiseChangeEvent(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
        public void ShowUpdateDetailWindow()
        {
            var window = new DetailUpdateWindow(
                methodArgs as NewNameArgs);

            if (window.ShowDialog() == true)
            {
                RaiseChangeEvent("Description");
            }
        }
    }
}

