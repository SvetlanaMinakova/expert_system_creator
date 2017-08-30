using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;


public static class HtmlHelperExtensions
{

  public static MvcHtmlString Part_Modal(this HtmlHelper helper,
                                         string modalid,
                                         string viewname = null,
                                         MvcHtmlString content = null,
                                         bool showheader = false,
                                         string header = null,
                                         bool showfooter = false,
                                         int width = 0)
  {
    helper.ViewBag._component_modal_modalid = modalid;
    helper.ViewBag._component_modal_viewname = viewname;
    helper.ViewBag._component_modal_content = content;
    helper.ViewBag._component_modal_showheader = showheader;
    helper.ViewBag._component_modal_header = header;
    helper.ViewBag._component_modal_showfooter = showfooter;
    helper.ViewBag._component_modal_width = width;

    return helper.Partial("~/Views/_components/part_Modal.cshtml");
  }

  public static MvcHtmlString Part_ModalButton(this HtmlHelper helper, string modalid, string content, string htmlclass = "btn")
  {
    return Part_ModalButton(helper, modalid, MvcHtmlString.Create(content), htmlclass);
  }

  public static MvcHtmlString Part_ModalButton(this HtmlHelper helper, string modalid, MvcHtmlString content, string htmlclass = "btn")
  {
    helper.ViewBag._component_modalbutton_modalid = modalid;
    helper.ViewBag._component_modalbutton_content = content;
    helper.ViewBag._component_modalbutton_class = htmlclass;

    return helper.Partial("~/Views/_components/part_showmodal_button.cshtml");
  }

  public static MvcHtmlString Part_Alert(this HtmlHelper helper)
  {
    return helper.Partial("~/Views/_components/part_Alert.cshtml");
  }

  public static string CurrentController(this HttpRequestBase request)
  {
    return request.RequestContext.RouteData.Values["controller"].ToString();
  }
}
