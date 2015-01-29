using Grit.Utility.Security;
using Newtonsoft.Json;
using Settings.Client;
using Settings.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Settings.Web.Controllers
{
    public class SettingsController : ApiController
    {
        public SettingsController(INodeService nodeService,
            IClientService clientService,
            Grit.Tree.ITreeService treeService)
        {
            this.NodeService = nodeService;
            this.TreeService = treeService;
            this.ClientService = clientService;
        }

        private Grit.Tree.ITreeService TreeService { get; set; }
        private INodeService NodeService  {get;set;}
        private IClientService ClientService { get; set; }

        [HttpPost]
        [Route("api/settings")]
        public HttpResponseMessage Index( 
            [System.Web.Mvc.ModelBinder(typeof(Grit.Utility.Web.Json.JsonNetModelBinder))] Envelope envelope)
        {
            var client = ClientService.GetClient(envelope.Id);
            if (client == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Client not found");
            }

            var decrypted = EnvelopeService.PublicDecrypt(envelope, client.PublicKey);
            var req = JsonConvert.DeserializeObject<SettingsRequest>(decrypted);
            if (req.Client != envelope.Id)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid client");
            }

            var tree = TreeService.GetTree(Constants.TREE_NODE);
            SettingsResponse settings = NodeService.GetClientSettings(client, tree)
                .Filter(req.Pattern);

            string json = JsonConvert.SerializeObject(settings);

            Envelope resp = EnvelopeService.Encrypt(client.Name, json, client.PublicKey);
            return Request.CreateResponse(HttpStatusCode.OK, resp);
        }
    }
}
