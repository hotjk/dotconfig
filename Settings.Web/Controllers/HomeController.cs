using Grit.Sequence;
using Grit.Utility.Security;
using Settings.Model;
using Settings.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Settings.Web;

namespace Settings.Web.Controllers
{
    public class HomeController : ControllerBase
    {
        public HomeController(ISequenceService sequenceService,
            Grit.Tree.ITreeService treeService,
            INodeService nodeService,
            IUserService userService)
        {
            this.SequenceService = sequenceService;
            this.NodeService = nodeService;
            this.TreeService = treeService;
            this.UserService = userService;
        }

        private ISequenceService SequenceService { get; set; }
        private Grit.Tree.ITreeService TreeService { get; set; }
        private INodeService NodeService { get; set; }
        private IUserService UserService { get; set; }

        [Auth]
        public ActionResult Index()
        {
            var nodes = NodeService.GetNodes();
            var root = TreeService.GetTree(Constants.TREE_NODE);
            ViewBag.Tree = new Grit.Tree.JsTree.JsTreeBuilder<Node>(
                x => x.Name,
                x => x.NodeId)
                .Build(root, nodes)
                .children;
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View(new LoginVM());
        }

        [HttpPost]
        public ActionResult Login(LoginVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            User user = UserService.GetUser(vm.Username);
            if (user == null || user.Deleted)
            {
                ModelState.AddModelError(string.Empty, "The username you entered can not find. Please double-check and try again");
                return View(vm);
            }

            if(!PasswordHash.ValidatePassword(vm.Password, user.PasswordHash))
            {
                ModelState.AddModelError(string.Empty, "The username and password you entered did not match. Please double-check and try again");
                return View(vm);
            }

            Response.SetAuthCookie(user.Username, false, user.Username);

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Auth]
        public ActionResult ChangePassword()
        {
            User user = UserService.GetUser(User.Identity.Name);

            var vm = new ChangePasswordVM
            {
                Username = user.Username
            };

            return View(vm);
        }

        [HttpPost]
        [Auth]
        public ActionResult ChangePassword(ChangePasswordVM vm)
        {
            vm.Username = User.Identity.Name;
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            User user = UserService.GetUser(User.Identity.Name);

            if (!PasswordHash.ValidatePassword(vm.OldPassword, user.PasswordHash))
            {
                ModelState.AddModelError(string.Empty, "The password you entered did not match. Please double-check and try again");
                return View(vm);
            }

            user.Password = vm.Password;
            UserService.SaveUser(user);

            Info = "Change password successfully";
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Auth]
        public ActionResult Signup()
        {
            return View(new SignupVM());
        }

        [HttpPost]
        [Auth]
        public ActionResult Signup(SignupVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            User found = UserService.GetUser(vm.Username);
            if (found != null)
            {
                ModelState.AddModelError(string.Empty, "The username already existed");
                return View(vm);
            }

            User user = new Model.User
            {
                Username = vm.Username,
                Password = vm.Password
            };

            UserService.SaveUser(user);
            
            Info = "Create user successfully";
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Auth]
        public ActionResult Logout()
        {
            System.Web.Security.FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}