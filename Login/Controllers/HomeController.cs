using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoginPageDemo.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // ����û��Ƿ��Ѿ���¼
            if (User.Identity.IsAuthenticated)
            {
                var userName = User.Identity.Name;  // ��ȡ�û�������
                                                    // ������ͼ����ʾ�û���
                ViewBag.UserName = userName;
            }
            else
            {
                ViewBag.UserName = "δ��¼";
            }
            return View();
        }
    }


}
