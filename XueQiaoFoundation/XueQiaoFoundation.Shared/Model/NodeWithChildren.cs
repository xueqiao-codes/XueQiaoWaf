using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoFoundation.Shared.Model
{
    public class NodeWithChildren<TNode, TChild> : System.Waf.Foundation.Model
    {
        private TNode node;

        public NodeWithChildren()
        {
            this.Children = new ObservableCollection<TChild>();
        }


        public TNode Node
        {
            get { return node; }
            set { SetProperty(ref node, value); }
        }

        public ObservableCollection<TChild> Children { get; private set; }
    }
}
