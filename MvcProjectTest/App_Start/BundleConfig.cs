using System.Web;
using System.Web.Optimization;

namespace MvcProjectTest
{
    public class BundleConfig
    {
        // 如需統合的詳細資訊，請瀏覽 https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // 使用開發版本的 Modernizr 進行開發並學習。然後，當您
            // 準備好可進行生產時，請使用 https://modernizr.com 的建置工具，只挑選您需要的測試。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
            bundles.Add(new StyleBundle("~/css/base").Include(
                "~/Assets/Css/index_stylesheet.css",
                "~/Assets/Css/for-change-style.css",
                "~/Assets/Css/RWD.css"

                ));

            bundles.Add(new StyleBundle("~/css/indexView").Include(

                "~/Assets/Css/new-product-card.css",
                "~/Assets/Css/newest-tab-block.css",
                "~/Assets/Css/index-bigimage.css",
                "~/Assets/Css/RWD.css"));
            bundles.Add(new StyleBundle("~/css/cartAndWishView").Include(

                "~/Assets/Css/new-product-card.css",
                "~/Assets/Css/wish-list-product-card-stylesheet.css",
                "~/Assets/Css/cart_stylesheet.css"));

            bundles.Add(new StyleBundle("~/Back-css/BackLayoutIndex").Include(
                "~/Assets/Css/BackStage/custom.min.css",
                "~/Assets/Css/BackStage/jquery.min.css",
                "~/Assets/Css/BackStage/bootstrap.min.css",
                "~/Assets/Css/BackStage/font-awesome.min.css"
               ));

            bundles.Add(new StyleBundle("~/Back-css/BackCustomerDetails").Include(
                "~/Assets/Css/BackStage/bootstrap.min.css",
                "~/Assets/Css/BackStage/font-awesome.min.css",
                "~/Assets/Css/BackStage/nprogress.css",
                 "~/Assets/Css/BackStage/green.css",
                 "~/Assets/Css/BackStage/prettify.min.css",
                 "~/Assets/Css/BackStage/select2.min.css",
                 "~/Assets/Css/BackStage/switchery.min.css",
                 "~/Assets/Css/BackStage/starrr.css",
                 "~/Assets/Css/BackStage/daterangepicker.css",
                 "~/Assets/Css/BackStage/custom.min.css",
                 "~/Assets/Css/BackStage/productCRUD.scss"
               ));
            bundles.Add(new StyleBundle("~/Back-css/BackCustomerIndex").Include(
                "~/Assets/Css/BackStage/bootstrap.min.css",
                "~/Assets/Css/BackStage/font-awesome.min.css",
                "~/Assets/Css/BackStage/nprogress.css",
                 "~/Assets/Css/BackStage/green.css",
                 "~/Assets/Css/BackStage/dataTables.bootstrap.min.css",
                 "~/Assets/Css/BackStage/buttons.bootstrap.min.css",
                 "~/Assets/Css/BackStage/fixedHeader.bootstrap.min.css",
                 "~/Assets/Css/BackStage/responsive.bootstrap.min.css",
                 "~/Assets/Css/BackStage/scroller.bootstrap.min.css",
                 "~/Assets/Css/BackStage/custom.min.css"
               ));

            bundles.Add(new ScriptBundle("~/Back-Script/BackIndex").Include(
                 "~/Assets/Script/BackStage/jquery.min.js",
                 "~/Assets/Script/BackStage/bootstrap.min.js",
                "~/Assets/Script/BackStage/bootstrap-progressbar.min.js",
                "~/Assets/Script/BackStage/Chart.min.js",
                "~/Assets/Script/BackStage/custom.js",
                "~/Assets/Script/BackStage/date.js",
                "~/Assets/Script/BackStage/gauge.min.js",
                "~/Assets/Script/BackStage/jquery.flot.js"
               ));
            bundles.Add(new ScriptBundle("~/Back-Script/BackCustomerDetails").Include(
                "~/Assets/Script/BackStage/jquery.min.js",
                "~/Assets/Script/BackStage/bootstrap.min.js",
                "~/Assets/Script/BackStage/fastclick.js",
                "~/Assets/Script/BackStage/nprogress.js",
                "~/Assets/Script/BackStage/bootstrap-progressbar.min.js",
                "~/Assets/Script/BackStage/icheck.min.js",
                "~/Assets/Script/BackStage/moment.min.js",
                "~/Assets/Script/BackStage/daterangepicker.js",
                "~/Assets/Script/BackStage/bootstrap-wysiwyg.min.js",
                "~/Assets/Script/BackStage/jquery.hotkeys.js",
                "~/Assets/Script/BackStage/prettify.js",
                "~/Assets/Script/BackStage/jquery.tagsinput.js",
                "~/Assets/Script/BackStage/switchery.min.js",
                "~/Assets/Script/BackStage/select2.full.min.js",
                "~/Assets/Script/BackStage/parsley.min.js",
                "~/Assets/Script/BackStage/autosize.min.js",
                "~/Assets/Script/BackStage/jquery.autocomplete.min.js",
                "~/Assets/Script/BackStage/starrr.js",
                "~/Assets/Script/BackStage/custom.min.js"
               ));
            bundles.Add(new ScriptBundle("~/Back-Script/BackCustomerIndex").Include(                
                "~/Assets/Script/BackStage/jquery.min.js",
                "~/Assets/Script/BackStage/bootstrap.min.js",
                "~/Assets/Script/BackStage/fastclick.js",
                "~/Assets/Script/BackStage/nprogress.js",
                "~/Assets/Script/BackStage/icheck.min.js",
                "~/Assets/Script/BackStage/jquery.dataTables.min.js",
                "~/Assets/Script/BackStage/dataTables.bootstrap.min.js",
                "~/Assets/Script/BackStage/dataTables.buttons.min.js",
                "~/Assets/Script/BackStage/buttons.bootstrap.min.js",
                "~/Assets/Script/BackStage/buttons.flash.min.js",
                "~/Assets/Script/BackStage/buttons.html5.min.js",
                "~/Assets/Script/BackStage/buttons.print.min.js",
                "~/Assets/Script/BackStage/dataTables.fixedHeader.min.js",
                "~/Assets/Script/BackStage/dataTables.keyTable.min.js",
                "~/Assets/Script/BackStage/dataTables.responsive.min.js",
                "~/Assets/Script/BackStage/responsive.bootstrap.js",
                "~/Assets/Script/BackStage/dataTables.scroller.min.js",
                "~/Assets/Script/BackStage/jszip.min.js",
                "~/Assets/Script/BackStage/pdfmake.min.js",
                "~/Assets/Script/BackStage/vfs_fonts.js",
                "~/Assets/Script/BackStage/custom.min.js"
               ));          

        }
    }
}
