using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using XueQiaoFoundation.UI.Controls.AutoCompleteTextBox;

namespace XueQiaoFoundation.UI.Extra.helper
{
    public delegate ICollectionView CollectionViewSuggestionGetter(string filter);

    public class CollectionViewSuggestionProvider : ISuggestionProvider
    {
        public CollectionViewSuggestionProvider(CollectionViewSuggestionGetter suggestionGetter)
        {
            this.SuggestionGetter = suggestionGetter;
        }

        public CollectionViewSuggestionGetter SuggestionGetter { get; private set; }

        public IEnumerable GetSuggestions(string filter)
        {
            filter = filter?.Trim();
            var list = SuggestionGetter?.Invoke(filter);
            return list;
        }
    }
}
