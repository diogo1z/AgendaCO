using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace AgendaClinica.Controllers
{
    public class ExtensaoController : Controller
    {
        public const string SystemMessage = "MY_DIALOG";

        protected void ShowMessage(string htmlContent, string htmlTitle = "Mensagem do Sistema", MyDialog.DialogType type = MyDialog.DialogType.Success)
        {
            this.ShowMessage(new MyDialog { Title = htmlTitle, Content = htmlContent, @Type = type });
        }

        protected void ShowMessage(MyDialog dialog)
        {
            this.TempData["SystemMessage"] = dialog.ToString();
        }        
    }

    public class MyDialog
    {
        public enum DialogType : short
        {
            Info = 0,
            Success = 1,
            Warning = 2,
            Error = 3
        }

        public string Title { get; set; }
        public string Content { get; set; }
        public DialogType @Type { get; set; }

        public override string ToString()
        {
            return string.Format("{{ \"title\": \"{0}\", \"content\": \"{1}\", \"type\": \"{2}\"  }}", Title, Content, @Type.ToString().ToLower());
        }
    }
}