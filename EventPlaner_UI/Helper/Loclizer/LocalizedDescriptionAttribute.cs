using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlaner_UI.Helper.Loclizer
{
    public class LocalizedDescriptionAttribute : DescriptionAttribute
    {
        private readonly string _resourceKey;
        public LocalizedDescriptionAttribute(string resourceKey, Type resourceType)
        {
            _resourceKey = resourceKey;
        }

        //public override string Description
        //{
        //    get
        //    {
        //        string displayName = ResourceHelper.Resources.Get(_resourceKey, typeof(Resource));

        //        return string.IsNullOrEmpty(displayName)
        //            ? string.Format("[[{0}]]", _resourceKey)
        //            : displayName;
        //    }
        //}
    }

}
