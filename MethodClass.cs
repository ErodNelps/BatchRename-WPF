using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WindowsProgramming;

namespace WindowsProgramming
{
    public delegate void UpdateEventHandler(object sender, UpdateEvent e);
    public class UpdateEvent : EventArgs
    {
    }
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
        public List<string> style = new List<string>()
        {
            "Case1", "Case2", "Case3"
        };
        public string selectedStyle { get; set; }
        public List<String> Style { get => style; }
    }
    //Replace Args
    [Serializable]
    public class ReplaceArgs : IMethodArgs, INotifyPropertyChanged
    {
        public string methodType
        {
            get
            {
                return "Replace";
            }
        }
        private string _target;
        private string _replacer;
        public string Target {
            get { return _target; }
            set { _target = value; RaiseChangeEvent("Target"); }
        }
        public string Replacer{
            get { return _replacer; }
            set { _replacer = value; RaiseChangeEvent("Replacer"); }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        void RaiseChangeEvent(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
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
        public int initialPos { get; set; }
        public int Length { get; set; }
    }
    [Serializable]
    public class FullnameNormalizeArgs : IMethodArgs
    {
        public string methodType
        {
            get
            {
                string result = "FullnameNormalize";
                return result;
            }
        }
    }
    [Serializable]
    public class UniqueIDArgs : IMethodArgs
    {
        public string methodType
        {
            get
            {
                string result = "UniqueID";
                return result;
            }
        }
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
        event UpdateEventHandler newNameEvent;
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
            var result = origin;
            if (args.Target =="" && args.Replacer =="")
            {
                return result;
            }
            result = origin.Replace(args.Target, args.Replacer);
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
        [field: NonSerialized]
        public event UpdateEventHandler newNameEvent;
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
                UpdateEvent e1 = new UpdateEvent();
                var args = methodArgs as ReplaceArgs;
                if (newNameEvent != null)
                {
                    newNameEvent(args, e1);
                }
                e1 = null;             
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
            string result = " ";
            return result;
        }
        public string Description
        {
            get
            {
                var args = methodArgs as NewCaseArgs;
                var result = args.selectedStyle;
                return result;
            }
        }

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
        [field: NonSerialized]
        public event UpdateEventHandler newNameEvent;
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
    public class FullnameNormalizeAction : IMethodAction, INotifyPropertyChanged
    {
        public string MethodName { get; set; }
        public bool IsChecked { get; set; }
        public IMethodArgs methodArgs { get; set; }
        public string Process(string origin)
        {
            var result = FullNameNormalize(origin);
            return result;
        }
        private static string FullNameNormalize(string source)
        {
            if (source == null)
                return null;

            var tempString = source;

            //Remove extra space
            tempString = String.Join(" ", tempString.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries));
            tempString = tempString.ToLower();
            CultureInfo culture_info = Thread.CurrentThread.CurrentCulture;
            TextInfo text_info = culture_info.TextInfo;
            tempString = text_info.ToTitleCase(tempString);

            return tempString;
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
        [field: NonSerialized]
        public event UpdateEventHandler newNameEvent;
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
                string message = " Updated";
                UpdateEvent e1 = new UpdateEvent();
                var args = methodArgs as ReplaceArgs;
                if (newNameEvent != null) ;
                {
                    newNameEvent(args, e1);
                }

                e1 = null;
                RaiseChangeEvent("Description");
            }
        }
    }
    [Serializable]
    public class UniqueIDAction : IMethodAction, INotifyPropertyChanged
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
                var args = methodArgs as NewCaseArgs;
                var result = args.selectedStyle;
                return result;
            }
        }
        private static string UniqueName(string source)
        {
            if (source == null)
                return null;

            string guid = Guid.NewGuid().ToString();

            return guid;
        }
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
        [field: NonSerialized]
        public event UpdateEventHandler newNameEvent;
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
        [field: NonSerialized]
        public event UpdateEventHandler newNameEvent;
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
            string result = " ";
            return result;
        }
        public string Description
        {
            get
            {
                var args = methodArgs as TrimArgs;
                var result = $"Trim character(s): ";
                return result;
            }
        }

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
        [field: NonSerialized]
        public event UpdateEventHandler newNameEvent;
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
            if (args.Length > 0 && args.FromPos > 0 && args.FromPos < origin.Length && args.ToPos > 0 && args.ToPos < origin.Length )
            {
                var substring = "";
                for(int i = args.FromPos-1; i < args.Length; i++)
                {
                    substring += result[i];
                }
                origin = origin.Insert(args.ToPos-1, substring);
                result = origin.Remove(args.FromPos - 1, args.Length);
            }
            return result;
        }
        public string Description
        {
            get
            {
                var args = methodArgs as MoveArgs;
                var result = $"Move {args.Length} character(s) \nfrom position {args.FromPos} to position {args.ToPos}";
                return result;
            }
        }

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
        [field: NonSerialized]
        public event UpdateEventHandler newNameEvent;
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
        [field: NonSerialized]
        public event UpdateEventHandler newNameEvent;
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