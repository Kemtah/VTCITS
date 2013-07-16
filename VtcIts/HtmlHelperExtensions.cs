using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Routing;


namespace VtcIts {

    public static class HtmlHelperExtensions {

        public static MvcHtmlString CheckBoxListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
                                                                       Expression<Func<TModel, IEnumerable<TProperty>>> expression,
                                                                       MultiSelectList multiSelectList, object htmlAttributes = null) {
            //Derive property name for checkbox name
            var memberExp = expression.Body as MemberExpression;
            var propertyName = memberExp.Member.Name;

            //Get currently select values from the ViewData model
            var list = expression.Compile().Invoke(htmlHelper.ViewData.Model);

            //Convert selected value list to a List<string> for easy manipulation
            var selectedValues = new List<string>();
            var bindingPropertyName = multiSelectList.DataValueField;

            if (list != null) {
                var type = typeof (TProperty);
                var property = type.GetProperty(bindingPropertyName);
                selectedValues = list.Select(val => property.GetValue(val).ToString()).ToList();
            }

            //Create div
            var divTag = new TagBuilder("div");
            divTag.MergeAttributes(new RouteValueDictionary(htmlAttributes), true);

            //Add checkboxes
            foreach (var item in multiSelectList) {
                divTag.InnerHtml += String.Format("<div><input type=\"checkbox\" name=\"{0}\" id=\"{0}_{1}\" " +
                                                  "value=\"{1}\" {2} /><label for=\"{0}_{1}\">{3}</label></div>",
                                                  propertyName,
                                                  item.Value,
                                                  selectedValues.Contains(item.Value) ? "checked=\"checked\"" : "",
                                                  item.Text);
            }

            return MvcHtmlString.Create(divTag.ToString());
        }



        public static MvcHtmlString RadioButtonListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
                                                                          Expression<Func<TModel, TProperty>> expression,
                                                                          SelectList selectList, object htmlAttributes = null) {
            //Derive property name for checkbox name
            var memberExp = expression.Body as MemberExpression;
            var propertyName = memberExp.Member.Name;

            //Get currently select values from the ViewData model
            var selectedValue = expression.Compile().Invoke(htmlHelper.ViewData.Model);

            //Create div
            var divTag = new TagBuilder("div");
            divTag.MergeAttributes(new RouteValueDictionary(htmlAttributes), true);

            //Add checkboxes
            foreach (var item in selectList) {
                divTag.InnerHtml += String.Format("<div><input type='radio' name='{0}' id='{0}_{1}' value='{1}' {2} />{3}</div>",
                                                  propertyName,
                                                  item.Value,
                                                  selectedValue != null && selectedValue.ToString().Equals(item.Value) ? "checked=\"checked\"" : "",
                                                  item.Text);
            }

            return MvcHtmlString.Create(divTag.ToString());
        }



        public static MvcHtmlString DisplayListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
                                                                       Expression<Func<TModel, IEnumerable<TProperty>>> expression,
                                                                       string displayProperty = null, object htmlAttributes = null) {
            //Get currently select values from the ViewData model
            var list = expression.Compile().Invoke(htmlHelper.ViewData.Model);

            //Convert selected value list to a List<string> for easy manipulation
            var values = new List<string>();

            if (list != null) {
                if (!String.IsNullOrEmpty(displayProperty)) {
                    var type = typeof (TProperty);
                    var property = type.GetProperty(displayProperty);
                    values = list.Select(val => property.GetValue(val).ToString()).ToList();
                }
                else {
                    values = list.Select(val => val.ToString()).ToList();
                }
            }

            //Create ul
            var ulTag = new TagBuilder("ul");
            ulTag.MergeAttributes(new RouteValueDictionary(htmlAttributes), true);
            
            //Add to list
            foreach (var val in values) {
                ulTag.InnerHtml += String.Format("<li>{0}</li>", val);
            }

            var divTag = new TagBuilder("div") {InnerHtml = ulTag.ToString()};

            return MvcHtmlString.Create(divTag.ToString());
        }



        public static MvcHtmlString DisplayEnumFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
                                                                      Expression<Func<TModel, TProperty>> expression,
                                                                      object htmlAttributes = null) {
            //Get current value from the ViewData model
            var value = expression.Compile().Invoke(htmlHelper.ViewData.Model);
            var type = typeof (TProperty);

            if (!type.IsEnum) {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof (Nullable<>)) {
                    type = Nullable.GetUnderlyingType(type);
                }

                if (!type.IsEnum) { // if we're *still* not an enumeration...
                    throw new NotSupportedException("Method cannot be invoked for non-enumeration type");
                }
            }

            var spanTag = new TagBuilder("span") {InnerHtml = value.Equals(null) ? "&nbsp;" : ((Enum) Enum.ToObject(type, value)).ToPrintText()};
            spanTag.MergeAttributes(new RouteValueDictionary(htmlAttributes), true);

            return MvcHtmlString.Create(spanTag.ToString());
        }



        public static MvcHtmlString DisplayWithBreaksFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression) {
            var value = expression.Compile().Invoke(htmlHelper.ViewData.Model);
            var model = htmlHelper.Encode(value).Replace("\r\n", "<br />\r\n");

            return string.IsNullOrEmpty(model) ? MvcHtmlString.Empty : MvcHtmlString.Create(model);
        }



        public static MvcHtmlString RenderForRole(this HtmlHelper helper, string role, MvcHtmlString html) {
            return helper.ViewContext.HttpContext.User.IsInRole(role) ? html : null;
        }
    }

}
