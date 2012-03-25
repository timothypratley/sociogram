using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using GraphSharp.Controls;
using QuickGraph;
using System.IO;

using MyType = System.Object;

namespace sociogram
{
    public class ViewModel : INotifyPropertyChanged
    {
        public ViewModel()
        {
            LayoutAlgorithmType = layoutAlgorithmTypes[0];
        }

        public BidirectionalGraph<MyType, IEdge<MyType>> Graph
        {
            get { return graph; }
            set { graph = value; OnPropertyChanged("Graph"); }
        }
        private BidirectionalGraph<MyType, IEdge<MyType>> graph = new BidirectionalGraph<MyType, IEdge<MyType>>();

        public void ReadGraph(TextReader reader) {
            var g = new BidirectionalGraph<MyType, IEdge<MyType>>();
            try {
                var csv = new CsvHelper.CsvReader(reader);
                while (csv.Read()) {
                    if (!csv.CurrentRecord.Any()) {
                        continue;
                    }
                    var node = csv.CurrentRecord.First();
                    var edges = csv.CurrentRecord.Skip(1)
                        .Where(x => !string.IsNullOrEmpty(x))
                        .Select(x => x.Trim())
                        .Where(x => !string.IsNullOrEmpty(x));
                    foreach (var edge in edges) {
                        g.AddVerticesAndEdge(new Edge<MyType>(node, edge));
                    }
                }

                var dict = new Dictionary<int, HashSet<MyType>>();
                HashSet<MyType> set;
                foreach (var v in g.Vertices) {
                    var edgeCount = g.InEdges(v).Count();
                    if (!dict.TryGetValue(edgeCount, out set)) {
                        set = new HashSet<MyType>();
                        dict.Add(edgeCount, set);
                    }
                    set.Add(v);
                }

                Graph = g;

                Summary = string.Join(Environment.NewLine,
                    dict
                        .OrderBy(kvp => kvp.Key)
                        .Select(kvp => kvp.Key + ": " + string.Join(", ", kvp.Value)))
                    + Environment.NewLine
                    + Environment.NewLine
                    + string.Join(Environment.NewLine,
                        g.Vertices
                            .Where(v => g.OutEdges(v).Count() != 3)
                            .Select(v => v + ": " + g.OutEdges(v).Count()));

                //Summary = string.Join(
                //    Environment.NewLine,
                //    graph.Vertices
                //        .OrderBy(x => graph.InEdges(x).Count())
                //        .Select(v => v + ": " + graph.InEdges(v).Count()));
            } catch (Exception e) {
                Summary = "Failed to read:" + Environment.NewLine + e;
            }
        }

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

        public string Summary {
            get { return summary; }
            set {
                summary = value;
                OnPropertyChanged("Summary");
            }
        }
        private string summary;

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        #endregion
    }
}
