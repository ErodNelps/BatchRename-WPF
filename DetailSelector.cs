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
        public DataTemplate NewCaseDetailTemplate { get; set; }
        public DataTemplate RemoveDetailTemplate { get; set; }
        public DataTemplate ReplaceDetailTemplate { get; set; }
        public DataTemplate TrimDetailTemplate { get; set; }
        public DataTemplate MoveDetailTemplate { get; set; }
        public DataTemplate NewNameDetailTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            ReplaceArgs args = item as ReplaceArgs;
            var strings = item.ToString();
            return ((FrameworkElement)container).FindResource("ReplaceDetailTemplate") as DataTemplate;
        }
    }

}
