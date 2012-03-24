using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using GraphSharp.Controls;
using QuickGraph;

namespace sociogram
{
    public class ViewModel : INotifyPropertyChanged
    {
        public ViewModel()
        {
            layoutAlgorithmType = layoutAlgorithmTypes[0];
        }

        public BidirectionalGraph<object, IEdge<object>> Graph
        {
            get { return graph; }
            set { graph = value; OnPropertyChanged("Graph"); }
        }
        private BidirectionalGraph<object, IEdge<object>> graph = new BidirectionalGraph<object, IEdge<object>>();

        public IEnumerable<string> LayoutAlgorithmTypes { get { return layoutAlgorithmTypes; } }
        private string[] layoutAlgorithmTypes = new [] {
            "BoundedFR", "Circular", "CompoundFDP", "EfficientSugiyama", "FR", "ISOM", "KK", "LinLog", "Tree"
        };

        public string LayoutAlgorithmType
        {
            get { return layoutAlgorithmType; }
            set
            {
                layoutAlgorithmType = value;
                OnPropertyChanged("LayoutAlgorithmType");
            }
        }
        private string layoutAlgorithmType;

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(String info)
        {
            var ev = PropertyChanged;
            if (ev != null)
            {
                ev(this, new PropertyChangedEventArgs(info));
            }
        }

        #endregion
    }
}
