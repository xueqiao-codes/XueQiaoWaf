using ContainerShell.Applications.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Input;

namespace ContainerShell.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class FeedbackVM : ViewModel<IFeedbackView>
    {
        [ImportingConstructor]
        public FeedbackVM(IFeedbackView view) : base(view)
        {
        }

        private string contractPersonName;
        public string ContractPersonName
        {
            get { return contractPersonName; }
            set { SetProperty(ref contractPersonName, value); }
        }

        private string contractInformation;
        public string ContractInformation
        {
            get { return contractInformation; }
            set { SetProperty(ref contractInformation, value); }
        }

        private ICommand selectPictureCmd;
        public ICommand SelectPictureCmd
        {
            get { return selectPictureCmd; }
            set { SetProperty(ref selectPictureCmd, value); }
        }

        private ICommand deletePictureCmd;
        public ICommand DeletePictureCmd
        {
            get { return deletePictureCmd; }
            set { SetProperty(ref deletePictureCmd, value); }
        }
        
        private string selectedPictureName;
        public string SelectedPictureName
        {
            get { return selectedPictureName; }
            set
            {
                SetProperty(ref selectedPictureName, value);
                this.IsSelectedPicture = !string.IsNullOrEmpty(value);
            }
        }

        private bool isSelectedPicture;
        public bool IsSelectedPicture
        {
            get { return isSelectedPicture; }
            private set { SetProperty(ref isSelectedPicture, value); }
        }

        private bool isUploadLogChecked;
        public bool IsUploadLogChecked
        {
            get { return isUploadLogChecked; }
            set { SetProperty(ref isUploadLogChecked, value); }
        }

        private string feedbackContent;
        public string FeedbackContent
        {
            get { return feedbackContent; }
            set { SetProperty(ref feedbackContent, value); }
        }

        private ICommand submitCmd;
        public ICommand SubmitCmd
        {
            get { return submitCmd; }
            set { SetProperty(ref submitCmd, value); }
        }

        private ICommand cancelCmd;
        public ICommand CancelCmd
        {
            get { return cancelCmd; }
            set { SetProperty(ref cancelCmd, value); }
        }

        private bool isSubmiting;
        public bool IsSubmiting
        {
            get { return isSubmiting; }
            set { SetProperty(ref isSubmiting, value); }
        }

    }
}
