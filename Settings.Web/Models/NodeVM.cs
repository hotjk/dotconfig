using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Grit.Utility.Web.Extensions;
using Settings.Model;
using AutoMapper;

namespace Settings.Web.Models
{
    public class NodeVM
    {
        public int NodeId { get; set; }

        [Display(Name = "Node Name")]
        [Required(ErrorMessage = "{0} is required")]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z0-9_]{0,99}$", ErrorMessage = "{0} must be less than 100 letters, numbers or underscores, and must begin with a letter")]
        public string Name { get; set; }
        
        [Display(Name = "Node Entries")]
        public List<EntryVM> Entries { get; set; }

        public bool Deleted { get; set; }
        public int Version { get; set; }

        private const int ENTRY_STEP = 4;

        static NodeVM()
        {
            Mapper.CreateMap<Node, NodeVM>();
            Mapper.CreateMap<NodeVM, Node>();
            Mapper.CreateMap<Entry, EntryVM>();
            Mapper.CreateMap<EntryVM, Entry>();
        }

        public bool IsValid(ModelStateDictionary ModelState)
        {
            bool isValid = true;
            if (this.Entries != null)
            {
                for (int i = 0; i < this.Entries.Count; i++)
                {
                    EntryVM entry = this.Entries[i];
                    if (!string.IsNullOrWhiteSpace(entry.Value) && string.IsNullOrWhiteSpace(entry.Key))
                    {
                        ModelState.AddModelError(this.GetExpressionText(x => this.Entries[i].Key),
                            string.Format(@"{0} is required", this.GetDisplayName(x => this.Entries[i].Key)));
                        isValid = false;
                    }
                }
            }
            return isValid;
        }

        public static Node ToModel(NodeVM vm)
        {
            var node = Mapper.Map<Node>(vm);
            node.Entries = node.Entries.Where(n => !string.IsNullOrWhiteSpace(n.Key)).OrderBy(n => n.Key).ToList();
            return node;
        }

        public static NodeVM FromModel(Node node = null)
        {
            NodeVM vm;
            if (node == null)
            {
                vm = new NodeVM();
            }
            else
            {
                vm = Mapper.Map<NodeVM>(node);
            }

            if (vm.Entries == null)
            {
                vm.Entries = new List<EntryVM>(ENTRY_STEP);
            }
            
            return vm;
        }

        public NodeVM Fill()
        {
            for (int i = 0; i < ENTRY_STEP; i++)
            {
                Entries.Add(new EntryVM());
            }
            return this;
        }
    }
}