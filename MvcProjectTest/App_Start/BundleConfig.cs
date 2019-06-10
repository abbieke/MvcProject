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
                "~/Assets/Css/for-change-style.css"
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
                "~/Assets/Css/BackStage/custom.min.css"
               ));

            bundles.Add(new ScriptBundle("~/Back-Script/BackLayoutIndex").Include(
                "~/Assets/Script/BackStage/bootstrap-progressbar.min.js",
                "~/Assets/Script/BackStage/Chart.min.js",
                "~/Assets/Script/BackStage/custom.js",
                "~/Assets/Script/BackStage/date.js",
                "~/Assets/Script/BackStage/gauge.min.js",
                "~/Assets/Script/BackStage/jquery.flot.js"
               ));

        }
    }
}
