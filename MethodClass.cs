using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WindowsProgramming
{
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
    public class RemovePatternArgs :IMethodArgs
    {
        public string Pattern { get; set; }
    }
    public class MoveArgs : IMethodArgs
    {
        public string Target { get; set; }
        public int NewPosition { get; set; }
    }
    public class NewNameArgs :IMethodArgs
    {
        public string NewName { get; set; }
    }
    public interface IMethodAction
    {
        string MethodName { get; set; }
        bool IsChecked { get; set; }
        IMethodArgs methodArgs { get; set; }
        string Process(string origin);
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

        public event PropertyChangedEventHandler PropertyChanged;

        void RaiseChangeEvent([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
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
    }
}

