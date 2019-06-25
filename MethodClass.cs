using System;
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
    //Method argument interface
    public interface IMethodArgs
    {

    }
    public class ReplaceArgs : IMethodArgs
    {
        public string Target { get; set; }
        public string Replacer { get; set; }
    }
    public class TrimArgs : IMethodArgs
    {
        public int initialPos { get; set; }
        public int Length { get; set; }
    }
    public class RemovePatternArgs : IMethodArgs
    {
        public string Pattern { get; set; }
    }
    public class MoveArgs : IMethodArgs
    {
        public string Target { get; set; }
        public int NewPosition { get; set; }
    }
    public class NewNameArgs : IMethodArgs
    {
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

    public class ReplaceAction : IMethodAction
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
                var result = "Replace";
                return result;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void RaiseChangeEvent(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public void ShowUpdateDetailWindow()
        {
            var screen = new DetailUpdateWindow(
                methodArgs as ReplaceArgs);

            if (screen.ShowDialog() == true)
            {
                RaiseChangeEvent("Description");
            }
        }
    }
    public class NewCaseAction : IMethodAction
    {
        public string MethodName { get; set; }
        public bool IsChecked { get; set; }
        public IMethodArgs methodArgs { get; set; }
        public string Process(string origin)
        {
            string result = " ";
            return result;
        }
        public string Description
        {
            get
            {
                var args = methodArgs as ReplaceArgs;
                var result = "NewCase";
                return result;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void RaiseChangeEvent(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
        public void ShowUpdateDetailWindow()
        {
            var screen = new DetailUpdateWindow(
                methodArgs as ReplaceArgs);

            if (screen.ShowDialog() == true)
            {
                RaiseChangeEvent("Description");
            }
        }
    }
    public class RemoveAction : IMethodAction
    {
        public string MethodName { get; set; }
        public bool IsChecked { get; set; }
        public IMethodArgs methodArgs { get; set; }
        public string Process(string origin)
        {
            string result = " ";
            return result;
        }
        public string Description
        {
            get
            {
                var args = methodArgs as RemovePatternArgs;
                var result = "NewCase";
                return result;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void RaiseChangeEvent(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
        public void ShowUpdateDetailWindow()
        {
            var screen = new DetailUpdateWindow(
                 methodArgs as RemovePatternArgs);

            if (screen.ShowDialog() == true)
            {
                RaiseChangeEvent("Description");
            }
        }
    }
    public class TrimAction : IMethodAction
    {
        public string MethodName { get; set; }
        public bool IsChecked { get; set; }
        public IMethodArgs methodArgs { get; set; }
        public string Process(string origin)
        {
            string result = " ";
            return result;
        }
        public string Description
        {
            get
            {
                var args = methodArgs as TrimArgs;
                var result = "NewCase";
                return result;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void RaiseChangeEvent(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
        public void ShowUpdateDetailWindow()
        {
            var screen = new DetailUpdateWindow(
                 methodArgs as TrimArgs);

            if (screen.ShowDialog() == true)
            {
                RaiseChangeEvent("Description");
            }
        }
    }
    public class MoveAction : IMethodAction
    {
        public string MethodName { get; set; }
        public bool IsChecked { get; set; }
        public IMethodArgs methodArgs { get; set; }
        public string Process(string origin)
        {
            string result = " ";
            return result;
        }
        public string Description
        {
            get
            {
                var args = methodArgs as ReplaceArgs;
                var result = "NewCase";
                return result;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void RaiseChangeEvent(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
        public void ShowUpdateDetailWindow()
        {
            RaiseChangeEvent("Description");
        }
    }
    public class NewNameAction : IMethodAction
    {
        public string MethodName { get; set; }
        public bool IsChecked { get; set; }
        public IMethodArgs methodArgs { get; set; }
        public string Process(string origin)
        {
            string result = " ";
            return result;
        }
        public string Description
        {
            get
            {
                var args = methodArgs as ReplaceArgs;
                var result = "NewCase";
                return result;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void RaiseChangeEvent(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
        public void ShowUpdateDetailWindow()
        {
            RaiseChangeEvent("Description");
        }
    }
}

