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
    public class NodeController : ControllerBase
    {
        public NodeController(ISequenceService sequenceService,
            Grit.Tree.ITreeService treeService,
            INodeService nodeService)
        {
            this.SequenceService = sequenceService;
            this.NodeService = nodeService;
            this.TreeService = treeService;
        }

        private ISequenceService SequenceService { get; set; }
        private Grit.Tree.ITreeService TreeService { get; set; }
        private INodeService NodeService { get; set; }

        [HttpGet]
        [Auth]
        public ActionResult Basic(int id)
        {
            Node node = NodeService.GetNode(id);
            if (node == null)
            {
                return new HttpNotFoundResult("Node not found");
            }
            NodeVM vm = NodeVM.FromModel(node);
            return new JsonNetResult(vm);
        }

        [HttpGet]
        [Auth]
        public ActionResult Edit(int? id)
        {
            if(id.HasValue)
            {
                Node node = NodeService.GetNode(id.Value);
                if (node == null)
                {
                    return new HttpNotFoundResult("Node not found");
                }
                NodeVM vm = NodeVM.FromModel(node).Fill();
                return View(vm);
            }
            return View(NodeVM.FromModel().Fill());
        }

        [HttpPost]
        [Auth]
        public ActionResult Edit(NodeVM vm)
        {
            if (!ModelState.IsValid || !vm.IsValid(ModelState))
            {
                return View(vm);
            }

            var node = NodeVM.ToModel(vm);
            node.UpdateAt = DateTime.Now;
            if (node.NodeId == 0)
            {
                node.NodeId = SequenceService.Next(Constants.SEQUENCE_SETTINGS_NODE, 1);
                node.CreateAt = node.UpdateAt;
            }

            if (vm.Deleted)
            {
                node.DeleteAt = node.UpdateAt;
                if (!NodeService.DeleteNode(node))
                {
                    ModelState.AddModelError(string.Empty, "Failed to delete, other users may have edited the data during your processing");
                    return View(vm);
                }
                Info = "Delete successfully";
                return RedirectToAction("Group", "Node");
            }
            else
            {
                if (!NodeService.SaveNode(node))
                {
                    ModelState.AddModelError(string.Empty, "Failed to save, other users may have edited the data during your processing");
                    return View(vm);
                }
                Info = "Saved successfully";
                return RedirectToAction("Edit", new { id = node.NodeId });
            }
        }

        [HttpGet]
        [Auth]
        public ActionResult Group()
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

        [HttpPost]
        [Auth]
        public ActionResult Group([ModelBinder(typeof(JsonNetModelBinder))] IList<JsTreeNode> nodes)
        {
            var root = new JsTreeParser().Parse(Constants.TREE_NODE, nodes);
            TreeService.SaveTree(root);
            return new JsonNetResult(nodes);
        }
    }
}