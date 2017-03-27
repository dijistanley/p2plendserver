using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Common
{
    public class AppContext
    {
        public const string Name = "AppContext";

        public AppContext() { }

        public string Error
        {
            get;
            private set;
        }

        public string ErrorDescription
        {
            get;
            private set;
        }

        public bool HasError
        {
            get
            {
                return !string.IsNullOrEmpty(Error);
            }
        }

        public void SetError(string error)
        {
            Error = error;
        }

        public void SetError(string error, string description)
        {
            Error = error;
            ErrorDescription = description;
        }
        
        public void ClearError()
        {
            Error = string.Empty;
            ErrorDescription = string.Empty;
        }

        public static AppContext Current
        {
            get
            {
                return HttpContext.Current.Items[AppContext.Name] as AppContext;
            }
            set
            {
                HttpContext.Current.Items[AppContext.Name] = value;
            }
        }
    }
}
