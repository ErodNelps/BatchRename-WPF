using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WindowsProgramming
{
    //Detail Data Template Selector
    public class DetailDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DefaultDetailTemplate { get; set; }
        public DataTemplate NewCaseDetailTemplate { get; set; }
        public DataTemplate RemovePatternDetailTemplate { get; set; }
        public DataTemplate ReplaceDetailTemplate { get; set; }
        public DataTemplate TrimDetailTemplate { get; set; }
        public DataTemplate MoveDetailTemplate { get; set; }
        public DataTemplate NewNameDetailTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            IMethodArgs args = item as IMethodArgs;
            if(item == null || args.methodType == "FullnameNormalize" || args.methodType == "UniqueID")
                return ((FrameworkElement)container).FindResource("DefaultDetailTemplate") as DataTemplate;
            return ((FrameworkElement)container).FindResource(args.methodType + "DetailTemplate") as DataTemplate;
        }
    }

}
