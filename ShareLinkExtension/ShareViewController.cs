using System;
using System.IO;
using Foundation;
using MobileCoreServices;
using Social;

namespace ShareLinkExtension
{
    public partial class ShareViewController : SLComposeServiceViewController
    {
        protected ShareViewController(IntPtr handle) : base(handle)
        {
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Do any additional setup after loading the view.
        }

        public override bool IsContentValid()
        {
            return true;
        }

        public override void DidSelectPost()
        {
            var description = "";
            var url = "";

            foreach (var extensionItem in ExtensionContext.InputItems)
            {
                if (extensionItem.Attachments != null)
                {
                    foreach (var attachment in extensionItem.Attachments)
                    {
                        if (attachment.HasItemConformingTo(UTType.URL))
                        {
                            attachment.LoadItem(UTType.URL, null, (data, error) =>
                            {
                                var nsUrl = data as NSUrl;
                                url = nsUrl.AbsoluteString;
                                WriteToDebugFile($"URL - {url}");
                                //Save  off the url and description here
                            });
                        }
                    }
                }

                if (!string.IsNullOrWhiteSpace(extensionItem.AttributedContentText.Value))
                {
                    description = extensionItem.AttributedContentText.Value;
                    WriteToDebugFile($"URL description - {description}");
                }
            }

            ExtensionContext.CompleteRequest(new NSExtensionItem[0], null);
        }

        public override SLComposeSheetConfigurationItem[] GetConfigurationItems()
        {
            return new SLComposeSheetConfigurationItem[0];
        }

        private void WriteToDebugFile(string dbgText)
        {
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var filename = Path.Combine(documents, "Debug.txt");

            if (!File.Exists(filename))
            {
                File.WriteAllText(filename, $"\n{DateTime.Now} - {dbgText}");
            }
            else
            {
                File.AppendAllText(filename, $"\n{DateTime.Now} - {dbgText}");
            }
        }
    }
}
