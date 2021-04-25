namespace XueQiaoFoundation.UI.Controls.AutoCompleteTextBox
{
    using System.Collections;

    public interface ISuggestionProvider
    {

        #region Public Methods

        IEnumerable GetSuggestions(string filter);

        #endregion Public Methods

    }
}
