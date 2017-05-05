// Code generated by Microsoft (R) AutoRest Code Generator 1.0.1.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace ApplicationGateway.Models
{
    using Microsoft.Rest;
    using Microsoft.Rest.Serialization;
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Virtual Network resource.
    /// </summary>
    [JsonTransformation]
    public partial class VirtualNetwork : Resource
    {
        /// <summary>
        /// Initializes a new instance of the VirtualNetwork class.
        /// </summary>
        public VirtualNetwork()
        {
          CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the VirtualNetwork class.
        /// </summary>
        /// <param name="id">Resource ID.</param>
        /// <param name="name">Resource name.</param>
        /// <param name="type">Resource type.</param>
        /// <param name="location">Resource location.</param>
        /// <param name="tags">Resource tags.</param>
        /// <param name="addressSpace">The AddressSpace that contains an array
        /// of IP address ranges that can be used by subnets.</param>
        /// <param name="dhcpOptions">The dhcpOptions that contains an array of
        /// DNS servers available to VMs deployed in the virtual
        /// network.</param>
        /// <param name="subnets">A list of subnets in a Virtual
        /// Network.</param>
        /// <param name="virtualNetworkPeerings">A list of peerings in a
        /// Virtual Network.</param>
        /// <param name="resourceGuid">The resourceGuid property of the Virtual
        /// Network resource.</param>
        /// <param name="provisioningState">The provisioning state of the
        /// PublicIP resource. Possible values are: 'Updating', 'Deleting', and
        /// 'Failed'.</param>
        /// <param name="etag">Gets a unique read-only string that changes
        /// whenever the resource is updated.</param>
        public VirtualNetwork(string id = default(string), string name = default(string), string type = default(string), string location = default(string), IDictionary<string, string> tags = default(IDictionary<string, string>), AddressSpace addressSpace = default(AddressSpace), DhcpOptions dhcpOptions = default(DhcpOptions), IList<Subnet> subnets = default(IList<Subnet>), IList<VirtualNetworkPeering> virtualNetworkPeerings = default(IList<VirtualNetworkPeering>), string resourceGuid = default(string), string provisioningState = default(string), string etag = default(string))
            : base(id, name, type, location, tags)
        {
            AddressSpace = addressSpace;
            DhcpOptions = dhcpOptions;
            Subnets = subnets;
            VirtualNetworkPeerings = virtualNetworkPeerings;
            ResourceGuid = resourceGuid;
            ProvisioningState = provisioningState;
            Etag = etag;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// Gets or sets the AddressSpace that contains an array of IP address
        /// ranges that can be used by subnets.
        /// </summary>
        [JsonProperty(PropertyName = "properties.addressSpace")]
        public AddressSpace AddressSpace { get; set; }

        /// <summary>
        /// Gets or sets the dhcpOptions that contains an array of DNS servers
        /// available to VMs deployed in the virtual network.
        /// </summary>
        [JsonProperty(PropertyName = "properties.dhcpOptions")]
        public DhcpOptions DhcpOptions { get; set; }

        /// <summary>
        /// Gets or sets a list of subnets in a Virtual Network.
        /// </summary>
        [JsonProperty(PropertyName = "properties.subnets")]
        public IList<Subnet> Subnets { get; set; }

        /// <summary>
        /// Gets or sets a list of peerings in a Virtual Network.
        /// </summary>
        [JsonProperty(PropertyName = "properties.virtualNetworkPeerings")]
        public IList<VirtualNetworkPeering> VirtualNetworkPeerings { get; set; }

        /// <summary>
        /// Gets or sets the resourceGuid property of the Virtual Network
        /// resource.
        /// </summary>
        [JsonProperty(PropertyName = "properties.resourceGuid")]
        public string ResourceGuid { get; set; }

        /// <summary>
        /// Gets or sets the provisioning state of the PublicIP resource.
        /// Possible values are: 'Updating', 'Deleting', and 'Failed'.
        /// </summary>
        [JsonProperty(PropertyName = "properties.provisioningState")]
        public string ProvisioningState { get; set; }

        /// <summary>
        /// Gets a unique read-only string that changes whenever the resource
        /// is updated.
        /// </summary>
        [JsonProperty(PropertyName = "etag")]
        public string Etag { get; set; }

    }
}
