using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Lunggo.ApCommon.Helper
{
    public enum AutoCompleteType
    {
        None,
        Airline,
        Airport,
        City,
        Country,
        Hotel
    }

    public static class AutoCompleteHelper
    {

        public static MvcHtmlString Autocomplete(this HtmlHelper helper, string name, string value, string text, string actionUrl, bool? isRequired = false, IDictionary<string, object> viewhtmlAttributes = null, string onselectfunction = "")
        {
            return GetAutocompleteString(helper, name, value, text, actionUrl, viewhtmlAttributes, isRequired, onselectfunction: onselectfunction);
        }
        private static MvcHtmlString GetAutocompleteString(HtmlHelper helper, string name, string value, string text, string actionUrl = "", IDictionary<string, object> viewhtmlAttributes = null, bool? isRequired = false, string onselectfunction = "")
        {
            if(viewhtmlAttributes==null)
                viewhtmlAttributes=new Dictionary<string, object>();
            
            viewhtmlAttributes.Add("data-autocomplete", true);

            viewhtmlAttributes.Add("data-sourceurl", actionUrl);

            
            viewhtmlAttributes.Add("data-valuetarget", name);

            if (!string.IsNullOrEmpty(onselectfunction))
            {
                viewhtmlAttributes.Add("data-electfunction", onselectfunction);
            }
            if (isRequired.HasValue && isRequired.Value)
            {
                viewhtmlAttributes.Add("data-val", "true");
                viewhtmlAttributes.Add("data-val-required", name + " is required");
            }

            var hidden = helper.Hidden(name, value);

            var textBox = helper.TextBox(name + "_AutoComplete", text, viewhtmlAttributes);

            var builder = new StringBuilder();

            builder.AppendLine(hidden.ToHtmlString());

            builder.AppendLine(textBox.ToHtmlString());

            return new MvcHtmlString(builder.ToString());
        }
        public static MvcHtmlString AutocompleteFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, string displayProperty, string actionUrl, AutoCompleteType autoCompleteType, IDictionary<string, object> viewhtmlAttributes = null, bool? isRequired = false, string onselectfunction = "")
        {
            return GetAutocompleteForString(helper, expression, displayProperty, actionUrl, autoCompleteType, viewhtmlAttributes, isRequired, onselectfunction: onselectfunction);
        }

        private static MvcHtmlString GetAutocompleteForString<TModel, TValue>(HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, string displayText, string actionUrl, AutoCompleteType autoCompleteType, IDictionary<string, object> viewhtmlAttributes = null, bool? isRequired = false, string onselectfunction = "")
        {
               if(viewhtmlAttributes==null)
                viewhtmlAttributes=new Dictionary<string, object>();

            viewhtmlAttributes.Add("data-autocomplete", true );

            viewhtmlAttributes.Add("data-sourceurl", actionUrl );

            viewhtmlAttributes.Add("data-autocompletetype", autoCompleteType.ToString());
            
            if (!string.IsNullOrEmpty(onselectfunction))
            {
                viewhtmlAttributes.Add("data-electfunction", onselectfunction);
            }
            Func<TModel, TValue> method = expression.Compile();
            object value = null;
            if (helper.ViewData.Model != null)
                value = method((TModel)helper.ViewData.Model);             

            string modelpropname = ((MemberExpression)expression.Body).ToString();

            modelpropname = modelpropname.Substring(modelpropname.IndexOf('.') + 1);

            viewhtmlAttributes.Add("data-valuetarget", modelpropname);

            
            if (isRequired.HasValue && isRequired.Value)
            {   viewhtmlAttributes.Add("data-val", "true");
                viewhtmlAttributes.Add("data-val-required", modelpropname + " is required");            
            }


            MvcHtmlString hidden = helper.HiddenFor(expression);

            MvcHtmlString textBox = helper.TextBox(modelpropname+"_AutoComplete", displayText, viewhtmlAttributes);           

            var builder = new StringBuilder();

            builder.AppendLine(hidden.ToHtmlString());

            builder.AppendLine(textBox.ToHtmlString());

            return new MvcHtmlString(builder.ToString());
        }

    }
     
}