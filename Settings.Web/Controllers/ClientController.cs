using Grit.Sequence;
using Grit.Tree.JsTree;
using Grit.Utility.Web.Json;
using Settings.Model;
using Settings.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Settings.Web.Controllers
{
    public class ClientController : ControllerBase
    {
        public ClientController(ISequenceService sequenceService,
            Grit.Tree.ITreeService treeService,
            IClientService clientService,
            INodeService nodeService)
        {
            this.SequenceService = sequenceService;
            this.ClientService = clientService;
            this.TreeService = treeService;
            this.NodeService = nodeService;
        }

        private ISequenceService SequenceService { get; set; }
        private Grit.Tree.ITreeService TreeService { get; set; }
        private IClientService ClientService { get; set; }
        private INodeService NodeService { get; set; }

        [HttpGet]
        [Auth]
        public ActionResult Edit(int? id)
        {
            if (id.HasValue)
            {
                Settings.Model.Client client = ClientService.GetClient(id.Value);
                if (client == null)
                {
                    return new HttpNotFoundResult("Client not found");
                }
                ClientVM vm = ClientVM.FromModel(client);
                return View(vm);
            }
            return View(ClientVM.FromModel());
        }

        [HttpPost]
        [Auth]
        [ValidateInput(false)]
        public ActionResult Edit(ClientVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var client = ClientVM.ToModel(vm);
            client.UpdateAt = DateTime.Now;
            if (client.ClientId == 0)
            {
                client.ClientId = SequenceService.Next(Constants.SEQUENCE_SETTINGS_CLIENT, 1);
                client.CreateAt = client.UpdateAt;
            }

            if (vm.Deleted)
            {
                client.DeleteAt = client.UpdateAt;
                if(!ClientService.DeleteClient(client))
                {
                    ModelState.AddModelError(string.Empty, "Failed to delete, other users may have edited the data during your processing");
                    return View(vm);
                }
                Info = "Delete successfully";
                return RedirectToAction("Map", "Client");
            }
            else
            {
                if (!ClientService.SaveClient(client))
                {
                    ModelState.AddModelError(string.Empty, "Failed to save, other users may have edited the data during your processing");
                    return View(vm);
                }
                Info = "Saved successfully";
                return RedirectToAction("Edit", new { id = client.ClientId });
            }
        }

        [HttpGet]
        [Auth]
        public ActionResult Map()
        {
            var clients = ClientService.GetClients();
            var leftTree = new Grit.Tree.Node(1);

            ViewBag.LeftTree = new Grit.Tree.JsTree.JsTreeBuilder<Settings.Model.Client>(x => x.Name, x => x.ClientId, x => x.Nodes)
                .Build(leftTree, clients)
                .children;

            var nodes = NodeService.GetNodes();
            var rightTree = TreeService.GetTree(Constants.TREE_NODE);
            ViewBag.RightTree = new Grit.Tree.JsTree.JsTreeBuilder<Node>(x => x.Name, x => x.NodeId)
                .Build(rightTree, nodes)
                .children;

            return View();
        }

        [HttpPost]
        [Auth]
        public ActionResult Map([ModelBinder(typeof(JsonNetModelBinder))] IList<Grit.Tree.JsTree.JsTreeNode> tree)
        {
            var root = new JsTreeParser().Parse(Constants.TREE_NODE, tree);
            var clients = ClientService.GetClients();
            var nodes = NodeService.GetNodes();
            root.Each(x =>
            {
                if (x.Elements != null)
                {
                    var client = clients.SingleOrDefault(n => n.ClientId == x.Data);
                    if (client != null)
                    {
                        client.Nodes = nodes.Where(n => x.Elements.Any(m => m == n.NodeId)).Select(n => n.NodeId).ToArray();
                    }
                }
            });
            ClientService.SaveClientNodes(clients);
            return new JsonNetResult(clients);
        }
    }
}