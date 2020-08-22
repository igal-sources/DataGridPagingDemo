using System;
using System.ComponentModel;
using System.Diagnostics;

namespace DataGridPagingDemo.Helpers
{
    public abstract class ViewModelBase : NotifyObject
    {
        #region Properties
        protected bool ThrowOnInvalidPropertyName
        {
            get;
            private set;
        }

        #endregion

        #region Constructor

        protected ViewModelBase()
        {
            ThrowOnInvalidPropertyName = true;
        }

        #endregion

        protected override void OnPropertyChanged(string propertyName)
        {
            VerifyPropertyName(propertyName);

            base.OnPropertyChanged(propertyName);
        }

        #region Public Methods

        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public void VerifyPropertyName(string propertyName)
        {
            // Verify that the property name matches a real,  
            // public, instance property on this object.
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                var msg = "Invalid property name: " + propertyName;

                if (ThrowOnInvalidPropertyName)
                {
                    throw new Exception(msg);
                }

                Debug.Fail(msg);
            }
        }

        #endregion
    }
}
